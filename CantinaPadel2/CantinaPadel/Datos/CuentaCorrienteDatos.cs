using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace CantinaPadel.Datos
{
    public class CuentaCorrienteDatos
    {
        private Conexion conexion = new Conexion();

        // Una fila por cuenta, con la deuda total de cada cliente
        public DataTable ObtenerCuentas()
        {
            DataTable tabla = new DataTable();
            using (MySqlConnection cn = conexion.CrearConexion())
            {
                try
                {
                    string query = @"
                        SELECT cc.id_cuenta,
                               c.id_cliente,
                               TRIM(CONCAT(p.nombre, ' ', COALESCE(p.apellido, ''))) AS cliente,
                               COALESCE(SUM(d.saldo_pendiente), 0) AS deuda_total
                        FROM cuenta_corriente cc
                        INNER JOIN cliente c ON cc.id_cliente = c.id_cliente
                        INNER JOIN persona p ON c.id_persona = p.id_persona
                        LEFT JOIN detalle_deuda d ON d.id_cuenta = cc.id_cuenta AND d.estado <> 'Pagada'
                        GROUP BY cc.id_cuenta, c.id_cliente, p.nombre, p.apellido
                        ORDER BY deuda_total DESC, p.nombre";

                    MySqlDataAdapter da = new MySqlDataAdapter(query, cn);
                    da.Fill(tabla);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al cargar las cuentas corrientes: " + ex.Message);
                }
            }
            return tabla;
        }

        // Detalle de lo que debe una cuenta (más viejo primero)
        public DataTable ObtenerDeudas(int idCuenta, bool incluirPagadas)
        {
            DataTable tabla = new DataTable();
            using (MySqlConnection cn = conexion.CrearConexion())
            {
                try
                {
                    string query = @"
                        SELECT id_detalle_deuda, descripcion, cantidad, precio_actual, saldo_pendiente, estado
                        FROM detalle_deuda
                        WHERE id_cuenta = @id_cuenta
                          AND (@incluir = 1 OR estado <> 'Pagada')
                        ORDER BY id_detalle_deuda";

                    MySqlCommand cmd = new MySqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@id_cuenta", idCuenta);
                    cmd.Parameters.AddWithValue("@incluir", incluirPagadas ? 1 : 0);

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(tabla);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al cargar el detalle de la deuda: " + ex.Message);
                }
            }
            return tabla;
        }

        // Deuda total de un cliente (0 si no tiene cuenta)
        public decimal ObtenerDeudaCliente(int idCliente)
        {
            using (MySqlConnection cn = conexion.CrearConexion())
            {
                try
                {
                    string query = @"
                        SELECT COALESCE(SUM(d.saldo_pendiente), 0)
                        FROM cuenta_corriente cc
                        INNER JOIN detalle_deuda d ON d.id_cuenta = cc.id_cuenta AND d.estado <> 'Pagada'
                        WHERE cc.id_cliente = @id_cliente";

                    MySqlCommand cmd = new MySqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@id_cliente", idCliente);
                    cn.Open();
                    return Convert.ToDecimal(cmd.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al consultar la deuda del cliente: " + ex.Message);
                }
            }
        }

        // Paga (total o parcialmente) UNA línea de deuda
        public void RegistrarPagoLinea(int idDetalleDeuda, decimal monto, string medioPago, int? idCaja, int? idUsuario)
        {
            using (MySqlConnection cn = conexion.CrearConexion())
            {
                cn.Open();
                using (MySqlTransaction tx = cn.BeginTransaction())
                {
                    try
                    {
                        AplicarPago(cn, tx, idDetalleDeuda, monto, medioPago, idCaja, idUsuario);
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw new Exception("No se pudo registrar el pago: " + ex.Message);
                    }
                }
            }
        }

        // Paga un monto "a cuenta": se descuenta de las deudas más viejas primero
        public void RegistrarPagoCuenta(int idCuenta, decimal monto, string medioPago, int? idCaja, int? idUsuario)
        {
            using (MySqlConnection cn = conexion.CrearConexion())
            {
                cn.Open();
                using (MySqlTransaction tx = cn.BeginTransaction())
                {
                    try
                    {
                        MySqlCommand cmdLineas = new MySqlCommand(@"
                            SELECT id_detalle_deuda, saldo_pendiente
                            FROM detalle_deuda
                            WHERE id_cuenta = @id_cuenta AND estado <> 'Pagada'
                            ORDER BY id_detalle_deuda
                            FOR UPDATE", cn, tx);
                        cmdLineas.Parameters.AddWithValue("@id_cuenta", idCuenta);

                        var lineas = new System.Collections.Generic.List<(int id, decimal saldo)>();
                        using (MySqlDataReader dr = cmdLineas.ExecuteReader())
                        {
                            while (dr.Read())
                                lineas.Add((Convert.ToInt32(dr["id_detalle_deuda"]), Convert.ToDecimal(dr["saldo_pendiente"])));
                        }

                        decimal deudaTotal = 0;
                        foreach (var l in lineas) deudaTotal += l.saldo;

                        if (monto > deudaTotal)
                            throw new Exception("El monto (" + monto.ToString("C2") + ") supera la deuda total (" + deudaTotal.ToString("C2") + ").");

                        decimal restante = monto;
                        foreach (var l in lineas)
                        {
                            if (restante <= 0) break;
                            decimal aPagar = Math.Min(restante, l.saldo);
                            AplicarPago(cn, tx, l.id, aPagar, medioPago, idCaja, idUsuario);
                            restante -= aPagar;
                        }

                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw new Exception("No se pudo registrar el pago: " + ex.Message);
                    }
                }
            }
        }

        // Baja el saldo de una línea, actualiza su estado y deja el registro del pago
        private void AplicarPago(MySqlConnection cn, MySqlTransaction tx, int idDetalleDeuda, decimal monto,
                                 string medioPago, int? idCaja, int? idUsuario)
        {
            MySqlCommand cmdSaldo = new MySqlCommand(
                "SELECT saldo_pendiente FROM detalle_deuda WHERE id_detalle_deuda = @id FOR UPDATE", cn, tx);
            cmdSaldo.Parameters.AddWithValue("@id", idDetalleDeuda);
            object? r = cmdSaldo.ExecuteScalar();
            if (r == null || r == DBNull.Value)
                throw new Exception("La deuda seleccionada no existe.");

            decimal saldo = Convert.ToDecimal(r);
            if (monto <= 0)
                throw new Exception("El monto debe ser mayor a cero.");
            if (monto > saldo)
                throw new Exception("El monto (" + monto.ToString("C2") + ") supera el saldo pendiente (" + saldo.ToString("C2") + ").");

            decimal nuevoSaldo = saldo - monto;
            string nuevoEstado = nuevoSaldo == 0 ? "Pagada" : "Parcial";

            MySqlCommand cmdUpd = new MySqlCommand(
                "UPDATE detalle_deuda SET saldo_pendiente = @saldo, estado = @estado WHERE id_detalle_deuda = @id", cn, tx);
            cmdUpd.Parameters.AddWithValue("@saldo", nuevoSaldo);
            cmdUpd.Parameters.AddWithValue("@estado", nuevoEstado);
            cmdUpd.Parameters.AddWithValue("@id", idDetalleDeuda);
            cmdUpd.ExecuteNonQuery();

            MySqlCommand cmdPago = new MySqlCommand(@"
                INSERT INTO pago_cuenta_corriente (id_detalle_deuda, fecha, monto_pagado, id_caja, id_usuario, medio_pago)
                VALUES (@id_detalle, @fecha, @monto, @id_caja, @id_usuario, @medio)", cn, tx);
            cmdPago.Parameters.AddWithValue("@id_detalle", idDetalleDeuda);
            cmdPago.Parameters.AddWithValue("@fecha", DateTime.Now);
            cmdPago.Parameters.AddWithValue("@monto", monto);
            cmdPago.Parameters.AddWithValue("@id_caja", (object?)idCaja ?? DBNull.Value);
            cmdPago.Parameters.AddWithValue("@id_usuario", (object?)idUsuario ?? DBNull.Value);
            cmdPago.Parameters.AddWithValue("@medio", medioPago);
            cmdPago.ExecuteNonQuery();
        }
    }
}

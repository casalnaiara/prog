using CantinaPadel.Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace CantinaPadel.Datos
{
    public class VentaDatos
    {
        private Conexion conexion = new Conexion();

        // Productos activos para el punto de venta (con stock y precio actual)
        public DataTable ObtenerProductosParaVenta(string texto)
        {
            DataTable tabla = new DataTable();
            using (MySqlConnection cn = conexion.CrearConexion())
            {
                try
                {
                    string query = @"
                        SELECT id_producto, codigo, nombre, stock, precio_venta
                        FROM producto
                        WHERE activo = 1
                          AND (@texto = '' OR nombre LIKE @like OR codigo LIKE @like)
                        ORDER BY nombre";

                    MySqlCommand cmd = new MySqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@texto", texto);
                    cmd.Parameters.AddWithValue("@like", "%" + texto + "%");

                    MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                    da.Fill(tabla);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al cargar los productos: " + ex.Message);
                }
            }
            return tabla;
        }

        // Clientes activos para el combo de la venta
        public DataTable ObtenerClientes()
        {
            DataTable tabla = new DataTable();
            using (MySqlConnection cn = conexion.CrearConexion())
            {
                try
                {
                    string query = @"
                        SELECT c.id_cliente,
                               TRIM(CONCAT(p.nombre, ' ', COALESCE(p.apellido, ''))) AS nombre
                        FROM cliente c
                        INNER JOIN persona p ON c.id_persona = p.id_persona
                        WHERE c.activo = 1 AND p.activo = 1
                        ORDER BY p.nombre, p.apellido";

                    MySqlDataAdapter da = new MySqlDataAdapter(query, cn);
                    da.Fill(tabla);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al cargar los clientes: " + ex.Message);
                }
            }
            return tabla;
        }

        // Registra la venta completa en UNA transacción:
        // cabecera + detalle + descuento de stock (+ deuda en cuenta corriente si es fiada).
        // Si algo falla (por ejemplo stock insuficiente) no se guarda nada.
        public int InsertarVenta(Venta venta)
        {
            using (MySqlConnection cn = conexion.CrearConexion())
            {
                cn.Open();
                using (MySqlTransaction tx = cn.BeginTransaction())
                {
                    try
                    {
                        int idCuenta = 0;
                        if (venta.EsFiada)
                        {
                            idCuenta = ObtenerOCrearCuenta(cn, tx, venta.IdCliente!.Value);
                        }

                        string queryVenta = @"
                            INSERT INTO venta (fecha, tipo_venta, medio_pago, tipo_comprobante, tipo_salida,
                                               total, estado, id_cliente, id_caja, id_reserva, id_usuario)
                            VALUES (@fecha, @tipo_venta, @medio_pago, @tipo_comprobante, @tipo_salida,
                                    @total, @estado, @id_cliente, @id_caja, @id_reserva, @id_usuario);
                            SELECT LAST_INSERT_ID();";

                        MySqlCommand cmdVenta = new MySqlCommand(queryVenta, cn, tx);
                        cmdVenta.Parameters.AddWithValue("@fecha", venta.Fecha);
                        cmdVenta.Parameters.AddWithValue("@tipo_venta", venta.TipoVenta);
                        cmdVenta.Parameters.AddWithValue("@medio_pago", (object?)venta.MedioPago ?? DBNull.Value);
                        cmdVenta.Parameters.AddWithValue("@tipo_comprobante", venta.TipoComprobante);
                        cmdVenta.Parameters.AddWithValue("@tipo_salida", venta.TipoSalida);
                        cmdVenta.Parameters.AddWithValue("@total", venta.Total);
                        cmdVenta.Parameters.AddWithValue("@estado", venta.Estado);
                        cmdVenta.Parameters.AddWithValue("@id_cliente", (object?)venta.IdCliente ?? DBNull.Value);
                        cmdVenta.Parameters.AddWithValue("@id_caja", (object?)venta.IdCaja ?? DBNull.Value);
                        cmdVenta.Parameters.AddWithValue("@id_reserva", (object?)venta.IdReserva ?? DBNull.Value);
                        cmdVenta.Parameters.AddWithValue("@id_usuario", (object?)venta.IdUsuario ?? DBNull.Value);

                        int idVenta = Convert.ToInt32(cmdVenta.ExecuteScalar());

                        foreach (DetalleVenta d in venta.Detalles)
                        {
                            // 1. Bloqueamos el producto y leemos stock y precio REALES de la base
                            MySqlCommand cmdProd = new MySqlCommand(
                                "SELECT nombre, stock, precio_venta, activo FROM producto WHERE id_producto = @id FOR UPDATE", cn, tx);
                            cmdProd.Parameters.AddWithValue("@id", d.IdProducto);

                            string nombre;
                            int stock;
                            decimal precioActual;
                            bool activo;
                            using (MySqlDataReader dr = cmdProd.ExecuteReader())
                            {
                                if (!dr.Read())
                                    throw new Exception("El producto con ID " + d.IdProducto + " ya no existe.");

                                nombre = dr["nombre"].ToString() ?? "";
                                stock = dr["stock"] != DBNull.Value ? Convert.ToInt32(dr["stock"]) : 0;
                                precioActual = Convert.ToDecimal(dr["precio_venta"]);
                                activo = Convert.ToBoolean(dr["activo"]);
                            }

                            if (!activo)
                                throw new Exception("El producto \"" + nombre + "\" está dado de baja.");

                            if (stock < d.Cantidad)
                                throw new Exception("Stock insuficiente de \"" + nombre + "\". Disponible: " + stock + ", pedido: " + d.Cantidad + ".");

                            if (precioActual != d.PrecioUnitario)
                                throw new Exception("El precio de \"" + nombre + "\" cambió mientras armaba la venta. Vuelva a cargar los productos.");

                            // 2. Descontamos stock
                            MySqlCommand cmdStock = new MySqlCommand(
                                "UPDATE producto SET stock = stock - @cantidad WHERE id_producto = @id", cn, tx);
                            cmdStock.Parameters.AddWithValue("@cantidad", d.Cantidad);
                            cmdStock.Parameters.AddWithValue("@id", d.IdProducto);
                            cmdStock.ExecuteNonQuery();

                            // 3. Guardamos la línea de la venta
                            MySqlCommand cmdDet = new MySqlCommand(@"
                                INSERT INTO detalle_venta (id_venta, id_producto, cantidad, precio_unitario, subtotal)
                                VALUES (@id_venta, @id_producto, @cantidad, @precio, @subtotal)", cn, tx);
                            cmdDet.Parameters.AddWithValue("@id_venta", idVenta);
                            cmdDet.Parameters.AddWithValue("@id_producto", d.IdProducto);
                            cmdDet.Parameters.AddWithValue("@cantidad", d.Cantidad);
                            cmdDet.Parameters.AddWithValue("@precio", d.PrecioUnitario);
                            cmdDet.Parameters.AddWithValue("@subtotal", d.Subtotal);
                            cmdDet.ExecuteNonQuery();

                            // 4. Si es fiada, cada línea queda como deuda en la cuenta del cliente
                            if (venta.EsFiada)
                            {
                                MySqlCommand cmdDeuda = new MySqlCommand(@"
                                    INSERT INTO detalle_deuda (id_cuenta, id_producto, descripcion, cantidad,
                                                               precio_original, precio_actual, precio_anclado,
                                                               saldo_pendiente, estado)
                                    VALUES (@id_cuenta, @id_producto, @descripcion, @cantidad,
                                            @precio, @precio, 0, @saldo, 'Pendiente')", cn, tx);
                                cmdDeuda.Parameters.AddWithValue("@id_cuenta", idCuenta);
                                cmdDeuda.Parameters.AddWithValue("@id_producto", d.IdProducto);
                                cmdDeuda.Parameters.AddWithValue("@descripcion", nombre);
                                cmdDeuda.Parameters.AddWithValue("@cantidad", d.Cantidad);
                                cmdDeuda.Parameters.AddWithValue("@precio", d.PrecioUnitario);
                                cmdDeuda.Parameters.AddWithValue("@saldo", d.Subtotal);
                                cmdDeuda.ExecuteNonQuery();
                            }
                        }

                        tx.Commit();
                        return idVenta;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw new Exception("No se pudo registrar la venta: " + ex.Message);
                    }
                }
            }
        }

        // Devuelve el id de la cuenta corriente del cliente; si todavía no tiene, la crea.
        private int ObtenerOCrearCuenta(MySqlConnection cn, MySqlTransaction tx, int idCliente)
        {
            MySqlCommand cmdBuscar = new MySqlCommand(
                "SELECT id_cuenta FROM cuenta_corriente WHERE id_cliente = @id_cliente", cn, tx);
            cmdBuscar.Parameters.AddWithValue("@id_cliente", idCliente);
            object? existente = cmdBuscar.ExecuteScalar();
            if (existente != null && existente != DBNull.Value)
                return Convert.ToInt32(existente);

            MySqlCommand cmdCrear = new MySqlCommand(
                "INSERT INTO cuenta_corriente (id_cliente) VALUES (@id_cliente); SELECT LAST_INSERT_ID();", cn, tx);
            cmdCrear.Parameters.AddWithValue("@id_cliente", idCliente);
            return Convert.ToInt32(cmdCrear.ExecuteScalar());
        }
    }
}

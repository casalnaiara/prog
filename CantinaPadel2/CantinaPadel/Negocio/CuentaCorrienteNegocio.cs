using CantinaPadel.Datos;
using System;
using System.Data;
using System.Linq;

namespace CantinaPadel.Negocio
{
    public class CuentaCorrienteNegocio
    {
        private CuentaCorrienteDatos datos = new CuentaCorrienteDatos();

        public DataTable ObtenerCuentas()
        {
            return datos.ObtenerCuentas();
        }

        public DataTable ObtenerDeudas(int idCuenta, bool incluirPagadas)
        {
            if (idCuenta <= 0) return new DataTable();
            return datos.ObtenerDeudas(idCuenta, incluirPagadas);
        }

        public decimal ObtenerDeudaCliente(int idCliente)
        {
            if (idCliente <= 0) return 0;
            return datos.ObtenerDeudaCliente(idCliente);
        }

        private void ValidarPago(decimal monto, string medioPago)
        {
            if (monto <= 0)
                throw new Exception("El monto a pagar debe ser mayor a cero.");

            if (string.IsNullOrWhiteSpace(medioPago) || !VentaNegocio.MediosPago.Contains(medioPago))
                throw new Exception("Debe seleccionar un medio de pago válido.");
        }

        // idCaja / idUsuario quedan en null hasta que existan los módulos de caja y login
        public void PagarLinea(int idDetalleDeuda, decimal monto, string medioPago)
        {
            if (idDetalleDeuda <= 0)
                throw new Exception("Seleccione la deuda que desea pagar.");
            ValidarPago(monto, medioPago);
            datos.RegistrarPagoLinea(idDetalleDeuda, monto, medioPago, null, null);
        }

        public void PagarCuenta(int idCuenta, decimal monto, string medioPago)
        {
            if (idCuenta <= 0)
                throw new Exception("Seleccione un cliente.");
            ValidarPago(monto, medioPago);
            datos.RegistrarPagoCuenta(idCuenta, monto, medioPago, null, null);
        }
    }
}

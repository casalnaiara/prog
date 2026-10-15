using CantinaPadel.Datos;
using CantinaPadel.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CantinaPadel.Negocio
{
    public class VentaNegocio
    {
        private VentaDatos ventaDatos = new VentaDatos();

        public static readonly string[] MediosPago = { "Efectivo", "Tarjeta", "Transferencia" };

        public DataTable ObtenerProductosParaVenta(string texto)
        {
            return ventaDatos.ObtenerProductosParaVenta((texto ?? "").Trim());
        }

        public DataTable ObtenerClientes()
        {
            return ventaDatos.ObtenerClientes();
        }

        // Valida todo antes de ir a la base y devuelve el id de la venta registrada
        public int RegistrarVenta(Venta venta)
        {
            if (venta.Detalles == null || venta.Detalles.Count == 0)
                throw new Exception("No se puede registrar una venta vacía. Agregue al menos un producto al carrito.");

            foreach (DetalleVenta d in venta.Detalles)
            {
                if (d.IdProducto <= 0)
                    throw new Exception("Se detectó un producto inválido en el carrito.");
                if (d.Cantidad <= 0)
                    throw new Exception("La cantidad de cada producto debe ser mayor a cero.");
                if (d.PrecioUnitario < 0)
                    throw new Exception("El precio de un producto no puede ser negativo.");
            }

            decimal suma = venta.Detalles.Sum(d => d.Subtotal);
            if (venta.Total != suma)
                throw new Exception("Inconsistencia de datos: el total no coincide con la suma de los productos.");

            if (venta.EsFiada)
            {
                // Fiar solo se puede a un cliente identificado
                if (venta.IdCliente == null || venta.IdCliente <= 0)
                    throw new Exception("Para fiar una venta debe seleccionar un cliente.");

                venta.MedioPago = null; // todavía no se pagó
            }
            else
            {
                if (venta.Estado != "Pagada")
                    throw new Exception("Estado de venta inválido.");

                if (string.IsNullOrWhiteSpace(venta.MedioPago) || !MediosPago.Contains(venta.MedioPago))
                    throw new Exception("Debe seleccionar un medio de pago válido.");
            }

            venta.Fecha = DateTime.Now;
            return ventaDatos.InsertarVenta(venta);
        }
    }
}

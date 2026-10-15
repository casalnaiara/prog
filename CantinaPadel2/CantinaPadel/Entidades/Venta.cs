using System;
using System.Collections.Generic;

namespace CantinaPadel.Entidades
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public DateTime Fecha { get; set; }

        // Valores del ENUM de la tabla venta
        public string TipoVenta { get; set; } = "Cantina";
        public string? MedioPago { get; set; }               // Efectivo / Tarjeta / Transferencia (null si es fiada)
        public string TipoComprobante { get; set; } = "Sin comprobante";
        public string TipoSalida { get; set; } = "NINGUNA";
        public decimal Total { get; set; }
        public string Estado { get; set; } = "Pagada";       // Pagada / Fiada / Anulada

        public int? IdCliente { get; set; }                  // null = consumidor final
        public int? IdCaja { get; set; }                     // pendiente hasta que exista el módulo de caja
        public int? IdReserva { get; set; }
        public int? IdUsuario { get; set; }

        public bool EsFiada => Estado == "Fiada";

        public List<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
    }
}

using System;

namespace CantinaPadel.Entidades
{
    public class DetalleVenta
    {
        public int IdDetalleVenta { get; set; }
        public int IdVenta { get; set; }
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        // Se calcula solo: así nunca puede quedar desfasado de cantidad * precio
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}

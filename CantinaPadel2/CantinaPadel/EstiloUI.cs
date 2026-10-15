using System;
using System.Drawing;
using System.Windows.Forms;

namespace CantinaPadel
{
    // Colores y estilos compartidos por las pantallas de Ventas y Cuenta Corriente,
    // para que mantengan el mismo look que el menú (bordó + gris pizarra).
    public static class EstiloUI
    {
        public static readonly Color Fondo = Color.FromArgb(240, 242, 245);
        public static readonly Color Bordo = Color.FromArgb(128, 0, 0);
        public static readonly Color BordoOscuro = Color.FromArgb(64, 0, 0);
        public static readonly Color Pizarra = Color.DarkSlateGray;
        public static readonly Color Verde = Color.FromArgb(34, 120, 70);

        public static readonly Font Normal = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font Negrita = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);
        public static readonly Font Titulo = new Font("Segoe UI", 20F, FontStyle.Bold, GraphicsUnit.Point);

        public static void Grilla(DataGridView g)
        {
            g.Dock = DockStyle.Fill;
            g.ReadOnly = true;
            g.AllowUserToAddRows = false;
            g.AllowUserToDeleteRows = false;
            g.AllowUserToResizeRows = false;
            g.MultiSelect = false;
            g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            g.RowHeadersVisible = false;
            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            g.BackgroundColor = Color.White;
            g.BorderStyle = BorderStyle.None;
            g.EnableHeadersVisualStyles = false;
            g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            g.ColumnHeadersHeight = 32;
            g.ColumnHeadersDefaultCellStyle.BackColor = Pizarra;
            g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            g.ColumnHeadersDefaultCellStyle.Font = Negrita;
            g.DefaultCellStyle.Font = Normal;
            g.DefaultCellStyle.SelectionBackColor = Bordo;
            g.DefaultCellStyle.SelectionForeColor = Color.White;
            g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(247, 248, 250);
            g.RowTemplate.Height = 28;
        }

        public static void Boton(Button b, Color fondo)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = fondo;
            b.ForeColor = Color.White;
            b.Font = Negrita;
            b.Height = 36;
            b.Cursor = Cursors.Hand;
            b.UseVisualStyleBackColor = false;
        }

        public static GroupBox Grupo(string titulo)
        {
            return new GroupBox
            {
                Text = titulo,
                Dock = DockStyle.Fill,
                Font = Negrita,
                ForeColor = Pizarra,
                Padding = new Padding(8)
            };
        }

        public static Label Etiqueta(string texto)
        {
            return new Label
            {
                Text = texto,
                AutoSize = true,
                Font = Normal,
                ForeColor = Color.Black,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(0, 6, 6, 0)
            };
        }

        // Escapa un texto para usarlo dentro de DataView.RowFilter con LIKE
        public static string EscaparLike(string texto)
        {
            var sb = new System.Text.StringBuilder();
            foreach (char c in texto)
            {
                if (c == '\'') sb.Append("''");
                else if (c == '[' || c == ']' || c == '%' || c == '*') sb.Append('[').Append(c).Append(']');
                else sb.Append(c);
            }
            return sb.ToString();
        }
    }
}

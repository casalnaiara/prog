using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CantinaPadel.Entidades;
using CantinaPadel.Negocio;

namespace CantinaPadel
{
    // Punto de venta: buscar productos, armar el carrito y cobrar (o fiar a un cliente).
    // La interfaz se arma por código (sin Designer) para que sea fácil de leer y modificar.
    public class FormVentas : Form
    {
        private readonly VentaNegocio ventaNegocio = new VentaNegocio();
        private readonly CuentaCorrienteNegocio cuentaNegocio = new CuentaCorrienteNegocio();

        // El carrito vive en memoria hasta que se confirma la venta
        private readonly BindingList<DetalleVenta> carrito = new BindingList<DetalleVenta>();

        private bool cargando = false;

        // Controles
        private TextBox txtBuscar = null!;
        private DataGridView dgvProductos = null!;
        private NumericUpDown nudCantidad = null!;
        private Button btnAgregar = null!;

        private DataGridView dgvCarrito = null!;
        private Button btnQuitar = null!;
        private Button btnVaciar = null!;

        private ComboBox cmbCliente = null!;
        private Label lblDeuda = null!;
        private CheckBox chkFiar = null!;
        private ComboBox cmbMedioPago = null!;
        private Label lblTotal = null!;
        private Button btnConfirmar = null!;

        public FormVentas()
        {
            Text = "Ventas";
            BackColor = EstiloUI.Fondo;
            Font = EstiloUI.Normal;
            ConstruirInterfaz();
            Load += FormVentas_Load;
        }

        // ---------------------------------------------------------------
        //  CONSTRUCCIÓN DE LA PANTALLA
        // ---------------------------------------------------------------
        private void ConstruirInterfaz()
        {
            TableLayoutPanel raiz = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Padding = new Padding(10)
            };
            raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 56F));
            raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 44F));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            raiz.Controls.Add(ConstruirPanelProductos(), 0, 0);
            raiz.Controls.Add(ConstruirPanelCarrito(), 1, 0);
            Controls.Add(raiz);
        }

        private Control ConstruirPanelProductos()
        {
            GroupBox grupo = EstiloUI.Grupo("Productos");

            TableLayoutPanel t = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Font = EstiloUI.Normal };
            t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            t.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            t.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // Buscador (también sirve para el lector de código de barras: escanea y manda Enter)
            txtBuscar = new TextBox { Dock = DockStyle.Top, Font = new Font("Segoe UI", 12F), PlaceholderText = "Buscar por nombre o código (Enter agrega si hay un solo resultado)" };
            txtBuscar.TextChanged += txtBuscar_TextChanged;
            txtBuscar.KeyDown += txtBuscar_KeyDown;

            dgvProductos = new DataGridView();
            EstiloUI.Grilla(dgvProductos);
            dgvProductos.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) AgregarAlCarrito(); };
            dgvProductos.DataBindingComplete += (s, e) => ConfigurarColumnasProductos();

            // Fila inferior: cantidad + botón agregar
            FlowLayoutPanel fila = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.LeftToRight, Padding = new Padding(0, 6, 0, 0) };
            nudCantidad = new NumericUpDown { Minimum = 1, Maximum = 9999, Value = 1, Width = 70, Font = new Font("Segoe UI", 12F) };
            btnAgregar = new Button { Text = "Agregar al carrito", Width = 180 };
            EstiloUI.Boton(btnAgregar, EstiloUI.Pizarra);
            btnAgregar.Click += (s, e) => AgregarAlCarrito();
            fila.Controls.Add(EstiloUI.Etiqueta("Cantidad:"));
            fila.Controls.Add(nudCantidad);
            fila.Controls.Add(btnAgregar);

            t.Controls.Add(txtBuscar, 0, 0);
            t.Controls.Add(dgvProductos, 0, 1);
            t.Controls.Add(fila, 0, 2);
            grupo.Controls.Add(t);
            return grupo;
        }

        private Control ConstruirPanelCarrito()
        {
            GroupBox grupo = EstiloUI.Grupo("Carrito");

            TableLayoutPanel t = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Font = EstiloUI.Normal };
            t.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            t.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // Grilla del carrito (columnas armadas a mano para controlar títulos y formato)
            dgvCarrito = new DataGridView { AutoGenerateColumns = false };
            EstiloUI.Grilla(dgvCarrito);
            dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NombreProducto", HeaderText = "Producto", FillWeight = 45 });
            dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Cantidad", HeaderText = "Cant.", FillWeight = 14 });
            dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PrecioUnitario", HeaderText = "Precio", FillWeight = 20, DefaultCellStyle = new DataGridViewCellStyle { Format = "C2", Alignment = DataGridViewContentAlignment.MiddleRight } });
            dgvCarrito.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Subtotal", HeaderText = "Subtotal", FillWeight = 21, DefaultCellStyle = new DataGridViewCellStyle { Format = "C2", Alignment = DataGridViewContentAlignment.MiddleRight } });
            dgvCarrito.DataSource = carrito;

            // Botones del carrito
            FlowLayoutPanel botones = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, Padding = new Padding(0, 6, 0, 6) };
            btnQuitar = new Button { Text = "Quitar producto", Width = 150 };
            btnVaciar = new Button { Text = "Vaciar carrito", Width = 150 };
            EstiloUI.Boton(btnQuitar, EstiloUI.Pizarra);
            EstiloUI.Boton(btnVaciar, Color.Gray);
            btnQuitar.Click += btnQuitar_Click;
            btnVaciar.Click += btnVaciar_Click;
            botones.Controls.Add(btnQuitar);
            botones.Controls.Add(btnVaciar);

            // Zona de cobro
            TableLayoutPanel cobro = new TableLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, ColumnCount = 2, RowCount = 6 };
            cobro.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            cobro.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            cmbCliente = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbCliente.SelectedIndexChanged += cmbCliente_SelectedIndexChanged;

            lblDeuda = new Label { Dock = DockStyle.Fill, AutoSize = true, ForeColor = EstiloUI.Bordo, Font = EstiloUI.Negrita, Margin = new Padding(0, 4, 0, 4) };

            chkFiar = new CheckBox { Text = "Fiar (cargar a la cuenta corriente del cliente)", AutoSize = true, Enabled = false };
            chkFiar.CheckedChanged += chkFiar_CheckedChanged;

            cmbMedioPago = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbMedioPago.Items.AddRange(VentaNegocio.MediosPago);
            cmbMedioPago.SelectedIndex = 0;

            lblTotal = new Label { Text = "TOTAL: " + 0m.ToString("C2"), Dock = DockStyle.Fill, AutoSize = true, Font = EstiloUI.Titulo, ForeColor = EstiloUI.Bordo, Margin = new Padding(0, 8, 0, 8) };

            btnConfirmar = new Button { Text = "Confirmar venta", Dock = DockStyle.Fill, Height = 46 };
            EstiloUI.Boton(btnConfirmar, EstiloUI.Verde);
            btnConfirmar.Height = 46;
            btnConfirmar.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnConfirmar.Click += btnConfirmar_Click;

            cobro.Controls.Add(EstiloUI.Etiqueta("Cliente:"), 0, 0);
            cobro.Controls.Add(cmbCliente, 1, 0);
            cobro.Controls.Add(lblDeuda, 1, 1);
            cobro.Controls.Add(chkFiar, 1, 2);
            cobro.Controls.Add(EstiloUI.Etiqueta("Medio de pago:"), 0, 3);
            cobro.Controls.Add(cmbMedioPago, 1, 3);
            cobro.Controls.Add(lblTotal, 0, 4);
            cobro.SetColumnSpan(lblTotal, 2);
            cobro.Controls.Add(btnConfirmar, 0, 5);
            cobro.SetColumnSpan(btnConfirmar, 2);

            t.Controls.Add(dgvCarrito, 0, 0);
            t.Controls.Add(botones, 0, 1);
            t.Controls.Add(cobro, 0, 2);
            grupo.Controls.Add(t);
            return grupo;
        }

        // ---------------------------------------------------------------
        //  CARGA DE DATOS
        // ---------------------------------------------------------------
        private void FormVentas_Load(object? sender, EventArgs e)
        {
            try
            {
                CargarClientes();
                CargarProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }
        }

        private void CargarProductos()
        {
            dgvProductos.DataSource = ventaNegocio.ObtenerProductosParaVenta(txtBuscar.Text);
        }

        private void ConfigurarColumnasProductos()
        {
            if (dgvProductos.Columns.Count == 0) return;
            dgvProductos.Columns["id_producto"]!.Visible = false;
            dgvProductos.Columns["codigo"]!.HeaderText = "Código";
            dgvProductos.Columns["codigo"]!.FillWeight = 22;
            dgvProductos.Columns["nombre"]!.HeaderText = "Producto";
            dgvProductos.Columns["nombre"]!.FillWeight = 48;
            dgvProductos.Columns["stock"]!.HeaderText = "Stock";
            dgvProductos.Columns["stock"]!.FillWeight = 12;
            dgvProductos.Columns["precio_venta"]!.HeaderText = "Precio";
            dgvProductos.Columns["precio_venta"]!.FillWeight = 18;
            dgvProductos.Columns["precio_venta"]!.DefaultCellStyle.Format = "C2";
            dgvProductos.Columns["precio_venta"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            // Los productos sin stock se ven en gris para que no se intenten vender
            foreach (DataGridViewRow fila in dgvProductos.Rows)
            {
                if (Convert.ToInt32(fila.Cells["stock"].Value) <= 0)
                    fila.DefaultCellStyle.ForeColor = Color.Gray;
            }
        }

        private void CargarClientes()
        {
            cargando = true;
            try
            {
                DataTable clientes = ventaNegocio.ObtenerClientes();
                DataRow consumidorFinal = clientes.NewRow();
                consumidorFinal["id_cliente"] = 0;
                consumidorFinal["nombre"] = "Consumidor final";
                clientes.Rows.InsertAt(consumidorFinal, 0);

                cmbCliente.DisplayMember = "nombre";
                cmbCliente.ValueMember = "id_cliente";
                cmbCliente.DataSource = clientes;
                cmbCliente.SelectedIndex = 0;
            }
            finally
            {
                cargando = false;
            }
            ActualizarEstadoCliente();
        }

        // ---------------------------------------------------------------
        //  BÚSQUEDA
        // ---------------------------------------------------------------
        private void txtBuscar_TextChanged(object? sender, EventArgs e)
        {
            try { CargarProductos(); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void txtBuscar_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;

            // Con lector de códigos: queda un solo resultado y se agrega directo
            if (dgvProductos.Rows.Count == 1)
            {
                dgvProductos.CurrentCell = dgvProductos.Rows[0].Cells["nombre"];
                AgregarAlCarrito();
                txtBuscar.Clear();
                txtBuscar.Focus();
            }
        }

        // ---------------------------------------------------------------
        //  CARRITO
        // ---------------------------------------------------------------
        private void AgregarAlCarrito()
        {
            if (dgvProductos.CurrentRow == null || dgvProductos.CurrentRow.DataBoundItem is not DataRowView fila)
            {
                MessageBox.Show("Seleccione un producto de la lista.");
                return;
            }

            int idProducto = Convert.ToInt32(fila["id_producto"]);
            string nombre = fila["nombre"].ToString() ?? "";
            int stock = Convert.ToInt32(fila["stock"]);
            decimal precio = Convert.ToDecimal(fila["precio_venta"]);
            int cantidad = Convert.ToInt32(nudCantidad.Value);

            DetalleVenta? existente = carrito.FirstOrDefault(d => d.IdProducto == idProducto);
            int yaEnCarrito = existente?.Cantidad ?? 0;

            // Control de stock: lo que ya está en el carrito + lo nuevo no puede pasar el stock
            if (yaEnCarrito + cantidad > stock)
            {
                MessageBox.Show("Stock insuficiente de \"" + nombre + "\".\nDisponible: " + stock + " | En el carrito: " + yaEnCarrito,
                                "Stock", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (existente != null)
            {
                existente.Cantidad += cantidad;
            }
            else
            {
                carrito.Add(new DetalleVenta
                {
                    IdProducto = idProducto,
                    NombreProducto = nombre,
                    Cantidad = cantidad,
                    PrecioUnitario = precio
                });
            }

            carrito.ResetBindings();
            nudCantidad.Value = 1;
            ActualizarTotal();
        }

        private void btnQuitar_Click(object? sender, EventArgs e)
        {
            if (dgvCarrito.CurrentRow?.DataBoundItem is DetalleVenta detalle)
            {
                carrito.Remove(detalle);
                ActualizarTotal();
            }
            else
            {
                MessageBox.Show("Seleccione el producto del carrito que desea quitar.");
            }
        }

        private void btnVaciar_Click(object? sender, EventArgs e)
        {
            if (carrito.Count == 0) return;

            if (MessageBox.Show("¿Vaciar todo el carrito?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                LimpiarCarrito();
            }
        }

        private void LimpiarCarrito()
        {
            carrito.Clear();
            ActualizarTotal();
        }

        private decimal CalcularTotal()
        {
            return carrito.Sum(d => d.Subtotal);
        }

        private void ActualizarTotal()
        {
            lblTotal.Text = "TOTAL: " + CalcularTotal().ToString("C2");
        }

        // ---------------------------------------------------------------
        //  CLIENTE / FIADO
        // ---------------------------------------------------------------
        private int IdClienteSeleccionado()
        {
            if (cmbCliente.SelectedValue == null || cmbCliente.SelectedValue is DataRowView) return 0;
            return Convert.ToInt32(cmbCliente.SelectedValue);
        }

        private void cmbCliente_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (cargando) return;
            ActualizarEstadoCliente();
        }

        // Muestra la deuda del cliente elegido y habilita "Fiar" solo si hay un cliente
        private void ActualizarEstadoCliente()
        {
            int idCliente = IdClienteSeleccionado();

            chkFiar.Enabled = idCliente > 0;
            if (idCliente <= 0)
            {
                chkFiar.Checked = false;
                lblDeuda.Text = "";
                return;
            }

            try
            {
                decimal deuda = cuentaNegocio.ObtenerDeudaCliente(idCliente);
                lblDeuda.Text = deuda > 0 ? "Deuda actual: " + deuda.ToString("C2") : "Sin deuda";
                lblDeuda.ForeColor = deuda > 0 ? EstiloUI.Bordo : EstiloUI.Verde;
            }
            catch (Exception ex)
            {
                lblDeuda.Text = "";
                MessageBox.Show(ex.Message);
            }
        }

        private void chkFiar_CheckedChanged(object? sender, EventArgs e)
        {
            // Si se fía no hay medio de pago todavía: se elige cuando el cliente pague
            cmbMedioPago.Enabled = !chkFiar.Checked;
            btnConfirmar.Text = chkFiar.Checked ? "Confirmar venta FIADA" : "Confirmar venta";
            btnConfirmar.BackColor = chkFiar.Checked ? EstiloUI.Bordo : EstiloUI.Verde;
        }

        // ---------------------------------------------------------------
        //  CONFIRMAR VENTA
        // ---------------------------------------------------------------
        private void btnConfirmar_Click(object? sender, EventArgs e)
        {
            if (carrito.Count == 0)
            {
                MessageBox.Show("El carrito está vacío. Agregue productos antes de confirmar.");
                return;
            }

            int idCliente = IdClienteSeleccionado();
            bool fiar = chkFiar.Checked;

            if (fiar && idCliente <= 0)
            {
                MessageBox.Show("Para fiar una venta debe seleccionar un cliente.");
                return;
            }

            decimal total = CalcularTotal();
            string resumen = fiar
                ? "Se cargarán " + total.ToString("C2") + " a la cuenta corriente de " + cmbCliente.Text + "."
                : "Total a cobrar: " + total.ToString("C2") + "\nMedio de pago: " + cmbMedioPago.Text;

            if (MessageBox.Show(resumen + "\n\n¿Confirmar la venta?", "Confirmar venta",
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            try
            {
                Venta venta = new Venta
                {
                    Total = total,
                    Estado = fiar ? "Fiada" : "Pagada",
                    MedioPago = fiar ? null : cmbMedioPago.Text,
                    IdCliente = idCliente > 0 ? idCliente : (int?)null,
                    Detalles = carrito.ToList()
                };

                int idVenta = ventaNegocio.RegistrarVenta(venta);

                MessageBox.Show(
                    fiar ? "Venta #" + idVenta + " registrada y cargada a la cuenta corriente."
                         : "Venta #" + idVenta + " registrada con éxito.",
                    "Venta", MessageBoxButtons.OK, MessageBoxIcon.Information);

                LimpiarCarrito();
                chkFiar.Checked = false;
                cmbCliente.SelectedIndex = 0;
                CargarProductos(); // el stock cambió
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo registrar la venta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                try { CargarProductos(); } catch { }
            }
        }
    }
}

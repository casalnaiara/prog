using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using CantinaPadel.Negocio;

namespace CantinaPadel
{
    // Cuenta corriente: lista de clientes con su deuda, detalle de lo que debe cada uno
    // y registro de pagos (por línea o "a cuenta").
    public class FormCuentaCorriente : Form
    {
        private readonly CuentaCorrienteNegocio cuentaNegocio = new CuentaCorrienteNegocio();

        private DataView? vistaCuentas;
        private bool cargando = false;

        // Controles
        private TextBox txtBuscar = null!;
        private CheckBox chkSoloConDeuda = null!;
        private DataGridView dgvCuentas = null!;

        private Label lblCliente = null!;
        private Label lblDeudaTotal = null!;
        private CheckBox chkVerPagadas = null!;
        private DataGridView dgvDeudas = null!;

        private NumericUpDown nudMonto = null!;
        private ComboBox cmbMedioPago = null!;
        private Button btnPagarLinea = null!;
        private Button btnPagarCuenta = null!;

        public FormCuentaCorriente()
        {
            Text = "Cuenta corriente";
            BackColor = EstiloUI.Fondo;
            Font = EstiloUI.Normal;
            ConstruirInterfaz();
            Load += FormCuentaCorriente_Load;
        }

        // ---------------------------------------------------------------
        //  CONSTRUCCIÓN DE LA PANTALLA
        // ---------------------------------------------------------------
        private void ConstruirInterfaz()
        {
            TableLayoutPanel raiz = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 1, Padding = new Padding(10) };
            raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36F));
            raiz.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 64F));
            raiz.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            raiz.Controls.Add(ConstruirPanelClientes(), 0, 0);
            raiz.Controls.Add(ConstruirPanelDetalle(), 1, 0);
            Controls.Add(raiz);
        }

        private Control ConstruirPanelClientes()
        {
            GroupBox grupo = EstiloUI.Grupo("Clientes");

            TableLayoutPanel t = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3, Font = EstiloUI.Normal };
            t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            t.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            txtBuscar = new TextBox { Dock = DockStyle.Top, Font = new Font("Segoe UI", 11F), PlaceholderText = "Buscar cliente..." };
            txtBuscar.TextChanged += (s, e) => AplicarFiltroClientes();

            chkSoloConDeuda = new CheckBox { Text = "Mostrar solo clientes con deuda", AutoSize = true, Checked = true, Margin = new Padding(0, 6, 0, 6) };
            chkSoloConDeuda.CheckedChanged += (s, e) => AplicarFiltroClientes();

            dgvCuentas = new DataGridView();
            EstiloUI.Grilla(dgvCuentas);
            dgvCuentas.SelectionChanged += dgvCuentas_SelectionChanged;
            dgvCuentas.DataBindingComplete += (s, e) => ConfigurarColumnasCuentas();

            t.Controls.Add(txtBuscar, 0, 0);
            t.Controls.Add(chkSoloConDeuda, 0, 1);
            t.Controls.Add(dgvCuentas, 0, 2);
            grupo.Controls.Add(t);
            return grupo;
        }

        private Control ConstruirPanelDetalle()
        {
            Panel panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(10, 0, 0, 0) };

            TableLayoutPanel t = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4 };
            t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            t.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            t.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            t.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // Encabezado: nombre del cliente y deuda total
            lblCliente = new Label { Text = "Seleccione un cliente", AutoSize = true, Font = new Font("Segoe UI", 14F, FontStyle.Bold), ForeColor = EstiloUI.Pizarra, Margin = new Padding(0, 4, 0, 0) };
            lblDeudaTotal = new Label { Text = "", AutoSize = true, Font = EstiloUI.Titulo, ForeColor = EstiloUI.Bordo };

            chkVerPagadas = new CheckBox { Text = "Incluir deudas ya pagadas", AutoSize = true, Margin = new Padding(0, 0, 0, 6) };
            chkVerPagadas.CheckedChanged += (s, e) => CargarDeudas();

            // Detalle de deudas
            dgvDeudas = new DataGridView();
            EstiloUI.Grilla(dgvDeudas);
            dgvDeudas.SelectionChanged += dgvDeudas_SelectionChanged;
            dgvDeudas.DataBindingComplete += (s, e) => ConfigurarColumnasDeudas();

            t.Controls.Add(ConstruirEncabezado(), 0, 0);
            t.Controls.Add(chkVerPagadas, 0, 1);
            t.Controls.Add(dgvDeudas, 0, 2);
            t.Controls.Add(ConstruirPanelPago(), 0, 3);

            panel.Controls.Add(t);
            return panel;
        }

        private Control ConstruirEncabezado()
        {
            FlowLayoutPanel f = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, FlowDirection = FlowDirection.TopDown, WrapContents = false };
            f.Controls.Add(lblCliente);
            f.Controls.Add(lblDeudaTotal);
            return f;
        }

        private Control ConstruirPanelPago()
        {
            GroupBox grupo = EstiloUI.Grupo("Registrar pago");
            grupo.Dock = DockStyle.Fill;
            grupo.AutoSize = true;

            TableLayoutPanel t = new TableLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, ColumnCount = 4, RowCount = 2, Font = EstiloUI.Normal };
            t.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            t.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            t.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            nudMonto = new NumericUpDown { DecimalPlaces = 2, Maximum = 99999999, Minimum = 0, ThousandsSeparator = true, Dock = DockStyle.Fill, Font = new Font("Segoe UI", 12F) };

            cmbMedioPago = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbMedioPago.Items.AddRange(VentaNegocio.MediosPago);
            cmbMedioPago.SelectedIndex = 0;

            btnPagarLinea = new Button { Text = "Pagar línea seleccionada", Dock = DockStyle.Fill };
            btnPagarCuenta = new Button { Text = "Pagar a cuenta (las más viejas primero)", Dock = DockStyle.Fill };
            EstiloUI.Boton(btnPagarLinea, EstiloUI.Verde);
            EstiloUI.Boton(btnPagarCuenta, EstiloUI.Pizarra);
            btnPagarLinea.Click += btnPagarLinea_Click;
            btnPagarCuenta.Click += btnPagarCuenta_Click;

            // Monto + atajo para completar con la deuda total del cliente
            Button btnUsarTotal = new Button { Text = "Usar deuda total", AutoSize = true, Dock = DockStyle.Right };
            EstiloUI.Boton(btnUsarTotal, Color.Gray);
            btnUsarTotal.Height = 28;
            btnUsarTotal.Font = EstiloUI.Normal;
            btnUsarTotal.Click += (s, e) => UsarDeudaTotal();

            Panel panelMonto = new Panel { Dock = DockStyle.Fill, Height = 32 };
            panelMonto.Controls.Add(nudMonto);
            panelMonto.Controls.Add(btnUsarTotal);
            nudMonto.Dock = DockStyle.Fill;

            t.Controls.Add(EstiloUI.Etiqueta("Monto:"), 0, 0);
            t.Controls.Add(panelMonto, 1, 0);
            t.Controls.Add(EstiloUI.Etiqueta("Medio de pago:"), 2, 0);
            t.Controls.Add(cmbMedioPago, 3, 0);
            t.Controls.Add(btnPagarLinea, 0, 1);
            t.SetColumnSpan(btnPagarLinea, 2);
            t.Controls.Add(btnPagarCuenta, 2, 1);
            t.SetColumnSpan(btnPagarCuenta, 2);

            grupo.Controls.Add(t);
            return grupo;
        }

        // ---------------------------------------------------------------
        //  CARGA DE DATOS
        // ---------------------------------------------------------------
        private void FormCuentaCorriente_Load(object? sender, EventArgs e)
        {
            CargarCuentas();
        }

        private int IdCuentaSeleccionada()
        {
            if (dgvCuentas.CurrentRow?.DataBoundItem is DataRowView fila)
                return Convert.ToInt32(fila["id_cuenta"]);
            return 0;
        }

        private int IdDeudaSeleccionada()
        {
            if (dgvDeudas.CurrentRow?.DataBoundItem is DataRowView fila)
                return Convert.ToInt32(fila["id_detalle_deuda"]);
            return 0;
        }

        private void CargarCuentas()
        {
            int idSeleccionada = IdCuentaSeleccionada();
            cargando = true;
            try
            {
                vistaCuentas = cuentaNegocio.ObtenerCuentas().DefaultView;
                dgvCuentas.DataSource = vistaCuentas;
                AplicarFiltroClientes();
                SeleccionarCuenta(idSeleccionada);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las cuentas: " + ex.Message);
            }
            finally
            {
                cargando = false;
            }
            CargarDeudas();
        }

        private void SeleccionarCuenta(int idCuenta)
        {
            if (idCuenta <= 0) return;
            foreach (DataGridViewRow fila in dgvCuentas.Rows)
            {
                if (fila.DataBoundItem is DataRowView drv && Convert.ToInt32(drv["id_cuenta"]) == idCuenta)
                {
                    dgvCuentas.CurrentCell = fila.Cells["cliente"];
                    return;
                }
            }
        }

        private void AplicarFiltroClientes()
        {
            if (vistaCuentas == null) return;

            string filtro = "";
            string texto = txtBuscar.Text.Trim();
            if (texto.Length > 0)
                filtro = "cliente LIKE '%" + EstiloUI.EscaparLike(texto) + "%'";

            if (chkSoloConDeuda.Checked)
                filtro += (filtro.Length > 0 ? " AND " : "") + "deuda_total > 0";

            vistaCuentas.RowFilter = filtro;
        }

        private void ConfigurarColumnasCuentas()
        {
            if (dgvCuentas.Columns.Count == 0) return;
            dgvCuentas.Columns["id_cuenta"]!.Visible = false;
            dgvCuentas.Columns["id_cliente"]!.Visible = false;
            dgvCuentas.Columns["cliente"]!.HeaderText = "Cliente";
            dgvCuentas.Columns["cliente"]!.FillWeight = 60;
            dgvCuentas.Columns["deuda_total"]!.HeaderText = "Debe";
            dgvCuentas.Columns["deuda_total"]!.FillWeight = 40;
            dgvCuentas.Columns["deuda_total"]!.DefaultCellStyle.Format = "C2";
            dgvCuentas.Columns["deuda_total"]!.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        private void dgvCuentas_SelectionChanged(object? sender, EventArgs e)
        {
            if (cargando) return;
            CargarDeudas();
        }

        private void CargarDeudas()
        {
            int idCuenta = IdCuentaSeleccionada();

            if (idCuenta <= 0)
            {
                lblCliente.Text = "Seleccione un cliente";
                lblDeudaTotal.Text = "";
                dgvDeudas.DataSource = null;
                nudMonto.Value = 0;
                return;
            }

            try
            {
                DataRowView cuenta = (DataRowView)dgvCuentas.CurrentRow!.DataBoundItem;
                lblCliente.Text = cuenta["cliente"].ToString();
                decimal deuda = Convert.ToDecimal(cuenta["deuda_total"]);
                lblDeudaTotal.Text = deuda > 0 ? "Debe: " + deuda.ToString("C2") : "Sin deuda pendiente";
                lblDeudaTotal.ForeColor = deuda > 0 ? EstiloUI.Bordo : EstiloUI.Verde;

                nudMonto.Value = 0;
                dgvDeudas.DataSource = cuentaNegocio.ObtenerDeudas(idCuenta, chkVerPagadas.Checked);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el detalle: " + ex.Message);
            }
        }

        private void ConfigurarColumnasDeudas()
        {
            if (dgvDeudas.Columns.Count == 0) return;
            dgvDeudas.Columns["id_detalle_deuda"]!.Visible = false;
            dgvDeudas.Columns["descripcion"]!.HeaderText = "Detalle";
            dgvDeudas.Columns["descripcion"]!.FillWeight = 38;
            dgvDeudas.Columns["cantidad"]!.HeaderText = "Cant.";
            dgvDeudas.Columns["cantidad"]!.FillWeight = 10;
            dgvDeudas.Columns["precio_actual"]!.HeaderText = "Precio";
            dgvDeudas.Columns["precio_actual"]!.FillWeight = 18;
            dgvDeudas.Columns["precio_actual"]!.DefaultCellStyle.Format = "C2";
            dgvDeudas.Columns["saldo_pendiente"]!.HeaderText = "Saldo";
            dgvDeudas.Columns["saldo_pendiente"]!.FillWeight = 18;
            dgvDeudas.Columns["saldo_pendiente"]!.DefaultCellStyle.Format = "C2";
            dgvDeudas.Columns["estado"]!.HeaderText = "Estado";
            dgvDeudas.Columns["estado"]!.FillWeight = 16;

            foreach (DataGridViewRow fila in dgvDeudas.Rows)
            {
                if (fila.Cells["estado"].Value?.ToString() == "Pagada")
                    fila.DefaultCellStyle.ForeColor = Color.Gray;
            }
        }

        private void UsarDeudaTotal()
        {
            if (dgvCuentas.CurrentRow?.DataBoundItem is DataRowView cuenta)
                nudMonto.Value = Math.Min(Convert.ToDecimal(cuenta["deuda_total"]), nudMonto.Maximum);
        }

        // Al elegir una línea, el monto se completa con su saldo (se puede cambiar para pagar parcial)
        private void dgvDeudas_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvDeudas.CurrentRow?.DataBoundItem is DataRowView fila)
            {
                decimal saldo = Convert.ToDecimal(fila["saldo_pendiente"]);
                if (saldo > 0)
                    nudMonto.Value = Math.Min(saldo, nudMonto.Maximum);
            }
        }

        // ---------------------------------------------------------------
        //  PAGOS
        // ---------------------------------------------------------------
        private void btnPagarLinea_Click(object? sender, EventArgs e)
        {
            int idDeuda = IdDeudaSeleccionada();
            if (idDeuda <= 0)
            {
                MessageBox.Show("Seleccione la línea de deuda que desea pagar.");
                return;
            }

            if (!Confirmar("¿Registrar el pago de " + nudMonto.Value.ToString("C2") + " (" + cmbMedioPago.Text + ")?")) return;

            try
            {
                cuentaNegocio.PagarLinea(idDeuda, nudMonto.Value, cmbMedioPago.Text);
                MessageBox.Show("Pago registrado correctamente.", "Pago", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarCuentas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo registrar el pago", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPagarCuenta_Click(object? sender, EventArgs e)
        {
            int idCuenta = IdCuentaSeleccionada();
            if (idCuenta <= 0)
            {
                MessageBox.Show("Seleccione un cliente.");
                return;
            }

            if (!Confirmar("¿Registrar un pago a cuenta de " + nudMonto.Value.ToString("C2") + " (" + cmbMedioPago.Text + ")?\nSe descuenta de las deudas más viejas primero.")) return;

            try
            {
                cuentaNegocio.PagarCuenta(idCuenta, nudMonto.Value, cmbMedioPago.Text);
                MessageBox.Show("Pago registrado correctamente.", "Pago", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CargarCuentas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "No se pudo registrar el pago", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool Confirmar(string mensaje)
        {
            return MessageBox.Show(mensaje, "Confirmar pago", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}

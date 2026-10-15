namespace CantinaPadel
{
    partial class MenuPrincipal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MenuPrincipal));
            splitContainer1 = new SplitContainer();
            panelBotones = new Panel();
            btnUsuarios = new Button();
            btnReportes = new Button();
            btnCanchas = new Button();
            btnPersonas = new Button();
            btnCompras = new Button();
            btnProductos = new Button();
            btnCaja = new Button();
            btnReservas = new Button();
            btnVentas = new Button();
            btnCuentaCorriente = new Button();
            btnSalir = new Button();
            panelHeader = new Panel();
            pictureBox1 = new PictureBox();
            lblNombre = new Label();
            panelContenedor = new Panel();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panelBotones.SuspendLayout();
            panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.BackColor = SystemColors.ButtonHighlight;
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.AccessibleRole = AccessibleRole.MenuBar;
            splitContainer1.Panel1.BackColor = Color.DarkSlateGray;
            splitContainer1.Panel1.Controls.Add(panelBotones);
            splitContainer1.Panel1.Controls.Add(panelHeader);
            splitContainer1.Panel1.ForeColor = SystemColors.ActiveCaption;
            splitContainer1.Panel1.ImeMode = ImeMode.NoControl;
            splitContainer1.Panel1.RightToLeft = RightToLeft.No;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.BackColor = Color.FromArgb(240, 242, 245);
            splitContainer1.Panel2.Controls.Add(panelContenedor);
            splitContainer1.Size = new Size(984, 561);
            splitContainer1.SplitterDistance = 187;
            splitContainer1.TabIndex = 1;
            // 
            // panelBotones
            // 
            panelBotones.AutoScroll = true;
            panelBotones.BackColor = Color.FromArgb(64, 0, 0);
            panelBotones.Controls.Add(btnUsuarios);
            panelBotones.Controls.Add(btnReportes);
            panelBotones.Controls.Add(btnCanchas);
            panelBotones.Controls.Add(btnPersonas);
            panelBotones.Controls.Add(btnCompras);
            panelBotones.Controls.Add(btnProductos);
            panelBotones.Controls.Add(btnCaja);
            panelBotones.Controls.Add(btnReservas);
            panelBotones.Controls.Add(btnCuentaCorriente);
            panelBotones.Controls.Add(btnVentas);
            panelBotones.Controls.Add(btnSalir);
            panelBotones.Dock = DockStyle.Fill;
            panelBotones.Location = new Point(0, 100);
            panelBotones.Name = "panelBotones";
            panelBotones.Size = new Size(187, 461);
            panelBotones.TabIndex = 1;
            // 
            // btnUsuarios
            // 
            btnUsuarios.Dock = DockStyle.Top;
            btnUsuarios.FlatAppearance.BorderSize = 0;
            btnUsuarios.FlatAppearance.MouseDownBackColor = Color.Maroon;
            btnUsuarios.FlatAppearance.MouseOverBackColor = Color.Maroon;
            btnUsuarios.FlatStyle = FlatStyle.Flat;
            btnUsuarios.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnUsuarios.ForeColor = Color.White;
            btnUsuarios.Image = Properties.Resources.manage_accounts_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            btnUsuarios.ImageAlign = ContentAlignment.MiddleLeft;
            btnUsuarios.Location = new Point(0, 320);
            btnUsuarios.Name = "btnUsuarios";
            btnUsuarios.Padding = new Padding(5, 0, 0, 0);
            btnUsuarios.Size = new Size(187, 40);
            btnUsuarios.TabIndex = 10;
            btnUsuarios.Text = "Usuarios";
            btnUsuarios.TextAlign = ContentAlignment.MiddleLeft;
            btnUsuarios.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnUsuarios.UseVisualStyleBackColor = true;
            // 
            // btnReportes
            // 
            btnReportes.Dock = DockStyle.Top;
            btnReportes.FlatAppearance.BorderSize = 0;
            btnReportes.FlatAppearance.MouseDownBackColor = Color.Maroon;
            btnReportes.FlatAppearance.MouseOverBackColor = Color.Maroon;
            btnReportes.FlatStyle = FlatStyle.Flat;
            btnReportes.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnReportes.ForeColor = Color.White;
            btnReportes.Image = Properties.Resources.bar_chart_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            btnReportes.ImageAlign = ContentAlignment.MiddleLeft;
            btnReportes.Location = new Point(0, 280);
            btnReportes.Name = "btnReportes";
            btnReportes.Padding = new Padding(5, 0, 0, 0);
            btnReportes.Size = new Size(187, 40);
            btnReportes.TabIndex = 6;
            btnReportes.Text = "Reportes";
            btnReportes.TextAlign = ContentAlignment.MiddleLeft;
            btnReportes.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnReportes.UseVisualStyleBackColor = true;
            // 
            // btnCanchas
            // 
            btnCanchas.Dock = DockStyle.Top;
            btnCanchas.FlatAppearance.BorderSize = 0;
            btnCanchas.FlatAppearance.MouseDownBackColor = Color.Maroon;
            btnCanchas.FlatAppearance.MouseOverBackColor = Color.Maroon;
            btnCanchas.FlatStyle = FlatStyle.Flat;
            btnCanchas.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCanchas.ForeColor = Color.White;
            btnCanchas.Image = Properties.Resources.padel_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            btnCanchas.ImageAlign = ContentAlignment.MiddleLeft;
            btnCanchas.Location = new Point(0, 240);
            btnCanchas.Name = "btnCanchas";
            btnCanchas.Padding = new Padding(5, 0, 0, 0);
            btnCanchas.Size = new Size(187, 40);
            btnCanchas.TabIndex = 9;
            btnCanchas.Text = "Canchas";
            btnCanchas.TextAlign = ContentAlignment.MiddleLeft;
            btnCanchas.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCanchas.UseVisualStyleBackColor = true;
            btnCanchas.Click += btnCanchas_Click;
            // 
            // btnPersonas
            // 
            btnPersonas.Dock = DockStyle.Top;
            btnPersonas.FlatAppearance.BorderSize = 0;
            btnPersonas.FlatAppearance.MouseDownBackColor = Color.Maroon;
            btnPersonas.FlatAppearance.MouseOverBackColor = Color.Maroon;
            btnPersonas.FlatStyle = FlatStyle.Flat;
            btnPersonas.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnPersonas.ForeColor = Color.White;
            btnPersonas.Image = Properties.Resources.group_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            btnPersonas.ImageAlign = ContentAlignment.MiddleLeft;
            btnPersonas.Location = new Point(0, 200);
            btnPersonas.Name = "btnPersonas";
            btnPersonas.Padding = new Padding(5, 0, 0, 0);
            btnPersonas.Size = new Size(187, 40);
            btnPersonas.TabIndex = 3;
            btnPersonas.Text = "Personas";
            btnPersonas.TextAlign = ContentAlignment.MiddleLeft;
            btnPersonas.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnPersonas.UseVisualStyleBackColor = true;
            btnPersonas.Click += btnPersonas_Click;
            // 
            // btnCompras
            // 
            btnCompras.Dock = DockStyle.Top;
            btnCompras.FlatAppearance.BorderSize = 0;
            btnCompras.FlatAppearance.MouseDownBackColor = Color.Maroon;
            btnCompras.FlatAppearance.MouseOverBackColor = Color.Maroon;
            btnCompras.FlatStyle = FlatStyle.Flat;
            btnCompras.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCompras.ForeColor = Color.White;
            btnCompras.Image = Properties.Resources.pallet_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            btnCompras.ImageAlign = ContentAlignment.MiddleLeft;
            btnCompras.Location = new Point(0, 160);
            btnCompras.Name = "btnCompras";
            btnCompras.Padding = new Padding(5, 0, 0, 0);
            btnCompras.Size = new Size(187, 40);
            btnCompras.TabIndex = 8;
            btnCompras.Text = "Compras";
            btnCompras.TextAlign = ContentAlignment.MiddleLeft;
            btnCompras.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCompras.UseVisualStyleBackColor = true;
            btnCompras.Click += btnCompras_Click;
            // 
            // btnProductos
            // 
            btnProductos.Dock = DockStyle.Top;
            btnProductos.FlatAppearance.BorderSize = 0;
            btnProductos.FlatAppearance.MouseDownBackColor = Color.Maroon;
            btnProductos.FlatAppearance.MouseOverBackColor = Color.Maroon;
            btnProductos.FlatStyle = FlatStyle.Flat;
            btnProductos.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnProductos.ForeColor = Color.White;
            btnProductos.Image = Properties.Resources.inventory_2_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            btnProductos.ImageAlign = ContentAlignment.MiddleLeft;
            btnProductos.Location = new Point(0, 120);
            btnProductos.Name = "btnProductos";
            btnProductos.Padding = new Padding(5, 0, 0, 0);
            btnProductos.Size = new Size(187, 40);
            btnProductos.TabIndex = 4;
            btnProductos.Text = "Productos";
            btnProductos.TextAlign = ContentAlignment.MiddleLeft;
            btnProductos.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnProductos.UseVisualStyleBackColor = true;
            btnProductos.Click += btnProductos_Click;
            // 
            // btnCaja
            // 
            btnCaja.Dock = DockStyle.Top;
            btnCaja.FlatAppearance.BorderSize = 0;
            btnCaja.FlatAppearance.MouseDownBackColor = Color.Maroon;
            btnCaja.FlatAppearance.MouseOverBackColor = Color.Maroon;
            btnCaja.FlatStyle = FlatStyle.Flat;
            btnCaja.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCaja.ForeColor = Color.White;
            btnCaja.Image = Properties.Resources.point_of_sale_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            btnCaja.ImageAlign = ContentAlignment.MiddleLeft;
            btnCaja.Location = new Point(0, 80);
            btnCaja.Name = "btnCaja";
            btnCaja.Padding = new Padding(5, 0, 0, 0);
            btnCaja.Size = new Size(187, 40);
            btnCaja.TabIndex = 2;
            btnCaja.Text = "Caja";
            btnCaja.TextAlign = ContentAlignment.MiddleLeft;
            btnCaja.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCaja.UseVisualStyleBackColor = true;
            // 
            // btnReservas
            // 
            btnReservas.BackgroundImageLayout = ImageLayout.None;
            btnReservas.Dock = DockStyle.Top;
            btnReservas.FlatAppearance.BorderSize = 0;
            btnReservas.FlatAppearance.MouseDownBackColor = Color.Maroon;
            btnReservas.FlatAppearance.MouseOverBackColor = Color.Maroon;
            btnReservas.FlatStyle = FlatStyle.Flat;
            btnReservas.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnReservas.ForeColor = Color.White;
            btnReservas.Image = Properties.Resources.event_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            btnReservas.ImageAlign = ContentAlignment.MiddleLeft;
            btnReservas.Location = new Point(0, 40);
            btnReservas.Name = "btnReservas";
            btnReservas.Padding = new Padding(5, 0, 0, 0);
            btnReservas.Size = new Size(187, 40);
            btnReservas.TabIndex = 0;
            btnReservas.Text = "Reservas";
            btnReservas.TextAlign = ContentAlignment.MiddleLeft;
            btnReservas.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnReservas.UseVisualStyleBackColor = true;
            btnReservas.Click += btnReservas_Click;
            // 
            // btnVentas
            // 
            btnVentas.Dock = DockStyle.Top;
            btnVentas.FlatAppearance.BorderSize = 0;
            btnVentas.FlatAppearance.MouseDownBackColor = Color.Maroon;
            btnVentas.FlatAppearance.MouseOverBackColor = Color.Maroon;
            btnVentas.FlatStyle = FlatStyle.Flat;
            btnVentas.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnVentas.ForeColor = Color.White;
            btnVentas.Image = Properties.Resources.sell_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            btnVentas.ImageAlign = ContentAlignment.MiddleLeft;
            btnVentas.Location = new Point(0, 0);
            btnVentas.Name = "btnVentas";
            btnVentas.Padding = new Padding(5, 0, 0, 0);
            btnVentas.Size = new Size(187, 40);
            btnVentas.TabIndex = 1;
            btnVentas.Text = "Ventas";
            btnVentas.TextAlign = ContentAlignment.MiddleLeft;
            btnVentas.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnVentas.UseVisualStyleBackColor = true;
            btnVentas.Click += btnVentas_Click;
            // 
            // btnCuentaCorriente
            // 
            btnCuentaCorriente.Dock = DockStyle.Top;
            btnCuentaCorriente.FlatAppearance.BorderSize = 0;
            btnCuentaCorriente.FlatAppearance.MouseDownBackColor = Color.Maroon;
            btnCuentaCorriente.FlatAppearance.MouseOverBackColor = Color.Maroon;
            btnCuentaCorriente.FlatStyle = FlatStyle.Flat;
            btnCuentaCorriente.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCuentaCorriente.ForeColor = Color.White;
            btnCuentaCorriente.Image = Properties.Resources.receipt_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            btnCuentaCorriente.ImageAlign = ContentAlignment.MiddleLeft;
            btnCuentaCorriente.Location = new Point(0, 40);
            btnCuentaCorriente.Name = "btnCuentaCorriente";
            btnCuentaCorriente.Padding = new Padding(5, 0, 0, 0);
            btnCuentaCorriente.Size = new Size(187, 40);
            btnCuentaCorriente.TabIndex = 11;
            btnCuentaCorriente.Text = "Cuenta Corriente";
            btnCuentaCorriente.TextAlign = ContentAlignment.MiddleLeft;
            btnCuentaCorriente.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCuentaCorriente.UseVisualStyleBackColor = true;
            btnCuentaCorriente.Click += btnCuentaCorriente_Click;
            // 
            // btnSalir
            // 
            btnSalir.Dock = DockStyle.Bottom;
            btnSalir.FlatAppearance.BorderSize = 0;
            btnSalir.FlatAppearance.MouseDownBackColor = Color.Maroon;
            btnSalir.FlatAppearance.MouseOverBackColor = Color.Maroon;
            btnSalir.FlatStyle = FlatStyle.Flat;
            btnSalir.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSalir.ForeColor = Color.White;
            btnSalir.Location = new Point(0, 432);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(187, 29);
            btnSalir.TabIndex = 7;
            btnSalir.Text = "Salir";
            btnSalir.TextAlign = ContentAlignment.BottomCenter;
            btnSalir.UseVisualStyleBackColor = true;
            btnSalir.Click += btnSalir_Click;
            // 
            // panelHeader
            // 
            panelHeader.BackColor = Color.FromArgb(64, 0, 0);
            panelHeader.Controls.Add(pictureBox1);
            panelHeader.Controls.Add(lblNombre);
            panelHeader.Dock = DockStyle.Top;
            panelHeader.Location = new Point(0, 0);
            panelHeader.MaximumSize = new Size(0, 100);
            panelHeader.Name = "panelHeader";
            panelHeader.Size = new Size(187, 100);
            panelHeader.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.padel_tennis_logo_banner_poster_260nw_1856766739;
            pictureBox1.InitialImage = (Image)resources.GetObject("pictureBox1.InitialImage");
            pictureBox1.Location = new Point(12, 9);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(92, 88);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblNombre.ForeColor = Color.White;
            lblNombre.Location = new Point(101, 18);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(70, 75);
            lblNombre.TabIndex = 0;
            lblNombre.Text = "CLUB\r\nDE\r\nPÁDEL";
            // 
            // panelContenedor
            // 
            panelContenedor.Dock = DockStyle.Fill;
            panelContenedor.Location = new Point(0, 0);
            panelContenedor.Name = "panelContenedor";
            panelContenedor.Size = new Size(793, 561);
            panelContenedor.TabIndex = 0;
            // 
            // MenuPrincipal
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(984, 561);
            Controls.Add(splitContainer1);
            MinimumSize = new Size(1000, 600);
            Name = "MenuPrincipal";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Show;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "MenuPrincipal";
            Load += MenuPrincipal_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panelBotones.ResumeLayout(false);
            panelHeader.ResumeLayout(false);
            panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private SplitContainer splitContainer1;
        private Panel panelHeader;
        private Label lblNombre;
        private Panel panelBotones;
        private Button btnReservas;
        private Button btnSalir;
        private Button btnReportes;
        private Button btnProductos;
        private Button btnPersonas;
        private Button btnCaja;
        private Button btnVentas;
        private Panel panelContenedor;
        private PictureBox pictureBox1;
        private Button btnUsuarios;
        private Button btnCanchas;
        private Button btnCompras;
        private Button btnCuentaCorriente;
    }
}
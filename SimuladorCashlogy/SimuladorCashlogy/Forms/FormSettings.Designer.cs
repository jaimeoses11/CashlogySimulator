namespace SimuladorCashlogy.Forms
{
    partial class FormSettings
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.lblConfig = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.panelServer = new System.Windows.Forms.GroupBox();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.lblHost = new System.Windows.Forms.Label();
            this.lblEnableServerConfig = new System.Windows.Forms.Label();
            this.EnableServerConfig = new System.Windows.Forms.ToggleButton();
            this.txtEntradaMonedas = new System.Windows.Forms.TextBox();
            this.lblEntradaMonedas = new System.Windows.Forms.Label();
            this.txtEntradaBilletes = new System.Windows.Forms.TextBox();
            this.lblEntradaBilletes = new System.Windows.Forms.Label();
            this.txtSalidaBilletes = new System.Windows.Forms.TextBox();
            this.lblSalidaBilletes = new System.Windows.Forms.Label();
            this.txtSalidaMonedas = new System.Windows.Forms.TextBox();
            this.lblSalidaMonedas = new System.Windows.Forms.Label();
            this.lblEnableInCad = new System.Windows.Forms.Label();
            this.lblEnableOutCad = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.errInCoin = new System.Windows.Forms.ErrorProvider(this.components);
            this.errInBill = new System.Windows.Forms.ErrorProvider(this.components);
            this.errOutCoin = new System.Windows.Forms.ErrorProvider(this.components);
            this.errOutBill = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblEnableSim = new System.Windows.Forms.Label();
            this.txtDevices = new System.Windows.Forms.TextBox();
            this.panelRegistro = new System.Windows.Forms.GroupBox();
            this.lblDipsList = new System.Windows.Forms.Label();
            this.enableSim = new System.Windows.Forms.ToggleButton();
            this.lblModoRun = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.MyButton();
            this.CadenciaSalida = new System.Windows.Forms.ToggleButton();
            this.CadenciaEntrada = new System.Windows.Forms.ToggleButton();
            this.panelServer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errInCoin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errInBill)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errOutCoin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errOutBill)).BeginInit();
            this.panelRegistro.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblConfig
            // 
            this.lblConfig.AutoSize = true;
            this.lblConfig.Font = new System.Drawing.Font("Century Gothic", 15F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConfig.Location = new System.Drawing.Point(74, 49);
            this.lblConfig.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblConfig.Name = "lblConfig";
            this.lblConfig.Size = new System.Drawing.Size(185, 23);
            this.lblConfig.TabIndex = 162;
            this.lblConfig.Text = "CONFIGURACIÓN";
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPort.Location = new System.Drawing.Point(77, 137);
            this.lblPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(35, 19);
            this.lblPort.TabIndex = 172;
            this.lblPort.Text = "Port";
            // 
            // txtPort
            // 
            this.txtPort.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPort.Location = new System.Drawing.Point(115, 134);
            this.txtPort.Margin = new System.Windows.Forms.Padding(4);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(116, 24);
            this.txtPort.TabIndex = 173;
            this.txtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // panelServer
            // 
            this.panelServer.Controls.Add(this.txtHost);
            this.panelServer.Controls.Add(this.lblHost);
            this.panelServer.Controls.Add(this.lblEnableServerConfig);
            this.panelServer.Controls.Add(this.EnableServerConfig);
            this.panelServer.Controls.Add(this.txtPort);
            this.panelServer.Controls.Add(this.lblPort);
            this.panelServer.Location = new System.Drawing.Point(519, 300);
            this.panelServer.Name = "panelServer";
            this.panelServer.Size = new System.Drawing.Size(370, 192);
            this.panelServer.TabIndex = 175;
            this.panelServer.TabStop = false;
            this.panelServer.Text = "Configuración Servidor";
            // 
            // txtHost
            // 
            this.txtHost.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHost.Location = new System.Drawing.Point(115, 97);
            this.txtHost.Margin = new System.Windows.Forms.Padding(4);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(170, 24);
            this.txtHost.TabIndex = 195;
            this.txtHost.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = true;
            this.lblHost.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHost.Location = new System.Drawing.Point(74, 100);
            this.lblHost.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(38, 19);
            this.lblHost.TabIndex = 194;
            this.lblHost.Text = "Host";
            // 
            // lblEnableServerConfig
            // 
            this.lblEnableServerConfig.AutoSize = true;
            this.lblEnableServerConfig.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEnableServerConfig.Location = new System.Drawing.Point(74, 50);
            this.lblEnableServerConfig.Name = "lblEnableServerConfig";
            this.lblEnableServerConfig.Size = new System.Drawing.Size(247, 19);
            this.lblEnableServerConfig.TabIndex = 193;
            this.lblEnableServerConfig.Text = "Habilita configuración del Servidor";
            // 
            // EnableServerConfig
            // 
            this.EnableServerConfig.AutoSize = false;
            this.EnableServerConfig.Cursor = System.Windows.Forms.Cursors.Hand;
            this.EnableServerConfig.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EnableServerConfig.Location = new System.Drawing.Point(19, 46);
            this.EnableServerConfig.Margin = new System.Windows.Forms.Padding(4);
            this.EnableServerConfig.MinimumSize = new System.Drawing.Size(45, 22);
            this.EnableServerConfig.Name = "EnableServerConfig";
            this.EnableServerConfig.OffBackColor = System.Drawing.Color.Gray;
            this.EnableServerConfig.OffToggleColor = System.Drawing.Color.WhiteSmoke;
            this.EnableServerConfig.OnBackColor = System.Drawing.Color.LimeGreen;
            this.EnableServerConfig.OnToggleColor = System.Drawing.Color.WhiteSmoke;
            this.EnableServerConfig.Size = new System.Drawing.Size(48, 23);
            this.EnableServerConfig.TabIndex = 192;
            this.EnableServerConfig.Text = "Cadencia Salida";
            this.EnableServerConfig.UseVisualStyleBackColor = true;
            this.EnableServerConfig.CheckedChanged += new System.EventHandler(this.EnableServerConfig_CheckedChanged);
            // 
            // txtEntradaMonedas
            // 
            this.txtEntradaMonedas.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEntradaMonedas.Location = new System.Drawing.Point(275, 154);
            this.txtEntradaMonedas.Margin = new System.Windows.Forms.Padding(4);
            this.txtEntradaMonedas.Name = "txtEntradaMonedas";
            this.txtEntradaMonedas.Size = new System.Drawing.Size(116, 24);
            this.txtEntradaMonedas.TabIndex = 177;
            this.txtEntradaMonedas.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtEntradaMonedas.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // lblEntradaMonedas
            // 
            this.lblEntradaMonedas.AutoSize = true;
            this.lblEntradaMonedas.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEntradaMonedas.Location = new System.Drawing.Point(91, 157);
            this.lblEntradaMonedas.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEntradaMonedas.Name = "lblEntradaMonedas";
            this.lblEntradaMonedas.Size = new System.Drawing.Size(166, 19);
            this.lblEntradaMonedas.TabIndex = 176;
            this.lblEntradaMonedas.Text = "Entrada Monedas (ms)";
            this.lblEntradaMonedas.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtEntradaBilletes
            // 
            this.txtEntradaBilletes.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEntradaBilletes.Location = new System.Drawing.Point(275, 197);
            this.txtEntradaBilletes.Margin = new System.Windows.Forms.Padding(4);
            this.txtEntradaBilletes.Name = "txtEntradaBilletes";
            this.txtEntradaBilletes.Size = new System.Drawing.Size(116, 24);
            this.txtEntradaBilletes.TabIndex = 179;
            this.txtEntradaBilletes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtEntradaBilletes.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // lblEntradaBilletes
            // 
            this.lblEntradaBilletes.AutoSize = true;
            this.lblEntradaBilletes.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEntradaBilletes.Location = new System.Drawing.Point(91, 200);
            this.lblEntradaBilletes.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEntradaBilletes.Name = "lblEntradaBilletes";
            this.lblEntradaBilletes.Size = new System.Drawing.Size(146, 19);
            this.lblEntradaBilletes.TabIndex = 178;
            this.lblEntradaBilletes.Text = "Entrada Billetes (ms)";
            this.lblEntradaBilletes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSalidaBilletes
            // 
            this.txtSalidaBilletes.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSalidaBilletes.Location = new System.Drawing.Point(747, 197);
            this.txtSalidaBilletes.Margin = new System.Windows.Forms.Padding(4);
            this.txtSalidaBilletes.Name = "txtSalidaBilletes";
            this.txtSalidaBilletes.Size = new System.Drawing.Size(116, 24);
            this.txtSalidaBilletes.TabIndex = 184;
            this.txtSalidaBilletes.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSalidaBilletes.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // lblSalidaBilletes
            // 
            this.lblSalidaBilletes.AutoSize = true;
            this.lblSalidaBilletes.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalidaBilletes.Location = new System.Drawing.Point(561, 200);
            this.lblSalidaBilletes.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSalidaBilletes.Name = "lblSalidaBilletes";
            this.lblSalidaBilletes.Size = new System.Drawing.Size(153, 19);
            this.lblSalidaBilletes.TabIndex = 183;
            this.lblSalidaBilletes.Text = "Salida Monedas (ms)";
            this.lblSalidaBilletes.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtSalidaMonedas
            // 
            this.txtSalidaMonedas.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSalidaMonedas.Location = new System.Drawing.Point(747, 154);
            this.txtSalidaMonedas.Margin = new System.Windows.Forms.Padding(4);
            this.txtSalidaMonedas.Name = "txtSalidaMonedas";
            this.txtSalidaMonedas.Size = new System.Drawing.Size(116, 24);
            this.txtSalidaMonedas.TabIndex = 182;
            this.txtSalidaMonedas.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSalidaMonedas.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // lblSalidaMonedas
            // 
            this.lblSalidaMonedas.AutoSize = true;
            this.lblSalidaMonedas.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSalidaMonedas.Location = new System.Drawing.Point(561, 157);
            this.lblSalidaMonedas.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSalidaMonedas.Name = "lblSalidaMonedas";
            this.lblSalidaMonedas.Size = new System.Drawing.Size(153, 19);
            this.lblSalidaMonedas.TabIndex = 181;
            this.lblSalidaMonedas.Text = "Salida Monedas (ms)";
            this.lblSalidaMonedas.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblEnableInCad
            // 
            this.lblEnableInCad.AutoSize = true;
            this.lblEnableInCad.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEnableInCad.Location = new System.Drawing.Point(137, 121);
            this.lblEnableInCad.Name = "lblEnableInCad";
            this.lblEnableInCad.Size = new System.Drawing.Size(319, 19);
            this.lblEnableInCad.TabIndex = 190;
            this.lblEnableInCad.Text = "Habilita cadencia en la entrada de efectivo";
            // 
            // lblEnableOutCad
            // 
            this.lblEnableOutCad.AutoSize = true;
            this.lblEnableOutCad.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEnableOutCad.Location = new System.Drawing.Point(611, 120);
            this.lblEnableOutCad.Name = "lblEnableOutCad";
            this.lblEnableOutCad.Size = new System.Drawing.Size(303, 19);
            this.lblEnableOutCad.TabIndex = 191;
            this.lblEnableOutCad.Text = "Habilita cadencia en la salida de efectivo";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gold;
            this.panel1.Location = new System.Drawing.Point(78, 255);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(903, 6);
            this.panel1.TabIndex = 192;
            // 
            // errInCoin
            // 
            this.errInCoin.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errInCoin.ContainerControl = this;
            // 
            // errInBill
            // 
            this.errInBill.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errInBill.ContainerControl = this;
            // 
            // errOutCoin
            // 
            this.errOutCoin.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errOutCoin.ContainerControl = this;
            // 
            // errOutBill
            // 
            this.errOutBill.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errOutBill.ContainerControl = this;
            // 
            // lblEnableSim
            // 
            this.lblEnableSim.AutoSize = true;
            this.lblEnableSim.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEnableSim.Location = new System.Drawing.Point(77, 27);
            this.lblEnableSim.Name = "lblEnableSim";
            this.lblEnableSim.Size = new System.Drawing.Size(142, 19);
            this.lblEnableSim.TabIndex = 195;
            this.lblEnableSim.Text = "Habilita Simulación";
            // 
            // txtDevices
            // 
            this.txtDevices.Location = new System.Drawing.Point(36, 81);
            this.txtDevices.Multiline = true;
            this.txtDevices.Name = "txtDevices";
            this.txtDevices.ReadOnly = true;
            this.txtDevices.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDevices.Size = new System.Drawing.Size(306, 99);
            this.txtDevices.TabIndex = 198;
            // 
            // panelRegistro
            // 
            this.panelRegistro.Controls.Add(this.lblDipsList);
            this.panelRegistro.Controls.Add(this.lblEnableSim);
            this.panelRegistro.Controls.Add(this.txtDevices);
            this.panelRegistro.Controls.Add(this.enableSim);
            this.panelRegistro.Location = new System.Drawing.Point(130, 300);
            this.panelRegistro.Name = "panelRegistro";
            this.panelRegistro.Size = new System.Drawing.Size(370, 192);
            this.panelRegistro.TabIndex = 199;
            this.panelRegistro.TabStop = false;
            this.panelRegistro.Text = "Configuración Registro ";
            // 
            // lblDipsList
            // 
            this.lblDipsList.AutoSize = true;
            this.lblDipsList.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDipsList.Location = new System.Drawing.Point(32, 59);
            this.lblDipsList.Name = "lblDipsList";
            this.lblDipsList.Size = new System.Drawing.Size(117, 19);
            this.lblDipsList.TabIndex = 199;
            this.lblDipsList.Text = "Lista Dispositivos";
            // 
            // enableSim
            // 
            this.enableSim.AutoSize = false;
            this.enableSim.Cursor = System.Windows.Forms.Cursors.Hand;
            this.enableSim.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.enableSim.Location = new System.Drawing.Point(22, 23);
            this.enableSim.Margin = new System.Windows.Forms.Padding(4);
            this.enableSim.MinimumSize = new System.Drawing.Size(45, 22);
            this.enableSim.Name = "enableSim";
            this.enableSim.OffBackColor = System.Drawing.Color.Gray;
            this.enableSim.OffToggleColor = System.Drawing.Color.WhiteSmoke;
            this.enableSim.OnBackColor = System.Drawing.Color.LimeGreen;
            this.enableSim.OnToggleColor = System.Drawing.Color.WhiteSmoke;
            this.enableSim.Size = new System.Drawing.Size(48, 23);
            this.enableSim.TabIndex = 194;
            this.enableSim.Text = "Cadencia Salida";
            this.enableSim.UseVisualStyleBackColor = true;
            // 
            // lblModoRun
            // 
            this.lblModoRun.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModoRun.ForeColor = System.Drawing.Color.Red;
            this.lblModoRun.Location = new System.Drawing.Point(130, 276);
            this.lblModoRun.Name = "lblModoRun";
            this.lblModoRun.Size = new System.Drawing.Size(759, 21);
            this.lblModoRun.TabIndex = 200;
            this.lblModoRun.Text = "Configuración deshabilitada al estar en el modo Run";
            this.lblModoRun.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Flip = System.Windows.Forms.FlipOrientation.Normal;
            this.btnSave.IconChar = System.Windows.Forms.IconChar.Save;
            this.btnSave.IconColor = System.Drawing.Color.Gold;
            this.btnSave.IconSize = 50;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.Location = new System.Drawing.Point(926, 437);
            this.btnSave.Name = "btnSave";
            this.btnSave.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.btnSave.Rotation = 0D;
            this.btnSave.Size = new System.Drawing.Size(55, 55);
            this.btnSave.TabIndex = 197;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // CadenciaSalida
            // 
            this.CadenciaSalida.AutoSize = false;
            this.CadenciaSalida.Checked = true;
            this.CadenciaSalida.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CadenciaSalida.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CadenciaSalida.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CadenciaSalida.Location = new System.Drawing.Point(552, 118);
            this.CadenciaSalida.Margin = new System.Windows.Forms.Padding(4);
            this.CadenciaSalida.MinimumSize = new System.Drawing.Size(45, 22);
            this.CadenciaSalida.Name = "CadenciaSalida";
            this.CadenciaSalida.OffBackColor = System.Drawing.Color.Gray;
            this.CadenciaSalida.OffToggleColor = System.Drawing.Color.WhiteSmoke;
            this.CadenciaSalida.OnBackColor = System.Drawing.Color.LimeGreen;
            this.CadenciaSalida.OnToggleColor = System.Drawing.Color.WhiteSmoke;
            this.CadenciaSalida.Size = new System.Drawing.Size(48, 23);
            this.CadenciaSalida.TabIndex = 180;
            this.CadenciaSalida.Text = "Cadencia Salida";
            this.CadenciaSalida.UseVisualStyleBackColor = true;
            this.CadenciaSalida.CheckedChanged += new System.EventHandler(this.CadenciaSalida_CheckedChanged);
            // 
            // CadenciaEntrada
            // 
            this.CadenciaEntrada.AutoSize = false;
            this.CadenciaEntrada.Checked = true;
            this.CadenciaEntrada.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CadenciaEntrada.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CadenciaEntrada.Font = new System.Drawing.Font("Century Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CadenciaEntrada.Location = new System.Drawing.Point(78, 118);
            this.CadenciaEntrada.Margin = new System.Windows.Forms.Padding(4);
            this.CadenciaEntrada.MinimumSize = new System.Drawing.Size(45, 22);
            this.CadenciaEntrada.Name = "CadenciaEntrada";
            this.CadenciaEntrada.OffBackColor = System.Drawing.Color.Gray;
            this.CadenciaEntrada.OffToggleColor = System.Drawing.Color.WhiteSmoke;
            this.CadenciaEntrada.OnBackColor = System.Drawing.Color.LimeGreen;
            this.CadenciaEntrada.OnToggleColor = System.Drawing.Color.WhiteSmoke;
            this.CadenciaEntrada.Size = new System.Drawing.Size(48, 23);
            this.CadenciaEntrada.TabIndex = 174;
            this.CadenciaEntrada.Text = "Cadencia Entrada";
            this.CadenciaEntrada.UseVisualStyleBackColor = true;
            this.CadenciaEntrada.CheckedChanged += new System.EventHandler(this.CadenciaEntrada_CheckedChanged);
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1069, 548);
            this.Controls.Add(this.lblModoRun);
            this.Controls.Add(this.panelRegistro);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblEnableOutCad);
            this.Controls.Add(this.lblEnableInCad);
            this.Controls.Add(this.txtSalidaBilletes);
            this.Controls.Add(this.lblSalidaBilletes);
            this.Controls.Add(this.txtSalidaMonedas);
            this.Controls.Add(this.lblSalidaMonedas);
            this.Controls.Add(this.CadenciaSalida);
            this.Controls.Add(this.txtEntradaBilletes);
            this.Controls.Add(this.lblEntradaBilletes);
            this.Controls.Add(this.txtEntradaMonedas);
            this.Controls.Add(this.lblEntradaMonedas);
            this.Controls.Add(this.panelServer);
            this.Controls.Add(this.CadenciaEntrada);
            this.Controls.Add(this.lblConfig);
            this.Font = new System.Drawing.Font("Century Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximumSize = new System.Drawing.Size(1069, 548);
            this.MinimumSize = new System.Drawing.Size(1069, 548);
            this.Name = "FormSettings";
            this.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.panelServer.ResumeLayout(false);
            this.panelServer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errInCoin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errInBill)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errOutCoin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errOutBill)).EndInit();
            this.panelRegistro.ResumeLayout(false);
            this.panelRegistro.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.Label lblConfig;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.ToggleButton CadenciaEntrada;
        private System.Windows.Forms.GroupBox panelServer;
        private System.Windows.Forms.TextBox txtEntradaMonedas;
        private System.Windows.Forms.Label lblEntradaMonedas;
        private System.Windows.Forms.TextBox txtEntradaBilletes;
        private System.Windows.Forms.Label lblEntradaBilletes;
        private System.Windows.Forms.TextBox txtSalidaBilletes;
        private System.Windows.Forms.Label lblSalidaBilletes;
        private System.Windows.Forms.TextBox txtSalidaMonedas;
        private System.Windows.Forms.Label lblSalidaMonedas;
        private System.Windows.Forms.ToggleButton CadenciaSalida;
        private System.Windows.Forms.Label lblEnableInCad;
        private System.Windows.Forms.Label lblEnableOutCad;
        private System.Windows.Forms.Label lblEnableServerConfig;
        private System.Windows.Forms.ToggleButton EnableServerConfig;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ErrorProvider errInCoin;
        private System.Windows.Forms.ErrorProvider errInBill;
        private System.Windows.Forms.ErrorProvider errOutCoin;
        private System.Windows.Forms.ErrorProvider errOutBill;
        private System.Windows.Forms.Label lblEnableSim;
        private System.Windows.Forms.ToggleButton enableSim;
        private System.Windows.Forms.MyButton btnSave;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.GroupBox panelRegistro;
        private System.Windows.Forms.TextBox txtDevices;
        private System.Windows.Forms.Label lblDipsList;
        private System.Windows.Forms.Label lblModoRun;
    }
}
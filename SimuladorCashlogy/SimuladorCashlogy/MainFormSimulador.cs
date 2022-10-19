using Cashlogy.Idiomas;
using SimuladorCashlogy;
using SimuladorCashlogy.Forms;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Cashlogy.Vistas
{
    public partial class MainFormSimulador : Form
    {
        private const int MAX_ERRORES = 10;

        private enum NumForm
        {
            FormCashlogy,
            FormConfigCont,
            FormSimErrores,
            FormOrdenesOPOS,
            FormSettings
        }

        private CashlogyDevice dev;
        private Idioma Idioma;

        private MyButton currentBtn;
        private Panel leftBorderBtn;
        private Form currentForm;
        private Form settingsForm;
        private Config cfg;

        #region Variables
        public bool depositEnabled = false;
        public bool deviceOpened = false;
        public bool errorsConfig = false;
        public int[] error;
        public int panel = (int)NumFrase.SinIniciar;
        private bool hayError = false;
        private int state = (int)NumFrase.Cerrado;

        private MyButton[] btnLateral;

        // Contabilidad
        private PictureBox[] imgCash;
        private Label[] lblCash;
        private TextBox[] txtAlmacen;
        private TextBox[] txtStacker;
        private VProgressBar[] prbCapacidad;
        private Label[] lblDevolucion;
        #endregion

        public delegate void onLanguageChanged();
        public event onLanguageChanged OnLanguageChanged;

        public MainFormSimulador(CashlogyDevice cashlogyDev, Config config)
        {
            InitializeComponent();
            dev = cashlogyDev;
            Idioma = new Idioma();
            cfg = config;
            error = new int[MAX_ERRORES];

            leftBorderBtn = new Panel();
            leftBorderBtn.Size = new Size(5, 55);
            panelBotones.Controls.Add(leftBorderBtn);

            #region Listas de Elementos del Form
            btnLateral = new MyButton[] { btnCashlogy, btnConfigCont, btnErrores, btnOrdenesOPOS, btnSettings };
            for (int i = 0; i < btnLateral.Length; i++)
            {
                btnLateral[i].Tag = i;
            }

            // Contabilidad
            imgCash = new PictureBox[] { imgCash1, imgCash2, imgCash3, imgCash4, imgCash5, imgCash6, imgCash7, imgCash8,
                                         imgCash9, imgCash10, imgCash11, imgCash12, imgCash13, imgCash14, imgCash15, imgCash16 };
            for (int i = 0; i < imgCash.Length; i++)
            {
                imgCash[i].Tag = i;
            }

            lblCash = new Label[] { lblCash1, lblCash2, lblCash3, lblCash4, lblCash5, lblCash6, lblCash7, lblCash8,
                                    lblCash9, lblCash10, lblCash11, lblCash12, lblCash13, lblCash14, lblCash15, lblCash16 };
            for (int i = 0; i < imgCash.Length; i++)
            {
                imgCash[i].Tag = i;
            }

            txtAlmacen = new TextBox[] { txtAlmacen1, txtAlmacen2, txtAlmacen3, txtAlmacen4, txtAlmacen5, txtAlmacen6, txtAlmacen7, txtAlmacen8,
                                         txtAlmacen9, txtAlmacen10, txtAlmacen11, txtAlmacen12, txtAlmacen13, txtAlmacen14, txtAlmacen15, txtAlmacen16 };
            for (int i = 0; i < txtAlmacen.Length; i++)
            {
                txtAlmacen[i].Tag = i;
            }

            txtStacker = new TextBox[] { txtStacker1, txtStacker2, txtStacker3, txtStacker4, txtStacker5, txtStacker6, txtStacker7, txtStacker8,
                                         txtStacker9, txtStacker10, txtStacker11, txtStacker12, txtStacker13, txtStacker14, txtStacker15, txtStacker16 };
            for (int i = 0; i < txtStacker.Length; i++)
            {
                txtStacker[i].Tag = i;
            }

            prbCapacidad = new VProgressBar[] { prbCapacidad1, prbCapacidad2, prbCapacidad3, prbCapacidad4, prbCapacidad5, prbCapacidad6, prbCapacidad7, prbCapacidad8,
                                                prbCapacidad9, prbCapacidad10, prbCapacidad11, prbCapacidad12, prbCapacidad13, prbCapacidad14, prbCapacidad15, prbCapacidad16 };
            for (int i = 0; i < prbCapacidad.Length; i++)
            {
                prbCapacidad[i].Tag = i;
            }

            lblDevolucion = new Label[] { lblDevolucion1, lblDevolucion2, lblDevolucion3, lblDevolucion4, lblDevolucion5, lblDevolucion6, lblDevolucion7, lblDevolucion8,
                                          lblDevolucion9, lblDevolucion10, lblDevolucion11, lblDevolucion12, lblDevolucion13, lblDevolucion14, lblDevolucion15, lblDevolucion16 };
            for (int i = 0; i < lblDevolucion.Length; i++)
            {
                lblDevolucion[i].Tag = i;
            }
            #endregion

            #region Suscripcion Eventos Form
            for (int i = 0; i < btnLateral.Length; i++)
            {
                btnLateral[i].Click += new EventHandler(this.BtnLateral_Click);
            }
            #endregion

            #region Suscripcion Eventos Cashlogy
            dev.OnDeviceOpened += Device_OnOpened;
            dev.OnViewChanged += this.ActualizaParteVisual;
            dev.OnContChanged += this.ActualizaCont;
            dev.OnStateChanged += ActualizaEstado;
            dev.OnDepositEnabled += Deposit_OnEnabled;
            dev.OnOrderReceived += ActualizaOrderReceived;
            this.OnLanguageChanged += this.ActualizaIdioma;
            dev.OnErrorForced += ForceErrors;
            dev.OnErrorCleaned += CleanErrors;
            #endregion
        }

        private void Device_OnOpened(bool opened)
        {
            deviceOpened = opened;
            if (opened)
            {
                timerClose.Stop();
                timerOpen.Start();
            }
            else
            {
                timerOpen.Stop();
                timerClose.Start();
            }
        }

        #region Botones Barra Lateral
        private void BtnLateral_Click(object sender, EventArgs e)
        {
            MyButton current = (MyButton)sender;

            if (!deviceOpened && (int)current.Tag == 1)
            {
                MessageBox.Show(Idioma.FrasesIdioma[(int)NumFrase.MboxNoDeviceOpened]);
                return;
            }

            if ((int)current.Tag != (int)NumForm.FormOrdenesOPOS) ActivateButton(sender);

            switch (current.Tag)
            {
                case (int)NumForm.FormCashlogy:
                    OpenChildForm(new FormCashlogy(dev, Idioma, this));
                    break;
                case (int)NumForm.FormConfigCont:
                    OpenChildForm(new FormConfigCont(dev, Idioma, this));
                    break;
                case (int)NumForm.FormSimErrores:
                    OpenChildForm(new FormSimErrores(dev, Idioma, this));
                    break;
                case (int)NumForm.FormOrdenesOPOS:
                    Form OPOSForm = new OPOSFunctions(dev);
                    OPOSForm.Show();
                    break;
                case (int)NumForm.FormSettings:
                    OpenSettings(new FormSettings(dev, Idioma, this, cfg));
                    break;
            }
        }

        private void ActivateButton(object senderBtn)
        {
            if (senderBtn != null)
            {
                DisableButton();
                currentBtn = (MyButton)senderBtn;
                currentBtn.BackColor = Color.Gainsboro;
                currentBtn.ForeColor = Color.Black;
                currentBtn.IconColor = Color.Black;
                currentBtn.Refresh();

                leftBorderBtn.BackColor = Color.Gold;
                leftBorderBtn.Height = currentBtn.Height;
                leftBorderBtn.Location = new Point(0, currentBtn.Location.Y);
                leftBorderBtn.Visible = true;
                leftBorderBtn.BringToFront();
                leftBorderBtn.Refresh();
            }
        }

        private void DisableButton()
        {
            if (currentBtn != null)
            {
                leftBorderBtn.Visible = false;
                currentBtn.BackColor = Color.WhiteSmoke;
                currentBtn.ForeColor = Color.DimGray;
                currentBtn.IconColor = Color.DimGray;
                currentBtn.Refresh();
            }
        }

        private void OpenChildForm(Form childForm)
        {
            if (currentForm != null) currentForm.Close();
            if (settingsForm != null) CloseSettings();

            currentForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelCurrentForm.Controls.Add(childForm);
            panelCurrentForm.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void OpenSettings(Form settings)
        {
            if (settingsForm != null) CloseSettings();

            settingsForm = settings;
            settings.TopLevel = false;
            settings.FormBorderStyle = FormBorderStyle.None;
            settings.Dock = DockStyle.Fill;
            panelPrincipal.Controls.Add(settings);
            settings.BringToFront();
            settings.Show();
        }

        private void CloseSettings()
        {
            settingsForm.Close();
            settingsForm = null;
        }
        #endregion

        #region Eventos Form
        private void FormSimulador_Load(object sender, EventArgs e)
        {
            string idioma = (string)SimuladorCashlogy.Properties.Settings.Default.Idioma;
            Idioma.ConfiguraIdioma(idioma);
            selecIdioma.SelectedItem = Idioma.FrasesIdioma[(int)NumFrase.Actual];

            if (deviceOpened) panelClosed.Width = 0;
            else panelClosed.Width = panelPrincipal.Width;

            SetToolTipBotones();
            ActivateButton(btnCashlogy);
            OpenChildForm(new FormCashlogy(dev, Idioma, this));
            if (cfg.isConfig)
            {
                ActivateButton(btnSettings);
                OpenSettings(new FormSettings(dev, Idioma, this, cfg));
            }
        }

        private void MainMenuSimulador_Shown(object sender, EventArgs e)
        {
            if (cfg.isConfig) MessageBox.Show(Idioma.FrasesIdioma[(int)NumFrase.ModoConfig]);
        }

        private void FormSimulador_FormClosing(object sender, FormClosingEventArgs e)
        {
            dev.SaveContToFile(dev.Def.DeviceName);

            string idioma = Idioma.FrasesIdioma[(int)NumFrase.Actual];
            SimuladorCashlogy.Properties.Settings.Default.Idioma = idioma;
            SimuladorCashlogy.Properties.Settings.Default.Save();

            timerOpen.Stop();
            timerClose.Stop();
        }

        private void SelecIdioma_SelectedIndexChanged(object sender, EventArgs e)
        {
            string idioma = selecIdioma.Text;
            Idioma.ConfiguraIdioma(idioma);
            OnLanguageChanged();
        }

        private void TimerOpen_Tick(object sender, EventArgs e)
        {
            if (panelClosed.Width > 0) panelClosed.Width -= 179;
            else timerOpen.Stop();
        }

        private void TimerClose_Tick(object sender, EventArgs e)
        {
            if (panelClosed.Width < panelPrincipal.Width) panelClosed.Width += 179;
            else timerClose.Stop();
        }
        #endregion

        private void ActualizaEstado()
        {
            if (hayError) return;

            int state = 0;
            dev.GetState(ref state);

            // Actualiza Label
            switch (state)
            {
                case Const.OPOS_S_CLOSED:
                    this.state = (int)NumFrase.Cerrado;
                    break;
                case Const.OPOS_S_IDLE:
                    this.state = (int)NumFrase.Reposo;
                    break;
                case Const.OPOS_S_BUSY:
                    this.state = (int)NumFrase.Ocupado;
                    break;
                case Const.OPOS_S_ERROR:
                    this.state = (int)NumFrase.Error;
                    break;
            }
            lblEstado.Text = Idioma.FrasesIdioma[(int)NumFrase.Estado] + Idioma.FrasesIdioma[this.state];
        }

        private void ActualizaOrderReceived(int num)
        {
            panel = num;
        }

        private void Deposit_OnEnabled(bool on)
        {
            depositEnabled = on;
        }

        private void ForceErrors()
        {
            hayError = true;
            state = (int)NumFrase.Error;
            lblEstado.Text = Idioma.FrasesIdioma[(int)NumFrase.Estado] + " " + Idioma.FrasesIdioma[state];
        }

        private void CleanErrors()
        {
            hayError = false;
            int estado = 0;
            dev.GetState(ref estado);
            switch (estado)
            {
                case Const.OPOS_S_CLOSED:
                    state = (int)NumFrase.Cerrado;
                    break;
                case Const.OPOS_S_IDLE:
                    state = (int)NumFrase.Reposo;
                    break;
                case Const.OPOS_S_BUSY:
                    state = (int)NumFrase.Ocupado;
                    break;
                case Const.OPOS_S_ERROR:
                    state = (int)NumFrase.Error;
                    break;
            }
            lblEstado.Text = Idioma.FrasesIdioma[(int)NumFrase.Estado] + " " + Idioma.FrasesIdioma[state];
        }

        #region Metodos Formulario
        private void ActualizaCont()
        {
            int stackerLevel = 0;
            int[] level = dev.GetItemsLevel(ref stackerLevel);
            int[,] dispensed = dev.GetDispensed();

            // Stacker
            Color colorStack = SetColour(stackerLevel);
            prbStacker.ForeColor = colorStack;
            prbStacker.Maximum = dev.Def.CapStacker;
            prbStacker.Value = dev.GetTotStacker();

            // Items
            int[] recyclers = dev.GetRecyclers();
            int[] stacker = dev.GetStacker();
            for (int i = 0; i < dev.Def.NumItems; i++)
            {
                int j = ObtainFormIndex(i);
                Color colorItem = SetColour(level[i]);
                if (dev.Def.ItemsDef[i].IsDispensable && !dev.Def.ItemsDef[i].IsDepositable)
                {
                    lblCash[j].ForeColor = colorItem;
                    txtAlmacen[j].ForeColor = colorItem;
                    txtAlmacen[j].Text = recyclers[i].ToString();
                    txtStacker[j].Text = null;
                    prbCapacidad[j].Maximum = dev.Def.ItemsDef[i].Capacity;
                    prbCapacidad[j].ForeColor = colorItem;
                    prbCapacidad[j].Value = recyclers[i];
                    lblDevolucion[j].Text = string.Format("{0}/{1}", dispensed[0, i], dispensed[1, i]);
                }
                else if (dev.Def.ItemsDef[i].IsDispensable && dev.Def.ItemsDef[i].IsDepositable)
                {
                    lblCash[j].ForeColor = colorItem;
                    txtAlmacen[j].ForeColor = colorItem;
                    txtAlmacen[j].Text = recyclers[i].ToString();
                    txtStacker[j].ForeColor = colorStack;
                    txtStacker[j].Text = stacker[i].ToString();
                    prbCapacidad[j].ForeColor = SetColour(level[i]);
                    prbCapacidad[j].Maximum = dev.Def.ItemsDef[i].Capacity;
                    prbCapacidad[j].Value = recyclers[i];
                    lblDevolucion[j].Text = string.Format("{0}/{1}", dispensed[0, i], dispensed[1, i]);
                }
                else if (!dev.Def.ItemsDef[i].IsDispensable && dev.Def.ItemsDef[i].IsDepositable)
                {
                    txtAlmacen[j].Text = null;
                    txtStacker[j].ForeColor = colorStack;
                    txtStacker[j].Text = stacker[i].ToString();
                }
            }
        }

        private void ActualizaParteVisual()
        {
            // Visibilidad
            for (int i = 0; i < txtAlmacen.Length; i++)
            {
                if ((i >= dev.Def.NumCoins && i < 8) || (i >= (8 + dev.Def.NumBills)))
                {
                    // Contabilidad
                    imgCash[i].Visible = false;
                    lblCash[i].Visible = false;
                    txtAlmacen[i].Visible = false;
                    txtStacker[i].Visible = false;
                    prbCapacidad[i].Visible = false;
                    lblDevolucion[i].Visible = false;
                }
                else
                {
                    // Contabilidad
                    imgCash[i].Visible = true;
                    lblCash[i].Visible = true;
                    txtAlmacen[i].Visible = true;
                    txtStacker[i].Visible = true;
                    prbCapacidad[i].Visible = true;
                    lblDevolucion[i].Visible = true;
                }

                int j = ObtainRealIndex(i);
                if (!dev.Def.ItemsDef[j].IsDispensable)
                {
                    prbCapacidad[i].Visible = false;
                    lblDevolucion[i].Visible = false;
                }
            }
            SetElementPosition(8 + dev.Def.NumBills);

            // Texto
            selecIdioma.SelectedItem = Idioma.FrasesIdioma[(int)NumFrase.Actual];
            for (int i = 0; i < lblCash.Length; i++)
            {
                int j = ObtainRealIndex(i);
                lblCash[i].Text = dev.Def.ItemsDef[j].Name;
            }

            // Imagenes
            for (int i = 0; i < imgCash.Length; i++)
            {
                if (i < dev.Def.NumCoins || (i >= 8 && i < 8 + dev.Def.NumBills))
                {
                    int j = ObtainRealIndex(i);
                    string imgPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                                     dev.Def.ItemsDef[j].Image);
                    imgCash[i].Image = Image.FromFile(imgPath);
                }
            }
        }

        private void ActualizaIdioma()
        {
            // Selecciona Idioma
            lblIdioma.Text = Idioma.FrasesIdioma[(int)NumFrase.Idioma];

            // Textos labels y botones
            lblEstado.Text = Idioma.FrasesIdioma[(int)NumFrase.Estado] + " " + Idioma.FrasesIdioma[state];
            this.Text = Idioma.FrasesIdioma[(int)NumFrase.SimCashlogy];
            lblCashlogySim.Text = Idioma.FrasesIdioma[(int)NumFrase.SimCashlogy].ToUpper();
            lblContabilidad.Text = Idioma.FrasesIdioma[(int)NumFrase.Contabilidad];
            lblAlmacen.Text = Idioma.FrasesIdioma[(int)NumFrase.Almacen];
            lblStacker.Text = Idioma.FrasesIdioma[(int)NumFrase.Stacker];
            lblDevol.Text = Idioma.FrasesIdioma[(int)NumFrase.Devolucion];
            lblClosed.Text = Idioma.FrasesIdioma[(int)NumFrase.DispClosed] + "\n" + Idioma.FrasesIdioma[(int)NumFrase.WaitOpen];
            SetToolTipBotones();

            // Seleccion
            selecIdioma.SelectedItem = Idioma.FrasesIdioma[(int)NumFrase.Actual];
        }

        private int ObtainFormIndex(int a)
        {
            int b = a;
            if (dev.Def.NumCoins < 8 && a >= dev.Def.NumCoins) b = a + (8 - dev.Def.NumCoins);
            return b;
        }

        private int ObtainRealIndex(int a)
        {
            int b = a;
            if (dev.Def.NumCoins < 8 && a >= 8 && a < (8 + dev.Def.NumBills)) b = a - (8 - dev.Def.NumCoins);
            else if (a >= (8 + dev.Def.NumBills)) b = a - (16 - dev.Def.NumItems);
            return b;
        }

        private Color SetColour(int level)
        {
            switch (level)
            {
                case Const.CHAN_STATUS_EMPTY:
                    return Color.Red;

                case Const.CHAN_STATUS_NEAREMPTY:
                    return Color.Red;

                case Const.CHAN_STATUS_OK:
                    return Color.Black;

                case Const.CHAN_STATUS_NEARFULL:
                    return Color.MediumBlue;

                case Const.CHAN_STATUS_FULL:
                    return Color.MediumBlue;

                default:
                    return Color.LimeGreen;
            }
        }

        private void SetElementPosition(int num)
        {
            if (num == 8) prbStacker.Visible = false;
            else prbStacker.Visible = true;

            prbStacker.Height = prbCapacidad[num - 1].Location.Y + prbCapacidad[num -1].Height - prbCapacidad[8].Location.Y ;
        }

        private void SetToolTipBotones()
        {
            int[] frase = {(int)NumFrase.BtnCashlogy, (int)NumFrase.BtnConfigCont, (int)NumFrase.BtnSimErr,
                           (int)NumFrase.BtnOrdenesOPOS, (int)NumFrase.BtnSettings };
            for (int i = 0; i < btnLateral.Length; i++)
            {
                toolTipBotones.SetToolTip(btnLateral[i], Idioma.FrasesIdioma[frase[i]]);
            }
        }
        #endregion
    }
}
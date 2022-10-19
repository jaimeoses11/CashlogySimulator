using Cashlogy;
using Cashlogy.Idiomas;
using Cashlogy.Vistas;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace SimuladorCashlogy.Forms
{
    public partial class FormCashlogy : Form
    {
        CashlogyDevice dev;
        Idioma Idioma;
        MainFormSimulador sim;

        #region Variables
        private bool stackerOpened = false;
        private int panel = (int)NumFrase.SinIniciar;
        private bool hayError = false;

        // Admision
        public Button[] btnDeposit;
        public NumericUpDown[] numSelecc;
        public Label[] lblDeposited;
        #endregion

        public FormCashlogy(CashlogyDevice device, Idioma language, MainFormSimulador simulator)

        {
            ControlHelper.SuspendDrawing(this);

            InitializeComponent();
            dev = device;
            Idioma = language;
            sim = simulator;
                        
            #region Lista de Elementos del Form
            btnDeposit = new Button[] { btnDeposit1, btnDeposit2, btnDeposit3, btnDeposit4, btnDeposit5, btnDeposit6, btnDeposit7, btnDeposit8,
                                        btnDeposit9, btnDeposit10, btnDeposit11, btnDeposit12, btnDeposit13, btnDeposit14, btnDeposit15, btnDeposit16 };
            for (int i = 0; i < btnDeposit.Length; i++)
            {
                btnDeposit[i].Tag = i;
            }

            numSelecc = new NumericUpDown[] { numSelecc1, numSelecc2, numSelecc3, numSelecc4, numSelecc5, numSelecc6, numSelecc7, numSelecc8,
                                              numSelecc9, numSelecc10, numSelecc11, numSelecc12, numSelecc13, numSelecc14, numSelecc15, numSelecc16 };
            for (int i = 0; i < numSelecc.Length; i++)
            {
                numSelecc[i].Tag = i;
            }

            lblDeposited = new Label[] { lblDeposited1, lblDeposited2, lblDeposited3, lblDeposited4, lblDeposited5, lblDeposited6, lblDeposited7, lblDeposited8,
                                         lblDeposited9, lblDeposited10, lblDeposited11, lblDeposited12, lblDeposited13, lblDeposited14, lblDeposited15, lblDeposited16 };
            for (int i = 0; i < lblDeposited.Length; i++)
            {
                lblDeposited[i].Tag = i;
            }
            #endregion

            #region Suscripcion Eventos Form
            for (int i = 0; i < CashlogyDevice.MAX_ITEMS; i++)
            {
                this.btnDeposit[i].Click += new System.EventHandler(this.BtnDeposit_Clik);
            }
            #endregion

            #region Suscripcion Eventos Cashlogy
            dev.OnViewChanged += this.ActualizaParteVisual;
            dev.OnContChanged += this.ActualizaCont;
            dev.OnStateChanged += ActualizaEstado;
            dev.OnOrderReceived += this.ActualizaOrderReceived;
            dev.OnBeginDeposit += this.HabilitaItems;
            dev.OnDepositEnabled += this.HabilitaAdmision;
            dev.OnDepositItemEnabled += this.HabilitaItems;
            dev.OnCollectStackerStarted += Dev_OnCollectStackerStarted;
            dev.OnCollectStackerEnded += Dev_OnCollectStackerEnded;
            sim.OnLanguageChanged += this.ActualizaIdioma;
            #endregion

            ControlHelper.ResumeDrawing(this);
        }

        private void HabilitaItems()
        {
            bool[] enabled = null;
            dev.GetEnableDepositItems(ref enabled);

            for (int i = 0; i < enabled.Length; i++)
            {
                int j = ObtainRealIndex(i);
                if (!enabled[i])
                {
                    toolTipItems.SetToolTip(btnDeposit[i], Idioma.FrasesIdioma[(int)NumFrase.Inhibido]);
                    btnDeposit[i].Enabled = false;
                    numSelecc[i].Enabled = false;
                    lblDeposited[i].Enabled = false;
                }
                else
                {
                    toolTipItems.SetToolTip(btnDeposit[i], dev.Def.ItemsDef[j].Name);
                    btnDeposit[i].Enabled = true;
                    numSelecc[i].Enabled = true;
                    lblDeposited[i].Enabled = true;
                }
            }
        }

        #region Eventos Form 
        private void FormCashlogy_Load(object sender, EventArgs e)
        {
            ControlHelper.SuspendDrawing(this);

            btnOpenStacker.Visible = false;

            ActualizaIdioma();
            ActualizaParteVisual();
            ActualizaCont();
            ActualizaEstado();
            ActualizaOrderReceived(sim.panel);
            HabilitaAdmision(sim.depositEnabled);

            ControlHelper.ResumeDrawing(this);
            this.Refresh();
        }

        private void FormCashlogy_FormClosing(object sender, FormClosingEventArgs e)
        {
            #region Desuscripcion Eventos Cashlogy
            dev.OnViewChanged -= this.ActualizaParteVisual;
            dev.OnContChanged -= this.ActualizaCont;
            dev.OnStateChanged -= ActualizaEstado;
            dev.OnOrderReceived -= this.ActualizaOrderReceived;
            dev.OnBeginDeposit -= this.HabilitaItems;
            dev.OnDepositEnabled -= this.HabilitaAdmision;
            dev.OnDepositItemEnabled -= this.HabilitaItems;
            dev.OnCollectStackerStarted -= Dev_OnCollectStackerStarted;
            dev.OnCollectStackerEnded -= Dev_OnCollectStackerEnded;
            sim.OnLanguageChanged -= this.ActualizaIdioma;
            #endregion
        }

        #region Añadir Monedas
        private void ListSeleccCoins_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (listSeleccCoins.Text == Idioma.FrasesIdioma[(int)NumFrase.OneCoin])
            {
                for (int i = 0; i < dev.Def.NumCoins; i++)
                {
                    numSelecc[i].Text = "1";
                }
            }
            else if (listSeleccCoins.Text == Idioma.FrasesIdioma[(int)NumFrase.Aleatorio])
            {
                int[] items = null;
                int stacker = 0;
                dev.GetAviableCapacity(ref items, ref stacker);
                Random rnd1 = new Random();
                for (int i = 0; i < dev.Def.NumCoins; i++)
                {
                    if (items[i] != 0) numSelecc[i].Text = Convert.ToString(rnd1.Next(1, items[i]));
                }
            }
        }

        private void BtnAddCoins_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dev.Def.NumCoins; i++)
            {
                dev.AddItem(i, Convert.ToInt32(numSelecc[i].Text));
            }
        }
        #endregion

        #region Añadir Billetes
        private void ListSeleccBills_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (listSeleccBills.Text == Idioma.FrasesIdioma[(int)NumFrase.OneBill])
            {
                for (int i = 8; i < 16; i++)
                {
                    numSelecc[i].Text = "1";
                }
            }
            else if (listSeleccBills.Text == Idioma.FrasesIdioma[(int)NumFrase.Aleatorio])
            {
                Random rnd2 = new Random();
                for (int i = 8; i < 16; i++)
                {
                    numSelecc[i].Text = Convert.ToString(rnd2.Next(1, 50));
                }
            }
        }

        private void BtnAddBills_Click(object sender, EventArgs e)
        {
            for (int i = 8; i < 8 + dev.Def.NumBills; i++)
            {
                int j = ObtainRealIndex(i);
                dev.AddItem(j, Convert.ToInt32(numSelecc[i].Text));
            }
        }
        #endregion

        private void BtnDeposit_Clik(object sender, EventArgs e)
        {
            Button btnClick = (Button)sender;
            int n = Convert.ToInt32(btnClick.Tag);
            int j = ObtainRealIndex(n);

            dev.AddItem(j, Convert.ToInt32(numSelecc[n].Text));
        }

        private void BtnForzarErr_Click(object sender, EventArgs e)
        {
            if (sim.errorsConfig)
            {
                if (!hayError)
                {
                    hayError = true;
                    dev.ForceErrors();
                }                
            }
            else MessageBox.Show(Idioma.FrasesIdioma[(int)NumFrase.MboxConfigErr]);
        }

        private void BtnLimpiarErr_Click(object sender, EventArgs e)
        {
            if (hayError) 
            {
                hayError = false;
                dev.CleanErrors();
            }
        }
        #endregion

        #region Eventos Cashlogy
        public void ActualizaEstado()
        {
            bool opened = false;
            bool claimed = false;
            bool deviceEnabled = false;
            dev.GetState(ref opened, ref claimed, ref deviceEnabled);

            // Actualiza LEDS
            if (opened) imgOpened.Image = global::SimuladorCashlogy.Properties.Resources.GreenLED_ON;
            else imgOpened.Image = global::SimuladorCashlogy.Properties.Resources.GreenLED_OFF;

            if (claimed) imgClaimed.Image = global::SimuladorCashlogy.Properties.Resources.GreenLED_ON;
            else imgClaimed.Image = global::SimuladorCashlogy.Properties.Resources.GreenLED_OFF;

            if (deviceEnabled) imgEnabled.Image = global::SimuladorCashlogy.Properties.Resources.GreenLED_ON;
            else imgEnabled.Image = global::SimuladorCashlogy.Properties.Resources.GreenLED_OFF;
        }

        public void ActualizaOrderReceived(int stpanel)
        {
            panel = stpanel;
            PanelEstado.Text = Idioma.FrasesIdioma[panel];
        }

        public void HabilitaAdmision(bool on)
        {
            panelAdmision.Enabled = on;
        }

        private void Dev_OnCollectStackerStarted()
        {
            btnOpenStacker.Visible = true;
        }

        private void Dev_OnCollectStackerEnded()
        {
            btnOpenStacker.Visible = false;
            stackerOpened = false;
            btnOpenStacker.IconChar = IconChar.DoorOpen;
            toolTipStacker.SetToolTip(btnOpenStacker, Idioma.FrasesIdioma[(int)NumFrase.OpenStack]);
            imgCashlogySim.Image = SimuladorCashlogy.Properties.Resources.Cashlogy_POS1500;
        }

        private void BtnOpenStacker_Click(object sender, EventArgs e)
        {
            if (stackerOpened)
            {
                stackerOpened = false;
                btnOpenStacker.IconChar = IconChar.DoorOpen;
                toolTipStacker.SetToolTip(btnOpenStacker, Idioma.FrasesIdioma[(int)NumFrase.OpenStack]);
                imgCashlogySim.Image = SimuladorCashlogy.Properties.Resources.Cashlogy_POS1500;
                int[] stacker = new int[CashlogyDevice.MAX_ITEMS];
                dev.SetStacker(stacker);
            }
            else
            {
                stackerOpened = true;
                btnOpenStacker.IconChar = IconChar.DoorClosed;
                toolTipStacker.SetToolTip(btnOpenStacker, Idioma.FrasesIdioma[(int)NumFrase.CloseStack]);
                imgCashlogySim.Image = SimuladorCashlogy.Properties.Resources.Cashlogy_POS1500_Opened;
            }
        }
        #endregion

        #region Metodos Formulario
        public void ActualizaCont()
        {
            int[] items = null;
            int capStacker = 0;
            dev.GetAviableCapacity(ref items, ref capStacker);

            int[] deposited = dev.GetDeposited();
            for (int i = 0; i < dev.Def.NumItems; i++)
            {
                int j = ObtainFormIndex(i);
                lblDeposited[j].Text = deposited[i].ToString();
                if (dev.Def.ItemsDef[i].IsDispensable && !dev.Def.ItemsDef[i].IsDepositable)
                {
                    numSelecc[j].Maximum = items[i];
                    if (items[i] == 0) dev.SetEnableItems(i, false);
                    else dev.SetEnableItems(i, true);
                }
                if (dev.Def.ItemsDef[i].IsDispensable && dev.Def.ItemsDef[i].IsDepositable)
                {
                    numSelecc[j].Maximum = items[i] + capStacker;
                    if (items[i] == 0 && capStacker == 0) dev.SetEnableItems(i, false);
                    else dev.SetEnableItems(i, true);
                }
                else if(!dev.Def.ItemsDef[i].IsDispensable && dev.Def.ItemsDef[i].IsDepositable)
                {
                    numSelecc[j].Maximum = capStacker;
                    if (capStacker == 0) dev.SetEnableItems(i, false);
                    else dev.SetEnableItems(i, true);
                }
            }
            HabilitaItems();

            float totCash = (float)dev.GetTotCash() / (float)dev.Def.Unitvalue;
            float totDeposited = (float)dev.GetTotDeposited() / (float)dev.Def.Unitvalue;
            float totDispensed = (float)dev.GetTotDispensed() / (float)dev.Def.Unitvalue;
            if (dev.Def.Symbol == "€")
            {
                txTotCash.Text = string.Format("{0} {1}", totCash.ToString("N2"), dev.Def.Symbol);
                txtTotAdmitido.Text = string.Format("{0} {1}", totDeposited.ToString("N2"), dev.Def.Symbol);
                txtTotDispensado.Text = string.Format("{0} {1}", totDispensed.ToString("N2"), dev.Def.Symbol);
            }
            else if (dev.Def.Symbol == "$")
            {
                txTotCash.Text = string.Format("{0} {1}", dev.Def.Symbol, totCash.ToString("N2"));
                txtTotAdmitido.Text = string.Format("{0} {1}", dev.Def.Symbol, totDeposited.ToString("N2"));
                txtTotDispensado.Text = string.Format("{0} {1}", dev.Def.Symbol, totDispensed.ToString("N2"));
            }
        }

        public void ActualizaParteVisual()
        {
            // Visibilidad
            for (int i = 0; i < CashlogyDevice.MAX_ITEMS; i++)
            {
                if ((i >= dev.Def.NumCoins && i < 8) || (i >= (8 + dev.Def.NumBills)))
                {
                    // Admision
                    btnDeposit[i].Visible = false;
                    numSelecc[i].Visible = false;
                    lblDeposited[i].Visible = false;
                }
                else
                {
                    // Admision
                    btnDeposit[i].Visible = true;
                    numSelecc[i].Visible = true;
                    lblDeposited[i].Visible = true;
                }
            }

            // Texto
            listSeleccCoins.SelectedItem = Idioma.FrasesIdioma[(int)NumFrase.Rellenar];
            listSeleccBills.SelectedItem = Idioma.FrasesIdioma[(int)NumFrase.Rellenar];

            // Imagenes
            for (int i = 0; i < CashlogyDevice.MAX_ITEMS; i++)
            {
                if (i < dev.Def.NumCoins || (i >= 8 && i < 8 + dev.Def.NumBills))
                {
                    int j = ObtainRealIndex(i);
                    string imgPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                                     dev.Def.ItemsDef[j].Image);
                    btnDeposit[i].BackgroundImage = Image.FromFile(imgPath);
                }
            }
        }

        public void ActualizaIdioma()
        {
            // Lista SeleccCoins
            listSeleccCoins.Items.Clear();
            listSeleccCoins.Items.AddRange(new object[] {
            Idioma.FrasesIdioma[(int)NumFrase.Rellenar],
            Idioma.FrasesIdioma[(int)NumFrase.OneCoin],
            Idioma.FrasesIdioma[(int)NumFrase.Aleatorio]});

            // Lista SeleccBills
            listSeleccBills.Items.Clear();
            listSeleccBills.Items.AddRange(new object[] {
            Idioma.FrasesIdioma[(int)NumFrase.Rellenar],
            Idioma.FrasesIdioma[(int)NumFrase.OneBill],
            Idioma.FrasesIdioma[(int)NumFrase.Aleatorio]});

            // Textos labels y botones
            lblAdmitido.Text = Idioma.FrasesIdioma[(int)NumFrase.Admitido] + " ..............................";
            lblDevuelto.Text = Idioma.FrasesIdioma[(int)NumFrase.Devuelto] + " ...........................";
            lblValorCash.Text = Idioma.FrasesIdioma[(int)NumFrase.ValEfectivo] + " ...........................";
            lblOpened.Text = Idioma.FrasesIdioma[(int)NumFrase.Abierto];
            lblReclamado.Text = Idioma.FrasesIdioma[(int)NumFrase.Reclamado];
            lblHabilitado.Text = Idioma.FrasesIdioma[(int)NumFrase.Habilitado];
            PanelEstado.Text = PanelEstado.Text = Idioma.FrasesIdioma[panel];
            lblAdmision.Text = Idioma.FrasesIdioma[(int)NumFrase.Admision];
            btnAddCoins.Text = Idioma.FrasesIdioma[(int)NumFrase.Add];
            btnAddBills.Text = Idioma.FrasesIdioma[(int)NumFrase.Add];
            btnForzarErr.Text = Idioma.FrasesIdioma[(int)NumFrase.ForzarErr];
            btnLimpiarErr.Text = Idioma.FrasesIdioma[(int)NumFrase.LimpiarErr];
            if (stackerOpened)
            {
                toolTipStacker.SetToolTip(btnOpenStacker, Idioma.FrasesIdioma[(int)NumFrase.CloseStack]);
            }
            else
            {
                toolTipStacker.SetToolTip(btnOpenStacker, Idioma.FrasesIdioma[(int)NumFrase.OpenStack]);
            }

            bool[] enabled = null;
            dev.GetEnableDepositItems(ref enabled);
            SetItemsToolTip(enabled);

            // Seleccion
            listSeleccCoins.SelectedItem = Idioma.FrasesIdioma[(int)NumFrase.Rellenar];
            listSeleccBills.SelectedItem = Idioma.FrasesIdioma[(int)NumFrase.Rellenar];
        }

        public int ObtainFormIndex(int a)
        {
            int b = a;
            if (dev.Def.NumCoins < 8 && a >= dev.Def.NumCoins) b = a + (8 - dev.Def.NumCoins);
            return b;
        }

        public int ObtainRealIndex(int a)
        {
            int b = a;
            if (dev.Def.NumCoins < 8 && a >= 8 && a < (8 + dev.Def.NumBills)) b = a - (8 - dev.Def.NumCoins);
            else if (a >= (8 + dev.Def.NumBills)) b = a - (16 - dev.Def.NumItems);
            return b;
        }
        #endregion

        private void SetItemsToolTip(bool[] enabled)
        {
            for (int i = 0; i < enabled.Length; i++)
            {
                int j = ObtainRealIndex(i);
                if (!enabled[i])
                {
                    toolTipItems.SetToolTip(btnDeposit[i], Idioma.FrasesIdioma[(int)NumFrase.Inhibido]);
                }
                else
                {
                    toolTipItems.SetToolTip(btnDeposit[i], dev.Def.ItemsDef[j].Name);
                }
            }
        }
    }
}
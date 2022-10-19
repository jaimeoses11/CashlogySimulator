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
    public partial class FormConfigCont : Form
    {
        CashlogyDevice dev;
        Idioma Idioma;
        MainFormSimulador sim;

        #region Variables
        public Label[] lblAlmacen;
        public Label[] lblStacker;

        // Configuracion
        public PictureBox[] imgCashConfig;
        public TextBox[] txtConfigA;
        public TextBox[] txtConfigS;
        #endregion

        public FormConfigCont(CashlogyDevice device, Idioma language, MainFormSimulador simulator)
        {
            ControlHelper.SuspendDrawing(this);

            InitializeComponent();
            dev = device;
            Idioma = language;
            sim = simulator;

            #region ListaElementos del FormConfiguracion"
            lblAlmacen = new Label[] { lblAlmacen1, lblAlmacen2, lblAlmacen3, lblAlmacen4 };
            lblStacker = new Label[] { lblStacker1, lblStacker2, lblStacker3, lblStacker4 };

            #region Contabilidad
            imgCashConfig = new PictureBox[] { imgCashConfig1, imgCashConfig2, imgCashConfig3, imgCashConfig4, imgCashConfig5, imgCashConfig6, imgCashConfig7, imgCashConfig8,
                                               imgCashConfig9, imgCashConfig10, imgCashConfig11, imgCashConfig12, imgCashConfig13, imgCashConfig14, imgCashConfig15, imgCashConfig16 };
            for (int i = 0; i < imgCashConfig.Length; i++)
            {
                imgCashConfig[i].Tag = i;
            }

            txtConfigA = new TextBox[] { txtConfigA1, txtConfigA2, txtConfigA3, txtConfigA4, txtConfigA5, txtConfigA6, txtConfigA7, txtConfigA8,
                                         txtConfigA9, txtConfigA10, txtConfigA11, txtConfigA12, txtConfigA13, txtConfigA14, txtConfigA15, txtConfigA16 };
            for (int i = 0; i < txtConfigA.Length; i++)
            {
                txtConfigA[i].Tag = i;
            }

            txtConfigS = new TextBox[] { txtConfigS1, txtConfigS2, txtConfigS3, txtConfigS4, txtConfigS5, txtConfigS6, txtConfigS7, txtConfigS8,
                                         txtConfigS9, txtConfigS10, txtConfigS11, txtConfigS12, txtConfigS13, txtConfigS14, txtConfigS15, txtConfigS16 };
            for (int i = 0; i < txtConfigS.Length; i++)
            {
                txtConfigS[i].Tag = i;
            }
            #endregion

            #endregion

            #region Suscripcion Eventos Form
            for (int i = 0; i < CashlogyDevice.MAX_ITEMS; i++)
            {
                this.txtConfigA[i].KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtConfig_KeyPress);
                this.txtConfigS[i].KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtConfig_KeyPress);
            }
            #endregion

            #region Suscripcion Eventos Cashlogy
            dev.OnViewChanged += this.ActualizaParteVisual;
            sim.OnLanguageChanged += this.ActualizaIdioma;
            #endregion

            ControlHelper.ResumeDrawing(this);
        }

        private void FormConfigCont_Load(object sender, EventArgs e)
        {
            ControlHelper.SuspendDrawing(this);
            ActualizaParteVisual();
            ActualizaConfig();
            ActualizaIdioma();
            ControlHelper.ResumeDrawing(this);
        }

        private void FormConfigCont_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Desuscripcion Eventos Cashlogy
            dev.OnViewChanged -= this.ActualizaParteVisual;
            sim.OnLanguageChanged -= this.ActualizaIdioma;
        }

        #region Configuracion Contabilidad
        private void BtnConfigCero_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < txtConfigA.Length; i++)
            {
                int j = ObtainRealIndex(i);
                if (dev.Def.ItemsDef[j].IsDispensable && !dev.Def.ItemsDef[j].IsDepositable)
                {
                    txtConfigA[i].Text = "0";
                    txtConfigS[i].Text = null;
                }
                else if (dev.Def.ItemsDef[j].IsDispensable && dev.Def.ItemsDef[j].IsDepositable)
                {
                    txtConfigA[i].Text = "0";
                    txtConfigS[i].Text = "0";
                }
                else if (!dev.Def.ItemsDef[j].IsDispensable && dev.Def.ItemsDef[j].IsDepositable)
                {
                    txtConfigA[i].Text = null;
                    txtConfigS[i].Text = "0";
                }
            }
        }

        private void BtnAceptConfig_Click(object sender, EventArgs e)
        {
            ExtraeConfig();

            dev.ResetDepositedCash();

            dev.Cont_OnChanged();
        }
        
        private void TxtConfig_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo dejar pasar numeros al textbox
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                e.Handled = true;
                return;
            }
        }
        #endregion

        #region Metodos Formulario
        public void ActualizaConfig()
        {
            int[] recyclers = dev.GetRecyclers();
            int[] stacker = dev.GetStacker();
            for (int i = 0; i < txtConfigA.Length; i++)
            {
                int j = ObtainRealIndex(i);
                if (dev.Def.ItemsDef[j].IsDispensable && !dev.Def.ItemsDef[j].IsDepositable)
                {
                    txtConfigA[i].Text = recyclers[j].ToString();
                    txtConfigS[i].Text = null;
                }
                else if (dev.Def.ItemsDef[j].IsDispensable && dev.Def.ItemsDef[j].IsDepositable)
                {
                    txtConfigA[i].Text = recyclers[j].ToString();
                    txtConfigS[i].Text = stacker[j].ToString();
                }
                else if (!dev.Def.ItemsDef[j].IsDispensable && dev.Def.ItemsDef[j].IsDepositable)
                {
                    txtConfigA[i].Text = null;
                    txtConfigS[i].Text = stacker[j].ToString();
                }
            }
        }

        public void ExtraeConfig()
        {
            int[] recyclers = new int[CashlogyDevice.MAX_ITEMS];
            int[] stacker = new int[CashlogyDevice.MAX_ITEMS];
            for (int i = 0; i < 8 + dev.Def.NumBills; i++)
            {
                int j = ObtainRealIndex(i);
                string r = txtConfigA[i].Text;
                string s = txtConfigS[i].Text;

                if (r == "") recyclers[j] = 0;
                else recyclers[j] = Convert.ToInt32(r);

                if (s == "") stacker[j] = 0;
                else stacker[j] = Convert.ToInt32(s);
            }

            dev.SetRecyclers(recyclers);
            dev.SetStacker(stacker);
        }

        public void ActualizaParteVisual()
        {
            // Visibilidad
            for (int i = 0; i < txtConfigA.Length; i++)
            {
                if ((i >= dev.Def.NumCoins && i < 8) || (i >= (8 + dev.Def.NumBills)))
                {
                    // Configuracion
                    imgCashConfig[i].Visible = false;
                    txtConfigA[i].Visible = false;
                    txtConfigS[i].Visible = false;
                }
                else
                {
                    // Configuracion
                    imgCashConfig[i].Visible = true;
                    txtConfigA[i].Visible = true;
                    txtConfigS[i].Visible = true;
                }

                int j = ObtainRealIndex(i);
                if (dev.Def.ItemsDef[j].IsDispensable) txtConfigA[i].ReadOnly = false;
                else txtConfigA[i].ReadOnly = true;

                if (dev.Def.ItemsDef[j].IsDepositable) txtConfigS[i].ReadOnly = false;
                else txtConfigS[i].ReadOnly = true;
            }

            // Imagenes
            for (int i = 0; i < imgCashConfig.Length; i++)
            {
                if (i < dev.Def.NumCoins || (i >= 8 && i < 8 + dev.Def.NumBills))
                {
                    int j = ObtainRealIndex(i);
                    string imgPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                                     dev.Def.ItemsDef[j].Image);
                    imgCashConfig[i].Image = Image.FromFile(imgPath);
                }
            }
        }

        public void ActualizaIdioma()
        {
            // Textos labels y botones
            for (int i = 0; i < lblAlmacen.Length; i++)
            {
                lblAlmacen[i].Text = Idioma.FrasesIdioma[(int)NumFrase.Almacen];
                lblStacker[i].Text = Idioma.FrasesIdioma[(int)NumFrase.Stacker];
            }

            PanelConfig.Text = Idioma.FrasesIdioma[(int)NumFrase.ConfigCont];
            btnConfigCero.Text = Idioma.FrasesIdioma[(int)NumFrase.PonerTodoA0];
        }

        public int ObtainRealIndex(int a)
        {
            int b = a;
            if (dev.Def.NumCoins < 8 && a >= 8 && a < (8 + dev.Def.NumBills)) b = a - (8 - dev.Def.NumCoins);
            else if (a >= (8 + dev.Def.NumBills)) b = a - (16 - dev.Def.NumItems);
            return b;
        }
        #endregion
    }
}
using Cashlogy;
using Cashlogy.Idiomas;
using Cashlogy.Vistas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace SimuladorCashlogy.Forms
{
    public partial class FormSimErrores : Form
    {
        CashlogyDevice dev;
        Idioma Idioma;
        MainFormSimulador sim;

        string[] lstErrorStr;
        ComboBox[] lstError;

        Errores errores;
        int[] auxerror;

        public FormSimErrores(CashlogyDevice device, Idioma language, MainFormSimulador simulator)
        {
            InitializeComponent();
            dev = device;
            Idioma = language;
            sim = simulator;

            #region ListasElementos
            lstError = new ComboBox[] { lstError1, lstError2, lstError3, lstError4, lstError5,
                                        lstError6, lstError7, lstError8, lstError9, lstError10 };
            for (int i = 0; i < lstError.Length; i++)
            {
                lstError[i].Tag = i;
                lstError[i].SelectionChangeCommitted += new System.EventHandler(LstErrors_SelectionChangeCommitted);
            }
            #endregion

            sim.OnLanguageChanged += ActualizaIdioma;
        }

        #region Eventos Form
        private void FormSimErrores_Load(object sender, System.EventArgs e)
        {
            auxerror = (int[])sim.error.Clone();
            ActualizaIdioma();
            ActualizaListErrorStr();
        }

        private void LstErrors_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            int i = (int)cb.Tag;
            if (lstError[i].Text == lstErrorStr[0])
            {
                auxerror[i] = 0;
                return;
            }

            var r = cb.Text.Split('-');
            int code;
            code = Convert.ToInt32(r[0]);
            auxerror[i] = code;
        }

        private void BtnConfigErr_Click(object sender, System.EventArgs e)
        {
            
            errores = new Errores();
            sim.errorsConfig = false;            

            Error error;
            for (int i = 0; i < auxerror.Length; i++)
            {
                if (auxerror[i] != 0)
                {
                    error = new Error();
                    error.Code = auxerror[i];
                    error.Severity = 200;
                    DateTime dt = DateTime.Now;
                    error.DateTime = dt.Year + "-" + dt.Month + "-" + dt.Day + "T" + dt.Hour + ":" + 
                                     dt.Minute + ":" + dt.Second;
                    error.Module = 10;
                    error.Description = ErrorCodeToStr(auxerror[i]).Split('-')[1];

                    errores.list.Add(error);
                    sim.errorsConfig = true;
                }
            }
            dev.SetReadStatusErrors(errores);
            sim.error = (int[])auxerror.Clone();
        }
        #endregion

        private void ActualizaIdioma()
        {
            lblConfigErrores.Text = Idioma.FrasesIdioma[(int)NumFrase.ConfiguracionErr];
            ActualizaListErrorStr();
        }

        private void ActualizaListErrorStr()
        {
            GetListErrorStr();
            for (int i = 0; i < lstError.Length; i++)
            {
                lstError[i].Items.Clear();
                lstError[i].Items.AddRange(lstErrorStr);
                lstError[i].SelectedItem = ErrorCodeToStr(auxerror[i]);
            }
        }

        private void GetListErrorStr()
        {
            List<string> list = new List<string>();
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                                       @"Errores\ListaErrores" + Idioma.FrasesIdioma[(int)NumFrase.Actual] + ".txt");

            StreamReader txt = new StreamReader(path);
            string str = txt.ReadLine();
            list.Add(str);
            while (str != null)
            {
                str = txt.ReadLine();
                if (str != null) list.Add(str);
            }

            lstErrorStr = list.ToArray();
        }

        private string ErrorCodeToStr(int code)
        {
            if (code == 0) return lstErrorStr[0];

            bool codeAppears;
            for (int i = 0; i < lstErrorStr.Length; i++)
            {
                codeAppears = lstErrorStr[i].Contains(code.ToString() + "-");
                if (codeAppears) return lstErrorStr[i];
            }

            return lstErrorStr[0];
        }
    }
}

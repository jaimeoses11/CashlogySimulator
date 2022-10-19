using Cashlogy;
using Cashlogy.Idiomas;
using Cashlogy.Vistas;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SimuladorCashlogy.Forms
{
    public partial class FormSettings : Form
    {
        CashlogyDevice dev;
        Idioma Idioma;
        MainFormSimulador sim;
        Config cfg;

        string[] devices;

        public FormSettings(CashlogyDevice device, Idioma language, MainFormSimulador simulator, Config config)
        {
            InitializeComponent();

            dev = device;
            Idioma = language;
            sim = simulator;
            cfg = config;

            sim.OnLanguageChanged += ActualizaIdioma;
        }

        #region Eventos Form
        private void FormSettings_Load(object sender, EventArgs e)
        {
            ActualizaIdioma();
            string host;
            int depCoin, depBill, dispCoin, dispBill, port;
            bool inCadence, outCadence;
            dev.GetConfig(out inCadence, out depCoin, out depBill, out outCadence, out dispCoin, out dispBill, out host, out port);
            txtHost.Text = host;
            txtPort.Text = port.ToString();

            #region Cadencia
            CadenciaEntrada.Checked = inCadence;
            if (CadenciaEntrada.Checked)
            {
                txtEntradaMonedas.Enabled = true;
                txtEntradaBilletes.Enabled = true;
            }
            else
            {
                txtEntradaMonedas.Enabled = false;
                txtEntradaBilletes.Enabled = false;
            }

            CadenciaSalida.Checked = outCadence;
            if (CadenciaSalida.Checked)
            {
                txtSalidaMonedas.Enabled = true;
                txtSalidaBilletes.Enabled = true;
            }
            else
            {
                txtSalidaMonedas.Enabled = false;
                txtSalidaBilletes.Enabled = false;
            }
            #endregion

            txtEntradaMonedas.Text = depCoin.ToString();
            txtEntradaBilletes.Text = depBill.ToString();
            txtSalidaMonedas.Text = dispCoin.ToString();
            txtSalidaBilletes.Text = dispBill.ToString();

            if (cfg.isRun)
            {
                lblModoRun.Visible = true;
                panelRegistro.Enabled = false;
                panelServer.Enabled = false;
            }
            else
            {
                lblModoRun.Visible = false;
                panelRegistro.Enabled = true;
                panelServer.Enabled = true;
            }

            EnableServerConfig.Checked = false;
            lblHost.Enabled = false;
            txtHost.Enabled = false;
            lblPort.Enabled = false;
            txtPort.Enabled = false;

            devices = GetDevices();
            enableSim.Checked = GetSimEnabled(devices);
            txtDevices.Text = "";
            for (int i = 0; i < devices.Length; i++)
            {
                txtDevices.Text += devices[i] +"\r\n";
            }
        }

        private void CadenciaEntrada_CheckedChanged(object sender, EventArgs e)
        {
            if (CadenciaEntrada.Checked)
            {
                txtEntradaMonedas.Enabled = true;
                txtEntradaBilletes.Enabled = true;
            }
            else
            {
                txtEntradaMonedas.Enabled = false;
                txtEntradaBilletes.Enabled = false;
            }
        }

        private void CadenciaSalida_CheckedChanged(object sender, EventArgs e)
        {
            if (CadenciaSalida.Checked)
            {
                txtSalidaMonedas.Enabled = true;
                txtSalidaBilletes.Enabled = true;
            }
            else
            {
                txtSalidaMonedas.Enabled = false;
                txtSalidaBilletes.Enabled = false;
            }
        }

        private void EnableServerConfig_CheckedChanged(object sender, EventArgs e)
        {
            if (EnableServerConfig.Checked)
            {
                lblHost.Enabled = true;
                txtHost.Enabled = true;
                lblPort.Enabled = true;
                txtPort.Enabled = true;
            }
            else
            {
                lblHost.Enabled = false;
                txtHost.Enabled = false;
                lblPort.Enabled = false;
                txtPort.Enabled = false;
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo dejar pasar numeros al textbox
            if ((e.KeyChar >= 32 && e.KeyChar <= 47) || (e.KeyChar >= 58 && e.KeyChar <= 255))
            {
                e.Handled = true;
                return;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            bool error = false;
            int depCoin = 0, depBill = 0;
            if (CadenciaEntrada.Checked)
            {
                depCoin = Convert.ToInt32(txtEntradaMonedas.Text);
                depBill = Convert.ToInt32(txtEntradaBilletes.Text);
                if (depCoin <= 0)
                {
                    errInCoin.SetError(txtEntradaMonedas, Idioma.FrasesIdioma[(int)NumFrase.CondInCad]);
                    error = true;
                }
                else errInCoin.Clear();
                if (depBill <= 0)
                {
                    errInBill.SetError(txtEntradaBilletes, Idioma.FrasesIdioma[(int)NumFrase.CondInCad]);
                    error = true;
                }
                else errInBill.Clear();
            }

            int dispCoin = 0, dispBill = 0;
            if (CadenciaSalida.Checked)
            {
                dispCoin = Convert.ToInt32(txtSalidaMonedas.Text);
                dispBill = Convert.ToInt32(txtSalidaBilletes.Text);
                if (dispCoin < 133)
                {
                    errOutCoin.SetError(txtSalidaMonedas, Idioma.FrasesIdioma[(int)NumFrase.CondOutCad]);
                    error = true;
                }
                else errOutCoin.Clear();
                if (dispBill < 133)
                {
                    errOutBill.SetError(txtSalidaBilletes, Idioma.FrasesIdioma[(int)NumFrase.CondOutCad]);
                    error = true;
                }
                else errOutBill.Clear();
            }

            bool simHooked = enableSim.Checked;

            string host = txtHost.Text;
            int port = Convert.ToInt32(txtPort.Text);
            bool changeServer = EnableServerConfig.Checked;
            if (!error)
            {
                dev.GuardarConfig(depCoin, depBill, dispCoin, dispBill, simHooked, devices, changeServer, host, port);
            }
        }
        #endregion

        private void ActualizaIdioma()
        {           
            lblConfig.Text = Idioma.FrasesIdioma[(int)NumFrase.Config];
            lblEnableInCad.Text = Idioma.FrasesIdioma[(int)NumFrase.EnableInCad];
            lblEntradaMonedas.Text = Idioma.FrasesIdioma[(int)NumFrase.InCoins_ms];
            lblEntradaBilletes.Text = Idioma.FrasesIdioma[(int)NumFrase.InBills_ms];
            lblEnableOutCad.Text = Idioma.FrasesIdioma[(int)NumFrase.EnableOutCad];
            lblSalidaMonedas.Text = Idioma.FrasesIdioma[(int)NumFrase.OutCoins_ms];
            lblSalidaBilletes.Text = Idioma.FrasesIdioma[(int)NumFrase.OutBills_ms];

            lblModoRun.Text = Idioma.FrasesIdioma[(int)NumFrase.ModoRun];
            panelRegistro.Text = Idioma.FrasesIdioma[(int)NumFrase.ConfigRegistro];
            lblEnableSim.Text = Idioma.FrasesIdioma[(int)NumFrase.EnableSim];
            lblDipsList.Text = Idioma.FrasesIdioma[(int)NumFrase.DispList];

            panelServer.Text = Idioma.FrasesIdioma[(int)NumFrase.ConfigServer];
            lblEnableServerConfig.Text = Idioma.FrasesIdioma[(int)NumFrase.EnableServerConfig];
                        
            errInCoin.Clear();
            errInBill.Clear();
            errOutCoin.Clear();
            errOutBill.Clear();
        }

        private bool GetSimEnabled(string[] devices)
        {
            bool enabled = true;
            string rootKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\OLEforRetail\ServiceOPOS\CashChanger\";
            string simulator = "AzkoyenOPOSCashChanger.CashlogySimulator";
            string value;
            for (int i = 0; i < devices.Length; i++)
            {
                value = (string)Registry.GetValue(rootKey + devices[i], "", "NotFound");
                if (value != simulator)
                {
                    enabled = false;
                    break;
                }
            }

            return enabled;
        }

        private string[] GetDevices()
        {
            List<string> devices = new List<string>();

            string rootKey = @"SOFTWARE\WOW6432Node\OLEforRetail\ServiceOPOS\CashChanger";
            RegistryKey cashChanger = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                                            RegistryView.Registry64).OpenSubKey(rootKey);
            string[] subKeys = cashChanger.GetSubKeyNames();
            for (int i = 0; i < subKeys.Length; i++)
            {
                if (subKeys[i] != "CashlogySimulator") devices.Add(subKeys[i]);
            }
            return devices.ToArray();
        }
    }
}
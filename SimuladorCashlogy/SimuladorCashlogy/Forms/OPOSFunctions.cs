using Cashlogy;
using System;
using System.Windows.Forms;

namespace Cashlogy.Vistas
{
    public partial class OPOSFunctions : Form
    {
        CashlogyDevice dev;

        public OPOSFunctions(CashlogyDevice d)
        {
            InitializeComponent();
            dev = d;
        }

        #region Eventos Form
        private void OPOSFunctions_Load(object sender, EventArgs e)
        {
            lstPauseDeposit.SelectedItem = "Control ...";
            lstEndDeposit.SelectedItem = "Success ...";
            lstGetProp.SelectedItem = "Selecc Property ...";
            lstSetProp.SelectedItem = "Selecc Property ...";
            lstEnable.SelectedItem = "true";
            lstDirectIO.SelectedItem = "Selecc DirectIO ...";
        }

        private void BtnOpen_Click(object sender, EventArgs e)
        {
            int r = dev.Open(txtOpen1.Text);
            SetResult(r);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            int r = dev.Close();
            SetResult(r);
        }

        private void BtnClaim_Click(object sender, EventArgs e)
        {
            int r = dev.Claim(Convert.ToInt32(txtClaim1.Text));
            SetResult(r);
        }

        private void BtnEnable_Click(object sender, EventArgs e)
        {
            bool en = false;
            if (lstEnable.Text == "true") en = true;
            else if (lstEnable.Text == "false") en = false;
            int r = dev.SetPropertyBool(Const.PIDX_DeviceEnabled, en);
            SetResult(r);
        }

        private void BtnRelease_Click(object sender, EventArgs e)
        {
            int r = dev.Release();
            SetResult(r);
        }

        private void BtnBeginDeposit_Click(object sender, EventArgs e)
        {
            int r = dev.BeginDeposit();
            SetResult(r);
        }

        private void BtnFixDeposit_Click(object sender, EventArgs e)
        {
            int r = dev.FixDeposit();
            txtDepositCounts.Text = dev.GetDepositCounts();
            txtDepositAmount.Text = Convert.ToString(dev.GetTotDeposited());
            SetResult(r);
        }

        private void BtnPauseDeposit_Click(object sender, EventArgs e)
        {
            int control = 0;
            switch (lstPauseDeposit.Text)
            {
                case "PAUSE":
                    control = Const.CHAN_DEPOSIT_PAUSE;
                    break;
                case "RESTART":
                    control = Const.CHAN_DEPOSIT_RESTART;
                    break;
                case "Control ...":
                    MessageBox.Show("Selccione el parametro Control");
                    return;
            }
            int r = dev.PauseDeposit(control);
            SetResult(r);
        }

        private void BtnEndDeposit_Click(object sender, EventArgs e)
        {
            int success = 0;
            switch (lstEndDeposit.Text)
            {
                case "CHANGE":
                    success = Const.CHAN_DEPOSIT_CHANGE;
                    break;
                case "NOCHANGE":
                    success = Const.CHAN_DEPOSIT_NOCHANGE;
                    break;
                case "REPAY":
                    success = Const.CHAN_DEPOSIT_REPAY;
                    break;
                case "Success ...":
                    MessageBox.Show("Selccione el parametro Succes");
                    return;
            }
            int r = dev.EndDeposit(success);
            SetResult(r);
        }

        private void BtnDispenseCash_Click(object sender, EventArgs e)
        {
            int r = dev.DispenseCash(txtDispenseCash.Text);
            SetResult(r);
        }

        private void BtnDispenseChange_Click(object sender, EventArgs e)
        {
            if (txtDispenseChange.Text == "") return;
            int r = dev.DispenseChange(Convert.ToInt32(txtDispenseChange.Text));
            SetResult(r);
        }

        private void BtnAdjustCashCounts_Click(object sender, EventArgs e)
        { 
            int r = dev.AdjustCashCounts(txtAdjustCashCounts.Text);
            SetResult(r);
        }

        private void BtnReadCashCounts_Click(object sender, EventArgs e)
        {
            string s = "";
            int r = dev.ReadCashCounts(ref s);
            txtReadCashCounts.Text = s;
            SetResult(r);
        }

        private void LstGetProp_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (lstGetProp.Text == "Selecc Property ...") return;

            int propIndex = -1;
            int type = -1;
            int r = Const.OPOS_E_FAILURE;
            ObtainOPOSIndex(lstGetProp.Text, ref propIndex, ref type);

            int num = -1; string str = ""; bool b = false;
            switch (type)
            {
                case 0:
                    r = dev.GetPropertyInt(propIndex, ref num);
                    txtGetProp.Text = Convert.ToString(num);
                    break;
                case 1:
                    r = dev.GetPropertyBool(propIndex, ref b);
                    if (b) txtGetProp.Text = "True";
                    else txtGetProp.Text = "False";
                    break;
                case 2:
                    r = dev.GetPropertyStr(propIndex, ref str);
                    txtGetProp.Text = str;
                    break;
                default:
                    txtGetProp.Text = "";
                    break;
            }
            SetResult(r);
        }

        private void LstSetProp_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (lstSetProp.Text == "Selecc Property ...") return;

            int propIndex = -1;
            int type = -1;
            int r = Const.OPOS_E_FAILURE;
            ObtainOPOSIndex(lstSetProp.Text, ref propIndex, ref type);

            int num = -1; string str = ""; bool b = false;
            switch (type)
            {
                case 0:
                    r = dev.GetPropertyInt(propIndex, ref num);
                    txtSetProp.Text = Convert.ToString(num);
                    break;
                case 1:
                    r = dev.GetPropertyBool(propIndex, ref b);
                    if (b) txtSetProp.Text = "True";
                    else txtSetProp.Text = "False";
                    break;
                case 2:
                    r = dev.GetPropertyStr(propIndex, ref str);
                    txtSetProp.Text = str;
                    break;
                default:
                    txtSetProp.Text = "";
                    break;
            }
            SetResult(r);
        }

        private void BtnSetProp_Click(object sender, EventArgs e)
        {
            int propIndex = -1;
            int type = -1;
            int r = Const.OPOS_E_FAILURE;
            ObtainOPOSIndex(lstSetProp.Text, ref propIndex, ref type);

            int num; string str; bool b;
            switch (type)
            {
                case 0:
                    num = Convert.ToInt32(txtSetProp.Text);
                    r = dev.SetPropertyInt(propIndex, num);
                    break;
                case 1:
                    if (txtSetProp.Text == "true" || txtSetProp.Text == "True") b = true;
                    else if (txtSetProp.Text == "false" || txtSetProp.Text == "False") b = false;
                    else break;
                    r = dev.SetPropertyBool(propIndex, b);
                    break;
                case 2:
                    str = txtSetProp.Text;
                    r = dev.SetPropertyStr(propIndex, str);
                    break;
            }
            SetResult(r);
        }

        private void LstDirectIO_SelectionChangeCommitted(object sender, EventArgs e)
        {
            txtpData.Text = "0";
            txtpString.Text = "";
        }

        private void BtnDirectIO_Click(object sender, EventArgs e)
        {
            if (lstDirectIO.Text == "Selecc DirectIO ...")
            {
                MessageBox.Show("Selecciona DirectIO");
                return;
            }

            var s = lstDirectIO.Text.Split(' ');
            int command = Convert.ToInt32(s[0]);
            int pData = Convert.ToInt32(txtpData.Text);
            string pString = txtpString.Text;
            int r = dev.DirectIO(command, ref pData, ref pString);

            txtpData.Text = Convert.ToString(pData);
            txtpString.Text = pString;

            SetResult(r);
        }
        #endregion

        public void ObtainOPOSIndex(string prop, ref int propIndex, ref int type)
        {
            switch (prop)
            {
                case "ResultCode":
                    propIndex = Const.PIDX_ResultCode;
                    type = 0; // 0-int, 1-bool, 2-string
                    break;
                case "ResultCodeExtended":
                    propIndex = Const.PIDX_ResultCodeExtended;
                    type = 0; // 0-int, 1-bool, 2-string
                    break;
                case "State":
                    propIndex = Const.PIDX_State;
                    type = 0; // 0-int, 1-bool, 2-string
                    break;
                case "BinaryConversion":
                    propIndex = Const.PIDX_BinaryConversion;
                    type = 0; // 0-int, 1-bool, 2-string
                    break;
                case "CapPowerReporting":
                    propIndex = Const.PIDX_CapPowerReporting;
                    type = 0; // 0-int, 1-bool, 2-string
                    break;
                case "DataCount":
                    propIndex = Const.PIDX_DataCount;
                    type = 0; // 0-int, 1-bool, 2-string
                    break;
                case "PowerNotify":
                    propIndex = Const.PIDX_PowerNotify;
                    type = 0; // 0-int, 1-bool, 2-string
                    break;
                case "PowerState":
                    propIndex = Const.PIDX_PowerState;
                    type = 0; // 0-int, 1-bool, 2-string
                    break;
                case "ServiceObjectVersion":
                    propIndex = Const.PIDX_ServiceObjectVersion;
                    type = 0; // 0-int, 1-bool, 2-string
                    break;
                case "AsyncResultCode":
                    propIndex = Const.PIDXChan_AsyncResultCode;
                    type = 0; // 0-int, 1-bool, 2-string
                    break;
                case "AsyncResultCodeExtended":
                    propIndex = Const.PIDXChan_AsyncResultCodeExtended;
                    type = 0; // 0-int, 1-bool, 2-string
                    break;
                case "CurrentExit":
                    propIndex = Const.PIDXChan_CurrentExit;
                    type = 0; // 0-int, 1-bool, 2-string
                    break;
                case "CurrentService":
                    propIndex = Const.PIDXChan_CurrentService;
                    type = 0; // 0-int, 1-bool, 2-string
                    break;
                case "DepositAmount":
                    propIndex = Const.PIDXChan_DepositAmount;
                    type = 0; // 0-int, 1-bool, 2-string
                    break;
                case "DepositStatus":
                    propIndex = Const.PIDXChan_DepositStatus;
                    type = 0; // 0-int, 1-bool, 2-string
                    break;
                case "DeviceExits":
                    propIndex = Const.PIDXChan_AsyncMode;
                    type = 0; // 0-int, 1-bool, 2-string
                    break;
                case "DeviceStatus":
                    propIndex = Const.PIDXChan_DeviceStatus;
                    type = 0; // 0-int, 1-bool, 2-string
                    break;
                case "FullStatus":
                    propIndex = Const.PIDXChan_FullStatus;
                    type = 0; // 0-int, 1-bool, 2-string
                    break;
                case "ServiceCount":
                    propIndex = Const.PIDXChan_ServiceCount;
                    type = 0; // 0-int, 1-bool, 2-string
                    break;
                case "ServiceIndex":
                    propIndex = Const.PIDXChan_ServiceIndex;
                    type = 0; // 0-int, 1-bool, 2-string
                    break;
                case "CapCompareFirmwareVersion":
                    propIndex = Const.PIDX_CapCompareFirmwareVersion;
                    type = 1; // 0-int, 1-bool, 2-string
                    break;
                case "CapStatisticsReporting":
                    propIndex = Const.PIDX_CapStatisticsReporting;
                    type = 1; // 0-int, 1-bool, 2-string
                    break;
                case "CapUpdateFirmware":
                    propIndex = Const.PIDX_CapUpdateFirmware;
                    type = 1; // 0-int, 1-bool, 2-string
                    break;
                case "CapUpdateStatistics":
                    propIndex = Const.PIDX_CapUpdateStatistics;
                    type = 1; // 0-int, 1-bool, 2-string
                    break;
                case "Claimed":
                    propIndex = Const.PIDX_Claimed;
                    type = 1; // 0-int, 1-bool, 2-string
                    break;
                case "DataEventEnabled":
                    propIndex = Const.PIDX_DataEventEnabled;
                    type = 1; // 0-int, 1-bool, 2-string
                    break;
                case "DeviceEnabled":
                    propIndex = Const.PIDX_DeviceEnabled;
                    type = 1; // 0-int, 1-bool, 2-string
                    break;
                case "FreezeEvents":
                    propIndex = Const.PIDX_FreezeEvents;
                    type = 1; // 0-int, 1-bool, 2-string
                    break;
                case "CapDeposit":
                    propIndex = Const.PIDXChan_CapDeposit;
                    type = 1; // 0-int, 1-bool, 2-string
                    break;
                case "CapDepositDataEvent":
                    propIndex = Const.PIDXChan_CapDepositDataEvent;
                    type = 1; // 0-int, 1-bool, 2-string
                    break;
                case "CapDiscrepancy":
                    propIndex = Const.PIDXChan_CapDiscrepancy;
                    type = 1; // 0-int, 1-bool, 2-string
                    break;
                case "CapEmptySensor":
                    propIndex = Const.PIDXChan_CapEmptySensor;
                    type = 1; // 0-int, 1-bool, 2-string
                    break;
                case "CapFullSensor":
                    propIndex = Const.PIDXChan_CapFullSensor;
                    type = 1; // 0-int, 1-bool, 2-string
                    break;
                case "CapJamSensor":
                    propIndex = Const.PIDXChan_CapJamSensor;
                    type = 1; // 0-int, 1-bool, 2-string
                    break;
                case "CapNearEmptySensor":
                    propIndex = Const.PIDXChan_CapNearEmptySensor;
                    type = 1; // 0-int, 1-bool, 2-string
                    break;
                case "CapNearFullSensor":
                    propIndex = Const.PIDXChan_CapNearFullSensor;
                    type = 1; // 0-int, 1-bool, 2-string
                    break;
                case "CapPauseDeposit":
                    propIndex = Const.PIDXChan_CapPauseDeposit;
                    type = 1; // 0-int, 1-bool, 2-string
                    break;
                case "CapRealTimeData":
                    propIndex = Const.PIDXChan_CapRealTimeData;
                    type = 1; // 0-int, 1-bool, 2-string
                    break;
                case "CapRepayDeposit":
                    propIndex = Const.PIDXChan_CapRepayDeposit;
                    type = 1; // 0-int, 1-bool, 2-string
                    break;
                case "AsyncMode":
                    propIndex = Const.PIDXChan_AsyncMode;
                    type = 1; // 0-int, 1-bool, 2-string
                    break;
                case "RealTimeDataEnabled":
                    propIndex = Const.PIDXChan_RealTimeDataEnabled;
                    type = 1; // 0-int, 1-bool, 2-string
                    break;
                case "CheckHealthText":
                    propIndex = Const.PIDX_CheckHealthText;
                    type = 2; // 0-int, 1-bool, 2-string
                    break;
                case "ServiceObjectDescription":
                    propIndex = Const.PIDX_ServiceObjectDescription;
                    type = 2; // 0-int, 1-bool, 2-string
                    break;
                case "DeviceDescription":
                    propIndex = Const.PIDX_DeviceDescription;
                    type = 2; // 0-int, 1-bool, 2-string
                    break;
                case "DeviceName":
                    propIndex = Const.PIDX_DeviceName;
                    type = 2; // 0-int, 1-bool, 2-string
                    break;
                case "CurrencyCashList":
                    propIndex = Const.PIDXChan_CurrencyCashList;
                    type = 2; // 0-int, 1-bool, 2-string
                    break;
                case "CurrencyCode":
                    propIndex = Const.PIDXChan_CurrencyCode;
                    type = 2; // 0-int, 1-bool, 2-string
                    break;
                case "CurrencyCodeList":
                    propIndex = Const.PIDXChan_CurrencyCodeList;
                    type = 2; // 0-int, 1-bool, 2-string
                    break;
                case "DepositCashList":
                    propIndex = Const.PIDXChan_DepositCashList;
                    type = 2; // 0-int, 1-bool, 2-string
                    break;
                case "DepositCodeList":
                    propIndex = Const.PIDXChan_DepositCodeList;
                    type = 2; // 0-int, 1-bool, 2-string
                    break;
                case "DepositCounts":
                    propIndex = Const.PIDXChan_DepositCounts;
                    type = 2; // 0-int, 1-bool, 2-string
                    break;
                case "ExitCashList":
                    propIndex = Const.PIDXChan_ExitCashList;
                    type = 2; // 0-int, 1-bool, 2-string
                    break;
            }
        }

        public void SetResult(int r)
        {
            int rc = -1;
            dev.GetPropertyInt(Const.PIDX_ResultCode, ref rc);
            txtReturnValue.Text = IntRCToString(r);
            txtResultCode.Text = IntRCToString(rc);
        }

        public string IntRCToString(int rc)
        {
            switch (rc)
            {
                case Const.OPOS_SUCCESS:
                    return "OPOS_SUCCESS";
                case Const.OPOS_E_CLOSED:
                    return "OPOS_E_CLOSED";
                case Const.OPOS_E_CLAIMED:
                    return "OPOS_E_CLAIMED";
                case Const.OPOS_E_NOTCLAIMED:
                    return "OPOS_E_NOTCLAIMED";
                case Const.OPOS_E_NOSERVICE:
                    return "OPOS_E_NOSERVICE";
                case Const.OPOS_E_DISABLED:
                    return "OPOS_E_DISABLED";
                case Const.OPOS_E_ILLEGAL:
                    return "OPOS_E_ILLEGAL";
                case Const.OPOS_E_NOHARDWARE:
                    return "OPOS_E_NOHARDWARE";
                case Const.OPOS_E_OFFLINE:
                    return "OPOS_E_OFFLINE";
                case Const.OPOS_E_NOEXIST:
                    return "OPOS_E_NOEXIST";
                case Const.OPOS_E_EXISTS:
                    return "OPOS_E_EXISTS";
                case Const.OPOS_E_FAILURE:
                    return "OPOS_E_FAILURE";
                case Const.OPOS_E_TIMEOUT:
                    return "OPOS_E_TIMEOUT";
                case Const.OPOS_E_BUSY:
                    return "OPOS_E_BUSY";
                case Const.OPOS_E_EXTENDED:
                    return "OPOS_E_EXTENDED";
                case Const.OPOS_E_DEPRECATED:
                    return "OPOS_E_DEPRECATED";
                case Const.OPOSERR:
                    return "OPOSERR";
                case Const.OPOSERREXT:
                    return "OPOSERREXT";
                default:
                    return "";
            }
        }
    }
}
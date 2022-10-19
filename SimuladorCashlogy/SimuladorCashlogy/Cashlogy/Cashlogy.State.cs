namespace Cashlogy
{
    public partial class CashlogyDevice
    {
        class CashlogyState
        {            
            #region Atributos Estado
            public bool Opened;
            public int OpenResult;

            public bool Claimed;
            public bool DeviceEnabled;

            public bool Busy;

            public bool DepositInCurse;
            public bool DepositInCursePaused;
            public bool DepositInCurseEnded;
            public bool DepositInCurseFixDepositCalled;
            public int DepositStatus;

            public bool DispenseInCurse;
            public int DispenseStatus;

            public int DispenseInCurseAmountRequired;
            public int[] DispenseInCurseCountsRequired;

            public int State;

            public int ResultCode;
            public int ResultCodeExtended;

            public string CheckHealthText;

            public int DeviceStatus;
            public int FullStatus;

            public int[] CashEmptyFullStatus;
            public int StackerEmptyFullStatus;

            public bool[] EnableDepositItems;

            public bool AsyncMode;

            public int AsyncResultCode;
            public int AsyncResultCodeExtended;
            #endregion

            public CashlogyState()
            {
                Opened = false;
                OpenResult = Const.OPOS_E_CLOSED;

                Claimed = false;
                DeviceEnabled = false;

                Busy = false;

                DepositInCurse = false;
                DepositInCursePaused = false;
                DepositInCurseEnded = false;
                DepositInCurseFixDepositCalled = false;
                DepositStatus = Const.CHAN_STATUS_DEPOSIT_END;

                DispenseInCurse = false;
                DispenseStatus = Const.CHAN_STATUS_DISPENSE_END;

                DispenseInCurseAmountRequired = 0;
                DispenseInCurseCountsRequired = new int[MAX_ITEMS];

                State = Const.OPOS_S_CLOSED;

                ResultCode = Const.OPOS_E_CLOSED;
                ResultCodeExtended = 0;
                CheckHealthText = "";

                DeviceStatus = Const.CHAN_STATUS_OK;
                FullStatus = Const.CHAN_STATUS_OK;

                CashEmptyFullStatus = new int[MAX_ITEMS];
                StackerEmptyFullStatus = Const.CHAN_STATUS_OK;

                EnableDepositItems = new bool[MAX_ITEMS];

                AsyncMode = false;

                AsyncResultCode = 0; //revisar
                AsyncResultCodeExtended = 0; // revisar
            }

            public void GetState(ref bool opened, ref bool claimed, ref bool deviceEnable)
            {
                opened = this.Opened;
                claimed = this.Claimed;
                deviceEnable = this.DeviceEnabled;
            }

            public void GetState(ref int state)
            {
                state = this.State;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Timers;
using Cashlogy.Idiomas;

namespace Cashlogy
{
    public partial class CashlogyDevice
    {
        #region Propiedades
        // Common
        private int dataCount = 0; //@ToDo
        private bool dataEventEnabled = false; //@ToDo
        private bool freezeEvents = true; //@ToDo

        private string serviceObjectDescription;
        private int serviceObjectVersion;
        private string deviceDescription;

        // Specific
        private string currencyCashList;
        private string currencyCode;
        private string currencyCodeList;
        private string depositCashList;
        private string depositCodeList;
        private string exitCashList;
        #endregion

        #region Get/Set Property
        // Datacount, dataeventenabled, freezeevents
        private int GetProperty(int propIndex, ref int n)
        {
            switch (propIndex)
            {
                case Const.PIDX_OpenResult:
                    int r = GetOpenRestult(ref n);
                    return SetRC(r);

                case Const.PIDX_ResultCode:
                    n = st.ResultCode;
                    return SetRC(Const.OPOS_SUCCESS);

                case Const.PIDX_ResultCodeExtended:
                    n = st.ResultCodeExtended;
                    return SetRC(Const.OPOS_SUCCESS);

                case Const.PIDX_State:
                    n = st.State;
                    return SetRC(Const.OPOS_SUCCESS);
            }

            n = 0;

            if (!st.Opened) return SetRC(Const.OPOS_E_CLOSED);

            switch (propIndex)
            {
                case Const.PIDX_BinaryConversion:
                    n = Const.OPOS_BC_NONE;
                    break;

                case Const.PIDX_CapPowerReporting:
                    n = Const.OPOS_PR_NONE;
                    break;

                case Const.PIDX_DataCount:
                    n = dataCount;
                    break;

                case Const.PIDX_PowerNotify:
                    n = Const.OPOS_PN_DISABLED;
                    break;

                case Const.PIDX_PowerState:
                    n = Const.OPOS_PS_UNKNOWN;
                    break;

                case Const.PIDX_ServiceObjectVersion:
                    n = serviceObjectVersion;
                    break;

                // Específicas
                case Const.PIDXChan_AsyncResultCode:
                    // Sólo si 'enable'
                    if (st.DeviceEnabled) n = st.AsyncResultCode;
                    break;

                case Const.PIDXChan_AsyncResultCodeExtended:
                    // Sólo si 'enable'
                    if (st.DeviceEnabled) n = st.AsyncResultCodeExtended;
                    break;

                case Const.PIDXChan_CurrentExit:
                    n = 1;
                    break;

                case Const.PIDXChan_CurrentService:
                    n = 0;
                    break;

                case Const.PIDXChan_DepositAmount:
                    n = cont.totDeposited;
                    break;

                case Const.PIDXChan_DepositStatus:
                    // Sólo si 'enable'
                    if (st.DeviceEnabled) n = st.DepositStatus;
                    break;

                case Const.PIDXChan_DeviceExits:
                    n = 1;
                    break;

                case Const.PIDXChan_DeviceStatus:
                    // Sólo si 'enable'
                    if (st.DeviceEnabled) n = st.DeviceStatus;
                    break;

                case Const.PIDXChan_FullStatus:
                    // Sólo si 'enable'
                    if (st.DeviceEnabled) n = st.FullStatus;
                    break;

                case Const.PIDXChan_ServiceCount:
                    n = 0;
                    break;

                case Const.PIDXChan_ServiceIndex:
                    n = 0;
                    break;

                default:
                    return SetRC(Const.OPOS_E_FAILURE);
            }

            return SetRC(Const.OPOS_SUCCESS);
        }
        private int GetProperty(int propIndex, ref bool b)
        {
            b = false;

            if (!st.Opened) return SetRC(Const.OPOS_E_CLOSED);

            switch (propIndex)
            {
                // Comunes OPOS
                case Const.PIDX_CapCompareFirmwareVersion:
                    b = false;
                    break;

                case Const.PIDX_CapStatisticsReporting:
                    b = true;
                    break;

                case Const.PIDX_CapUpdateFirmware:
                    b = false;
                    break;

                case Const.PIDX_CapUpdateStatistics:
                    b = true;
                    break;

                case Const.PIDX_Claimed:
                    b = st.Claimed;
                    break;

                case Const.PIDX_DataEventEnabled:
                    b = dataEventEnabled;
                    break;

                case Const.PIDX_DeviceEnabled:
                    // Sólo si 'claim'
                    if (st.Claimed) b = st.DeviceEnabled;
                    break;

                case Const.PIDX_FreezeEvents:
                    b = freezeEvents;
                    break;

                // Específicas
                case Const.PIDXChan_CapDeposit:
                    b = true;
                    break;

                case Const.PIDXChan_CapDepositDataEvent:
                    b = true;
                    break;

                case Const.PIDXChan_CapDiscrepancy:
                    b = false;
                    break;

                case Const.PIDXChan_CapEmptySensor:
                    b = true;
                    break;

                case Const.PIDXChan_CapFullSensor:
                    b = true;
                    break;

                case Const.PIDXChan_CapJamSensor:
                    b = true;
                    break;

                case Const.PIDXChan_CapNearEmptySensor:
                    b = true;
                    break;

                case Const.PIDXChan_CapNearFullSensor:
                    b = true;
                    break;

                case Const.PIDXChan_CapPauseDeposit:
                    b = true;
                    break;

                case Const.PIDXChan_CapRealTimeData:
                    b = true;
                    break;

                case Const.PIDXChan_CapRepayDeposit:
                    b = true;
                    break;

                case Const.PIDXChan_AsyncMode:
                    b = st.AsyncMode;
                    break;

                case Const.PIDXChan_RealTimeDataEnabled:
                    b = true;
                    break;

                default:
                    return Const.OPOS_E_FAILURE;
            }

            return SetRC(Const.OPOS_SUCCESS);
        }
        private int GetProperty(int propIndex, ref string s)
        {
            s = "[Error]";

            if (!st.Opened) return SetRC(Const.OPOS_E_CLOSED);

            switch (propIndex)
            {
                // Comunes
                case Const.PIDX_CheckHealthText:
                    s = st.CheckHealthText;
                    break;

                case Const.PIDX_ServiceObjectDescription:
                    s = serviceObjectDescription;
                    break;

                case Const.PIDX_DeviceDescription:
                    s = deviceDescription;
                    break;

                case Const.PIDX_DeviceName:
                    s = def.DeviceName;
                    break;

                // Específicas de CashChanger
                case Const.PIDXChan_CurrencyCashList:
                    s = currencyCashList;
                    break;

                case Const.PIDXChan_CurrencyCode:
                    s = currencyCode;
                    break;

                case Const.PIDXChan_CurrencyCodeList:
                    s = currencyCodeList;
                    break;

                case Const.PIDXChan_DepositCashList:
                    s = depositCashList;
                    break;

                case Const.PIDXChan_DepositCodeList:
                    s = depositCodeList;
                    break;

                case Const.PIDXChan_DepositCounts:
                    s = cont.depositCounts;
                    break;

                case Const.PIDXChan_ExitCashList:
                    s = exitCashList;
                    break;

                default:
                    s = "[Not Supported]";
                    return SetRC(Const.OPOS_E_FAILURE);
            }

            return SetRC(Const.OPOS_SUCCESS);
        }
        
        private int SetProperty(int propIndex, int value)
        {
            if (!st.Opened) return SetRC(Const.OPOS_E_CLOSED);

            switch (propIndex)
            {
                // Comunes OPOS
                case Const.PIDX_BinaryConversion:
                    if(value != Const.OPOS_BC_NONE) return SetRC(Const.OPOS_E_ILLEGAL);

                    break;

                case Const.PIDX_PowerNotify:
                    // No se puede si 'enable'
                    if (st.DeviceEnabled) return SetRC(Const.OPOS_E_ILLEGAL);
                    if (value != Const.OPOS_PN_DISABLED) return SetRC(Const.OPOS_E_ILLEGAL);

                    break;

                // Específicas
                case Const.PIDXChan_CurrentExit:
                    if (value != 1) return SetRC(Const.OPOS_E_ILLEGAL);

                    break;

                case Const.PIDXChan_CurrentService:
                    if (value != 0) return SetRC(Const.OPOS_E_ILLEGAL);

                    break;

                default:
                    // Propiedad no válida
                    return SetRC(Const.OPOS_E_FAILURE);
            }

            return SetRC(Const.OPOS_SUCCESS);
        }
        private int SetProperty(int propIndex, bool value)
        {
            if (!st.Opened) return SetRC(Const.OPOS_E_CLOSED);

            switch (propIndex)
            {
                // Comunes OPOS
                case Const.PIDX_DataEventEnabled:
                    dataEventEnabled = value;
                    break;

                case Const.PIDX_DeviceEnabled:
                    // Solo si 'claim'
                    if (!st.Claimed) return SetRC(Const.OPOS_E_NOTCLAIMED);
                    if(st.Busy) return SetRC(Const.OPOS_E_BUSY);
                    
                    st.DeviceEnabled = value;
                    OnStateChanged();
                    OnOrderReceived((int)NumFrase.RecEnable);

                    break;

                case Const.PIDX_FreezeEvents:
                    freezeEvents = value;
                    break;

                // Específicas
                case Const.PIDXChan_AsyncMode:
                    // No se puede cambiar si hay un pago en curso
                    if (st.DispenseInCurse) return SetRC(Const.OPOS_E_ILLEGAL);

                    st.AsyncMode = value;
                    break;

                case Const.PIDXChan_RealTimeDataEnabled:
                    // Solo si 'enabled'
                    if(!st.DeviceEnabled) return SetRC(Const.OPOS_E_DISABLED);
                    if (!value) return SetRC(Const.OPOS_E_ILLEGAL);

                    break;

                default:
                    // Propiedad no válida
                    return SetRC(Const.OPOS_E_FAILURE);
            }

            return SetRC(Const.OPOS_SUCCESS);
        }
        private int SetProperty(int propIndex, string value)
        {
            if (!st.Opened) return SetRC(Const.OPOS_E_CLOSED);

            switch (propIndex)
            {
                case Const.PIDXChan_CurrencyCode:
                    if (value != currencyCodeList) return SetRC(Const.OPOS_E_ILLEGAL);
                    currencyCode = value;
                    break;

                default:
                    // Propiedad no válida
                    return SetRC(Const.OPOS_E_FAILURE);
            }

            return SetRC(Const.OPOS_SUCCESS);
        }
        #endregion

        #region Metodos
        // Common
        public int Open(string deviceName)
        {
            if (st.Opened)
            {
                st.OpenResult = Const.OPOS_OR_ALREADYOPEN;
                return Const.OPOS_E_ILLEGAL;
            }
            
            st.State = Const.OPOS_S_CLOSED;
            st.OpenResult = Const.OPOS_E_CLOSED;
            
            try
            {
                def.ReadFromFile(deviceName);
                cont.ReadFromFile(deviceName);
            }
            catch (Exception)
            {
                st.OpenResult = Const.OPOS_OR_REGBADNAME;
                return SetRC(Const.OPOS_E_NOEXIST);
            }

            serviceObjectDescription = "Cashlogy OPOS Service Object v1.11.2";
            serviceObjectVersion = 1011002;
            deviceDescription = "Cashlogy smart cash drawer";

            currencyCashList = def.CurrencyCashList();
            currencyCode = def.CurrencyCode;
            currencyCodeList = currencyCode;
            depositCashList = def.DepositCashList();
            depositCodeList = currencyCodeList;
            exitCashList = currencyCashList;

            st.EnableDepositItems = new bool[MAX_ITEMS];
            for (int i = 0; i < def.NumItems; i++)
            {
                st.EnableDepositItems[i] = true;
            }

            cont.ResetDepositedCash();

            st.Opened = true;
            st.Claimed = false;
            st.DeviceEnabled = false;

            st.OpenResult = Const.OPOS_SUCCESS;
            st.State = Const.OPOS_S_IDLE;
            st.DepositStatus = Const.CHAN_STATUS_DEPOSIT_END;
            st.DispenseStatus = Const.CHAN_STATUS_DISPENSE_END;

            OnViewChanged();
            OnContChanged();
            OnStateChanged();
            OnOrderReceived((int)NumFrase.RecOpen);
            OnDeviceOpened(true);

            return SetRC(Const.OPOS_SUCCESS);
        }
        
        public int GetOpenRestult(ref int OpenResult)
        {
            OpenResult = st.OpenResult;

            return Const.OPOS_SUCCESS;
        }

        public int Close()
        {
            if (!st.Opened) return SetRC(Const.OPOS_E_CLOSED);

            if (st.DepositInCurse)
            {
                FixDeposit();
                EndDeposit(Const.CHAN_DEPOSIT_NOCHANGE);
            }

            if (st.DispenseInCurse)
            {
                // EndDispense
                tmrDispenseCoins.Stop();
                tmrDispenseBills.Stop();
            }

            cont.SaveToFile(def.DeviceName);

            st = new CashlogyState();
            def = new CashlogyDef();
            cont = new CashlogyCont(def, st);
            // Resuscribir eventos cont (se pierden al volver a instanciar)
            cont.OnChanged += this.Cont_OnChanged;

            OnStateChanged();
            OnOrderReceived((int)NumFrase.SinIniciar);
            OnDeviceOpened(false);

            return Const.OPOS_SUCCESS;
        }

        public int Claim(int _timeout)
        {
            if (!st.Opened) return SetRC(Const.OPOS_E_CLOSED);
            if (st.Claimed) return SetRC(Const.OPOS_E_CLAIMED);

            st.Claimed = true;
            st.DeviceEnabled = false;

            OnStateChanged();
            OnOrderReceived((int)NumFrase.RecClaim);

            return SetRC(Const.OPOS_SUCCESS);
        }

        public int Release()
        {
            if (!st.Opened) return SetRC(Const.OPOS_E_CLOSED);
            if (!st.Claimed) return SetRC(Const.OPOS_E_NOTCLAIMED);
            if(st.Busy) return SetRC(Const.OPOS_E_BUSY);

            st.Claimed = false;
            st.DeviceEnabled = false;
            st.State = Const.OPOS_S_IDLE;

            OnStateChanged();
            OnOrderReceived((int)NumFrase.RecRelease);

            return SetRC(Const.OPOS_SUCCESS);
        }

        public int DirectIO(int command, ref int pData, ref string pString)
        {
            if (!st.Opened) return SetRC(Const.OPOS_E_ILLEGAL);

            switch (command)
            {
                // Recaudacion de billetes
                case Const.DIO_ReadDispensableCashCounts:
                    return DIO_ReadDispensableCashCounts(ref pString);

                case Const.DIO_ReadNotDispensableCashCounts:
                    return DIO_ReadNotDispensableCashCounts(ref pString);

                case Const.DIO_DispenseCashBillsToStacker:
                    return DIO_DispenseCashBillsToStacker(ref pString);

                // Dispense
                case Const.DIO_DispenseAmount:
                    return DIO_DispenseAmount(ref pData);

                case Const.DIO_DispenseCounts:
                    return DIO_DispenseCounts(ref pString);

                case Const.DIO_DispenseStatus:
                    return DIO_DispenseStatus(ref pData);

                case Const.DIO_CapPauseDispense:
                    pData = 1;
                    return SetRC(Const.OPOS_SUCCESS);
                    
                case Const.DIO_PauseDispense:
                    return DIO_PauseDispense();

                case Const.DIO_DispenseAll:
                    return DIO_DispenseAll(ref pData);

                // Estado lleno/vacío
                case Const.DIO_ReadCashAvailableCapacity:
                    return DIO_ReadCashAvailableCapacity(ref pString);

                case Const.DIO_ReadCashEmptyFullStatus:
                    return DIO_ReadCashEmptyFullStatus(ref pString);

                case Const.DIO_ResultCodeExtended:
                    return DIO_ResultCodeExtended(ref pData);

                case Const.DIO_ReadResultCodeExtendedInfo:
                    return DIO_ReadResultCodeExtendedInfo(ref pData, ref pString);

                case Const.DIO_ReadDevicesCapacities:
                    return DIO_ReadDevicesCapacities(ref pString);

                // Habilitar/deshabilitar admisión de items
                case Const.DIO_EnableDepositItem:
                    return DIO_EnableDepositItem(ref pString);

                case Const.DIO_GetEnableDepositItem:
                    return DIO_GetEnableDepositItem(ref pString);

                case Const.DIO_DispenseChangeOnlyCoins:
                    return DIO_DispenseChangeOnlyCoins(ref pData);

                case Const.DIO_DispenseAllOnlyCoins:
                    return DIO_DispenseAllOnlyCoins(ref pData);

                case Const.DIO_ReadStatus:
                    return DIO_ReadStatus(ref pString);

                case Const.DIO_DispenseAllOnlyBills:
                    return DIO_DispenseAllOnlyBills(ref pData);

                case Const.DIO_DispenseAllByItems:
                    return DIO_DispenseAllByItems(ref pData, ref pString);

                default:
                    return SetRC(Const.OPOS_E_FAILURE);
            }
        }

        public int CheckHealth(int level)
        {
            if (level != Const.OPOS_CH_INTERNAL)
            {
                st.CheckHealthText = "";
                return SetRC(Const.OPOS_E_ILLEGAL);
            }

            st.CheckHealthText = "Internal HCheck: Successful";
            return SetRC(Const.OPOS_SUCCESS);
        }

        // Specific
        public int AdjustCashCounts(string cashCounts)
        {
            if (!st.DeviceEnabled) return SetRC(Const.OPOS_E_DISABLED);
            if (st.Busy) return SetRC(Const.OPOS_E_BUSY);

            List<int> list = new List<int>();
            bool startCollect = false;
            bool collected = false;
            int r = ParseAdjustCounts(cashCounts, ref list, ref startCollect, ref collected);
            if (r != Const.OPOS_SUCCESS) return SetRC(Const.OPOS_E_ILLEGAL);

            if (startCollect && collectSatckerEnded)
            {
                collectSatckerEnded = false;
                if (OnCollectStackerStarted != null) OnCollectStackerStarted();
                return SetRC(Const.OPOS_SUCCESS);
            }
            else if (collected)
            {
                bool isCollected = true;
                for (int i = 0; i < cont.stacker.Length; i++)
                {
                    if (cont.stacker[i] != 0) isCollected = false;
                }

                if (isCollected) return SetRC(Const.OPOS_SUCCESS);
                else return SetRC(Const.OPOS_E_FAILURE);
            }
            else
            {
                int[] recyclers = cont.recyclers;
                for (int i = 0; i < list.Count; i++)
                {
                    recyclers[list[i]] = 0;
                }
                SetRecyclers(recyclers);
            }

            return SetRC(Const.OPOS_SUCCESS);
        }

        public int BeginDeposit()
        {
            if (!st.DeviceEnabled) return SetRC(Const.OPOS_E_DISABLED);
            if (st.Busy) return SetRC(Const.OPOS_E_BUSY);

            cont.ResetDepositedCash();
            OnContChanged();
            
            st.Busy = true;
            st.DepositInCurse = true;
            st.DepositInCursePaused = false;
            st.DepositInCurseEnded = false;
            st.DepositInCurseFixDepositCalled = false;
            if(OnBeginDeposit != null) OnBeginDeposit();
            OnDepositEnabled(true);

            st.State = Const.OPOS_S_BUSY;
            st.DepositStatus = Const.CHAN_STATUS_DEPOSIT_START;
            OnStateChanged();
            OnOrderReceived((int)NumFrase.Depositando);

            return SetRC(Const.OPOS_SUCCESS);
        }

        public int DispenseCash(string cashCount)
        {
            if (!st.DeviceEnabled) return SetRC(Const.OPOS_E_DISABLED);
            if (st.Busy) return SetRC(Const.OPOS_E_BUSY);

            int[] items = new int[MAX_ITEMS];
            int r = ParseCashCounts(cashCount, ref items);
            if (r != Const.OPOS_SUCCESS) return SetRC(Const.OPOS_E_ILLEGAL);

            // Compruebo que tengo los items (aprovecho y calculo el total)
            int amount = 0;
            for (int i = 0; i < def.NumItems; i++)
            {
                amount += def.ItemsDef[i].Value * items[i];
                if (items[i] > cont.recyclers[i]) return SetRC(Const.OPOS_E_FAILURE);
            }

            st.Busy = true;
            st.DispenseInCurse = true;
            st.State = Const.OPOS_S_BUSY;
            st.DispenseStatus = Const.CHAN_STATUS_DISPENSE_START;
            
            OnStateChanged();
            OnOrderReceived((int)NumFrase.Dispensando);

            cont.ResetDispensedCash();

            st.DispenseInCurseAmountRequired = amount;
            st.DispenseInCurseCountsRequired = (int[])items.Clone();

            if (outCadence)
            {
                auxDispense = (int[])items.Clone();
                auxToStacker = false;
                tmrDispenseCoins.Start();
                tmrDispenseBills.Start();
            }
            else DispenseSinCadencia(items, false);

            return SetRC(Const.OPOS_SUCCESS);
        }

        public int DispenseChange(int amount)
        {
            if (!st.DeviceEnabled) return SetRC(Const.OPOS_E_DISABLED);
            if (st.Busy) return SetRC(Const.OPOS_E_BUSY);

            int[] items = new int[MAX_ITEMS];
            int r = GetDispenseItems(amount, ref items, false);
            if (r != Const.OPOS_SUCCESS) return SetRC(Const.OPOS_E_FAILURE);

            st.Busy = true;
            st.DispenseInCurse = true;
            st.State = Const.OPOS_S_BUSY;
            st.DispenseStatus = Const.CHAN_STATUS_DISPENSE_START;

            OnStateChanged();
            OnOrderReceived((int)NumFrase.Dispensando);

            cont.ResetDispensedCash();

            st.DispenseInCurseAmountRequired = amount;
            st.DispenseInCurseCountsRequired = (int[])items.Clone();

            if (outCadence)
            {
                auxDispense = (int[])items.Clone();
                auxToStacker = false;
                tmrDispenseCoins.Start();
                tmrDispenseBills.Start();
            }
            else DispenseSinCadencia(items, false);

            return SetRC(Const.OPOS_SUCCESS);
        }

        public int FixDeposit()
        {
            if (!st.DepositInCurse) return SetRC(Const.OPOS_E_ILLEGAL);

            st.DepositInCurseFixDepositCalled = true;

            return SetRC(Const.OPOS_SUCCESS);
        }

        public int EndDeposit(int _succes)
        {
            if (!st.DepositInCurse) return SetRC(Const.OPOS_E_ILLEGAL);
            FixDeposit();
            if (!st.DepositInCurseFixDepositCalled) return SetRC(Const.OPOS_E_ILLEGAL);

            OnDepositEnabled(false);
            if (tmrDepositCoins.Enabled || tmrDepositBills.Enabled)
            {
                tmrDepositCoins.Stop();
                tmrDepositBills.Stop();
                auxDeposit = new int[MAX_ITEMS];
            }

            st.Busy = false;
            st.State = Const.OPOS_S_IDLE;
            st.DepositStatus = Const.CHAN_STATUS_DEPOSIT_END;
            st.DepositInCurse = false;
            st.DepositInCursePaused = false;
            st.DepositInCurseFixDepositCalled = false;
            st.DepositInCurseEnded = true;

            OnStateChanged();
            OnOrderReceived((int)NumFrase.RecEndDeposit);

            return SetRC(Const.OPOS_SUCCESS);
        }

        public int PauseDeposit(int control)
        {
            if (!st.DepositInCurse) return SetRC(Const.OPOS_E_ILLEGAL);

            if (control == Const.CHAN_DEPOSIT_PAUSE)
            {
                OnDepositEnabled(false);

                st.DepositInCurse = true;
                st.DepositInCursePaused = true;
                st.DepositInCurseEnded = false;
                st.DepositInCurseFixDepositCalled = false;

                cont.DepositCounts();
                OnOrderReceived((int)NumFrase.RecPauseDeposit);
            }
            else if (control == Const.CHAN_DEPOSIT_RESTART)
            {
                if (!st.DepositInCursePaused)
                {
                    return SetRC(Const.OPOS_E_ILLEGAL);
                }

                OnDepositEnabled(true);
                st.DepositInCurse = true;
                st.DepositInCursePaused = false;
                st.DepositInCurseEnded = false;
                st.DepositInCurseFixDepositCalled = false;
                OnOrderReceived((int)NumFrase.Depositando);
            }

            return SetRC(Const.OPOS_SUCCESS);
        }

        public int ReadCashCounts(ref string cashCounts)
        {
            if (!st.DeviceEnabled) return SetRC(Const.OPOS_E_DISABLED);
            if (st.Busy) return SetRC(Const.OPOS_E_BUSY);

            cashCounts = cont.ReadCashCounts();

            return SetRC(Const.OPOS_SUCCESS);
        }
        #endregion

        #region Dispense Asincrono
        private void TmrDispenseCoins_Elapsed(object sender, ElapsedEventArgs e)
        {
            Action action = () => Finish_DispenseCoins();
            disp.Invoke(action);
        }
        private void Finish_DispenseCoins()
        {
            bool stop = false;
            int i = SelectDispenseItemCoin();
            if (i == -1) stop = true;

            if (!stop)
            {
                DispenseItems(i, auxToStacker);
                auxDispense[i]--;
            }
            else
            {
                tmrDispenseCoins.Stop();

                if (!tmrDispenseCoins.Enabled && !tmrDispenseBills.Enabled)
                {
                    st.Busy = false;
                    st.DispenseInCurse = false;

                    st.State = Const.OPOS_S_IDLE;
                    st.DispenseStatus = Const.CHAN_STATUS_DISPENSE_END;
                    if (st.AsyncMode) st.AsyncResultCode = Const.OPOS_SUCCESS;
                    OnStateChanged();
                    OnOrderReceived((int)NumFrase.DevolEnd);
                }
            }
        }

        private void TmrDispenseBills_Elapsed(object sender, ElapsedEventArgs e)
        {
            Action action = () => Finish_DispenseBills();
            disp.Invoke(action);
        }
        private void Finish_DispenseBills()
        {
            bool stop = false;
            int i = SelectDispenseItemBill();
            if (i == -1) stop = true;
            
            if (!stop)
            {
                DispenseItems(i, auxToStacker);
                auxDispense[i]--;
            }
            else
            {
                tmrDispenseBills.Stop();

                if (!tmrDispenseCoins.Enabled && !tmrDispenseBills.Enabled)
                {
                    st.Busy = false;
                    st.DispenseInCurse = false;

                    st.State = Const.OPOS_S_IDLE;
                    st.DispenseStatus = Const.CHAN_STATUS_DISPENSE_END;
                    if (st.AsyncMode) st.AsyncResultCode = Const.OPOS_SUCCESS;
                    OnStateChanged();
                    OnOrderReceived((int)NumFrase.DevolEnd);
                }
            }
        }
        #endregion

        #region Funciones Auxiliares
        private int SetRC(int result)
        {
            st.ResultCode = result;
            st.ResultCodeExtended = 0;
            return result;
        }

        private int ParseCashCounts(string cashCounts, ref int[] list)
        {
            list = new int[MAX_ITEMS];
            if (cashCounts == "") return Const.OPOS_SUCCESS; // Revisar
            
            int k = -1;
            bool correctValue;
            var items = cashCounts.Split(';');
            if (items.Length > 2) return Const.OPOS_E_ILLEGAL;

            if (items[0] != "")
            {
                var coinList = items[0].Split(',');
                for (int i = 0; i < coinList.Length; i++)
                {
                    var coin = coinList[i].Split(':');
                    if (coin.Length > 2) return Const.OPOS_E_ILLEGAL;

                    int coinValue;
                    int numCoins;
                    try
                    {
                        coinValue = Convert.ToInt32(coin[0]);
                        numCoins = Convert.ToInt32(coin[1]);
                    }
                    catch (Exception) { return Const.OPOS_E_ILLEGAL; }

                    correctValue = false;
                    for (int j = 0; j < def.NumCoins; j++)
                    {
                        if (coinValue == def.ItemsDef[j].Value && j != k)
                        {
                            list[j] = numCoins;
                            k = j;
                            correctValue = true;
                            break;
                        }
                    }
                    if (!correctValue) return Const.OPOS_E_ILLEGAL;
                }
            }

            if (items.Length == 2)
            {
                if (items[1] != "")
                {
                    var billList = items[1].Split(',');
                    for (int i = 0; i < billList.Length; i++)
                    {
                        var bill = billList[i].Split(':');
                        if (bill.Length > 2) return Const.OPOS_E_ILLEGAL;

                        int billValue;
                        int numBills;
                        try
                        {
                            billValue = Convert.ToInt32(bill[0]);
                            numBills = Convert.ToInt32(bill[1]);
                        }
                        catch (Exception) { return Const.OPOS_E_ILLEGAL; }

                        correctValue = false;
                        for (int j = def.NumCoins; j < def.NumItems; j++)
                        {
                            if (billValue == def.ItemsDef[j].Value && j != k)
                            {
                                list[j] = numBills;
                                k = j;
                                correctValue = true;
                                break;
                            }
                        }
                        if (!correctValue) return Const.OPOS_E_ILLEGAL;
                    }
                }
                else return Const.OPOS_E_ILLEGAL;
            }

            return Const.OPOS_SUCCESS;
        }

        private int ParseAdjustCounts(string cashCounts, ref List<int> list, ref bool startCollect, ref bool collected)
        {
            if (cashCounts == "") return Const.OPOS_SUCCESS; // Duda

            list = new List<int>();
            startCollect = false;
            collected = false;
            int k = -1;
            bool correctValue;
            var items = cashCounts.Split(';');
            if (items.Length > 2) return Const.OPOS_E_ILLEGAL;

            if (items.Length == 2)
            {
                if (items[0] == "")
                {
                    if (items[1] == "STACKER:COLLECTINPROGRESS")
                    {
                        startCollect = true;
                        return Const.OPOS_SUCCESS;
                    }
                    else if (items[1] == "STACKER:COLLECTED")
                    {
                        collected = true;
                        return Const.OPOS_SUCCESS;
                    }
                    else return Const.OPOS_E_ILLEGAL;
                }
            }

            if (items[0] != "")
            {
                var coinList = items[0].Split(',');
                for (int i = 0; i < coinList.Length; i++)
                {
                    var coin = coinList[i].Split(':');
                    if (coin.Length > 2) return Const.OPOS_E_ILLEGAL;

                    int coinValue;
                    int numCoins;
                    try
                    {
                        coinValue = Convert.ToInt32(coin[0]);
                        numCoins = Convert.ToInt32(coin[1]);
                    }
                    catch (Exception) { return Const.OPOS_E_ILLEGAL; }

                    correctValue = false;
                    for (int j = 0; j < def.NumCoins; j++)
                    {
                        if (coinValue == def.ItemsDef[j].Value && j != k)
                        {
                            if (numCoins != 0) return Const.OPOS_E_ILLEGAL;
                            list.Add(j);
                            k = j;
                            correctValue = true;
                            break;
                        }
                    }
                    if(!correctValue) return Const.OPOS_E_ILLEGAL;
                }
            }

            return Const.OPOS_SUCCESS;
        }
         
        private int GetDispenseItems(int amount, ref int[] items, bool onlyCoins)
        {
            items = new int[MAX_ITEMS];
            if (amount < 0) return Const.OPOS_E_FAILURE;

            int num;
            if (onlyCoins) num = def.NumCoins;
            else num = def.NumItems;

            for (int i = num - 1; i >= 0; i--)
            {
                if (amount <= 0) break;

                if (def.ItemsDef[i].IsDispensable)
                {
                    int result = (amount / def.ItemsDef[i].Value);
                    if (result > 0 && cont.recyclers[i] >= result)
                    {
                        items[i] = result;
                        amount %= def.ItemsDef[i].Value;
                    }
                    else if (result > 0 && cont.recyclers[i] < result)
                    {
                        items[i] = cont.recyclers[i];
                        amount -= cont.recyclers[i] * def.ItemsDef[i].Value;
                    }
                }
            }
            if (amount != 0) return Const.OPOS_E_FAILURE;

            return Const.OPOS_SUCCESS;
        }

        private int SelectDispenseItemCoin()
        {
            for (int i = 0; i < def.NumCoins; i++)
            {
                if (auxDispense[i] != 0)
                {
                    return i;
                }
            }

            return -1;
        }

        private int SelectDispenseItemBill()
        {
            for (int i = def.NumCoins; i < def.NumItems; i++)
            {
                if (auxDispense[i] != 0)
                {
                    return i;
                }
            }

            return -1;
        }

        private void DispenseSinCadencia(int[] items, bool ToStacker)
        {
            DispenseItems(items, ToStacker);

            st.Busy = false;
            st.DispenseInCurse = false;

            st.State = Const.OPOS_S_IDLE;
            st.DispenseStatus = Const.CHAN_STATUS_DISPENSE_END;
            if (st.AsyncMode) st.AsyncResultCode = Const.OPOS_SUCCESS;
            OnStateChanged();
            OnOrderReceived((int)NumFrase.DevolEnd);
        }
        #endregion
    }
}
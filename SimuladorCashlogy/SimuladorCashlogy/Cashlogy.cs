using Cashlogy.SocketOPOS;
using Microsoft.Win32;
using SimuladorCashlogy;
using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows.Threading;

namespace Cashlogy
{
    public partial class CashlogyDevice
    {
        public const int MAX_ITEMS = 16;

        private Dispatcher disp;
        private ServerOPOS server;

        private CashlogyState st;
        private CashlogyDef def;
        private CashlogyCont cont;
        private Config cfg;

        public CashlogyDef Def { get => def; }

        private bool collectSatckerEnded = true; 

        #region Cadencia Entrada/Salida efectivo
        private Timer tmrDepositCoins;
        private Timer tmrDepositBills;
        private int[] auxDeposit;

        private Timer tmrDispenseCoins;
        private Timer tmrDispenseBills;

        private bool inCadence = true;
        private bool outCadence = true;
        private int[] auxDispense;
        private bool auxToStacker;
        #endregion

        private string xmlErrores = "";
        private bool hayError = false;

        #region Eventos
        public delegate void onDeviceOpened(bool opened);
        public event onDeviceOpened OnDeviceOpened;

        public delegate void onViewChanged();
        public event onViewChanged OnViewChanged;

        public delegate void onContChanged();
        public event onContChanged OnContChanged;

        public delegate void onStateChanged();
        public event onStateChanged OnStateChanged;

        public delegate void onOrderReceived(int order);
        public event onOrderReceived OnOrderReceived;

        public delegate void onDepositEnabled(bool enabled);
        public event onDepositEnabled OnDepositEnabled;

        public delegate void onBeginDeposit();
        public event onBeginDeposit OnBeginDeposit;

        public delegate void onDepositItemEnabled();
        public event onDepositItemEnabled OnDepositItemEnabled;

        public delegate void onCollectStackerStarted();
        public event onCollectStackerStarted OnCollectStackerStarted;

        public delegate void onCollectStackerEnded();
        public event onCollectStackerEnded OnCollectStackerEnded;

        public delegate void onErrorForced();
        public event onErrorForced OnErrorForced;

        public delegate void onErrorCleaned();
        public event onErrorCleaned OnErrorCleaned;
        #endregion

        public CashlogyDevice(ServerOPOS servidor,Dispatcher dispatcher,Config config)
        {
            disp = dispatcher;
            server = servidor;
            st = new CashlogyState();
            def = new CashlogyDef();
            cont = new CashlogyCont(def,st);
            cfg = config;

            // Deposit
            tmrDepositCoins = new Timer();
            tmrDepositCoins.Interval = 333;
            tmrDepositCoins.Stop();
            tmrDepositBills = new Timer();
            tmrDepositBills.Interval = 667;
            tmrDepositBills.Stop();

            // Dispense
            tmrDispenseCoins = new Timer();
            tmrDispenseCoins.Interval = 133;
            tmrDispenseCoins.Stop();
            tmrDispenseBills = new Timer();
            tmrDispenseBills.Interval = 667;
            tmrDispenseBills.Stop();

            auxDispense = new int[MAX_ITEMS];
            auxDeposit = new int[MAX_ITEMS];
            auxToStacker = false;

            #region Suscripcion Eventos
            this.OnContChanged += SetCashlogyStatus;
            cont.OnChanged += this.Cont_OnChanged;
            server.OnDataReceived += this.OnDataReceived_Threaded;
            tmrDispenseCoins.Elapsed += TmrDispenseCoins_Elapsed;
            tmrDispenseBills.Elapsed += TmrDispenseBills_Elapsed;
            tmrDepositCoins.Elapsed += TmrDepositCoins_Elapsed;
            tmrDepositBills.Elapsed += TmrDepositBills_Elapsed;
            #endregion
        }

        private ResponseOPOS OnDataReceived_Threaded(RequestOPOS req)
        {
            ResponseOPOS response = new ResponseOPOS();
            Action action = () => response = OnDataReceived(req);
            disp.Invoke(action);
            return response;
        }
        private ResponseOPOS OnDataReceived(RequestOPOS req)
        {
            ResponseOPOS response;
            int r;
            switch (req.Function)
            {
                case "GetProperty":
                    string getProp = (string)req.Params[0];
                    int GetPropIndex = -1;
                    int GetType = -1;
                    ObtainOPOSIndex(getProp, ref GetPropIndex, ref GetType);
                    switch (GetType)
                    {
                        case 0:
                            int num = 0;
                            r = GetPropertyInt(GetPropIndex, ref num);
                            response = new ResponseOPOS(req.ID, r, new List<object> { req.Params[0], num });
                            break;
                            
                        case 1:
                            bool b = false;
                            int bInt;
                            r = GetPropertyBool(GetPropIndex, ref b);
                            if (b) bInt = 1;
                            else bInt = 0;
                            response = new ResponseOPOS(req.ID, r, new List<object> { req.Params[0], bInt });
                            break;

                        case 2:
                            string str = "";
                            r = GetPropertyStr(GetPropIndex, ref str);
                            response = new ResponseOPOS(req.ID, r, new List<object> { req.Params[0], str });
                            break;

                        default:
                            response = new ResponseOPOS(req.ID, Const.OPOS_E_FAILURE, new List<object> { req.Params[0] });
                            break;

                    }
                    break;

                case "SetProperty":
                    string setProp = (string)req.Params[0];
                    int setPropIndex = -1;
                    int setType = -1;
                    ObtainOPOSIndex(setProp, ref setPropIndex, ref setType);
                    switch (setType)
                    {
                        case 0:
                            int num = (int)req.Params[1];
                            r = SetPropertyInt(setPropIndex, num);
                            response = new ResponseOPOS(req.ID, r, req.Params);
                            break;

                        case 1:
                            bool b = false;
                            if ((int)req.Params[1] == 0) b = false;
                            else if((int)req.Params[1] == 1) b = true;
                            r = SetPropertyBool(setPropIndex, b);
                            response = new ResponseOPOS(req.ID, r, req.Params);
                            break;

                        case 2:
                            string str = (string)req.Params[1];
                            r = SetPropertyStr(setPropIndex, str);
                            response = new ResponseOPOS(req.ID, r, req.Params);
                            break;
                        default:
                            response = new ResponseOPOS(req.ID, Const.OPOS_E_FAILURE, new List<object> { req.Params[0] });
                            break;
                    }
                    break;
                    
                // Common
                case "Open":
                    string device = (string)req.Params[0];
                    r = Open(device);
                    response = new ResponseOPOS(req.ID, r, new List<object>());
                    break;

                case "Close":
                    r = Close();
                    response = new ResponseOPOS(req.ID, r, new List<object>());
                    break;

                case "ClaimDevice":
                    int timeout = (int)req.Params[0];
                    r = Claim(timeout);
                    response = new ResponseOPOS(req.ID, r, new List<object>());
                    break;

                case "ReleaseDevice":
                    r = Release();
                    response = new ResponseOPOS(req.ID, r, new List<object>());
                    break;

                case "CheckHealth":
                    int level = (int)req.Params[0];
                    r = CheckHealth(level);
                    response = new ResponseOPOS(req.ID, r, new List<object> { level });
                    break;

                case "DirectIO":
                    int command = (int)req.Params[0];
                    int pData = (int)req.Params[1];
                    string pString = (string)req.Params[2];
                    r = DirectIO(command, ref pData, ref pString);
                    response = new ResponseOPOS(req.ID, r, new List<object> { req.Params[0], pData, pString });
                    break;

                // Specific
                case "AdjustCashCounts":
                    string adjustCashCounts = (string)req.Params[0];
                    r = AdjustCashCounts(adjustCashCounts);
                    response = new ResponseOPOS(req.ID, r, new List<object>());
                    break;

                case "BeginDeposit":
                    r = BeginDeposit();
                    response = new ResponseOPOS(req.ID, r, new List<object>());
                    break;

                case "DispenseCash":
                    string cashCounts = (string)req.Params[0];
                    r = DispenseCash(cashCounts);
                    response = new ResponseOPOS(req.ID, r, new List<object>());
                    break;

                case "DispenseChange":
                    int amount = (int)req.Params[0];
                    r = DispenseChange(amount);
                    response = new ResponseOPOS(req.ID, r, new List<object>());
                    break;

                case "FixDeposit":
                    r = FixDeposit();
                    string depositCounts = cont.depositCounts;
                    int totDeposited = cont.GetTotDeposited();
                    response = new ResponseOPOS(req.ID, r, new List<object> { depositCounts, totDeposited });
                    break;

                case "EndDeposit":
                    int succes = (int)req.Params[0];
                    r = EndDeposit(succes);
                    response = new ResponseOPOS(req.ID, r, new List<object> { succes });
                    break;

                case "PauseDeposit":
                    int control = (int)req.Params[0];
                    r = PauseDeposit(control);
                    response = new ResponseOPOS(req.ID, r, new List<object> { control });
                    break;

                case "ReadCashCounts":
                    string readCashCounts = (string)req.Params[0];
                    r = ReadCashCounts(ref readCashCounts);
                    response = new ResponseOPOS(req.ID, r, new List<object> { readCashCounts });
                    break;

                default:
                    r = Const.OPOS_E_ILLEGAL;
                    response = new ResponseOPOS(req.ID, r, new List<object>());
                    break;
            }
            
            return response;
        }
        
        #region Forzar Errores
        public void SetReadStatusErrors(Errores errores)
        {
            if (errores.list.Count == 0) CleanErrors();
            errores.ToXml();
            xmlErrores = errores.xmlErrores;
        }

        public void ForceErrors()
        {
            hayError = true;
            OnErrorForced();
        }

        public void CleanErrors()
        {
            hayError = false;
            OnErrorCleaned();
        }
        #endregion

        public void GetState(ref bool opened, ref bool claimed, ref bool deviceEnable)
        {
            st.GetState(ref opened, ref claimed, ref deviceEnable);  
        }

        public void GetState(ref int state)
        {
            st.GetState(ref state);
        }

        public void GetEnableDepositItems(ref bool[] enabled)
        {
            enabled = st.EnableDepositItems;
        }

        private void SetCashlogyStatus()
        {
            // Para CashEmptyFullStatus
            for (int i = 0; i < def.ItemsDef.Count; i++) 
            {
                if (def.ItemsDef[i].IsDispensable)
                {
                    st.CashEmptyFullStatus[i] = Const.CHAN_STATUS_OK;
                    if (cont.recyclers[i] == 0)
                    {
                        st.CashEmptyFullStatus[i] = Const.CHAN_STATUS_EMPTY;
                    }
                    else if (cont.recyclers[i] < def.ItemsDef[i].PorNearEmpty * def.ItemsDef[i].Capacity)
                    {
                        st.CashEmptyFullStatus[i] = Const.CHAN_STATUS_NEAREMPTY;
                    }
                    else if (cont.recyclers[i] > def.ItemsDef[i].PorNearFull * def.ItemsDef[i].Capacity && cont.recyclers[i] < def.ItemsDef[i].Capacity)
                    {
                        st.CashEmptyFullStatus[i] = Const.CHAN_STATUS_NEARFULL;
                    }
                    else if (cont.recyclers[i] == def.ItemsDef[i].Capacity)
                    {
                        st.CashEmptyFullStatus[i] = Const.CHAN_STATUS_FULL;
                    }
                }
            }

            st.StackerEmptyFullStatus = Const.CHAN_STATUS_OK;
            if (cont.totStacker == 0)
            {
                st.StackerEmptyFullStatus = Const.CHAN_STATUS_EMPTY;
            }
            else if (cont.totStacker < def.PorNearEmptySt * def.CapStacker)
            {
                st.StackerEmptyFullStatus = Const.CHAN_STATUS_NEAREMPTY;
            }
            else if (cont.totStacker > def.PorNearFullSt * def.CapStacker && cont.totStacker < def.CapStacker)
            {
                st.StackerEmptyFullStatus = Const.CHAN_STATUS_NEARFULL;
            }
            else if (cont.totStacker == def.CapStacker)
            {
                st.StackerEmptyFullStatus = Const.CHAN_STATUS_FULL;
            }

            // Para DeviceStatus
            st.DeviceStatus = Const.CHAN_STATUS_OK;
            for (int i = 0; i < def.ItemsDef.Count; i++)
            {
                if (def.ItemsDef[i].IsDispensable)
                {
                    if (st.CashEmptyFullStatus[i] == Const.CHAN_STATUS_EMPTY || st.StackerEmptyFullStatus == Const.CHAN_STATUS_EMPTY)
                    {
                        st.DeviceStatus = Const.CHAN_STATUS_EMPTY;
                        break;
                    }
                    else if (st.CashEmptyFullStatus[i] == Const.CHAN_STATUS_NEAREMPTY || st.StackerEmptyFullStatus == Const.CHAN_STATUS_NEAREMPTY)
                    {
                        st.DeviceStatus = Const.CHAN_STATUS_NEAREMPTY;
                    }
                }
            }

            // Para FullStatus
            st.FullStatus = Const.CHAN_STATUS_OK;
            for (int i = 0; i < def.ItemsDef.Count; i++)
            {
                if (def.ItemsDef[i].IsDispensable)
                {
                    if (st.CashEmptyFullStatus[i] == Const.CHAN_STATUS_FULL || st.StackerEmptyFullStatus == Const.CHAN_STATUS_FULL)
                    {
                        st.FullStatus = Const.CHAN_STATUS_FULL;
                        break;
                    }
                    else if (st.CashEmptyFullStatus[i] == Const.CHAN_STATUS_NEARFULL || st.StackerEmptyFullStatus == Const.CHAN_STATUS_NEARFULL)
                    {
                        st.FullStatus = Const.CHAN_STATUS_NEARFULL;
                    }
                }
            }
        }

        public int[] GetItemsLevel(ref int stackerLevel)
        {
            stackerLevel = st.StackerEmptyFullStatus;
            return st.CashEmptyFullStatus;
        }

        #region Metodos Contabilidad
        public void SaveContToFile(string name)
        {
            cont.SaveToFile(name);
        }

        public void SetRecyclers(int[] r)
        {
            cont.SetRecyclers(r);
        }

        public void SetStacker(int[] s)
        {
            cont.SetStacker(s);
        }

        public void ResetDepositedCash()
        {
            cont.ResetDepositedCash();
        }

        public int GetTotCash()
        {
            return cont.GetTotCash();
        }

        public int GetTotDeposited()
        {
            return cont.GetTotDeposited();
        }

        public int GetTotDispensed()
        {
            return cont.GetTotDispensed();
        }

        public int GetTotStacker()
        {
            return cont.totStacker;
        }

        public int[] GetRecyclers()
        {
            return cont.GetRecyclers();
        }

        public int[] GetStacker()
        {
            return cont.GetStacker();
        }

        public void GetAviableCapacity(ref int[] items, ref int stacker)
        {
            cont.GetAviableCapacity(ref items, ref stacker);
        }

        public int[] GetDeposited()
        {
            return cont.GetDeposited();
        }
        
        public string GetDepositCounts()
        {
            return cont.depositCounts;
        }

        public int[,] GetDispensed()
        {
            int[,] dispensed = new int[2,MAX_ITEMS];
            for (int i = 0; i < MAX_ITEMS; i++)
            {
                int num = cont.GetDispensed()[i];
                dispensed[0, i] = num;
                dispensed[1, i] = st.DispenseInCurseCountsRequired[i];
            }
            return dispensed;
        }

        public void SetEnableItems(int i, bool enable)
        {
            st.EnableDepositItems[i] = enable;
        }

        public void AddItem(int i, int num)
        {
            if (inCadence)
            {
                auxDeposit[i] += num;
                if (!tmrDepositCoins.Enabled) tmrDepositCoins.Start();
                if (!tmrDepositBills.Enabled) tmrDepositBills.Start();
            }
            else
            {
                cont.AddItem(i, num);
            }
        }

        public void DispenseItems(int i, bool ToStacker)
        {
            cont.RemoveItem(i, ToStacker);
        }

        public void DispenseItems(int[] cashCounts, bool ToStacker)
        {
            for (int i = 0; i < def.NumItems; i++)
            {
                cont.RemoveItem(i, cashCounts[i], ToStacker);
            }
        }

        public void Cont_OnChanged()
        {
            OnContChanged();
        }
        #endregion

        #region Get/Set Property
        public int GetPropertyInt(int propIndex, ref int n)
        {
            return GetProperty(propIndex, ref n);
        }
        public int SetPropertyInt(int propIndex, int value)
        {
            return SetProperty(propIndex, value);
        }

        public int GetPropertyBool(int propIndex, ref bool b)
        {
            return GetProperty(propIndex, ref b);
        }
        public int SetPropertyBool(int propIndex, bool value)
        {
            return SetProperty(propIndex, value);
        }

        public int GetPropertyStr(int propIndex, ref string s)
        {
            return GetProperty(propIndex, ref s);
        }
        public int SetPropertyStr(int propIndex, string value)
        {
            return SetProperty(propIndex, value);
        }
        #endregion

        public void GetConfig(out bool InCadence, out int depCoin, out int depBill, out bool OutCadence, 
                              out int dispCoin, out int dispBill, out string host, out int port)
        {
            InCadence = inCadence;
            depCoin = (int)tmrDepositCoins.Interval;
            depBill = (int)tmrDepositBills.Interval;
            OutCadence = outCadence;
            dispCoin = (int)tmrDispenseCoins.Interval;
            dispBill = (int)tmrDispenseBills.Interval;
            host = server.Host;
            port = server.Port;
        }

        public void GuardarConfig(int depCoin, int depBill, int dispCoin, int dispBill, bool simHooked, 
                                  string[] devices, bool changeServer, string host, int port)
        {
            if (depCoin == 0 && depBill == 0) inCadence = false;
            else
            {
                inCadence = true;
                tmrDepositCoins.Interval = depCoin;
                tmrDepositBills.Interval = depBill;
            }

            if (dispCoin == 0 && dispBill == 0) outCadence = false;
            else
            {
                outCadence = true;
                tmrDispenseCoins.Interval = dispCoin;
                tmrDispenseBills.Interval = dispBill;
            }

            string rootKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\OLEforRetail\ServiceOPOS\CashChanger\";
            string simulator = "AzkoyenOPOSCashChanger.CashlogySimulator";
            if (simHooked)
            {                
                string value;
                for (int i = 0; i < devices.Length; i++)
                {
                    value = (string)Registry.GetValue(rootKey + devices[i], "", "NotFound");
                    if (value != simulator)
                    {
                        Registry.SetValue(rootKey + devices[i], "Backup", value);
                        Registry.SetValue(rootKey + devices[i], "", simulator);
                    }
                }
            }
            else
            {
                string backup;
                for (int i = 0; i < devices.Length; i++)
                {
                    backup = (string)Registry.GetValue(rootKey + devices[i], "Backup", "NotFound");
                    if (backup != "NotFound") Registry.SetValue(rootKey + devices[i], "", backup);
                }
            }

            if (changeServer)
            {
                string key = @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\OLEforRetail\ServiceOPOS\CashChanger\CashlogySimulator";
                Registry.SetValue(key, "Host", host);
                Registry.SetValue(key, "Port", port);

                if (port != server.Port)
                {
                    server.DisposeServer();
                    server = new ServerOPOS(host, port);
                    server.OnDataReceived += this.OnDataReceived_Threaded;
                    server.StartListening();
                }                
            }
        }

        private void ObtainOPOSIndex(string prop, ref int propIndex, ref int type)
        {
            switch (prop)
            {
                case "OpenResult":
                    propIndex = Const.PIDX_OpenResult;
                    type = 0; // 0-int, 1-bool, 2-string
                    break;
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

        #region Cadencia Entrada Efectivo
        private void TmrDepositCoins_Elapsed(object sender, ElapsedEventArgs e)
        {
            Action action = () => Finish_DepositCoins();
            disp.Invoke(action);
        }

        private void TmrDepositBills_Elapsed(object sender, ElapsedEventArgs e)
        {
            Action action = () => Finish_DepositBills();
            disp.Invoke(action);
        }

        private void Finish_DepositCoins()
        {
            bool stop = false;
            int j = SelectDepositItemCoin();
            if (j == -1) stop = true;
            if (!stop)
            {
                cont.AddItem(j);
                auxDeposit[j]--;
            }
            else
            {
                tmrDepositCoins.Stop();

                if (!tmrDepositCoins.Enabled && !tmrDepositBills.Enabled) auxDeposit = new int[MAX_ITEMS];
            }
        }

        private void Finish_DepositBills()
        {
            bool stop = false;
            int j = SelectDepositItemBill();
            if (j == -1) stop = true;
            if (!stop)
            {
                cont.AddItem(j);
                auxDeposit[j]--;
            }
            else
            {
                tmrDepositBills.Stop();

                if (!tmrDepositCoins.Enabled && !tmrDepositBills.Enabled) auxDeposit = new int[MAX_ITEMS];
            }
        }

        private int SelectDepositItemCoin()
        {
            for (int i = 0; i < def.NumCoins; i++)
            {
                if (auxDeposit[i] != 0)
                {
                    return i;
                }
            }

            return -1;
        }

        private int SelectDepositItemBill()
        {
            for (int i = def.NumCoins; i < def.NumItems; i++)
            {
                if (auxDeposit[i] != 0)
                {
                    return i;
                }
            }

            return -1;
        }
        #endregion
    }
}
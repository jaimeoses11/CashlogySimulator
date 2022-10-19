namespace Cashlogy
{
    public static partial class Const
    {
        public const int PIDX_CHAN = 13000;
        public const int PIDX_NUMBER = 0;
        public const int PIDX_STRING = 1000000;

        #region Common Numeric Property Index Values
        // Properties
        public const int PIDX_Claimed = 1 + PIDX_NUMBER;
        public const int PIDX_DataEventEnabled = 2 + PIDX_NUMBER;
        public const int PIDX_DeviceEnabled = 3 + PIDX_NUMBER;
        public const int PIDX_FreezeEvents = 4 + PIDX_NUMBER;
        public const int PIDX_OutputID = 5 + PIDX_NUMBER;
        public const int PIDX_ResultCode = 6 + PIDX_NUMBER; //f
        public const int PIDX_ResultCodeExtended = 7 + PIDX_NUMBER; //f
        public const int PIDX_ServiceObjectVersion = 8 + PIDX_NUMBER;
        public const int PIDX_State = 9 + PIDX_NUMBER;

        public const int PIDX_AutoDisable = 10 + PIDX_NUMBER;
        public const int PIDX_BinaryConversion = 11 + PIDX_NUMBER; //f
        public const int PIDX_DataCount = 12 + PIDX_NUMBER;

        public const int PIDX_PowerNotify = 13 + PIDX_NUMBER;
        public const int PIDX_PowerState = 14 + PIDX_NUMBER;
        public const int PIDX_OpenResult = 15 + PIDX_NUMBER;

        // Capabilities
        public const int PIDX_CapPowerReporting = 501 + PIDX_NUMBER;

        public const int PIDX_CapStatisticsReporting = 502 + PIDX_NUMBER;
        public const int PIDX_CapUpdateStatistics = 503 + PIDX_NUMBER;

        public const int PIDX_CapCompareFirmwareVersion = 504 + PIDX_NUMBER;
        public const int PIDX_CapUpdateFirmware = 505 + PIDX_NUMBER;
        #endregion
        
        #region Common String Property Index Values
        // Properties
        public const int PIDX_CheckHealthText = 1 + PIDX_STRING;
        public const int PIDX_DeviceDescription = 2 + PIDX_STRING;
        public const int PIDX_DeviceName = 3 + PIDX_STRING;
        public const int PIDX_ServiceObjectDescription = 4 + PIDX_STRING;
        #endregion
        
        #region Specific Numeric Property Index Values
        // Properties
        public const int PIDXChan_AsyncMode = 1 + PIDX_CHAN + PIDX_NUMBER;
        public const int PIDXChan_AsyncResultCode = 2 + PIDX_CHAN + PIDX_NUMBER;
        public const int PIDXChan_AsyncResultCodeExtended = 3 + PIDX_CHAN + PIDX_NUMBER;
        public const int PIDXChan_CurrentExit = 4 + PIDX_CHAN + PIDX_NUMBER;
        public const int PIDXChan_DeviceExits = 5 + PIDX_CHAN + PIDX_NUMBER;
        public const int PIDXChan_DeviceStatus = 6 + PIDX_CHAN + PIDX_NUMBER;
        public const int PIDXChan_FullStatus = 7 + PIDX_CHAN + PIDX_NUMBER;

        public const int PIDXChan_DepositAmount = 8 + PIDX_CHAN + PIDX_NUMBER;
        public const int PIDXChan_DepositStatus = 9 + PIDX_CHAN + PIDX_NUMBER;

        public const int PIDXChan_CurrentService = 10 + PIDX_CHAN + PIDX_NUMBER;
        public const int PIDXChan_RealTimeDataEnabled = 11 + PIDX_CHAN + PIDX_NUMBER;
        public const int PIDXChan_ServiceCount = 12 + PIDX_CHAN + PIDX_NUMBER;
        public const int PIDXChan_ServiceIndex = 13 + PIDX_CHAN + PIDX_NUMBER;

        // Capabilities
        public const int PIDXChan_CapDiscrepancy = 501 + PIDX_CHAN + PIDX_NUMBER;
        public const int PIDXChan_CapEmptySensor = 502 + PIDX_CHAN + PIDX_NUMBER;
        public const int PIDXChan_CapFullSensor = 503 + PIDX_CHAN + PIDX_NUMBER;
        public const int PIDXChan_CapNearEmptySensor = 504 + PIDX_CHAN + PIDX_NUMBER;
        public const int PIDXChan_CapNearFullSensor = 505 + PIDX_CHAN + PIDX_NUMBER;

        public const int PIDXChan_CapDeposit = 506 + PIDX_CHAN + PIDX_NUMBER;
        public const int PIDXChan_CapDepositDataEvent = 507 + PIDX_CHAN + PIDX_NUMBER;
        public const int PIDXChan_CapPauseDeposit = 508 + PIDX_CHAN + PIDX_NUMBER;
        public const int PIDXChan_CapRepayDeposit = 509 + PIDX_CHAN + PIDX_NUMBER;

        public const int PIDXChan_CapJamSensor = 510 + PIDX_CHAN + PIDX_NUMBER;
        public const int PIDXChan_CapRealTimeData = 511 + PIDX_CHAN + PIDX_NUMBER;
        #endregion

        #region Specific String Property Index Values
        // Properties
        public const int PIDXChan_CurrencyCashList = 1 + PIDX_CHAN + PIDX_STRING;
        public const int PIDXChan_CurrencyCode = 2 + PIDX_CHAN + PIDX_STRING;
        public const int PIDXChan_CurrencyCodeList = 3 + PIDX_CHAN + PIDX_STRING;
        public const int PIDXChan_ExitCashList = 4 + PIDX_CHAN + PIDX_STRING;

        public const int PIDXChan_DepositCashList = 5 + PIDX_CHAN + PIDX_STRING;
        public const int PIDXChan_DepositCodeList = 6 + PIDX_CHAN + PIDX_STRING;
        public const int PIDXChan_DepositCounts = 7 + PIDX_CHAN + PIDX_STRING;
        #endregion

        #region CapPowerReporting, PowerState, PowerNotify Property Constants
        public const int OPOS_PR_NONE = 0;
        public const int OPOS_PR_STANARD = 1;
        public const int OPOS_PR_ADVANCED = 2;

        public const int OPOS_PN_DISABLED = 0;
        public const int OPOS_PN_ENABLED = 1;

        public const int OPOS_PS_UNKNOWN = 2000;
        public const int OPOS_PS_ONLINE = 2001;
        public const int OPOS_PS_OFF = 2002;
        public const int OPOS_PS_OFFLINE = 2003;
        public const int OPOS_PS_OFF_OFFLINE = 2004;
        #endregion

        #region State Property Constants
        public const int OPOS_S_CLOSED = 1;
        public const int OPOS_S_IDLE = 2;
        public const int OPOS_S_BUSY = 3;
        public const int OPOS_S_ERROR = 4;
        #endregion

        #region ResultCode Property Constants
        public const int OPOS_SUCCESS = 0;
        public const int OPOS_E_CLOSED = 101;
        public const int OPOS_E_CLAIMED = 102;
        public const int OPOS_E_NOTCLAIMED = 103;
        public const int OPOS_E_NOSERVICE = 104;
        public const int OPOS_E_DISABLED = 105;
        public const int OPOS_E_ILLEGAL = 106;
        public const int OPOS_E_NOHARDWARE = 107;
        public const int OPOS_E_OFFLINE = 108;
        public const int OPOS_E_NOEXIST = 109;
        public const int OPOS_E_EXISTS = 110;
        public const int OPOS_E_FAILURE = 111;
        public const int OPOS_E_TIMEOUT = 112;
        public const int OPOS_E_BUSY = 113;
        public const int OPOS_E_EXTENDED = 114;
        public const int OPOS_E_DEPRECATED = 115;

        public const int OPOSERR = 100;
        public const int OPOSERREXT = 200;
        #endregion

        #region OPOS ResultCodeExtended Property Constants
        public const int OPOS_ESTATS_ERROR = 280;
        public const int OPOS_ESTATS_DEPENDENCY = 282;
        public const int OPOS_EFIRMWARE_BAD_FILE = 281;
        #endregion

        #region OpenResult Property Constants
        public const int OPOS_OR_ALREADYOPEN = 301;
        public const int OPOS_OR_REGBADNAME = 302;
        public const int OPOS_OR_REGPROGID = 303;
        public const int OPOS_OR_CREATE = 304;
        public const int OPOS_OR_BADIF = 305;
        public const int OPOS_OR_FAILEDOPEN = 306;
        public const int OPOS_OR_BADVERSION = 307;
        public const int OPOS_ORS_NOPORT = 401;
        public const int OPOS_ORS_NOTSUPPORTED = 402;
        public const int OPOS_ORS_CONFIG = 403;
        public const int OPOS_ORS_SPECIFIC = 450;
        #endregion

        #region BinaryConversion Property Constants
        public const int OPOS_BC_NONE = 0;
        public const int OPOS_BC_NIBBLE = 1;
        public const int OPOS_BC_DECIMAL = 2;
        #endregion
        
        #region CheckHealth Method: Level Parameter Constants
        public const int OPOS_CH_INTERNAL = 1;
        public const int OPOS_CH_EXTERNAL = 2;
        public const int OPOS_CH_INTERACTIVE = 3;
        #endregion

        #region CompareFirmwareVersion Method: Result Parameter Constants
        public const int OPOS_CFV_FIRMWARE_OLDER = 1;
        public const int OPOS_CFV_FIRMWARE_SAME = 2;
        public const int OPOS_CFV_FIRMWARE_NEWER = 3;
        public const int OPOS_CFV_FIRMWARE_DIFFERENT = 4;
        public const int OPOS_CFV_FIRMWARE_UNKNOWN = 5;
        #endregion

        #region ErrorEvent Event: ErrorLocus Parameter Constants
        public const int OPOS_EL_OUTPUT = 1;
        public const int OPOS_EL_INPUT = 2;
        public const int OPOS_EL_INPUT_DATA = 3;
        #endregion
        
        #region ErrorEvent Event: ErrorResponse Constants
        public const int OPOS_ER_RETRY = 11;
        public const int OPOS_ER_CLEAR = 12;
        public const int OPOS_ER_CONTINUEINPUT = 13;
        #endregion
       
        #region StatusUpdateEvent Event: Common Status Constants
        public const int OPOS_SUE_POWER_ONLINE = 2001;
        public const int OPOS_SUE_POWER_OFF = 2002;
        public const int OPOS_SUE_POWER_OFFLINE = 2003;
        public const int OPOS_SUE_POWER_OFF_OFFLINE = 2004;

        public const int OPOS_SUE_UF_PROGRESS = 2100;
        public const int OPOS_SUE_UF_COMPLETE = 2200;
        public const int OPOS_SUE_UF_COMPLETE_DEV_NOT_RESTORED = 2205;
        public const int OPOS_SUE_UF_FAILED_DEV_OK = 2201;
        public const int OPOS_SUE_UF_FAILED_DEV_UNRECOVERABLE = 2202;
        public const int OPOS_SUE_UF_FAILED_DEV_NEEDS_FIRMWARE = 2203;
        public const int OPOS_SUE_UF_FAILED_DEV_UNKNOWN = 2204;
        #endregion
        
        #region General Constants
        public const int OPOS_FOREVER = -1;
        #endregion

        #region DeviceStatus and FullStatus Property Constants
        public const int CHAN_STATUS_OK =           0;
        public const int CHAN_STATUS_EMPTY =        11;
        public const int CHAN_STATUS_NEAREMPTY =    12; 
        public const int CHAN_STATUS_EMPTYOK =      13;

        public const int CHAN_STATUS_FULL =         21;
        public const int CHAN_STATUS_NEARFULL =     22;
        public const int CHAN_STATUS_FULLOK =       23; 

        public const int CHAN_STATUS_JAM =          31; 
        public const int CHAN_STATUS_JAMOK =        32;

        public const int CHAN_STATUS_ASYNC =        91;
        #endregion

        #region EndDeposit Method: Success Parameter Constants
        public const int CHAN_DEPOSIT_CHANGE =      1;
        public const int CHAN_DEPOSIT_NOCHANGE =    2;
        public const int CHAN_DEPOSIT_REPAY =       3;
        #endregion

        #region PauseDeposit Method: Control Parameter
        public const int CHAN_DEPOSIT_PAUSE =       11;
        public const int CHAN_DEPOSIT_RESTART =     12;
        #endregion

        #region DepositStatus Property Constants
        public const int CHAN_STATUS_DEPOSIT_START = 1;
        public const int CHAN_STATUS_DEPOSIT_END = 2;
        public const int CHAN_STATUS_DEPOSIT_NONE = 3;
        public const int CHAN_STATUS_DEPOSIT_COUNT = 4;
        public const int CHAN_STATUS_DEPOSIT_JAM = 5;
        #endregion

        #region DispenseStatus Property Constants
        public const int CHAN_STATUS_DISPENSE_START = 1;
        public const int CHAN_STATUS_DISPENSE_END = 2;
        public const int CHAN_STATUS_DISPENSE_NONE = 3;
        public const int CHAN_STATUS_DISPENSE_COUNT = 4;
        public const int CHAN_STATUS_DISPENSE_JAM = 5;
        #endregion

        #region DispenseAll (DirectIO)
        public const int ALL = 0;
        public const int ALL_FORCED = 1;
        public const int ALL_BOCA = 2;
        public const int ALL_BOCA_FORCED = 3;
        public const int ALL_SAFE = 4;
        public const int ALL_SAFE_FORCED = 5;
        #endregion

        #region ResultCodeExtended Property Constants
        public const int OPOS_ECHAN_OVERDISPENSE = 201;
        #endregion
    }
}
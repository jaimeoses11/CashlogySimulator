namespace Cashlogy
{
    public static partial class Const
    {
        public const int DIO_ReadDispensableCashCounts = 1;
        public const int DIO_ReadNotDispensableCashCounts = 2;
        public const int DIO_DispenseCashBillsToStacker = 3;

        public const int DIO_DispenseAmount = 4;
        public const int DIO_DispenseCounts = 5;
        public const int DIO_DispenseStatus = 6;
        public const int DIO_CapPauseDispense = 7;
        public const int DIO_PauseDispense = 8;

        public const int DIO_DispenseAll = 9;

        public const int DIO_ReadCashAvailableCapacity = 10;
        public const int DIO_ReadCashEmptyFullStatus = 11;

        public const int DIO_ResultCodeExtended = 12;
        public const int DIO_ReadResultCodeExtendedInfo = 13;
        public const int DIO_ReadResultCodeExtendedLogs = 14;

        public const int DIO_ReadDevicesCapacities = 15;
        public const int DIO_EnableDepositItem = 16;
        public const int DIO_GetEnableDepositItem = 17;
        public const int DIO_EnableDevice = 18;

        public const int DIO_ManageManualReplenishment = 19;
        public const int DIO_DispenseChangeOnlyCoins = 20;

        public const int DIO_Maintenance = 22;

        public const int DIO_DispenseAllOnlyCoins = 30;
        public const int DIO_ReadStatus = 31;
        public const int DIO_DispenseAllOnlyBills = 32;
        // public const int DIO_FuncParamsGeneric_33 = 33;
        public const int DIO_DispenseAllByItems = 34;
        // ...........................................
        // public const int DIO_FuncParamsGeneric_38 = 38;
        public const int DIO_ReadDeviceInfo = 39;

        public const int DIO_ReadInfoErrorsNoXML = 100;
        public const int DIO_ReadInfoDevicesProvisional = 101;
    }
}
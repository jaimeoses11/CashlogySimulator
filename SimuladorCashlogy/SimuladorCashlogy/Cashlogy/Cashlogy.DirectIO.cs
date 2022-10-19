using Cashlogy.Idiomas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace Cashlogy
{
    public partial class CashlogyDevice
    {
        // Recaudacion de billetes
        private int DIO_ReadDispensableCashCounts(ref string pString)
        {
            if (!st.DeviceEnabled) return SetRC(Const.OPOS_E_ILLEGAL);

            pString = cont.ReadDispensableCashCounts();

            return SetRC(Const.OPOS_SUCCESS);
        }

        private int DIO_ReadNotDispensableCashCounts(ref string pString)
        {
            if (!st.DeviceEnabled) return SetRC(Const.OPOS_E_ILLEGAL);

            pString = cont.ReadNotDispensableCashCounts();

            return SetRC(Const.OPOS_SUCCESS);
        }

        private int DIO_DispenseCashBillsToStacker(ref string pString)
        {
            if (st.Busy) return SetRC(Const.OPOS_E_BUSY);

            int[] items = new int[MAX_ITEMS];
            int r = ParseCashCounts(pString, ref items);
            if (r != Const.OPOS_SUCCESS) return SetRC(Const.OPOS_E_ILLEGAL);

            // Compruebo que tengo los items (aprovecho y calculo el total de monedas)
            int totCoins = 0;
            int itemsToStacker = 0;
            for (int i = 0; i < def.NumItems; i++)
            {
                if(def.ItemsDef[i].IsCoin) totCoins += def.ItemsDef[i].Value * items[i];
                if (items[i] > cont.recyclers[i]) return SetRC(Const.OPOS_E_FAILURE);
                if (def.ItemsDef[i].IsDepositable) itemsToStacker += items[i];
            }
            if (itemsToStacker > (def.CapStacker - cont.totStacker)) return SetRC(Const.OPOS_E_ILLEGAL);

            st.Busy = true;
            st.DispenseInCurse = true;
            //st.DispenseInCursePaused = false;
            //st.DispenseInCurseDepositRepay = false;
            //st.DispenseResultCode = false;
            //st.DispenseResultCodeExtended = false;
            st.State = Const.OPOS_S_BUSY;
            st.DispenseStatus = Const.CHAN_STATUS_DISPENSE_START;

            OnStateChanged();
            OnOrderReceived((int)NumFrase.Dispensando);

            cont.ResetDispensedCash();

            st.DispenseInCurseAmountRequired = totCoins;
            st.DispenseInCurseCountsRequired = (int[])items.Clone();

            if (outCadence)
            {
                auxDispense = (int[])items.Clone();
                auxToStacker = true;
                tmrDispenseCoins.Start();
                tmrDispenseBills.Start();
            }
            else DispenseSinCadencia(items, true);

            return SetRC(Const.OPOS_SUCCESS);
        }

        // Dispense
        private int DIO_DispenseAmount(ref int pData)
        {
            if (!st.Opened) return SetRC(Const.OPOS_E_ILLEGAL);

            pData = cont.totDispensed;

            return SetRC(Const.OPOS_SUCCESS);
        }

        private int DIO_DispenseCounts(ref string pString) 
        {
            if (!st.Opened) return SetRC(Const.OPOS_E_ILLEGAL);

            cont.DispenseCounts();
            pString = cont.dispenseCounts;

            return SetRC(Const.OPOS_SUCCESS);
        }

        private int DIO_DispenseStatus(ref int pData)
        {
            if (!st.DeviceEnabled) return SetRC(Const.OPOS_E_ILLEGAL);

            pData = st.DispenseStatus;

            return SetRC(Const.OPOS_SUCCESS);
        }

        private int DIO_PauseDispense()
        {
            if (!st.DeviceEnabled) return SetRC(Const.OPOS_E_ILLEGAL);

            collectSatckerEnded = true;
            if (OnCollectStackerEnded != null) OnCollectStackerEnded();

            return SetRC(Const.OPOS_SUCCESS);
        }

        private int DIO_DispenseAll(ref int pData)
        {
            if (!st.DeviceEnabled) return SetRC(Const.OPOS_E_ILLEGAL);
            if (st.Busy) return SetRC(Const.OPOS_E_BUSY);

            bool toStack = false;
            if (pData == Const.ALL || pData == Const.ALL_FORCED) toStack = true;
            else if ((pData == Const.ALL_BOCA || pData == Const.ALL_BOCA_FORCED)) toStack = false;

            int totDispense = 0;
            int[] items = new int[MAX_ITEMS];
            int itemsToStacker = 0;
            for (int i = 0; i < def.NumItems; i++)
            {
                if (def.ItemsDef[i].IsDispensable)
                {
                    items[i] = cont.recyclers[i];
                    if (toStack && def.ItemsDef[i].IsBill) itemsToStacker += items[i];
                    else totDispense += cont.recyclers[i] * def.ItemsDef[i].Value;
                }
            }
            if (itemsToStacker > (def.CapStacker - cont.totStacker)) return SetRC(Const.OPOS_E_ILLEGAL);

            st.Busy = true;
            st.DispenseInCurse = true;
            st.State = Const.OPOS_S_BUSY;
            st.DispenseStatus = Const.CHAN_STATUS_DISPENSE_START;

            OnStateChanged();
            OnOrderReceived((int)NumFrase.Dispensando);

            cont.ResetDispensedCash();

            st.DispenseInCurseAmountRequired = totDispense;
            st.DispenseInCurseCountsRequired = (int[])items.Clone();

            if (outCadence)
            {
                auxDispense = (int[])items.Clone();
                auxToStacker = toStack;
                tmrDispenseCoins.Start();
                tmrDispenseBills.Start();
            }
            else DispenseSinCadencia(items, toStack);

            return SetRC(Const.OPOS_SUCCESS);
        }

        // Estado lleno/vacío
        private int DIO_ReadCashAvailableCapacity(ref string pString)
        {
            if (!st.DeviceEnabled) return SetRC(Const.OPOS_E_ILLEGAL);

            int[] items = new int[MAX_ITEMS];
            int stacker = 0;
            cont.GetAviableCapacity(ref items, ref stacker);
            
            pString = GetStringOPOS(items,stacker);

            return SetRC(Const.OPOS_SUCCESS);
        }

        private int DIO_ReadCashEmptyFullStatus(ref string pString) 
        {
            if (!st.DeviceEnabled) return SetRC(Const.OPOS_E_ILLEGAL);

            pString = GetStringOPOS(st.CashEmptyFullStatus, st.StackerEmptyFullStatus);

            return SetRC(Const.OPOS_SUCCESS);
        }

        private int DIO_ResultCodeExtended(ref int pData)
        {
            if (!st.Opened) return SetRC(Const.OPOS_E_ILLEGAL);

            pData = st.ResultCodeExtended;

            return SetRC(Const.OPOS_SUCCESS);
        }

        private int DIO_ReadResultCodeExtendedInfo(ref int pData, ref string pString)
        {
            if (!st.Opened) return SetRC(Const.OPOS_E_ILLEGAL);

            string pathOK = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                                         def.DeviceName + @"\DIOReadResultCodeExtendedInfo_OK.xml");

            pString = XDocument.Load(pathOK).ToString();
            pData = 1;

            return SetRC(Const.OPOS_SUCCESS);
        }

        private int DIO_ReadDevicesCapacities(ref string pString)
        {
            if (!st.Opened) return SetRC(Const.OPOS_E_ILLEGAL);

            int[] items = new int[MAX_ITEMS];
            for (int i = 0; i < def.NumItems; i++)
            {
                if (def.ItemsDef[i].IsDispensable)
                {
                    items[i] = def.ItemsDef[i].Capacity;
                }
            }
            int stacker = def.CapStacker;

            pString = GetStringOPOS(items, stacker);

            return SetRC(Const.OPOS_SUCCESS);   
        }
        
        // Habilitar/deshabilitar admisión de items
        private int DIO_EnableDepositItem(ref string pString)
        {
            bool[] items = st.EnableDepositItems;
            int r = ParseaEnableDepositItems(pString, ref items);
            if (r != Const.OPOS_SUCCESS) return SetRC(Const.OPOS_E_ILLEGAL);

            for (int i = 0; i < def.NumItems; i++)
            {
                st.EnableDepositItems[i] = items[i];
            }
            if (OnDepositItemEnabled != null) OnDepositItemEnabled();

            return SetRC(Const.OPOS_SUCCESS);
        }

        private int DIO_GetEnableDepositItem(ref string pString)
        {
            pString = GetStringOPOS(st.EnableDepositItems);

            return SetRC(Const.OPOS_SUCCESS);
        }

        private int DIO_DispenseChangeOnlyCoins(ref int pData)
        {
            if (!st.DeviceEnabled) return SetRC(Const.OPOS_E_ILLEGAL);
            if (st.Busy) return SetRC(Const.OPOS_E_BUSY);

            int[] items = new int[MAX_ITEMS];
            int r = GetDispenseItems(pData, ref items, true);
            if (r != Const.OPOS_SUCCESS) return SetRC(Const.OPOS_E_FAILURE);

            st.Busy = true;
            st.DispenseInCurse = true;
            //st.DispenseInCursePaused = false;
            //st.DispenseInCurseDepositRepay = false;
            //st.DispenseResultCode = false;
            //st.DispenseResultCodeExtended = false;
            st.State = Const.OPOS_S_BUSY;
            st.DispenseStatus = Const.CHAN_STATUS_DISPENSE_START;

            OnStateChanged();
            OnOrderReceived((int)NumFrase.Dispensando);

            cont.ResetDispensedCash();

            st.DispenseInCurseAmountRequired = pData;
            st.DispenseInCurseCountsRequired = (int[])items.Clone();

            if (outCadence)
            {
                auxDispense = (int[])items.Clone();
                auxToStacker = false;
                tmrDispenseCoins.Start();
            }
            else DispenseSinCadencia(items, false);

            return SetRC(Const.OPOS_SUCCESS);
        }

        private int DIO_DispenseAllOnlyCoins(ref int pData)
        {
            if (!st.DeviceEnabled) return SetRC(Const.OPOS_E_ILLEGAL);
            if (st.Busy) return SetRC(Const.OPOS_E_BUSY);

            st.Busy = true;
            st.DispenseInCurse = true;
            //st.DispenseInCursePaused = false;
            //st.DispenseInCurseDepositRepay = false;
            //st.DispenseResultCode = false;
            //st.DispenseResultCodeExtended = false;
            st.State = Const.OPOS_S_BUSY;
            st.DispenseStatus = Const.CHAN_STATUS_DISPENSE_START;

            OnStateChanged();
            OnOrderReceived((int)NumFrase.Dispensando);

            cont.ResetDispensedCash();

            pData = 0;
            int totCoins = 0;
            int[] items = new int[MAX_ITEMS];
            for (int i = 0; i < def.NumCoins; i++)
            {
                if (def.ItemsDef[i].IsDispensable)
                {
                    totCoins += cont.recyclers[i] * def.ItemsDef[i].Value;
                    items[i] = cont.recyclers[i];
                }
            }

            st.DispenseInCurseAmountRequired = totCoins;
            st.DispenseInCurseCountsRequired = (int[])items.Clone();

            if (outCadence)
            {
                auxDispense = (int[])items.Clone();
                auxToStacker = false;
                tmrDispenseCoins.Start();
            }
            else DispenseSinCadencia(items, false);

            return SetRC(Const.OPOS_SUCCESS);
        }

        private int DIO_ReadStatus(ref string pString)
        {
            if (!st.Opened) return SetRC(Const.OPOS_E_ILLEGAL);

            
            string pathErr = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                                         def.DeviceName + @"\DIOReadStatus_Err.xml");

            string xmlReadStatus = XDocument.Load(pathErr).ToString();
            string change = "<Change>#DescripcionErroresAqui#</Change>";
            string noErrores = "<General>\r\n    <Status id=\"0\">Correcto</Status>\r\n" +
                               "    <NumberOfIssuese>0</NumberOfIssuese>\r\n  </General>";

            if (hayError) pString = xmlReadStatus.Replace(change, xmlErrores);
            else pString = xmlReadStatus.Replace(change, noErrores);

            return SetRC(Const.OPOS_SUCCESS);
        }

        private int DIO_DispenseAllOnlyBills(ref int pData)
        {
            if (!st.DeviceEnabled) return SetRC(Const.OPOS_E_ILLEGAL);
            if (st.Busy) return SetRC(Const.OPOS_E_BUSY);

            bool toStack = false;
            if (pData == Const.ALL || pData == Const.ALL_FORCED) toStack = true;
            else if ((pData == Const.ALL_BOCA || pData == Const.ALL_BOCA_FORCED)) toStack = false;

            int totBills = 0;
            int[] items = new int[MAX_ITEMS];
            int itemsToStacker = 0;
            for (int i = def.NumCoins; i < def.NumItems; i++)
            {
                if (def.ItemsDef[i].IsDispensable)
                {
                    totBills += cont.recyclers[i] * def.ItemsDef[i].Value;
                    items[i] = cont.recyclers[i];
                    if (toStack) itemsToStacker += items[i];
                }
            }
            if (itemsToStacker > (def.CapStacker - cont.totStacker)) return SetRC(Const.OPOS_E_ILLEGAL);

            st.Busy = true;
            st.DispenseInCurse = true;
            //st.DispenseInCursePaused = false;
            //st.DispenseInCurseDepositRepay = false;
            //st.DispenseResultCode = false;
            //st.DispenseResultCodeExtended = false;
            st.State = Const.OPOS_S_BUSY;
            st.DispenseStatus = Const.CHAN_STATUS_DISPENSE_START;

            OnStateChanged();
            OnOrderReceived((int)NumFrase.Dispensando);

            cont.ResetDispensedCash();

            pData = 0;
            
            st.DispenseInCurseAmountRequired = totBills;
            st.DispenseInCurseCountsRequired = (int[])items.Clone();

            if (outCadence)
            {
                auxDispense = (int[])items.Clone();
                auxToStacker = toStack;
                tmrDispenseBills.Start();
            }
            else DispenseSinCadencia(items, toStack);

            return SetRC(Const.OPOS_SUCCESS);
        }

        private int DIO_DispenseAllByItems(ref int pData, ref string pString)
        {
            if (!st.DeviceEnabled) return SetRC(Const.OPOS_E_ILLEGAL);
            if (st.Busy) return SetRC(Const.OPOS_E_BUSY);

            bool toStack = false;
            if (pData == Const.ALL || pData == Const.ALL_FORCED) toStack = true;
            else if ((pData == Const.ALL_BOCA || pData == Const.ALL_BOCA_FORCED)) toStack = false;

            int[] dispenseItems= null;
            int r = ParseDispenseAllByItems(pString,ref dispenseItems);
            if (r != Const.OPOS_SUCCESS) return SetRC(Const.OPOS_E_ILLEGAL);

            int totDispense = 0;
            int[] items = new int[MAX_ITEMS];
            int itemsToStacker = 0;
            for (int i = 0; i < dispenseItems.Length; i++)
            {
                int j = dispenseItems[i];
                if (def.ItemsDef[j].IsDispensable)
                {
                    items[j] = cont.recyclers[j];
                    if (toStack && def.ItemsDef[j].IsBill) itemsToStacker += items[j];
                    else totDispense += cont.recyclers[j] * def.ItemsDef[j].Value;
                }
            }
            if (itemsToStacker > (def.CapStacker - cont.totStacker)) return SetRC(Const.OPOS_E_ILLEGAL);

            st.Busy = true;
            st.DispenseInCurse = true;
            //st.DispenseInCursePaused = false;
            //st.DispenseInCurseDepositRepay = false;
            //st.DispenseResultCode = false;
            //st.DispenseResultCodeExtended = false;
            st.State = Const.OPOS_S_BUSY;
            st.DispenseStatus = Const.CHAN_STATUS_DISPENSE_START;

            OnStateChanged();
            OnOrderReceived((int)NumFrase.Dispensando);

            cont.ResetDispensedCash();

            pData = 0;

            st.DispenseInCurseAmountRequired = totDispense;
            st.DispenseInCurseCountsRequired = (int[])items.Clone();

            if (outCadence)
            {
                auxDispense = (int[])items.Clone();
                auxToStacker = toStack;
                tmrDispenseCoins.Start();
                tmrDispenseBills.Start();
            }
            else DispenseSinCadencia(items, toStack);

            return SetRC(Const.OPOS_SUCCESS);
        }

        #region Funciones Auxiliares
        public string GetStringOPOS(int[] items, int stacker) 
        {
            string pString = "";
            bool flagComa = false;
            bool flagBills = false;
            for (int i = 0; i < def.NumItems; i++)
            {
                if (def.ItemsDef[i].IsDispensable)
                {
                    if (def.ItemsDef[i].IsBill && !flagBills)
                    {
                        flagBills = true;
                        flagComa = false;
                        pString += ";";
                    }

                    if (flagComa) pString += ",";
                    else flagComa = true;
                    pString += def.ItemsDef[i].Value + ":" + items[i];
                }
            }
            // Si param "stacker" es -1 (menor que 0), no se imprime en la cadena
            if (stacker >= 0) pString += ",STACKER:" + stacker;

            return pString;
        }

        public string GetStringOPOS(bool[] items)
        {
            string pString = "";
            bool flagComa = false;
            bool flagBills = false;
            for (int i = 0; i < def.NumItems; i++)
            {
                if (def.ItemsDef[i].IsBill && !flagBills)
                {
                    flagBills = true;
                    flagComa = false;
                    pString += ";";
                }

                if (flagComa) pString += ",";
                else flagComa = true;
                pString += def.ItemsDef[i].Value + ":";
                if (items[i]) pString += "1";
                else pString += "0";
            }

            return pString;
        }

        private int ParseDispenseAllByItems(string cashCounts, ref int[] list)
        {
            List<int> _list = new List<int>();
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
                    int coinValue;
                    try
                    {
                        coinValue = Convert.ToInt32(coinList[i]);
                    }
                    catch (Exception) { return Const.OPOS_E_ILLEGAL; }

                    correctValue = false;
                    for (int j = 0; j < def.NumCoins; j++)
                    {
                        if (coinValue == def.ItemsDef[j].Value && j != k)
                        {
                            _list.Add(j);
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
                        int billValue;
                        try
                        {
                            billValue = Convert.ToInt32(billList[i]);
                        }
                        catch (Exception) { return Const.OPOS_E_ILLEGAL; }

                        correctValue = false;
                        for (int j = def.NumCoins; j < def.NumItems; j++)
                        {
                            if (billValue == def.ItemsDef[j].Value && j != k)
                            {
                                _list.Add(j);
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

            list = _list.ToArray();

            return Const.OPOS_SUCCESS;
        }

        public int ParseaEnableDepositItems(string cashCounts, ref bool[] list)
        {
            if (cashCounts == "") return Const.OPOS_E_ILLEGAL; // Duda

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
                    int enableCoin;
                    try
                    {
                        coinValue = Convert.ToInt32(coin[0]);
                        enableCoin = Convert.ToInt32(coin[1]);
                    }
                    catch (Exception) { return Const.OPOS_E_ILLEGAL; }

                    correctValue = false;
                    for (int j = 0; j < def.NumCoins; j++)
                    {
                        if (coinValue == def.ItemsDef[j].Value && j != k)
                        {
                            if (enableCoin == 1) list[j] = true;
                            else if (enableCoin == 0) list[j] = false;
                            else return Const.OPOS_E_ILLEGAL;
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
                        int enableBill;
                        try
                        {
                            billValue = Convert.ToInt32(bill[0]);
                            enableBill = Convert.ToInt32(bill[1]);
                        }
                        catch (Exception) { return Const.OPOS_E_ILLEGAL; }

                        correctValue = false;
                        for (int j = def.NumCoins; j < def.NumItems; j++)
                        {
                            if (billValue == def.ItemsDef[j].Value && j != k)
                            {
                                if (enableBill == 1) list[j] = true;
                                else if (enableBill == 0) list[j] = false;
                                else return Const.OPOS_E_ILLEGAL;
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
        #endregion
    }
}
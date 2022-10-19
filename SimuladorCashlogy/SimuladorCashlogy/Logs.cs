using Cashlogy;
using Cashlogy.SocketOPOS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace LogsSimulador
{
    public class Logs
    {
        private string path;
        private string saltoLinea = "\n";
        private string tab = "\t";
        private string Function;

        public Logs()
        {
            path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Logs");
        }

        public void Add(object write)
        {
            CreateDirectory();
            string fileName = GetFileName();
            string log;

            string date = GetDate();
            int code = 0;
            List<object> Params = new List<object>();

            bool request = false;
            if (write is RequestOPOS)
            {
                RequestOPOS req = (RequestOPOS)write;
                request = true;
                Function = req.Function;
                Params = req.Params;
            }
            else if (write is ResponseOPOS)
            {
                ResponseOPOS resp = (ResponseOPOS)write;
                request = false;
                code = resp.Code;
                Params = resp.Params;
            }

            #region Escribir Logs
            if (request)
            {
                if (Function.Length < 8) log = date + " " + Function + tab;
                else log = date + " " + Function;

                switch (Function)
                {
                    case "GetProperty":
                        log += tab + Params[0].ToString() + saltoLinea;
                        break;

                    case "SetProperty":
                        log += tab + Params[0].ToString() + " -> value=" + Params[1].ToString()+ saltoLinea;
                        break;

                    case "DirectIO":
                        int command = (int)Params[0];
                        string directIO = GetDirectIO(command);
                        log += tab + tab + directIO + "(cmd=" + command + ")" + "   " + "pData=\"" + Params[1].ToString() +
                               "\"   pString=\""+ Params[2].ToString() + "\"" + saltoLinea;
                        break;

                    default:
                        for (int i = 0; i < Params.Count; i++)
                        {
                            if (i == 0) log += tab + "[";
                            else if (i > 0 && i < Params.Count) log += ", ";
                            log += "\"" + Params[i].ToString() + "\"";
                            if (i == (Params.Count - 1)) log += "]";
                        }
                        log += saltoLinea;
                        break;
                }
            }
            else
            {
                log = date + tab + tab + tab + "RC= " + GetStrRC(code) + "(" + code + ");";
                switch (Function)
                {
                    case "GetProperty":
                        log += "   <" + Params[1].ToString() + ">" + saltoLinea;
                        break;

                    case "SetProperty":
                        log += "   <" + Params[1].ToString() + ">" + saltoLinea;
                        break;

                    case "DirectIO":
                        int command = (int)Params[0];
                        if (command != Const.DIO_ReadStatus && command != Const.DIO_ReadResultCodeExtendedInfo)
                        {
                            string directIO = GetDirectIO(command);
                            log += "   <pData=\"" + Params[1].ToString() + "\", pString=\"" + Params[2].ToString() +
                                   "\">" + saltoLinea;
                        }
                        else log += saltoLinea;
                        break;

                    case "FixDeposit":
                        log += "   <DepositAmount=\""+ Params[1].ToString() + "\", DepositCounts=\"" + 
                               Params[0].ToString() + "\">" + saltoLinea;
                        break;

                    default:
                        for (int i = 0; i < Params.Count; i++)
                        {
                            if (i == 0) log += "   <";
                            else if (i > 0 && i < Params.Count) log += ", ";
                            log += "\"" + Params[i].ToString() + "\"";
                            if (i == (Params.Count - 1)) log += ">";
                        }
                        log += saltoLinea;
                        break;
                }
            }
            #endregion

            StreamWriter sw = new StreamWriter(path + @"\" + fileName, true);
            sw.Write(log);
            sw.Close();
        }

        #region Funciones Auxiliares
        private void CreateDirectory()
        {
            try
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            }
            catch { }
            
        }

        private string GetFileName()
        {
            string fileName = "Log_";

            fileName += DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString("D2") + DateTime.Now.Day.ToString("D2") + ".txt";

            return fileName;
        }

        private string GetDate()
        {
            string date = "[" + DateTime.Now.Hour.ToString("D2") + ":" + DateTime.Now.Minute.ToString("D2") + ":" +
                          DateTime.Now.Second.ToString("D2")+ "." + DateTime.Now.Millisecond.ToString("D3") + "]";

            return date;
        }

        private string GetDirectIO(int command)
        {
            switch (command)
            {
                case Const.DIO_ReadDispensableCashCounts:
                    return "ReadDispensableCashCounts";
                case Const.DIO_ReadNotDispensableCashCounts:
                    return "ReadNotDispensableCashCounts";

                case Const.DIO_DispenseCashBillsToStacker:
                    return "DispenseCashBillsToStacker";

                // Dispense
                case Const.DIO_DispenseAmount:
                    return "DispenseAmount";

                case Const.DIO_DispenseCounts:
                    return "DispenseCounts";

                case Const.DIO_DispenseStatus:
                    return "DispenseStatus";

                case Const.DIO_CapPauseDispense:
                   return "CapPauseDispense";

                case Const.DIO_PauseDispense:
                    return "PauseDispense";

                case Const.DIO_DispenseAll:
                    return "DispenseAll";

                // Estado lleno/vacío
                case Const.DIO_ReadCashAvailableCapacity:
                    return "ReadCashAvailableCapacity";

                case Const.DIO_ReadCashEmptyFullStatus:
                    return "ReadCashEmptyFullStatus";

                case Const.DIO_ResultCodeExtended:
                    return "ResultCodeExtended";

                case Const.DIO_ReadResultCodeExtendedInfo:
                    return "ReadResultCodeExtendedInfo";

                case Const.DIO_ReadDevicesCapacities:
                    return "ReadDevicesCapacities";

                // Habilitar/deshabilitar admisión de items
                case Const.DIO_EnableDepositItem:
                    return "EnableDepositItem";

                case Const.DIO_GetEnableDepositItem:
                    return "GetEnableDepositItem";

                //case Const.DIO_EnableDevice:
                //    return "EnableDevice";

                case Const.DIO_DispenseChangeOnlyCoins:
                    return "DispenseChangeOnlyCoins";

                case Const.DIO_DispenseAllOnlyCoins:
                    return "DispenseAllOnlyCoins";

                case Const.DIO_ReadStatus:
                    return "ReadStatus";

                case Const.DIO_DispenseAllOnlyBills:
                    return "DispenseAllOnlyBills";

                case Const.DIO_DispenseAllByItems:
                    return "DispenseAllByItems";

                default:
                    return "NotImplemented";
            }
        }

        private string GetStrRC(int rc)
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
        #endregion
    }
}

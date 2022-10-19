using Cashlogy.SocketOPOS;
using Cashlogy.Vistas;
using Microsoft.Win32;
using SimuladorCashlogy;
using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Cashlogy
{
    class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            var exes = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length;

            if (exes > 1) return; // Ya hay una instancia de la app en ejecucuión

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            int num = -1;
            string str = "";
            string keyName = @"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\OLEforRetail\ServiceOPOS" +
                             @"\CashChanger\CashlogySimulator";
            try
            {
                str = (string)Registry.GetValue(keyName, "Host", "NotFound");
                num = (int)Registry.GetValue(keyName, "Port", -1);
            }
            catch { }

            string host;
            int portOPOS;
            if (str == "NotFound" || str == "") host = "localhost";
            else host = str;
            if (num == -1 || num < IPEndPoint.MinPort || num > IPEndPoint.MaxPort) portOPOS = 8091;
            else portOPOS = num;
            ServerOPOS serv = new ServerOPOS(host, portOPOS);

            Config config = new Config(args);
            Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
            CashlogyDevice theCashlogy = new CashlogyDevice(serv, dispatcher, config);

            serv.StartListening();
            Application.Run(new MainFormSimulador(theCashlogy, config));
        }
    }
}
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Cashlogy.Idiomas
{
    public enum NumFrase
    {
        Actual,
        Idioma,
        SeleccIdioma,
        Estado,
        Cerrado,
        Reposo,
        Ocupado,
        Error,
        SimCashlogy,
        BtnCashlogy,
        BtnConfigCont,
        BtnSimErr,
        BtnOrdenesOPOS,
        BtnSettings,
        MboxNoDeviceOpened,
        ModoConfig,
        DispClosed,
        WaitOpen,
        Contabilidad,
        Almacen,
        Stacker,
        Devolucion,
        ConfigCont,
        PonerTodoA0,
        Aceptar,
        Admitido,
        Devuelto,
        OpenStack,
        CloseStack,
        ValEfectivo,
        Abierto,
        Reclamado,
        Habilitado,
        Admision,
        Inhibido,
        Rellenar,
        OneCoin,
        OneBill,
        Aleatorio,
        Add,
        MboxConfigErr,
        ForzarErr,
        LimpiarErr,
        SinIniciar,
        RecOpen,
        RecClaim,
        RecEnable,
        RecRelease,
        Depositando,
        RecEndDeposit,
        RecPauseDeposit,
        Dispensando,
        DevolEnd,        
        ConfiguracionErr,
        Config,
        EnableInCad,
        InCoins_ms,
        InBills_ms,
        CondInCad,
        EnableOutCad,
        OutCoins_ms,
        OutBills_ms,
        CondOutCad,
        ModoRun,
        ConfigRegistro,
        EnableSim,
        DispList,
        ConfigServer,
        EnableServerConfig        
    };

    public class Idioma
    {
        private List<string> frasesIdioma;

        public List<string> FrasesIdioma { get => frasesIdioma; }

        public Idioma()
        {
            frasesIdioma = new List<string>();
        }

        public void ConfiguraIdioma(string code)
        {
            frasesIdioma.Clear();
            string idioma = GetIdioma(code);
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), 
                                       @"Idioma\" + idioma + ".txt");

            StreamReader txt = new StreamReader(path);
            string str = txt.ReadLine();
            frasesIdioma.Add(str);
            while (str != null)
            {
                str = txt.ReadLine();
                if(str != null)
                {
                    var result = str.Split('\t');
                    frasesIdioma.Add(result[1]);
                }
            }
        }

        private string GetIdioma(string code)
        {
            string idioma = "";
            switch (code)
            {
                case "ES":
                    idioma = "Español";
                    break;
                case "EN":
                    idioma = "Ingles";
                    break;
                case "FR":
                    idioma = "Frances";
                    break;
                case "NL":
                    idioma = "Neerlandes";
                    break;
                case "DE":
                    idioma = "Aleman";
                    break;
                case "IT":
                    idioma = "Italiano";
                    break;
            }
            return idioma;
        }
    }
}
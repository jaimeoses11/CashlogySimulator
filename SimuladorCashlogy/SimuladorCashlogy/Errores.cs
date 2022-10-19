using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace SimuladorCashlogy
{
    public class Error
    {
        public int Code;
        public int Severity;
        public string DateTime;
        public int Module;
        public string Description;

        public Error() { }

        public Error(int code, int severity, string date, int module, string descr)
        {
            Code = code;
            Severity = severity;
            DateTime = date;
            Module = module;
            Description = descr;
        }
    }

    public class Errores
    {
        public List<Error> list;
        public string xmlErrores;

        public Errores()
        {
            list = new List<Error>();
            xmlErrores = "";
        }

        public void ToXml()
        {
            int num = list.Count;
            StringWriter strXml = new StringWriter();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            XmlWriter xmlWriter = XmlWriter.Create(strXml,settings);

            #region EscribleXml
            xmlWriter.WriteStartElement("Borrar");
            xmlWriter.WriteStartElement("General");

            xmlWriter.WriteStartElement("Status");
            xmlWriter.WriteAttributeString("id", "100");
            xmlWriter.WriteString("Aviso");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("NumberOfIssuese");
            xmlWriter.WriteString(num.ToString());
            xmlWriter.WriteEndElement();

            for (int i = 0; i < list.Count; i++)
            {
                xmlWriter.WriteStartElement("Issue");

                xmlWriter.WriteStartElement("Code");
                xmlWriter.WriteString(list[i].Code.ToString());
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("Severity");
                xmlWriter.WriteAttributeString("id", list[i].Severity.ToString());
                xmlWriter.WriteString(SeverityStr(list[i].Severity));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("DateTime");
                xmlWriter.WriteString(list[i].DateTime);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("Module");
                xmlWriter.WriteAttributeString("id", list[i].Module.ToString());
                xmlWriter.WriteString(ModuleStr(list[i].Module));
                xmlWriter.WriteEndElement();

                xmlWriter.WriteStartElement("Description");
                xmlWriter.WriteString(list[i].Description);
                xmlWriter.WriteEndElement();

                xmlWriter.WriteEndElement();
            }
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            #endregion

            string s = strXml.ToString();
            s = s.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<Borrar>\r\n  ", "");
            xmlErrores = s.Replace("\r\n</Borrar>", "");
        }

        private string SeverityStr(int severity)
        {
            switch (severity)
            {
                case 0:
                    return "OK";
                case 100:
                    return "Warning";
                case 200:
                    return "Error";
                default:
                    return "";
            }
        }

        private string ModuleStr(int module)
        {
            switch (module)
            {
                case 10:
                    return "Unknown";
                case 0:
                    return "None";
                case 1:
                    return "Configuración";
                case 2:
                    return "HardwareGeneral";
                case 3:
                    return "Status";
                case 11:
                    return "Bulk";
                case 12:
                    return "ModuloAdmision";
                case 13:
                    return "Billetes";
                default:
                    return "";
            }
        }
    }
}

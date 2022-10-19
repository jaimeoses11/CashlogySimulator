using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Cashlogy.SocketOPOS
{
    public class ResponseOPOS
    {
        public string Response;
        public int ID;
        public int Code;
        public List<object> Params;

        public ResponseOPOS()
        {
            Params = new List<object>();
        }
        public ResponseOPOS(int id, int code, List<object> paramList)
        {
            ID = id;
            Code = code;
            Params = paramList;
            ToXml();
        }

        private void ToXml()
        {
            StringWriter strXml = new StringWriter();
            XmlWriter xmlWriter = XmlWriter.Create(strXml);

            xmlWriter.WriteProcessingInstruction("xml", "version =\"1.0\" encoding =\"utf-8\"");

            xmlWriter.WriteStartElement("response");
            xmlWriter.WriteAttributeString("id", ID.ToString());

            xmlWriter.WriteStartElement("code");
            xmlWriter.WriteString(Code.ToString());
            xmlWriter.WriteEndElement();

            for (int i = 0; i < Params.Count; i++)
            {
                xmlWriter.WriteStartElement("param");
                xmlWriter.WriteAttributeString("order", (i + 1).ToString());
                string str = "";
                if (Params[i].GetType().ToString() == "System.String") str = "string";
                else if (Params[i].GetType().ToString() == "System.Int32") str = "int";
                xmlWriter.WriteAttributeString("type", str);
                xmlWriter.WriteString(Params[i].ToString());
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndDocument();
            xmlWriter.Close();

            Response = strXml.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Xml;

namespace Cashlogy.SocketOPOS
{
    public class RequestOPOS
    {
        public string Request;
        public int ID;
        public string Function;
        public List<object> Params;

        public RequestOPOS(string request)
        {
            Request = request;
            Params = new List<object>();
            Parse();
        }

        private void Parse()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(Request);

            #region ParsearNodosXML
            foreach (XmlNode xmlNode in xmlDoc)
            {
                if (xmlNode.Name == "request")
                {
                    ID = Convert.ToInt32(xmlNode.Attributes["id"].Value);
                    foreach (XmlNode xmlNode1 in xmlNode)
                    {
                        switch (xmlNode1.Name)
                        {
                            case "function":
                                Function = xmlNode1.InnerText;
                                break;

                            case "param":
                                switch (xmlNode1.Attributes["type"].Value)
                                {
                                    case "int":
                                        Params.Add(Convert.ToInt32(xmlNode1.InnerText));
                                        break;
                                    case "string":
                                        Params.Add(xmlNode1.InnerText);
                                        break;
                                }
                                break;
                        }
                    }
                }
            }
            #endregion
        }
    }
}

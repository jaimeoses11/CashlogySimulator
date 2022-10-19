using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Cashlogy
{
    public partial class CashlogyDevice
    {
        public class CashlogyDef
        {
            public enum VarItemDef
            {
                isCoin = 0,
                isBill = 1,
                isDepositable = 2,
                isDispensable = 3,
                name = 4,
                value = 5,
                image = 6,
                capacity = 7,
                porNearFull = 8,
                porNearEmpty = 9
            }

            public class ItemDef
            {
                private bool isCoin;
                private bool isBill;
                private bool isDepositable;
                private bool isDispensable;
                private string name;
                private int value;
                private string image;
                private int capacity;
                private float porNearFull;
                private float porNearEmpty;

                #region "Encapsulado"
                public bool IsCoin { get => isCoin; }
                public bool IsBill { get => isBill; }
                public bool IsDepositable { get => isDepositable; }
                public bool IsDispensable { get => isDispensable; }
                public string Name { get => name; }
                public int Value { get => value; }
                public string Image { get => image; }
                public int Capacity { get => capacity; }
                public float PorNearFull { get => porNearFull; }
                public float PorNearEmpty { get => porNearEmpty; }
                #endregion

                public ItemDef()
                {
                    isCoin = false;
                    isBill = false;
                    isDepositable = false;
                    isDispensable = false;
                    name = "";
                    value = 0;
                    image = "";
                    capacity = 0;
                }

                public void SetItemDef(int selecc, bool True)
                {
                    switch (selecc)
                    {
                        case (int)VarItemDef.isCoin:
                            isCoin = True;
                            break;
                        case (int)VarItemDef.isBill:
                            isBill = True;
                            break;
                        case (int)VarItemDef.isDepositable:
                            isDepositable = True;
                            break;
                        case (int)VarItemDef.isDispensable:
                            isDispensable = True;
                            break;
                    }
                }
                public void SetItemDef(int selecc, string str)
                {
                    switch (selecc)
                    {
                        case (int)VarItemDef.name:
                            name = str;
                            break;
                        case (int)VarItemDef.image:
                            image = str;
                            break;
                    }
                }
                public void SetItemDef(int selecc, int i)
                {
                    switch (selecc)
                    {
                        case (int)VarItemDef.value:
                            value = i;
                            break;
                        case (int)VarItemDef.capacity:
                            capacity = i;
                            break;
                    }
                }
                public void SetItemDef(int selecc, float f)
                {
                    switch (selecc)
                    {
                        case (int)VarItemDef.porNearFull:
                            porNearFull = f;
                            break;
                        case (int)VarItemDef.porNearEmpty:
                            porNearEmpty = f;
                            break;
                    }
                }
            }

            #region Atributos Definición CashChanger
            private string deviceName;
            private string currencyCode;
            private string symbol;
            private int unitValue;
            private int numItems;
            private int numCoins;
            private int numBills;
            private int capStacker;
            private float porNearFullStacker;
            private float porNearEmptyStacker;
            List<ItemDef> itemsDef;
            #endregion

            #region Encapsulado
            public string DeviceName { get => deviceName; }
            public string CurrencyCode { get => currencyCode; }
            public string Symbol { get => symbol; }
            public int Unitvalue { get => unitValue; }
            public int NumItems { get => numItems; }
            public int NumCoins { get => numCoins; }
            public int NumBills { get => numBills; }
            public int CapStacker { get => capStacker; }
            public float PorNearFullSt { get => porNearFullStacker; }
            public float PorNearEmptySt { get => porNearEmptyStacker; }
            public List<ItemDef> ItemsDef { get => itemsDef; }
            #endregion

            public CashlogyDef()
            {
                deviceName = "";
                currencyCode = "";
                symbol = "";
                unitValue = 0;
                numItems = 0;
                numCoins = 0;
                numBills = 0;
                capStacker = 0;
                porNearFullStacker = 0;
                porNearEmptyStacker = 0;
                itemsDef = new List<ItemDef>();
            }

            #region Metodos Definición CashChanger
            public void ReadFromFile(string logicalDeviceName)
            {
                deviceName = logicalDeviceName;
                string ConfigPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                                                     logicalDeviceName + @"\cashlogyConfig.xml");

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigPath);

                ItemDef it;
                //int cont;
                itemsDef.Clear();

                #region ParsearNodosXML
                foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes)
                {
                    foreach (XmlNode xmlNode1 in xmlNode.ChildNodes)
                    {
                        switch (xmlNode1.Name)
                        {
                            case "EUR":
                                //cont = 0;
                                foreach (XmlNode xmlNode2 in xmlNode1.ChildNodes)
                                {
                                    switch (xmlNode2.Name)
                                    {
                                        case "Code":
                                            currencyCode = xmlNode2.InnerText;
                                            break;
                                        case "symbol":
                                            symbol = xmlNode2.InnerText;
                                            break;
                                        case "unitvalue":
                                            if (xmlNode2.InnerText != "") unitValue = Convert.ToInt32(xmlNode2.InnerText);
                                            else unitValue = 0;
                                            break;
                                        case "Coins":
                                            if (xmlNode2.InnerText != "") numCoins = Convert.ToInt32(xmlNode2.InnerText);
                                            else numCoins = 0;
                                            break;
                                        case "Bills":
                                            if (xmlNode2.InnerText != "") numBills = Convert.ToInt32(xmlNode2.InnerText);
                                            else numBills = 0;
                                            break;
                                        case "TotalItems":
                                            if (xmlNode2.InnerText != "") numItems = Convert.ToInt32(xmlNode2.InnerText);
                                            else numItems = 0;
                                            break;
                                        case "CapStacker":
                                            if (xmlNode2.InnerText != "") capStacker = Convert.ToInt32(xmlNode2.InnerText);
                                            else capStacker = 0;
                                            break;
                                        case "PorNearFullSt":
                                            if (xmlNode2.InnerText != "") porNearFullStacker = Convert.ToInt32(xmlNode2.InnerText)/100f;
                                            else porNearFullStacker = 0;
                                            break;
                                        case "PorNearEmptySt":
                                            if (xmlNode2.InnerText != "") porNearEmptyStacker = Convert.ToInt32(xmlNode2.InnerText)/100f;
                                            else porNearEmptyStacker = 0;
                                            break;
                                        case "CashInfo":
                                            it = new ItemDef();
                                            foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
                                            {
                                                switch (xmlNode3.Name)
                                                {
                                                    case "Denomination":
                                                        it.SetItemDef((int)VarItemDef.name, xmlNode3.InnerText);
                                                        break;
                                                    case "Value":
                                                        if (xmlNode3.InnerText != "") it.SetItemDef((int)VarItemDef.value, Convert.ToInt32(xmlNode3.InnerText));
                                                        else it.SetItemDef((int)VarItemDef.value, 0);
                                                        break;
                                                    case "Type":
                                                        if (xmlNode3.InnerText == "Coin")
                                                        {
                                                            it.SetItemDef((int)VarItemDef.isCoin, true);
                                                            it.SetItemDef((int)VarItemDef.isBill, false);
                                                        }
                                                        else if (xmlNode3.InnerText == "Bill")
                                                        {
                                                            it.SetItemDef((int)VarItemDef.isCoin, false);
                                                            it.SetItemDef((int)VarItemDef.isBill, true);
                                                        }
                                                        break;
                                                    case "Image":
                                                        it.SetItemDef((int)VarItemDef.image, xmlNode3.InnerText);
                                                        break;
                                                    case "Dispensed":
                                                        if (xmlNode3.InnerText != "") it.SetItemDef((int)VarItemDef.isDispensable, true);
                                                        else it.SetItemDef((int)VarItemDef.isDispensable, false);
                                                        break;
                                                    case "StoredIn":
                                                        if (xmlNode3.InnerText != "") it.SetItemDef((int)VarItemDef.isDepositable, true);
                                                        else it.SetItemDef((int)VarItemDef.isDepositable, false);
                                                        break;
                                                    case "Capacity":
                                                        if (xmlNode3.InnerText != "") it.SetItemDef((int)VarItemDef.capacity, Convert.ToInt32(xmlNode3.InnerText));
                                                        else it.SetItemDef((int)VarItemDef.capacity, 0);
                                                        break;
                                                    case "PorNearFull":
                                                        if (xmlNode3.InnerText != "") it.SetItemDef((int)VarItemDef.porNearFull, Convert.ToInt32(xmlNode3.InnerText)/100f);
                                                        else it.SetItemDef((int)VarItemDef.porNearFull, 0);
                                                        break;
                                                    case "PorNearEmpty":
                                                        if (xmlNode3.InnerText != "") it.SetItemDef((int)VarItemDef.porNearEmpty, Convert.ToInt32(xmlNode3.InnerText)/100f);
                                                        else it.SetItemDef((int)VarItemDef.porNearEmpty, 0);
                                                        break;
                                                }
                                            }
                                            itemsDef.Add(it);
                                            //itemsDef[cont] = it;
                                            //cont++;
                                            break;
                                    }
                                }
                                break;
                            case "USD":
                                //cont = 0;
                                foreach (XmlNode xmlNode2 in xmlNode1.ChildNodes)
                                {
                                    switch (xmlNode2.Name)
                                    {
                                        case "Code":
                                            currencyCode = xmlNode2.InnerText;
                                            break;
                                        case "symbol":
                                            symbol = xmlNode2.InnerText;
                                            break;
                                        case "unitvalue":
                                            if (xmlNode2.InnerText != "") unitValue = Convert.ToInt32(xmlNode2.InnerText);
                                            else unitValue = 0;
                                            break;
                                        case "Coins":
                                            if (xmlNode2.InnerText != "") numCoins = Convert.ToInt32(xmlNode2.InnerText);
                                            else numCoins = 0;
                                            break;
                                        case "Bills":
                                            if (xmlNode2.InnerText != "") numBills = Convert.ToInt32(xmlNode2.InnerText);
                                            else numBills = 0;
                                            break;
                                        case "TotalItems":
                                            if (xmlNode2.InnerText != "") numItems = Convert.ToInt32(xmlNode2.InnerText);
                                            else numItems = 0;
                                            break;
                                        case "CapStacker":
                                            if (xmlNode2.InnerText != "") capStacker = Convert.ToInt32(xmlNode2.InnerText);
                                            else capStacker = 0;
                                            break;
                                        case "PorNearFullSt":
                                            if (xmlNode2.InnerText != "") porNearFullStacker = Convert.ToInt32(xmlNode2.InnerText) / 100f;
                                            else porNearFullStacker = 0;
                                            break;
                                        case "PorNearEmptySt":
                                            if (xmlNode2.InnerText != "") porNearEmptyStacker = Convert.ToInt32(xmlNode2.InnerText) / 100f;
                                            else porNearEmptyStacker = 0;
                                            break;
                                        case "CashInfo":
                                            it = new ItemDef();
                                            foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
                                            {
                                                switch (xmlNode3.Name)
                                                {
                                                    case "Denomination":
                                                        it.SetItemDef((int)VarItemDef.name, xmlNode3.InnerText);
                                                        break;
                                                    case "Value":
                                                        if (xmlNode3.InnerText != "") it.SetItemDef((int)VarItemDef.value, Convert.ToInt32(xmlNode3.InnerText));
                                                        else it.SetItemDef((int)VarItemDef.value, 0);
                                                        break;
                                                    case "Type":
                                                        if (xmlNode3.InnerText == "Coin")
                                                        {
                                                            it.SetItemDef((int)VarItemDef.isCoin, true);
                                                            it.SetItemDef((int)VarItemDef.isBill, false);
                                                        }
                                                        else if (xmlNode3.InnerText == "Bill")
                                                        {
                                                            it.SetItemDef((int)VarItemDef.isCoin, false);
                                                            it.SetItemDef((int)VarItemDef.isBill, true);
                                                        }
                                                        break;
                                                    case "Image":
                                                        it.SetItemDef((int)VarItemDef.image, xmlNode3.InnerText);
                                                        break;
                                                    case "Dispensed":
                                                        if (xmlNode3.InnerText != "") it.SetItemDef((int)VarItemDef.isDispensable, true);
                                                        else it.SetItemDef((int)VarItemDef.isDispensable, false);
                                                        break;
                                                    case "StoredIn":
                                                        if (xmlNode3.InnerText != "") it.SetItemDef((int)VarItemDef.isDepositable, true);
                                                        else it.SetItemDef((int)VarItemDef.isDepositable, false);
                                                        break;
                                                    case "Capacity":
                                                        if (xmlNode3.InnerText != "") it.SetItemDef((int)VarItemDef.capacity, Convert.ToInt32(xmlNode3.InnerText));
                                                        else it.SetItemDef((int)VarItemDef.capacity, 0);
                                                        break;
                                                    case "PorNearFull":
                                                        if (xmlNode3.InnerText != "") it.SetItemDef((int)VarItemDef.porNearFull, Convert.ToInt32(xmlNode3.InnerText) / 100f);
                                                        else it.SetItemDef((int)VarItemDef.porNearFull, 0);
                                                        break;
                                                    case "PorNearEmpty":
                                                        if (xmlNode3.InnerText != "") it.SetItemDef((int)VarItemDef.porNearEmpty, Convert.ToInt32(xmlNode3.InnerText) / 100f);
                                                        else it.SetItemDef((int)VarItemDef.porNearEmpty, 0);
                                                        break;
                                                }
                                            }
                                            itemsDef.Add(it);
                                            //itemsDef[cont] = it;
                                            //cont++;
                                            break;
                                    }
                                }
                                break;
                            case "MXN":
                                //cont = 0;
                                foreach (XmlNode xmlNode2 in xmlNode1.ChildNodes)
                                {
                                    switch (xmlNode2.Name)
                                    {
                                        case "Code":
                                            currencyCode = xmlNode2.InnerText;
                                            break;
                                        case "symbol":
                                            symbol = xmlNode2.InnerText;
                                            break;
                                        case "unitvalue":
                                            if (xmlNode2.InnerText != "") unitValue = Convert.ToInt32(xmlNode2.InnerText);
                                            else unitValue = 0;
                                            break;
                                        case "Coins":
                                            if (xmlNode2.InnerText != "") numCoins = Convert.ToInt32(xmlNode2.InnerText);
                                            else numCoins = 0;
                                            break;
                                        case "Bills":
                                            if (xmlNode2.InnerText != "") numBills = Convert.ToInt32(xmlNode2.InnerText);
                                            else numBills = 0;
                                            break;
                                        case "TotalItems":
                                            if (xmlNode2.InnerText != "") numItems = Convert.ToInt32(xmlNode2.InnerText);
                                            else numItems = 0;
                                            break;
                                        case "CapStacker":
                                            if (xmlNode2.InnerText != "") capStacker = Convert.ToInt32(xmlNode2.InnerText);
                                            else capStacker = 0;
                                            break;
                                        case "PorNearFullSt":
                                            if (xmlNode2.InnerText != "") porNearFullStacker = Convert.ToInt32(xmlNode2.InnerText) / 100f;
                                            else porNearFullStacker = 0;
                                            break;
                                        case "PorNearEmptySt":
                                            if (xmlNode2.InnerText != "") porNearEmptyStacker = Convert.ToInt32(xmlNode2.InnerText) / 100f;
                                            else porNearEmptyStacker = 0;
                                            break;
                                        case "CashInfo":
                                            it = new ItemDef();
                                            foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
                                            {
                                                switch (xmlNode3.Name)
                                                {
                                                    case "Denomination":
                                                        it.SetItemDef((int)VarItemDef.name, xmlNode3.InnerText);
                                                        break;
                                                    case "Value":
                                                        if (xmlNode3.InnerText != "") it.SetItemDef((int)VarItemDef.value, Convert.ToInt32(xmlNode3.InnerText));
                                                        else it.SetItemDef((int)VarItemDef.value, 0);
                                                        break;
                                                    case "Type":
                                                        if (xmlNode3.InnerText == "Coin")
                                                        {
                                                            it.SetItemDef((int)VarItemDef.isCoin, true);
                                                            it.SetItemDef((int)VarItemDef.isBill, false);
                                                        }
                                                        else if (xmlNode3.InnerText == "Bill")
                                                        {
                                                            it.SetItemDef((int)VarItemDef.isCoin, false);
                                                            it.SetItemDef((int)VarItemDef.isBill, true);
                                                        }
                                                        break;
                                                    case "Image":
                                                        it.SetItemDef((int)VarItemDef.image, xmlNode3.InnerText);
                                                        break;
                                                    case "Dispensed":
                                                        if (xmlNode3.InnerText != "") it.SetItemDef((int)VarItemDef.isDispensable, true);
                                                        else it.SetItemDef((int)VarItemDef.isDispensable, false);
                                                        break;
                                                    case "StoredIn":
                                                        if (xmlNode3.InnerText != "") it.SetItemDef((int)VarItemDef.isDepositable, true);
                                                        else it.SetItemDef((int)VarItemDef.isDepositable, false);
                                                        break;
                                                    case "Capacity":
                                                        if (xmlNode3.InnerText != "") it.SetItemDef((int)VarItemDef.capacity, Convert.ToInt32(xmlNode3.InnerText));
                                                        else it.SetItemDef((int)VarItemDef.capacity, 0);
                                                        break;
                                                    case "PorNearFull":
                                                        if (xmlNode3.InnerText != "") it.SetItemDef((int)VarItemDef.porNearFull, Convert.ToInt32(xmlNode3.InnerText) / 100f);
                                                        else it.SetItemDef((int)VarItemDef.porNearFull, 0);
                                                        break;
                                                    case "PorNearEmpty":
                                                        if (xmlNode3.InnerText != "") it.SetItemDef((int)VarItemDef.porNearEmpty, Convert.ToInt32(xmlNode3.InnerText) / 100f);
                                                        else it.SetItemDef((int)VarItemDef.porNearEmpty, 0);
                                                        break;
                                                }
                                            }
                                            itemsDef.Add(it);
                                            //itemsDef[cont] = it;
                                            //cont++;
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                }
                #endregion
            }

            public string CurrencyCashList()
            {
                string s = "";
                bool flagComa = false;
                bool flagBills = false;
                for (int i = 0; i < numItems; i++)
                {
                    if (itemsDef[i].IsDispensable)
                    {
                        if (ItemsDef[i].IsBill && !flagBills)
                        {
                            flagBills = true;
                            flagComa = false;
                            s += ";";
                        }

                        if (flagComa) s += ",";
                        else flagComa = true;

                        s += ItemsDef[i].Value;
                    }
                }

                return s;
            }

            public string DepositCashList()
            {
                string s = "";
                bool flagComa = false;
                bool flagBills = false;
                for (int i = 0; i < numItems; i++)
                {
                    if (ItemsDef[i].IsBill && !flagBills)
                    {
                        flagBills = true;
                        flagComa = false;
                        s += ";";
                    }

                    if (flagComa) s += ",";
                    else flagComa = true;

                    s += ItemsDef[i].Value;
                }

                return s;
            }
            #endregion
        }
    }
}

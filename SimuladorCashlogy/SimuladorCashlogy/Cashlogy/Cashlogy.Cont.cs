using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace Cashlogy
{
    public partial class CashlogyDevice
    {
        class CashlogyCont
        {
            public delegate void onChanged();
            public event onChanged OnChanged;

            CashlogyDef def;
            CashlogyState st;

            #region Atributos Contabilidad
            public int totDeposited;
            public int totDispensed;
            public int totCash;
            public int totStacker;

            public int[] recyclers;
            public int[] stacker;
            public int[] safebox; // No se usa, pdte. para próximas versiones 

            public int[] deposited;
            public string depositCounts;
            public int[] dispensed;
            public string dispenseCounts;
            #endregion

            public CashlogyCont(CashlogyDef d, CashlogyState s)
            {
                def = d;
                st = s;
                recyclers = new int[MAX_ITEMS];
                stacker = new int[MAX_ITEMS];
                safebox = new int[MAX_ITEMS];
                deposited = new int[MAX_ITEMS];
                dispensed = new int[MAX_ITEMS];
            }

            #region Metodos Contabilidad
            public void DepositCounts()
            {
                bool flagComa = false;
                bool flagBills = false;
                depositCounts = "";
                for (int i = 0; i < def.NumItems; i++)
                {
                    if (def.ItemsDef[i].IsBill && !flagBills)
                    {
                        flagBills = true;
                        flagComa = false;
                        depositCounts += ";";
                    }

                    if (flagComa) depositCounts += ",";
                    else flagComa = true;
                    depositCounts += def.ItemsDef[i].Value + ":" + deposited[i];
                }
            }

            public void DispenseCounts()
            {
                bool flagComa = false;
                bool flagBills = false;
                dispenseCounts = "";
                for (int i = 0; i < def.NumItems; i++)
                {
                    if (def.ItemsDef[i].IsDispensable)
                    {
                        if (def.ItemsDef[i].IsBill && !flagBills)
                        {
                            flagBills = true;
                            flagComa = false;
                            dispenseCounts += ";";
                        }

                        if (flagComa) dispenseCounts += ",";
                        else flagComa = true;
                        dispenseCounts += def.ItemsDef[i].Value + ":" + dispensed[i];
                    }
                }
            }

            public void SetRecyclers(int[] r)
            {
                totCash = 0;
                recyclers = new int[MAX_ITEMS];
                for (int i = 0; i < def.NumItems; i++)
                {
                    if (def.ItemsDef[i].IsDispensable)
                    {
                        if (r[i] > def.ItemsDef[i].Capacity) recyclers[i] = def.ItemsDef[i].Capacity;
                        else recyclers[i] = r[i];
                    }

                    totCash += recyclers[i] * def.ItemsDef[i].Value + stacker[i] * def.ItemsDef[i].Value;
                }
                OnChanged();
            }

            public void SetStacker(int[] s)
            {
                totStacker = 0;
                totCash = 0;
                stacker = new int[MAX_ITEMS];
                for (int i = 0; i < def.NumItems; i++)
                {
                    if (def.ItemsDef[i].IsDepositable)
                    {
                        if ((totStacker + s[i]) > def.CapStacker) stacker[i] = def.CapStacker - totStacker;
                        else stacker[i] = s[i];
                    }

                    totStacker += stacker[i];
                    totCash += recyclers[i] * def.ItemsDef[i].Value + stacker[i] * def.ItemsDef[i].Value;
                }
                OnChanged();
            }

            public void ResetDepositedCash()
            {
                for (int i = 0; i < deposited.Length; i++)
                {
                    deposited[i] = 0;
                }

                totDeposited = 0;
                DepositCounts();
            }

            public void ResetDispensedCash()
            {
                for (int i = 0; i < dispensed.Length; i++)
                {
                    dispensed[i] = 0;
                }

                totDispensed = 0;
                DispenseCounts();
            }

            public int GetTotCash()
            {
                totCash = 0;
                for (int i = 0; i < def.NumItems; i++)
                {
                    totCash += def.ItemsDef[i].Value * recyclers[i] + def.ItemsDef[i].Value * stacker[i];
                }
                return totCash;
            }

            public int GetTotDeposited()
            {
                totDeposited = 0;
                for (int i = 0; i < def.NumItems; i++)
                {
                    totDeposited += def.ItemsDef[i].Value * deposited[i];
                }
                return totDeposited;
            }

            public int GetTotDispensed()
            {
                totDispensed = 0;
                for (int i = 0; i < def.NumItems; i++)
                {
                    totDispensed += def.ItemsDef[i].Value * dispensed[i];
                }
                return totDispensed;
            }

            public int[] GetRecyclers()
            {
                return recyclers;
            }

            public int[] GetStacker()
            {
                return stacker;
            }

            public void GetAviableCapacity(ref int[] items, ref int stacker)
            {
                items = new int[MAX_ITEMS];
                for (int i = 0; i < def.NumItems; i++)
                {
                    if (def.ItemsDef[i].IsDispensable)
                    {
                        if (def.ItemsDef[i].Capacity >= recyclers[i])
                        {
                            items[i] = def.ItemsDef[i].Capacity - recyclers[i];
                        }
                        else
                        {
                            items[i] = 0;
                        }
                    }
                }
                stacker = def.CapStacker - totStacker;
            }

            public string ReadCashCounts()
            {
                string cashCounts = "";
                bool flagComa = false;
                bool flagBills = false;
                int[] totalItems = new int[MAX_ITEMS];

                for (int i = 0; i < def.NumItems; i++)
                {
                    totalItems[i] = recyclers[i] + stacker[i];

                    if (def.ItemsDef[i].IsBill && !flagBills)
                    {
                        flagBills = true;
                        flagComa = false;
                        cashCounts += ";";
                    }

                    if (flagComa) cashCounts += ",";
                    else flagComa = true;
                    cashCounts += def.ItemsDef[i].Value + ":" + totalItems[i];
                }

                return cashCounts;
            }

            public string ReadDispensableCashCounts()
            {
                string cashCounts = "";
                bool flagComa = false;
                bool flagBills = false;

                for (int i = 0; i < def.NumItems; i++)
                {
                    if (def.ItemsDef[i].IsBill && !flagBills)
                    {
                        flagBills = true;
                        flagComa = false;
                        cashCounts += ";";
                    }

                    if (flagComa) cashCounts += ",";
                    else flagComa = true;

                    if (def.ItemsDef[i].IsDispensable) cashCounts += def.ItemsDef[i].Value + ":" + recyclers[i];
                    else cashCounts += def.ItemsDef[i].Value + ":0";
                }

                return cashCounts;
            }

            public string ReadNotDispensableCashCounts()
            {
                string cashCounts = "";
                bool flagComa = false;
                bool flagBills = false;

                for (int i = 0; i < def.NumItems; i++)
                {
                    if (def.ItemsDef[i].IsBill && !flagBills)
                    {
                        flagBills = true;
                        flagComa = false;
                        cashCounts += ";";
                    }

                    if (flagComa) cashCounts += ",";
                    else flagComa = true;

                    if (def.ItemsDef[i].IsDepositable) cashCounts += def.ItemsDef[i].Value + ":" + stacker[i];
                    else cashCounts += def.ItemsDef[i].Value + ":0";
                }

                return cashCounts;
            }

            public int[] GetDeposited()
            {
                return deposited;
            }

            public int[] GetDispensed()
            {
                return dispensed;
            }

            public void AddItem(int i)
            {
                if (def.ItemsDef[i].IsDispensable && !def.ItemsDef[i].IsDepositable && st.EnableDepositItems[i])
                {
                    if ((recyclers[i] + 1) > def.ItemsDef[i].Capacity) return;
                    else recyclers[i]++;
                }
                else if (def.ItemsDef[i].IsDispensable && def.ItemsDef[i].IsDepositable && st.EnableDepositItems[i])
                {
                    if ((recyclers[i] + 1) > def.ItemsDef[i].Capacity)
                    {
                        if ((totStacker + 1) > def.CapStacker) return;
                        else
                        {
                            totStacker = 0;
                            stacker[i]++;
                            for (int j = 0; j < def.NumItems; j++)
                            {
                                totStacker += stacker[j];
                            }
                        }
                    }
                    else recyclers[i]++;
                }
                else if (!def.ItemsDef[i].IsDispensable && def.ItemsDef[i].IsDepositable && st.EnableDepositItems[i])
                {
                    if ((totStacker + 1) > def.CapStacker) return;
                    else
                    {
                        totStacker = 0;
                        stacker[i]++;
                        for (int j = 0; j < def.NumItems; j++)
                        {
                            totStacker += stacker[j];
                        }
                    }
                }

                if (st.EnableDepositItems[i])
                {
                    deposited[i]++;
                    totDeposited += def.ItemsDef[i].Value;
                    DepositCounts();
                    totCash += def.ItemsDef[i].Value;
                    OnChanged();
                }
            }

            public void AddItem(int i, int num)
            {
                int realDeposit = 0;
                if (def.ItemsDef[i].IsDispensable && !def.ItemsDef[i].IsDepositable && st.EnableDepositItems[i])
                {
                    if ((recyclers[i] + num) > def.ItemsDef[i].Capacity)
                    {
                        recyclers[i] = def.ItemsDef[i].Capacity;
                        realDeposit = def.ItemsDef[i].Capacity;
                    }
                    else
                    {
                        recyclers[i] += num;
                        realDeposit = num;
                    }
                }
                else if (def.ItemsDef[i].IsDispensable && def.ItemsDef[i].IsDepositable && st.EnableDepositItems[i])
                {
                    if ((recyclers[i] + num) > def.ItemsDef[i].Capacity)
                    {
                        int auxNum = def.ItemsDef[i].Capacity - recyclers[i];
                        recyclers[i] += auxNum;
                        int auxStack = num - auxNum;
                        if ((totStacker + auxStack) > def.CapStacker)
                        {
                            stacker[i] += def.CapStacker - totStacker;
                            realDeposit = def.CapStacker - totStacker + auxNum;
                            totStacker = 0;
                            for (int j = 0; j < def.NumItems; j++)
                            {
                                totStacker += stacker[j];
                            }
                        }
                        else
                        {
                            stacker[i] += auxStack;
                            realDeposit = auxStack + auxNum;
                            totStacker = 0;
                            for (int j = 0; j < def.NumItems; j++)
                            {
                                totStacker += stacker[j];
                            }
                        }
                    }
                    else
                    { 
                        recyclers[i] += num;
                        realDeposit = num;
                    }
                }
                else if (!def.ItemsDef[i].IsDispensable && def.ItemsDef[i].IsDepositable && st.EnableDepositItems[i])
                {
                    if ((totStacker + num) > def.CapStacker)
                    {
                        realDeposit = def.CapStacker - totStacker;
                        stacker[i] += realDeposit;
                        totStacker = 0;
                        for (int j = 0; j < def.NumItems; j++)
                        {
                            totStacker += stacker[j];
                        }
                    }
                    else
                    {
                        stacker[i] += num;
                        realDeposit = num;
                        totStacker = 0;
                        for (int j = 0; j < def.NumItems; j++)
                        {
                            totStacker += stacker[j];
                        }
                    }
                }

                if (st.EnableDepositItems[i])
                {
                    deposited[i] += realDeposit;
                    totDeposited += def.ItemsDef[i].Value * realDeposit;
                    DepositCounts();
                    totCash += def.ItemsDef[i].Value *  realDeposit;
                    OnChanged();
                }
            }

            public void RemoveItem(int i, bool ToStacker)
            {
                if (def.ItemsDef[i].IsDispensable && !def.ItemsDef[i].IsDepositable)
                {
                    if ((recyclers[i] - 1) < 0) return;
                    else recyclers[i]--;
                    dispensed[i]++;
                    totDispensed += def.ItemsDef[i].Value;
                    totCash -= def.ItemsDef[i].Value;
                }
                else if(def.ItemsDef[i].IsDispensable && def.ItemsDef[i].IsDepositable && !ToStacker)
                {
                    if ((recyclers[i] - 1) < 0) return;
                    else recyclers[i]--;
                    dispensed[i]++;
                    totDispensed += def.ItemsDef[i].Value;
                    totCash -= def.ItemsDef[i].Value;
                }
                else if (def.ItemsDef[i].IsDispensable && def.ItemsDef[i].IsDepositable && ToStacker)
                {
                    if ((recyclers[i] - 1) < 0 && (totStacker + 1) > def.CapStacker) return;
                    else
                    {
                        recyclers[i]--;
                        stacker[i]++;
                        totStacker++;
                    }
                }
                
                OnChanged();
            }

            public void RemoveItem(int i, int num, bool ToStacker)
            {
                if (def.ItemsDef[i].IsDispensable && !def.ItemsDef[i].IsDepositable)
                {
                    if ((recyclers[i] - num) < 0)
                    {
                        recyclers[i] = 0;
                        dispensed[i] += recyclers[i];
                        totDispensed += def.ItemsDef[i].Value * recyclers[i];
                        totCash -= def.ItemsDef[i].Value * recyclers[i];
                    }
                    else
                    {
                        recyclers[i] -= num;
                        dispensed[i] += num;
                        totDispensed += def.ItemsDef[i].Value * num;
                        totCash -= def.ItemsDef[i].Value * num;
                    }
                }
                else if (def.ItemsDef[i].IsDispensable && def.ItemsDef[i].IsDepositable && !ToStacker)
                {
                    if ((recyclers[i] - num) < 0)
                    {
                        recyclers[i] = 0;
                        dispensed[i] += recyclers[i];
                        totDispensed += def.ItemsDef[i].Value * recyclers[i];
                        totCash -= def.ItemsDef[i].Value * recyclers[i];
                    }
                    else
                    {
                        recyclers[i] -= num;
                        dispensed[i] += num;
                        totDispensed += def.ItemsDef[i].Value * num;
                        totCash -= def.ItemsDef[i].Value * num;
                    }
                }
                else if (def.ItemsDef[i].IsDispensable && def.ItemsDef[i].IsDepositable && ToStacker)
                {
                    if ((recyclers[i] - num) < 0 )
                    {
                        if ((totStacker + recyclers[i]) > def.CapStacker && totStacker != def.CapStacker)
                        {
                            recyclers[i] = 0;
                            stacker[i] += def.CapStacker - totStacker;
                            totStacker = def.CapStacker;
                        }
                        else if ((totStacker + recyclers[i]) > def.CapStacker && totStacker == def.CapStacker) return;
                        else
                        {
                            recyclers[i] = 0;
                            stacker[i] += recyclers[i];
                            totStacker += recyclers[i];
                        }
                    }
                    else
                    {
                        if ((totStacker + num) > def.CapStacker && totStacker != def.CapStacker)
                        {
                            recyclers[i] -= num;
                            stacker[i] += def.CapStacker - totStacker;
                            totStacker = def.CapStacker;
                        }
                        else if ((totStacker + num) > def.CapStacker && totStacker == def.CapStacker) return;
                        else
                        {
                            recyclers[i] -= num;
                            stacker[i] += num;
                            totStacker += num;
                        }
                    }
                }

                OnChanged();
            }

            public void ReadFromFile(string logicalDeviceName)
            {
                string ContPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                                           logicalDeviceName + @"\cashlogyCont.xml");

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ContPath);

                int cont;

                #region ParsearNodosXML
                foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes)
                {
                    switch (xmlNode.Name)
                    {
                        case "TotDeposited":
                            if (xmlNode.InnerText != "") totDeposited = Convert.ToInt32(xmlNode.InnerText);
                            else totDeposited = 0;
                            break;
                        case "TotDispensed":
                            if (xmlNode.InnerText != "") totDispensed = Convert.ToInt32(xmlNode.InnerText);
                            else totDispensed = 0;
                            break;
                        case "TotCash":
                            if (xmlNode.InnerText != "") totCash = Convert.ToInt32(xmlNode.InnerText);
                            else totCash = 0;
                            break;
                        case "TotStacker":
                            if (xmlNode.InnerText != "") totStacker = Convert.ToInt32(xmlNode.InnerText);
                            else totStacker = 0;
                            break;
                        case "Recyclers":
                            cont = 0;
                            foreach (XmlNode xmlNode1 in xmlNode.ChildNodes)
                            {
                                if (xmlNode1.Name == "int")
                                {
                                    if (xmlNode1.InnerText != "") recyclers[cont] = Convert.ToInt32(xmlNode1.InnerText);
                                    else recyclers[cont] = 0;
                                    cont++;
                                }
                            }
                            break;
                        case "Stacker":
                            cont = 0;
                            foreach (XmlNode xmlNode1 in xmlNode.ChildNodes)
                            {
                                if (xmlNode1.Name == "int")
                                {
                                    if (xmlNode1.InnerText != "") stacker[cont] = Convert.ToInt32(xmlNode1.InnerText);
                                    else stacker[cont] = 0;
                                    cont++;
                                }
                            }
                            break;
                        case "Safebox":
                            cont = 0;
                            foreach (XmlNode xmlNode1 in xmlNode.ChildNodes)
                            {
                                if (xmlNode1.Name == "int")
                                {
                                    if (xmlNode1.InnerText != "") safebox[cont] = Convert.ToInt32(xmlNode1.InnerText);
                                    else safebox[cont] = 0;
                                    cont++;
                                }
                            }
                            break;
                    }
                }
                #endregion
            }

            public void SaveToFile(string logicalDeviceName)
            {
                if (!st.Opened) return;

                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration declaracion;
                XmlNode rootNode;
                XmlNode node;
                XmlNode subnode;

                #region Escribir Fichero
                declaracion = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDoc.AppendChild(declaracion);

                rootNode = xmlDoc.CreateElement("Contabilidad");
                xmlDoc.AppendChild(rootNode);

                node = xmlDoc.CreateElement("TotCash");
                node.InnerText = Convert.ToString(totCash);
                rootNode.AppendChild(node);

                node = xmlDoc.CreateElement("TotStacker");
                node.InnerText = Convert.ToString(totStacker);
                rootNode.AppendChild(node);

                node = xmlDoc.CreateElement("Recyclers");
                for (int i = 0; i < recyclers.Length; i++)
                {
                    subnode = xmlDoc.CreateElement("int");
                    subnode.InnerText = Convert.ToString(recyclers[i]);
                    node.AppendChild(subnode);
                }
                rootNode.AppendChild(node);

                node = xmlDoc.CreateElement("Stacker");
                for (int i = 0; i < stacker.Length; i++)
                {
                    subnode = xmlDoc.CreateElement("int");
                    subnode.InnerText = Convert.ToString(stacker[i]);
                    node.AppendChild(subnode);
                }
                rootNode.AppendChild(node);

                node = xmlDoc.CreateElement("Safebox");
                for (int i = 0; i < safebox.Length; i++)
                {
                    subnode = xmlDoc.CreateElement("int");
                    subnode.InnerText = Convert.ToString(safebox[i]);
                    node.AppendChild(subnode);
                }
                rootNode.AppendChild(node);

                xmlDoc.Save(logicalDeviceName + @"\cashlogyCont.xml");
                #endregion
            }
            #endregion
        }
    }
}
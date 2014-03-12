using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data;
using System.Collections;
using System.IO;

namespace CoreLib
{
    public class DataDef
    {
        // Fields
        private static Exception m_ex;
        public static XmlDocument m_XmlD;
        public static XmlNode m_XmlNode;
        public static XmlNodeList m_XmlNodeList;

        // Methods
        public DataDef()
        {
            OpenXml();
        }

        public static string FilterFormatCode(string strLevel, string strDesignElement)
        {
            if (m_XmlD == null)
            {
                OpenXml();
            }
            m_XmlNode = m_XmlD.SelectSingleNode("/DataDef/FormatCodes");
            if (m_XmlNode != null)
            {
                m_XmlNodeList = m_XmlNode.SelectNodes("FormatCode");
                for (int i = 0; i < m_XmlNodeList.Count; i++)
                {
                    XmlNode node = m_XmlNodeList[i];
                    XmlNodeList list = node.SelectNodes("Rule");
                    for (int j = 0; j < list.Count; j++)
                    {
                        string str = "";
                        string str2 = "";
                        foreach (XmlAttribute attribute in list[j].Attributes)
                        {
                            if (attribute.Name == "DesignElement")
                            {
                                str = attribute.Value.ToString().Trim();
                            }
                            if (attribute.Name == "Level")
                            {
                                str2 = attribute.Value.ToString().Trim();
                            }
                            if ((str != "") && (str2 != ""))
                            {
                                break;
                            }
                        }
                        if ((str == strDesignElement) && (str2 == strLevel))
                        {
                            return node.Attributes["ID"].Value.ToString().Trim();
                        }
                    }
                }
            }
            return "";
        }

        private static string FilterNull(string strValue)
        {
            return ((strValue == null) ? "" : strValue);
        }

        public static DataTable GetCombo(string strNodeName)
        {
            int num = strNodeName.LastIndexOf('/');
            DataTable table = new DataTable(strNodeName.Substring(num + 1, (strNodeName.Length - num) - 1));
            if (m_XmlD == null)
            {
                OpenXml();
            }
            m_XmlNode = m_XmlD.SelectSingleNode(strNodeName);
            if (m_XmlNode != null)
            {
                m_XmlNodeList = m_XmlNode.ChildNodes;
                string name = "";
                ArrayList list = new ArrayList();
                if (m_XmlNodeList.Count <= 0)
                {
                    return table;
                }
                XmlNode node = m_XmlNodeList[0];
                if (node.Attributes != null)
                {
                    for (int i = 0; i < node.Attributes.Count; i++)
                    {
                        name = node.Attributes[i].Name;
                        list.Add(name);
                        table.Columns.Add(name);
                    }
                }
                for (int j = 0; j < m_XmlNodeList.Count; j++)
                {
                    DataRow row = table.Rows.Add(new object[0]);
                    for (int k = 0; k < list.Count; k++)
                    {
                        string str3 = list[k].ToString();
                        if (m_XmlNodeList[j].Attributes[str3] != null)
                        {
                            row[str3] = m_XmlNodeList[j].Attributes[str3].Value;
                        }
                    }
                }
            }
            return table;
        }

        public static string GetFormatcodeAttr(string strFormatcodeID, string strAttrName)
        {
            if (m_XmlD == null)
            {
                OpenXml();
            }
            m_XmlNode = m_XmlD.SelectSingleNode("/DataDef/FormatCodes");
            string str = "";
            if (m_XmlNode != null)
            {
                XmlNode node = null;
                m_XmlNodeList = m_XmlNode.SelectNodes("FormatCode");
                for (int i = 0; i < m_XmlNodeList.Count; i++)
                {
                    if (m_XmlNodeList[i].Attributes["ID"].Value.ToString().Trim() == strFormatcodeID)
                    {
                        node = m_XmlNodeList[i];
                        break;
                    }
                }
                if ((node != null) && (node.Attributes[strAttrName] != null))
                {
                    str = node.Attributes[strAttrName].ToString();
                }
            }
            return str;
        }

        public static DataSet GetNode(string strNodeName)
        {
            DataSet set = new DataSet();
            DataTable table = new DataTable();
            if (m_XmlD == null)
            {
                OpenXml();
            }
            m_XmlNode = m_XmlD.SelectSingleNode(strNodeName);
            if (m_XmlNode != null)
            {
                XmlNodeList list = m_XmlNode.SelectNodes(strNodeName);
                for (int i = 0; i < list.Count; i++)
                {
                    string outerXml = list[i].OuterXml;
                    string name = list[i].ChildNodes[0].Name;
                    StringReader input = new StringReader(outerXml);
                    XmlTextReader reader = new XmlTextReader(input);
                    XmlReader reader3 = XmlReader.Create(input);
                    try
                    {
                        set.ReadXml(reader);
                    }
                    catch (XmlException)
                    {
                    }
                }
            }
            return set;
        }

        public static DataSet GetRuleOfFormatcode(string strFormatcodeID)
        {
            DataSet set = new DataSet();
            if (m_XmlD == null)
            {
                OpenXml();
            }
            m_XmlNode = m_XmlD.SelectSingleNode("/DataDef/FormatCodes");
            if (m_XmlNode != null)
            {
                XmlNode node = null;
                m_XmlNodeList = m_XmlNode.SelectNodes("FormatCode");
                for (int i = 0; i < m_XmlNodeList.Count; i++)
                {
                    if (m_XmlNodeList[i].Attributes["ID"].Value.ToString().Trim() == strFormatcodeID)
                    {
                        node = m_XmlNodeList[i];
                        break;
                    }
                }
                if (node != null)
                {
                    StringReader input = new StringReader(node.OuterXml);
                    XmlTextReader reader = new XmlTextReader(input);
                    XmlReader reader3 = XmlReader.Create(input);
                    try
                    {
                        set.ReadXml(reader);
                    }
                    catch (XmlException exception)
                    {
                        m_ex = exception;
                    }
                }
            }
            return set;
        }

        public static DataTable getruleoffromatcode_dyh(string strFormatcodeID)
        {
            DataTable table = new DataTable();
            int num = 0;
            table.Columns.Add("Level");
            table.Columns.Add("DesignElement");
            table.Columns.Add("AssemblingPosition");
            table.Columns.Add("LengthAdjust");
            table.Columns.Add("Quantity");
            table.Columns.Add("Clearance");
            table.Columns.Add("Text");
            table.Columns.Add("RuleType");
            if (m_XmlD == null)
            {
                OpenXml();
            }
            m_XmlNode = m_XmlD.SelectSingleNode("/DataDef/FormatCodes");
            if (m_XmlNode != null)
            {
                int num2;
                XmlNode node = null;
                m_XmlNodeList = m_XmlNode.SelectNodes("FormatCode");
                for (num2 = 0; num2 < m_XmlNodeList.Count; num2++)
                {
                    if (m_XmlNodeList[num2].Attributes["ID"].Value.ToString().Trim() == strFormatcodeID)
                    {
                        node = m_XmlNodeList[num2];
                    }
                }
                XmlNodeList childNodes = node.ChildNodes;
                for (num2 = 0; num2 < childNodes.Count; num2++)
                {
                    if (childNodes[num2] != null)
                    {
                        table.Rows.Add(new object[0]);
                        table.Rows[0]["Level"] = childNodes[num2].Attributes["Level"].Value.ToString();
                        table.Rows[0]["DesignElement"] = childNodes[num2].Attributes["DesignElement"].Value.ToString();
                        table.Rows[0]["AssemblingPosition"] = childNodes[num2].Attributes["AssemblingPosition"].Value.ToString();
                        table.Rows[0]["LengthAdjust"] = childNodes[num2].Attributes["LengthAdjust"].Value.ToString();
                        table.Rows[0]["Quantity"] = childNodes[num2].Attributes["Quantity"].Value.ToString();
                        table.Rows[0]["Clearance"] = childNodes[num2].Attributes["Clearance"].Value.ToString();
                        table.Rows[0]["Text"] = childNodes[num2].Attributes["Text"].Value.ToString();
                        table.Rows[0]["RuleType"] = childNodes[num2].Attributes["RuleType"].Value.ToString();
                        num++;
                    }
                }
            }
            return table;
        }

        private static bool OpenXml()
        {
            bool flag = false;
            m_XmlD = new XmlDocument();
            try
            {
				m_XmlD.Load(CoreLib.Common.XmlPath() +@"DataDef.xml");
                if (m_XmlD.GetElementsByTagName("EncryptedData").Count > 0)
                {
					m_XmlD = Encrypt.DecryptInMemory(CoreLib.Common.XmlPath() + @"DataDef.xml", "decasoft");
                }
                flag = true;
            }
            catch (XmlException exception)
            {
                throw new XmlException(exception.Message);
            }
            return flag;
        }

        public static string ShowExMessage()
        {
            return m_ex.Message;
        }
    }

}

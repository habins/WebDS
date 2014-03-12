using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Xml;
namespace CoreLib
{
    public class SystemData
    {
        public XmlDocument m_XmlD;
        public XmlNode m_XmlNode;
        public XmlNodeList m_XmlNodeList;
        private string ID;
        private string DataVersion;
        public bool SysExit;
        public string ExMessage;
        public string SysID
        {
            get
            {
                return this.ID;
            }
            set
            {
                this.ID = value;
            }
        }
        public SystemData()
        {
            this.OpenXml();
        }
        public SystemData(string strSysID, string strDataVersion)
        {
            this.ID = strSysID;
            this.DataVersion = strDataVersion;
            this.OpenXml();
            this.SysExit = this.GetSystem();
        }
        public bool ishave(string ID)
        {
            bool result = false;
            foreach (XmlNode xmlNode in this.m_XmlNodeList)
            {
                if (xmlNode.Attributes["ID"].ToString() == ID)
                {
                    result = true;
                }
            }
            return result;
        }
        public string getSysNode(string ID)
        {
            string result = "";
            if (this.ishave(ID))
            {
                foreach (XmlNode xmlNode in this.m_XmlNodeList)
                {
                    if (xmlNode.Attributes["ID"].ToString() == ID)
                    {
                        result = xmlNode.BaseURI;
                    }
                }
            }
            return result;
        }
        public string GetSysAttribute(string ID, string attriName)
        {
            string result = "";
            foreach (XmlNode xmlNode in this.m_XmlNodeList)
            {
                if (xmlNode.Attributes["ID"].Value.ToString() == ID)
                {
                    result = xmlNode.Attributes["Version"].ToString();
                }
            }
            return result;
        }
        public void SetSysAtrribute(string ID, string attributeName, string value)
        {
            if (this.ishave(ID))
            {
                foreach (XmlNode xmlNode in this.m_XmlNodeList)
                {
                    if (xmlNode.Attributes["ID"].ToString() == ID)
                    {
                        xmlNode.Attributes[attributeName].Value = value;
                    }
                }
            }
        }
        public void New()
        {
            this.m_XmlNode = this.m_XmlD.SelectSingleNode("/WSMD");
            XmlNode newChild = this.m_XmlD.CreateNode(XmlNodeType.Element, "SystemData", null);
            this.m_XmlNode = this.m_XmlNode.AppendChild(newChild);
        }
        private bool GetSystem()
        {
            bool result;
            for (int i = 0; i < this.m_XmlNodeList.Count; i++)
            {
                XmlNode xmlNode = this.m_XmlNodeList[i];
                if (this.ID == xmlNode.Attributes["ID"].Value && this.DataVersion == xmlNode.Attributes["DataVersion"].Value)
                {
                    this.m_XmlNode = this.m_XmlNodeList[i];
                    this.m_XmlNodeList = this.m_XmlNode.ChildNodes;
                    result = true;
                    return result;
                }
            }
            result = false;
            return result;
        }
        private bool OpenXml()
        {
            bool result = false;
            this.m_XmlD = new XmlDocument();
            try
            {
                this.m_XmlD.Load(CoreLib.Common.XmlPath() + "wsmd.xml");
                XmlNodeList elementsByTagName = this.m_XmlD.GetElementsByTagName("EncryptedData");
                if (elementsByTagName.Count > 0)
                {
					this.m_XmlD = Encrypt.DecryptInMemory(CoreLib.Common.XmlPath() + "wsmd.xml", "decasoft");
                }
                this.m_XmlNode = this.m_XmlD.SelectSingleNode("/WSMD");
                this.m_XmlNodeList = this.m_XmlNode.SelectNodes("SystemData");
                result = true;
            }
            catch (XmlException ex)
            {
                throw new XmlException(ex.Message);
            }
            return result;
        }
        public DataTable GetSysList()
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable();
            string text = "VendorDescr";
            string text2 = "Description";
            string text3 = "ID";
            string text4 = "SystemType";
            string text5 = "DataVersion";
            string text6 = "VendorID";
            dataTable.Columns.Add(text);
            dataTable.Columns.Add(text2);
            dataTable.Columns.Add(text3);
            dataTable.Columns.Add(text4);
            dataTable.Columns.Add(text5);
            dataTable.Columns.Add(text6);
            DataTable combo = DataDef.GetCombo("/DataDef/SystemTypes");
            try
            {
                if (this.m_XmlD != null)
                {
                    XmlNodeList xmlNodeList = this.m_XmlD.SelectNodes("/WSMD/SystemData");
                    for (int i = 0; i < xmlNodeList.Count; i++)
                    {
                        dataTable.Rows.Add(new object[0]);
                        if (xmlNodeList[i].Attributes[text] != null)
                        {
                            dataTable.Rows[i][text] = xmlNodeList[i].Attributes[text].Value.ToString();
                        }
                        if (xmlNodeList[i].Attributes[text2] != null)
                        {
                            dataTable.Rows[i][text2] = this.FilterNull(xmlNodeList[i].Attributes[text2].Value.ToString());
                        }
                        if (xmlNodeList[i].Attributes[text3] != null)
                        {
                            dataTable.Rows[i][text3] = this.FilterNull(xmlNodeList[i].Attributes[text3].Value.ToString());
                        }
                        if (xmlNodeList[i].Attributes[text5] != null)
                        {
                            dataTable.Rows[i][text5] = this.FilterNull(xmlNodeList[i].Attributes[text5].Value.ToString());
                        }
                        string str = (xmlNodeList[i].Attributes[text4] == null) ? "" : xmlNodeList[i].Attributes[text4].Value.ToString();
                        DataRow[] array = combo.Select("ID='" + str + "'");
                        if (array.Length > 0)
                        {
                            dataTable.Rows[i][text4] = array[0]["Description"];
                        }
                        if (xmlNodeList[i].Attributes[text6] != null)
                        {
                            dataTable.Rows[i][text6] = this.FilterNull(xmlNodeList[i].Attributes[text6].Value.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.ExMessage = ex.Message;
            }
            return dataTable;
        }
        private string FilterNull(string strValue)
        {
            return (strValue == null) ? "" : strValue;
        }
        private string FilterNull(XmlAttribute strValue)
        {
            string result;
            if (strValue != null)
            {
                result = strValue.Value.ToString();
            }
            else
            {
                result = "";
            }
            return result;
        }
        public string GetSysAttribute(string AttrName)
        {
            return this.FilterNull(this.m_XmlNode.Attributes[AttrName]);
        }
        public void SetSysAtrribute(string AttrName, string value)
        {
            if (this.m_XmlNode.Attributes[AttrName] != null)
            {
                this.m_XmlNode.Attributes[AttrName].Value = value;
            }
            else
            {
                XmlAttribute xmlAttribute = this.m_XmlD.CreateAttribute(AttrName);
                xmlAttribute.Value = value;
                this.m_XmlNode.Attributes.Append(xmlAttribute);
            }
        }
        private void SetAtrribute(XmlNode xln, string AttrName, string value)
        {
            if (xln.Attributes[AttrName] != null)
            {
                xln.Attributes[AttrName].Value = value;
            }
            else
            {
                XmlAttribute xmlAttribute = this.m_XmlD.CreateAttribute(AttrName);
                xmlAttribute.Value = value;
                xln.Attributes.Append(xmlAttribute);
            }
        }
        private void SetAtrribute(XmlElement xle, string AttrName, string value)
        {
            if (xle.Attributes[AttrName] != null)
            {
                xle.Attributes[AttrName].Value = value;
            }
            else
            {
                XmlAttribute xmlAttribute = this.m_XmlD.CreateAttribute(AttrName);
                xmlAttribute.Value = value;
                xle.Attributes.Append(xmlAttribute);
            }
        }
        public DataTable NodeTable(string strNodeName)
        {
            int num = strNodeName.LastIndexOf('/');
            string text = strNodeName.Substring(num + 1, strNodeName.Length - num - 1);
            DataTable dataTable = new DataTable(text);
            if (this.m_XmlNode != null)
            {
                XmlNodeList xmlNodeList = this.m_XmlNode.SelectNodes(text);
                ArrayList arrayList = new ArrayList();
                if (xmlNodeList.Count > 0)
                {
                    for (int i = 0; i < xmlNodeList.Count; i++)
                    {
                        XmlNode xmlNode = xmlNodeList[i];
                        for (int j = 0; j < xmlNode.Attributes.Count; j++)
                        {
                            string name = xmlNode.Attributes[j].Name;
                            if (!arrayList.Contains(name))
                            {
                                arrayList.Add(name);
                                dataTable.Columns.Add(name);
                            }
                        }
                    }
                    for (int k = 0; k < xmlNodeList.Count; k++)
                    {
                        if (xmlNodeList[k].Attributes.Count > 0)
                        {
                            DataRow dataRow = dataTable.Rows.Add(new object[0]);
                            for (int l = 0; l < arrayList.Count; l++)
                            {
                                string text2 = arrayList[l].ToString();
                                if (xmlNodeList[k].Attributes[text2] != null)
                                {
                                    string value = xmlNodeList[k].Attributes[text2].Value;
                                    dataRow[text2] = ((value == null) ? "" : value);
                                }
                            }
                        }
                    }
                }
            }
            return dataTable;
        }
        public DataSet GetNode(string strNodeName)
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable();
            if (this.m_XmlNode != null)
            {
                XmlNodeList xmlNodeList = this.m_XmlNode.SelectNodes(strNodeName);
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    string outerXml = xmlNodeList[i].OuterXml;
                    StringReader input = new StringReader(outerXml);
                    XmlTextReader reader = new XmlTextReader(input);
                    dataSet.ReadXml(reader);
                }
            }
            return dataSet;
        }
        public XmlNodeList GetNodeList(string strNodeName)
        {
            XmlNodeList result = null;
            if (this.m_XmlNode != null)
            {
                result = this.m_XmlNode.SelectNodes(strNodeName);
            }
            return result;
        }
        public DataTable GetDetial(string strNodeName)
        {
            DataTable dataTable = new DataTable();
            int num = 0;
            dataTable.Columns.Add("LowerLimit");
            dataTable.Columns.Add("UpperLimit");
            dataTable.Columns.Add("StockNo");
            if (this.m_XmlNode != null)
            {
                XmlNodeList xmlNodeList = this.m_XmlNode.SelectNodes(strNodeName);
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    string value = xmlNodeList[i].Attributes["ProfileList"].Value.ToString();
                    XmlNodeList childNodes = xmlNodeList[i].ChildNodes;
                    for (int j = 0; j < childNodes.Count; j++)
                    {
                        dataTable.Rows.Add(new object[0]);
                        dataTable.Rows[j]["LowerLimit"] = childNodes[j].Attributes["LowerLimit"].Value.ToString();
                        dataTable.Rows[j]["UpperLimit"] = childNodes[j].Attributes["UpperLimit"].Value.ToString();
                        dataTable.Rows[j]["StockNo"] = value;
                        num++;
                    }
                }
            }
            return dataTable;
        }
        public DataTable GetRule(string strNodeName)
        {
            DataTable dataTable = new DataTable();
            if (this.m_XmlNode != null)
            {
                XmlNodeList xmlNodeList = this.m_XmlNode.SelectNodes(strNodeName);
                ArrayList arrayList = new ArrayList();
                if (xmlNodeList.Count > 0)
                {
                    for (int i = 0; i < xmlNodeList.Count; i++)
                    {
                        XmlNode xmlNode = xmlNodeList[i];
                        for (int j = 0; j < xmlNode.Attributes.Count; j++)
                        {
                            string name = xmlNode.Attributes[j].Name;
                            if (!arrayList.Contains(name))
                            {
                                arrayList.Add(name);
                                dataTable.Columns.Add(name);
                            }
                        }
                    }
                    dataTable.Columns.Add("Condition");
                    dataTable.Columns.Add("AssemblingPoints");
                    for (int k = 0; k < xmlNodeList.Count; k++)
                    {
                        DataRow dataRow = dataTable.Rows.Add(new object[0]);
                        if (xmlNodeList[k].Attributes.Count > 0)
                        {
                            for (int l = 0; l < arrayList.Count; l++)
                            {
                                string text = arrayList[l].ToString();
                                if (xmlNodeList[k].Attributes[text] != null)
                                {
                                    string value = xmlNodeList[k].Attributes[text].Value;
                                    dataRow[text] = ((value == null) ? "" : value);
                                }
                            }
                        }
                        if (xmlNodeList[k].HasChildNodes)
                        {
                            XmlNodeList childNodes = xmlNodeList[k].ChildNodes;
                            for (int m = 0; m < childNodes.Count; m++)
                            {
                                XmlNode xmlNode2 = childNodes[m];
                                if (xmlNode2.Name == "Condition")
                                {
                                    dataRow["Condition"] = dataRow["Condition"] + xmlNode2.OuterXml;
                                }
                                if (xmlNode2.Name == "AssemblingPoints")
                                {
                                    dataRow["AssemblingPoints"] = dataRow["AssemblingPoints"] + xmlNode2.OuterXml;
                                }
                            }
                        }
                    }
                }
            }
            return dataTable;
        }
        public void SaveGlazingTable(GlassMounting[] glMounting)
        {
            if (this.m_XmlNode != null)
            {
                this.DelNode("GlazingTable");
                for (int i = 0; i < glMounting.Length; i++)
                {
                    XmlNode xmlNode = this.m_XmlD.CreateNode(XmlNodeType.Element, "GlazingTable", null);
                    this.SetAtrribute(xmlNode, "ProfileList", "");
                    ArrayList glassRow = glMounting[i].GlassRow;
                    for (int j = 0; j < glassRow.Count; j++)
                    {
                        GlassMounting.GlazingRow glazingRow = (GlassMounting.GlazingRow)glassRow[j];
                        XmlElement xmlElement = this.m_XmlD.CreateElement("GlazingRow");
                        this.SetAtrribute(xmlElement, "UpperLimit", glazingRow.Upperlimit);
                        this.SetAtrribute(xmlElement, "LowerLimit", glazingRow.Lowerlimit);
                        this.SetAtrribute(xmlElement, "Bead", glazingRow.Bead);
                        this.SetAtrribute(xmlElement, "FSGasket", glazingRow.FSGasket);
                        this.SetAtrribute(xmlElement, "BGasket", glazingRow.BGasket);
                        xmlNode.AppendChild(xmlElement);
                    }
                    this.m_XmlNode.AppendChild(xmlNode);
                }
            }
        }
        public void deleteSys()
        {
            if (this.m_XmlNode != null)
            {
                XmlNode xmlNode = this.m_XmlD.SelectSingleNode("/WSMD");
                xmlNode.RemoveChild(this.m_XmlNode);
                this.m_XmlNode = null;
                this.m_XmlNodeList = null;
            }
        }
        public void DelNode(string strNodeName)
        {
            if (this.m_XmlNode != null)
            {
                XmlNodeList xmlNodeList = this.m_XmlNode.SelectNodes(strNodeName);
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    this.m_XmlNode.RemoveChild(xmlNodeList[i]);
                }
            }
        }
        public void AddJoint(Joints js)
        {
            if (this.m_XmlNode != null)
            {
                XmlNode xmlNode = this.m_XmlD.CreateNode(XmlNodeType.Element, "Joint", null);
                this.SetAtrribute(xmlNode, "ID", js.ID);
                this.SetAtrribute(xmlNode, "Description", js.Description);
                this.SetAtrribute(xmlNode, "GroupID", js.LeftTop);
                this.SetAtrribute(xmlNode, "Default", js.LeftBottom);
                this.SetAtrribute(xmlNode, "OptionList", js.RightTop);
                this.m_XmlNode.AppendChild(xmlNode);
            }
        }
        public void AddPart(Part pt)
        {
            if (this.m_XmlNode != null)
            {
                XmlNode xmlNode = this.m_XmlD.CreateNode(XmlNodeType.Element, "Part", null);
                this.SetAtrribute(xmlNode, "StockNo", pt.StockNo);
                this.SetAtrribute(xmlNode, "PackNum", pt.PackNum);
                this.SetAtrribute(xmlNode, "Description", pt.Description);
                this.SetAtrribute(xmlNode, "FormatCode", pt.FormatCode);
                this.SetAtrribute(xmlNode, "Weight", pt.Weight);
                this.SetAtrribute(xmlNode, "Wastage", pt.Wastage);
                this.SetAtrribute(xmlNode, "FullSize", pt.FullSize);
                this.SetAtrribute(xmlNode, "InertiaY", pt.InteriaY);
                this.SetAtrribute(xmlNode, "InertiaZ", pt.InteriaZ);
                this.SetAtrribute(xmlNode, "ShortDescription", pt.ShortDescription);
                this.m_XmlNode.AppendChild(xmlNode);
            }
        }
        public void AddRule(Rule ru)
        {
            if (this.m_XmlNode != null)
            {
                XmlNode xmlNode = this.m_XmlD.CreateNode(XmlNodeType.Element, "Rule", null);
                this.SetAtrribute(xmlNode, "ID", ru.ID);
                this.SetAtrribute(xmlNode, "StockNo", ru.StockNo);
                this.SetAtrribute(xmlNode, "Level", ru.Level);
                this.SetAtrribute(xmlNode, "AssemblingPosition", ru.AsmPosition);
                this.SetAtrribute(xmlNode, "DesignElement", ru.DesignElement);
                this.SetAtrribute(xmlNode, "LengthAdjust", ru.LengthAdjust);
                this.SetAtrribute(xmlNode, "Quantity", ru.Quantity);
                this.SetAtrribute(xmlNode, "Clearance", ru.Clearance);
                this.SetAtrribute(xmlNode, "Text", ru.Text);
                this.SetAtrribute(xmlNode, "RuleType", ru.Ruletype);
                if (ru.Condition != "")
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    string xml = "<EL>\n\r" + ru.Condition + "\r\n</EL>";
                    xmlDocument.LoadXml(xml);
                    XmlNodeList xmlNodeList = xmlDocument.SelectNodes("EL/Condition");
                    for (int i = 0; i < xmlNodeList.Count; i++)
                    {
                        XmlNode newChild = this.m_XmlD.ImportNode(xmlNodeList[i], true);
                        xmlNode.AppendChild(newChild);
                    }
                }
                if (ru.AssemblingPoints != "")
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    string xml = "<EL>\n\r" + ru.AssemblingPoints + "\r\n</EL>";
                    xmlDocument.LoadXml(xml);
                    XmlNodeList xmlNodeList = xmlDocument.SelectNodes("EL/AssemblingPoints");
                    for (int i = 0; i < xmlNodeList.Count; i++)
                    {
                        XmlNode newChild = this.m_XmlD.ImportNode(xmlNodeList[i], true);
                        xmlNode.AppendChild(newChild);
                    }
                }
                this.m_XmlNode.AppendChild(xmlNode);
            }
        }
        public void AddASMPair(AsmPair ap)
        {
            if (this.m_XmlNode != null)
            {
                XmlNode xmlNode = this.m_XmlD.CreateNode(XmlNodeType.Element, "AssemblingPair", null);
                this.SetAtrribute(xmlNode, "StockNo", ap.StockNO);
                XmlNode xmlNode2 = this.m_XmlD.CreateNode(XmlNodeType.Element, "PairRelation", null);
                this.SetAtrribute(xmlNode2, "StockNo", "@MJ@");
                this.SetAtrribute(xmlNode2, "OffsetValue", ap.MJValue);
                this.SetAtrribute(xmlNode2, "Manual", ap.MJManual);
                xmlNode.AppendChild(xmlNode2);
                XmlNode xmlNode3 = this.m_XmlD.CreateNode(XmlNodeType.Element, "PairRelation", null);
                this.SetAtrribute(xmlNode3, "StockNo", "@G@");
                this.SetAtrribute(xmlNode3, "OffsetValue", ap.GValue);
                this.SetAtrribute(xmlNode3, "Manual", ap.GManual);
                xmlNode.AppendChild(xmlNode3);
                XmlNode xmlNode4 = this.m_XmlD.CreateNode(XmlNodeType.Element, "PairRelation", null);
                this.SetAtrribute(xmlNode4, "StockNo", "@S@");
                this.SetAtrribute(xmlNode4, "OffsetValue", ap.SValue);
                this.SetAtrribute(xmlNode4, "Manual", ap.SManual);
                xmlNode.AppendChild(xmlNode4);
                XmlNode xmlNode5 = this.m_XmlD.CreateNode(XmlNodeType.Element, "PairRelation", null);
                this.SetAtrribute(xmlNode5, "StockNo", "@F@");
                this.SetAtrribute(xmlNode5, "OffsetValue", ap.UValue);
                this.SetAtrribute(xmlNode5, "Manual", ap.UManual);
                xmlNode.AppendChild(xmlNode5);
                this.m_XmlNode.AppendChild(xmlNode);
            }
        }
        public void AddOption(string strXMLOption)
        {
            if (this.m_XmlNode != null)
            {
                if (strXMLOption != "")
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    strXMLOption = "<EL>\n\r" + strXMLOption + "\r\n</EL>";
                    xmlDocument.LoadXml(strXMLOption);
                    XmlNodeList xmlNodeList = xmlDocument.SelectNodes("EL/Option");
                    for (int i = 0; i < xmlNodeList.Count; i++)
                    {
                        XmlNode newChild = this.m_XmlD.ImportNode(xmlNodeList[i], true);
                        this.m_XmlNode.AppendChild(newChild);
                    }
                }
            }
        }
        public void AddJoint(XmlNodeList xnl)
        {
            if (this.m_XmlNode != null)
            {
                for (int i = 0; i < xnl.Count; i++)
                {
                    XmlNode newChild = this.m_XmlD.ImportNode(xnl[i], true);
                    this.m_XmlNode.AppendChild(newChild);
                }
            }
        }
        public void save()
        {
			this.m_XmlD.Save(CoreLib.Common.XmlPath() + @"wsmd.xml");
			Encrypt.EncryptInMemory(CoreLib.Common.XmlPath() + @"wsmd.xml", "decasoft");
        }
        public string SystXmlToString()
        {
            string result;
            if (this.m_XmlNode != null)
            {
                result = this.m_XmlNode.OuterXml;
            }
            else
            {
                result = "";
            }
            return result;
        }
    }
}

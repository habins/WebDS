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
	public class FactoryDef
    {
        // Fields
        private static Exception m_ex;
        public static XmlDocument m_XmlD;
        public static XmlNode m_XmlNode;
        public static XmlNodeList m_XmlNodeList;

        // Methods
        private static string FilterNull(XmlAttribute strValue)
        {
            if (strValue != null)
            {
                return strValue.Value.ToString();
            }
            return "";
        }

        public static DataTable GetFactory()
        {
            DataTable table = new DataTable();
            if (OpenXml())
            {
                table.Columns.Add("ID");
                table.Columns.Add("Description");
                table.Columns.Add("imgcolid");
                for (int i = 0; i < m_XmlNodeList.Count; i++)
                {
                    if (m_XmlNodeList[i].Attributes["ID"] != null)
                    {
                        table.Rows.Add(new object[0]);
                        table.Rows[i]["ID"] = m_XmlNodeList[i].Attributes["ID"].Value;
                        table.Rows[i]["Description"] = FilterNull(m_XmlNodeList[i].Attributes["Description"]);
                        table.Rows[i]["imgcolid"] = FilterNull(m_XmlNodeList[i].Attributes["imgcolid"]);
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
				string filename = CoreLib.Common.XmlPath() + @"FactoryDef.xml";
                if (filename != "")
                {
                    m_XmlD.Load(filename);
                    m_XmlNode = m_XmlD.SelectSingleNode("/FacData");
                    m_XmlNodeList = m_XmlNode.ChildNodes;
                    flag = true;
                }
            }
            catch (XmlException exception)
            {
                throw new XmlException(exception.Message);
            }
            return flag;
        }
    }

}

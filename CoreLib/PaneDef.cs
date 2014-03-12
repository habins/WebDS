using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data;
using System.Collections;
using System.IO;
using Microsoft.Win32;

namespace CoreLib
{
    public class PaneDef
    {
        // Fields
        private static Exception m_ex;
        public static XmlDocument m_XmlD;
        public static XmlNode m_XmlNode;
        public static XmlNodeList m_XmlNodeList;

        // Methods
        private static string FilterNull(string strValue)
        {
            return ((strValue == null) ? "" : strValue);
        }

        public static DataTable GetPane()
        {
            DataTable table = new DataTable();
            if (OpenXml())
            {
                table.Columns.Add("ID");
                table.Columns.Add("Description");
                table.Columns.Add("ImageID");
                for (int i = 0; i < m_XmlNodeList.Count; i++)
                {
                    if (m_XmlNodeList[i].Attributes["ID"] != null)
                    {
                        table.Rows.Add(new object[0]);
                        table.Rows[i]["ID"] = m_XmlNodeList[i].Attributes["ID"].Value;
                        if (m_XmlNodeList[i].Attributes["Description"] != null)
                        {
                            table.Rows[i]["Description"] = m_XmlNodeList[i].Attributes["Description"].Value;
                        }
                        if (m_XmlNodeList[i].Attributes["ImageFromOutsideHingle"] != null)
                        {
                            table.Rows[i]["ImageID"] = FilterNull(m_XmlNodeList[i].Attributes["ImageFromOutsideHingle"].Value);
                        }
                    }
                }
            }
            return table;
        }

        public static string GetPanePath(string strKeyName)
        {
			return CoreLib.Common.XmlPath() + strKeyName + @".xml";
			//RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Decasoft\WstarBeta");
			//string str = "";
			//if (key != null)
			//{
			//	str = key.GetValue(strKeyName).ToString();
			//}
			//return str;
        }

        private static bool OpenXml()
        {
            bool flag = false;
            m_XmlD = new XmlDocument();
            try
            {
				string filename = CoreLib.Common.XmlPath() +@"panedef12.xml";
                if (filename != "")
                {
                    m_XmlD.Load(filename);
                    m_XmlNode = m_XmlD.SelectSingleNode("/PaneStruct/PaneDef");
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

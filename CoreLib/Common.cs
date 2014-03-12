using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;

namespace CoreLib
{
    public class Common
    {

        public static IList<T> GetList<T>(DataTable table)
        {
            IList<T> list = new List<T>(); //里氏替换原则
            T t = default(T);
            PropertyInfo[] propertypes = null;
            string tempName = string.Empty;
            foreach (DataRow row in table.Rows)
            {
                t = Activator.CreateInstance<T>(); ////创建指定类型的实例

                propertypes = t.GetType().GetProperties(); //得到类的属性
                foreach (PropertyInfo pro in propertypes)
                {
                    tempName = pro.Name;
                    if (table.Columns.Contains(tempName.ToUpper()))
                    {
                        object value = row[tempName];
                        if (value is System.DBNull)
                        {
                            value = "";
                        }
                        pro.SetValue(t, value, null);
                    }
                }
                list.Add(t);
            }
            return list;
        }

		//XML文件路径
		public static string XmlPath() {
			string strExePath = System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);
			
			if (System.IO.Directory.Exists(strExePath + "\\Xml"))
				return strExePath + "\\Xml\\";
			
			if (System.IO.Directory.Exists(strExePath + "\\..\\Xml"))
				return strExePath + "\\..\\Xml\\";

			if (System.IO.Directory.Exists(strExePath + "\\..\\..\\Xml"))
				return strExePath + "\\..\\..\\Xml\\";

			if (System.IO.Directory.Exists(strExePath + "\\..\\..\\..\\Xml"))
				return strExePath + "\\..\\..\\..\\Xml\\";

			if (System.IO.Directory.Exists(strExePath + "\\..\\..\\..\\..\\Xml"))
				return strExePath + "\\..\\..\\..\\..\\Xml\\";

			if (System.IO.Directory.Exists(strExePath + "\\..\\..\\..\\..\\..\\Xml"))
				return strExePath + "\\..\\..\\..\\..\\..\\Xml\\";

			if (System.IO.Directory.Exists(strExePath + "\\..\\..\\..\\..\\..\\..\\Xml"))
				return strExePath + "\\..\\..\\..\\..\\..\\..\\Xml\\";

			return strExePath + "\\Xml\\";
		}

		
    }
}

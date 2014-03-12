using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreLib;
using System.Data;

namespace WebDS.Controllers
{
    public class AjaxServiceController : Controller
    {
        //
        // GET: /AjaxService/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetVendorInfo(string id)
        {
            string text = id;
            if (text.Length == 1)
            {
                text = "00" + text;
            }
            else
            {
                if (text.Length == 2)
                {
                    text = "0" + text;
                }
            }
            SystemData systemData = new SystemData();
            DataTable dtList = systemData.GetSysList();
            IList<Vendor> list = CoreLib.Common.GetList<Vendor>(dtList);
            var newList = from Vendor s in list
                    where s.VendorID == text 
                    select s;
            return Json(newList);

        }

        [HttpPost]
        public ActionResult GetPattern()
        {
            DataTable dtList = DataDef.GetCombo("/DataDef/SystemPatterns");
            return Json(CoreLib.Common.GetList<CoreLib.Combo>(dtList));
            
        }

        [HttpPost]
        public ActionResult GetSysType()
        {
            DataTable dtList = DataDef.GetCombo("/DataDef/SystemTypes");
            return Json(CoreLib.Common.GetList<CoreLib.Combo>(dtList));
        }


        [HttpPost]
        public ActionResult GetGlazingType()
        {
            DataTable dtList = DataDef.GetCombo("/DataDef/GlazingTypes");
            return Json(CoreLib.Common.GetList<CoreLib.Combo>(dtList));
        }

        [HttpPost]
        public ActionResult GetFactory()
        {
            DataTable dtList = FactoryDef.GetFactory();
            return Json(CoreLib.Common.GetList<CoreLib.Combo>(dtList));
        }

        [HttpGet]
        public ActionResult GetFormatCode()
        {
            DataTable dtList = DataDef.GetCombo("/DataDef/FormatCodes");
            return Json(CoreLib.Common.GetList<CoreLib.Combo>(dtList), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetLevel()
        {
            DataTable dtList = DataDef.GetCombo("/DataDef/Levels");
            return Json(CoreLib.Common.GetList<CoreLib.Combo>(dtList), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDesignElement()
        {
            DataTable dtList = DataDef.GetCombo("/DataDef/DesignElements");
            return Json(CoreLib.Common.GetList<CoreLib.Combo>(dtList), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAssemblingPosition()
        {
            DataTable dtList = DataDef.GetCombo("/DataDef/AssemblingPositions");
            return Json(CoreLib.Common.GetList<CoreLib.Combo>(dtList), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDataPart()
        {
            string id = Request["ids"];
            string dataVersion = Request["dv"];
            CoreLib.SystemData sysData = new SystemData(id, dataVersion);
            DataTable dataTable = sysData.NodeTable("Part");
            return Json(CoreLib.Common.GetList<CoreLib.Part>(dataTable), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDataRule()
        {
            string id = Request["ids"];
            string dataVersion = Request["dv"];
            CoreLib.SystemData sysData = new SystemData(id, dataVersion);
            DataTable dataTable = sysData.NodeTable("Rule");
            return Json(CoreLib.Common.GetList<CoreLib.Rule>(dataTable), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDataAsmPair()
        {
            string id = Request["ids"];
            string dataVersion = Request["dv"];
            CoreLib.SystemData sysData = new SystemData(id, dataVersion);
            //DataTable dataTable = sysData.NodeTable("AssemblingPair");

            DataSet node = sysData.GetNode("AssemblingPair");
            if (node.Tables.Contains("AssemblingPair"))
            {
                DataTable dataTable = node.Tables["AssemblingPair"];
                return Json(CoreLib.Common.GetList<CoreLib.AsmPair>(dataTable), JsonRequestBehavior.AllowGet);
            }
            else
                return null;
        }
        
        public ActionResult GetTestData()
        {
            string data=string.Empty;
            int counts=20;
            data+="{"; 
            data+="\"total\":"; 
            data+= counts+","; 
            data+="\"items\":["; 
            for( int i = 1;i<=30;i++)
            {
               data+="{"; 
               data+="\"id\":"+i; 
               data+=","; 
               data+="\"title\":\"newstitle"+i+"\""; 
               data+=","; 
               data+="\"author\":\"author"+i+"\""; 
               data+=","; 
               data+="\"hits\":5"; 
               data+=","; 
               data+="\"addtime\":\""+DateTime.Now.ToString()+"\""; 
               data+=","; 
               if( i % 4 == 0)
                  data +="\"checked\":\"true\""; 
               else
                  data+="\"checked\":\"false\""; 

               data+="}"; 
               if ( i<30 ) data+=","; 
    
            }
            data += "]}"; 
            return Json(data);
        }

    }
}

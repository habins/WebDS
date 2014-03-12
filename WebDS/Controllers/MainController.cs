using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreLib;
using System.Data;

namespace WebDS.Controllers
{
    public class MainController : Controller
    {
        //
        // GET: /Main/

        public ActionResult Index()
        {
			ViewData["flag"] = 0;
            #region 加载厂家信息
                SystemData systemData = new SystemData();
                DataTable sysList = systemData.GetSysList();
                Dictionary<string, string> vendorList = new Dictionary<string, string>();
                if (sysList.Rows.Count > 0)
                {
                    for (int i = 0; i < sysList.Rows.Count; i++)
                    {
                        string id = sysList.Rows[i]["VendorID"].ToString();
                        string text = sysList.Rows[i]["VendorDescr"].ToString();
                        if (!vendorList.ContainsValue(text) && text != "")
                        {
                            vendorList.Add(id, text);

                        }
                    }
                }
                ViewData["VendorList"] = vendorList;
            #endregion
            return View();
        }


		public ActionResult Edit(string id,string dataversion) {

            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(dataversion))
            {
                SystemData systemData = new SystemData(id, dataversion);
	            ViewData["ID"] = id;//ID

	            ViewData["SystemType"] = systemData.GetSysAttribute("SystemType");//系统类型
	            ViewData["SystemPattern"] = systemData.GetSysAttribute("SystemPattern");//系统模式
	            ViewData["Description"] = systemData.GetSysAttribute("Description");//系统描述
	            ViewData["VendorID"] = systemData.GetSysAttribute("VendorID");//厂家ID
	            ViewData["VendorDescr"] = systemData.GetSysAttribute("VendorDescr");//厂家名称
	            ViewData["ProfileWidth"] = systemData.GetSysAttribute("ProfileWidth");//型材宽度
	            ViewData["ProfileThickness"] = systemData.GetSysAttribute("ProfileThickness");//型材壁厚
	            ViewData["ExtrudedGasket"] = systemData.GetSysAttribute("ExtrudedGasket");//胶条共挤
	            ViewData["SizeC"] = systemData.GetSysAttribute("SizeC");//尺寸差
	            ViewData["OverlapH"] = systemData.GetSysAttribute("OverlapH");//水平搭接量
	            ViewData["OverlapV"] = systemData.GetSysAttribute("OverlapV");//垂直搭接量
	            ViewData["ClearanceG"] = systemData.GetSysAttribute("ClearanceG");//玻璃间隙
	            ViewData["GlazingType"] = systemData.GetSysAttribute("GlazingType");//压条安装类型
            }
			ViewData["flag"] = 1;
			return View("index");
		}

		public ActionResult Add() {
			ViewData["flag"] = 2;
			return View("index");
		}

		public ActionResult Del() {
			ViewData["flag"] = 3;
			return View("index");
		}
		
		public ActionResult Role() {

			return View();
		}

		public PartialViewResult Tab1() {

			return PartialView();
		}

		public PartialViewResult Tab2() {

			return PartialView();
		}

    }
}

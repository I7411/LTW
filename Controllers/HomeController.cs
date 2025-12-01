using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LTWeb01_Bai01.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Buoi1()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult HienThi(string id)
        {
            return Content("ID = " +id);
        }

        public ActionResult Index3(string id, string name)
        {
            //Use ViewBag
            ViewBag.Id = id;
            ViewBag.Name = name;

            //Use ViewData
            ViewData["Id"] = id;
            ViewData["Name"] = name;
            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
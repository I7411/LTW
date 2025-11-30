using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library_Management.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult ReaderHome()
        {
            return View();
        }
        public ActionResult AdminHome() => View();
        public ActionResult LibrarianHome() => View();
    }
}
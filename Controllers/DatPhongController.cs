using QLTHUVIEN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLTHUVIEN.Controllers
{
    public class DatPhongController : Controller
    {
        QUANLYTHUVIENEntities db = new QUANLYTHUVIENEntities();

        public ActionResult Index()
        {
            var dsPhong = db.PHONGHOC.Where(x => x.TRANGTHAI == "Hoạt động").ToList();
            return View(dsPhong);
        }

        // Form đặt phòng
        public ActionResult Dat(string maPhong)
        {
            ViewBag.MaPhong = maPhong;
            return View();
        }

        [HttpPost]
        public ActionResult Dat(DATPHONG dp)
        {
            dp.MADATPHONG = "DP" + DateTime.Now.Ticks;
            dp.NGAYDAT = DateTime.Now;
            dp.TRANGTHAI = "Chờ duyệt";

            db.DATPHONG.Add(dp);
            db.SaveChanges();

            return RedirectToAction("ThanhCong");
        }

        public ActionResult ThanhCong()
        {
            return View();
        }
    }
}
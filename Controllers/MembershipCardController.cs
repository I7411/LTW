using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library_Management.Models;

namespace Library_Management.Controllers
{
    public class MembershipCardController : Controller
    {
        // GET: MembershipCard
        QUANLYTHUVIENEntities1 Database = new QUANLYTHUVIENEntities1();
        public ActionResult CardRegistration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddCard()
        {
            var userId = Session["MANGUOIDUNG"];
            var trangThai = Database.THEBANDOCs.FirstOrDefault(t => t.MATHANHVIEN == (string)userId);
            if(trangThai.TINHTRANGTHE.Equals("Yêu cầu cấp thẻ"))
            {
                TempData["Notice"] = "Thẻ đang được chuẩn bị!";
            }
            else
            {
                trangThai.TINHTRANGTHE = "Yêu cầu cấp thẻ";
                Database.SaveChanges();
                TempData["Notice"] = "Đã gửi yêu cầu!";
            }
            return RedirectToAction("CardRegistration");
        }
        [HttpPost]
        public ActionResult renew()
        {
            var userId = Session["MANGUOIDUNG"];
            var trangThai = Database.THEBANDOCs.FirstOrDefault(t => t.MATHANHVIEN == (string)userId);
            if (trangThai.TINHTRANGTHE.Equals("Yêu cầu gia hạn"))
            {
                TempData["Notice"] = "Đã gửi yêu cầu gia hạn trước đó!";
            }
            else
            {
                trangThai.TINHTRANGTHE = "Yêu cầu gia hạn";
                Database.SaveChanges();
                TempData["Notice"] = "Đã gửi yêu cầu gia hạn!";
            }
            return RedirectToAction("CardRegistration");
        }
        public ActionResult reissueCard() => View();
        public ActionResult re_issueCard(string lydo)
        {
            var userId = Session["MANGUOIDUNG"];
            var trangThai = Database.THEBANDOCs.FirstOrDefault(t => t.MATHANHVIEN == (string)userId);
            if (trangThai.TINHTRANGTHE.Equals("Mất thẻ") || trangThai.TINHTRANGTHE.Equals("Hỏng thẻ") || trangThai.TINHTRANGTHE.Equals("Thay đổi thông tin"))
            {
                TempData["Notice"] = "Đã gửi yêu cầu trước đó!";
            }
            else
            {
                trangThai.TINHTRANGTHE = lydo;
                Database.SaveChanges();
                TempData["Notice"] = "Đã gửi yêu cầu gia hạn!";
            }
            return RedirectToAction("reissueCard");
        }
    }
}
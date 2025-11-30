using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library_Management.Models;

namespace Library_Management.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Report()
        {
            return View();
        }
        public ActionResult RequestsAndReports() => View();
        private QUANLYTHUVIENEntities1 db = new QUANLYTHUVIENEntities1();

        [HttpPost]
        public JsonResult CapNhatTrangThai(string maBaoCao, string trangThaiMoi)
        {
            try
            {
                var baoCao = db.MUATAILIEUMOIs.Find(maBaoCao);
                if (baoCao == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy báo cáo!" });
                }

                baoCao.TRANGTHAI = trangThaiMoi;
                db.SaveChanges();

                return Json(new { success = true, message = "Cập nhật trạng thái thành công!" });
            }
            catch
            {
                return Json(new { success = false, message = "Đã xảy ra lỗi khi cập nhật!" });
            }
        }
    }
}
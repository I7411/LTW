using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library_Management.Models;

namespace Library_Management.Controllers
{
    public class RequiredDocumentsController : Controller
    {
        // GET: RequiredDocuments
        public ActionResult Documents()
        {
            return View();
        }
        private readonly QUANLYTHUVIENEntities db = new QUANLYTHUVIENEntities();

        [HttpGet]
        public JsonResult GetDocumentRequestInfo(string maTaiLieu, string maDocGia)
        {
            if (string.IsNullOrEmpty(maTaiLieu) || string.IsNullOrEmpty(maDocGia))
                return Json(null, JsonRequestBehavior.AllowGet);

            var data = (from p in db.PHIEUMUONs
                        join the in db.THEBANDOCs on p.MASOTHE equals the.MASOTHE
                        join dg in db.DOCGIAs on the.MATHANHVIEN equals dg.MATHANHVIEN
                        join k in db.GIAHANTAILIEUx on p.MAPHIEUMUON equals k.MAPHIEUMUON
                        join t in db.CHITIETPHIEUMUONs on p.MAPHIEUMUON equals t.MAPHIEUMUON
                        join tl in db.TAILIEUx on t.MATAILIEU equals tl.MATAILIEU
                        where t.MATAILIEU == maTaiLieu && p.MASOTHE == maDocGia
                        select new
                        {
                            MaPhieu = k.MAGIAHAN,
                            YeuCau = k.YEUCAU,
                            MaDocGia = p.MASOTHE,
                            MaTaiLieu = t.MATAILIEU,
                            TenTaiLieu = tl.TENSACH,
                            TacGia = tl.TENTACGIA,
                            Khoa = dg.KHOAHOC,
                            NhaXuatBan = tl.NXB,
                            Lop = "",
                            NgonNgu = tl.NGONNGU,
                            SoLanGiaHan = "",
                            NgayMuon = p.NGAYMUON.ToString(),
                            NgayTra = p.NGAYTRA.ToString()
                        }).FirstOrDefault();

            if (data == null)
                return Json(null, JsonRequestBehavior.AllowGet);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpPost]
        public ActionResult Accept(string maGH)
        {

            if (string.IsNullOrEmpty(maGH))
            {
                return HttpNotFound();
            }

            // Lấy bản ghi đặt phòng theo mã phòng
            var giahan = db.GIAHANTAILIEUx.FirstOrDefault(t => t.MAGIAHAN == maGH);

            if (giahan == null)
            {
                return HttpNotFound();
            }

            if (giahan.TRANGTHAI == "Chờ duyệt")
            {
                giahan.TRANGTHAI = "Được gia hạn";
                db.SaveChanges();
                TempData["Notice"] = "Gia hạn thành công.";
            }
            else
            {
                TempData["Notice"] = "Yêu cầu đang không ở trạng thái chờ duyệt, không thể gia hạn.";
            }

            return RedirectToAction("Documents");
        }
        [HttpGet]
        public ActionResult CreateLoanForm()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Add(string maPhieu, string maDocGia, string nguoiLap, string ngayLap)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maPhieu) || string.IsNullOrWhiteSpace(maDocGia) ||
                    string.IsNullOrWhiteSpace(nguoiLap) || string.IsNullOrWhiteSpace(ngayLap))
                {
                    return Json(new { success = false, message = "Vui lòng nhập đầy đủ thông tin!" });
                }

                if (db.PHIEUMUONs.Any(p => p.MAPHIEUMUON == maPhieu))
                {
                    return Json(new { success = false, message = "Mã phiếu đã tồn tại!" });
                }
                if (!DateTime.TryParse(ngayLap, out DateTime ngayMuon))
                {
                    return Json(new { success = false, message = "Ngày lập không hợp lệ!" });
                }
                if (!db.DATMUONTRUOCs.Any(p => p.MASOTHE == maDocGia))
                {
                    return Json(new { success = false, message = "Độc giả chưa mượn tài liệu!" });
                }
                var datMuon = db.DATMUONTRUOCs.Where(t => t.MASOTHE.Equals(maDocGia)).ToList();
                foreach (var item in datMuon)
                {
                    item.TRANGTHAI = "Đang mượn";
                }
                var pm = new PHIEUMUON
                {
                    MAPHIEUMUON = maPhieu,
                    MASOTHE = maDocGia,
                    MANV = nguoiLap,
                    NGAYMUON = ngayMuon,
                    NGAYTRA = ngayMuon.AddDays(30)
                };

                db.PHIEUMUONs.Add(pm);
                db.SaveChanges();

                return Json(new { success = true, message = "Thêm phiếu mượn thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Lỗi khi cập nhật phiếu mượn: " + ex.Message + " | Chi tiết: " + ex.InnerException?.Message
                });
            }
        }        // Xóa phiếu mượn
        [HttpPost]
        public JsonResult Delete(string maPhieu)
        {
            try
            {
                var pm = db.PHIEUMUONs.FirstOrDefault(p => p.MAPHIEUMUON == maPhieu);
                var giahan = db.GIAHANTAILIEUx.FirstOrDefault(t => t.MAPHIEUMUON == maPhieu);
                var ct = db.CHITIETPHIEUMUONs.FirstOrDefault(t => t.MAPHIEUMUON == maPhieu);
                var pp = db.PHIEUPHATs.FirstOrDefault(t => t.MAPHIEUMUON == maPhieu);
                if (pm == null)
                    return Json(new { success = false, message = "Không tìm thấy phiếu mượn để xóa." });

                db.GIAHANTAILIEUx.Remove(giahan);
                db.CHITIETPHIEUMUONs.Remove(ct);
                db.PHIEUPHATs.Remove(pp);
                db.PHIEUMUONs.Remove(pm);
              
                db.SaveChanges();

                return Json(new { success = true, message = "Xóa phiếu mượn thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi xóa phiếu: " + ex.Message });
            }
        }

        // Cập nhật phiếu mượn
        [HttpPost]
        public JsonResult Update(string maPhieu, string maDocGia, string nguoiLap, string ngayLap)
        {
            try
            {
                var pm = db.PHIEUMUONs.FirstOrDefault(p => p.MAPHIEUMUON == maPhieu);
                if (pm == null)
                    return Json(new { success = false, message = "Không tìm thấy phiếu để cập nhật." });

                pm.MASOTHE = maDocGia;
                pm.MANV = nguoiLap;
                pm.NGAYMUON = DateTime.Parse(ngayLap);
                db.SaveChanges();

                return Json(new { success = true, message = "Cập nhật phiếu mượn thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi cập nhật phiếu: " + ex.Message });
            }
        }

        [HttpGet]
        public JsonResult Report()
        {
            try
            {
                var report = db.PHIEUMUONs
                    .GroupBy(p => new { p.NGAYMUON.Month, p.NGAYMUON.Year })
                    .Select(g => new
                    {
                        Thang = g.Key.Month,
                        Nam = g.Key.Year,
                        SoLuong = g.Count()
                    }).OrderBy(r => r.Nam).ThenBy(r => r.Thang)
                    .ToList();

                return Json(report, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi khi tạo báo cáo: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

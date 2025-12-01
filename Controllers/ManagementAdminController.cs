using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library_Management.Models;

namespace Library_Management.Controllers
{
    public class ManagementAdminController : Controller
    {
        // GET: ManagementAdmin
        public ActionResult HRM()
        {
            return View();
        }
        private readonly QUANLYTHUVIENEntities db = new QUANLYTHUVIENEntities();
        [HttpPost]
        public JsonResult ThemNhanVien(NHANVIEN model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });

            try
            {
                var exists = db.NHANVIENs.Find(model.MANV);
                if (exists != null)
                    return Json(new { success = false, message = "Mã nhân viên đã tồn tại!" });
                var nhanvien = new NHANVIEN
                {
                    MANV = model.MANV,
                    TENNV = model.TENNV,
                    NGSINH = model.NGSINH,
                    VAITRO = model.VAITRO,
                    DIACHI = model.DIACHI,
                    SODIENTHOAI = model.SODIENTHOAI,
                    TAIKHOAN = model.MANV,
                    MATKHAU = "123"
                };
                db.NHANVIENs.Add(nhanvien);
                db.SaveChanges();

                return Json(new { success = true, message = "Thêm nhân viên thành công!" });
            }
            catch (Exception ex)
            {
                // có thể log ex.Message ở đây nếu cần
                return Json(new { success = false, message = "Thêm thất bại: " + ex.Message });
            }
        }

        [HttpPost]
        public JsonResult XoaNhanVien(string ma)
        {
            if (string.IsNullOrEmpty(ma))
                return Json(new { success = false, message = "Mã nhân viên không được để trống!" });

            try
            {
                var nv = db.NHANVIENs.Find(ma);
                if (nv == null)
                    return Json(new { success = false, message = "Không tìm thấy nhân viên!" });



                var quanlytailieux = db.QUANLYTAILIEUx.Where(x => x.MANV == ma);
                db.QUANLYTAILIEUx.RemoveRange(quanlytailieux);


                var muatailieumois = db.MUATAILIEUMOIs.Where(x => x.MANV == ma);
                db.MUATAILIEUMOIs.RemoveRange(muatailieumois);

                var thanhlaytailieux = db.THANHLYTAILIEUx.Where(x => x.MANV == ma);
                db.THANHLYTAILIEUx.RemoveRange(thanhlaytailieux);


                var phieumuons = db.PHIEUMUONs.Where(x => x.MANV == ma);
                db.PHIEUMUONs.RemoveRange(phieumuons);

                var phieuphats = db.PHIEUPHATs.Where(x => x.MANV == ma);
                db.PHIEUPHATs.RemoveRange(phieuphats);

                // Cuối cùng xóa nhân viên
                db.NHANVIENs.Remove(nv);

                db.SaveChanges();

                return Json(new { success = true, message = "Xóa thành công!" });
            }
            catch (Exception ex)
            {
                var inner = ex.InnerException != null ? ex.InnerException.Message : "";
                return Json(new { success = false, message = "Xóa thất bại: " + ex.Message + " - " + inner });
            }

        }
        [HttpPost]
        public JsonResult CapNhatNhanVien(NHANVIEN model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });

            try
            {
                var nv = db.NHANVIENs.Find(model.MANV);
                if (nv == null)
                    return Json(new { success = false, message = "Không tìm thấy nhân viên!" });

                nv.TENNV = model.TENNV;
                nv.NGSINH = model.NGSINH;
                nv.DIACHI = model.DIACHI;
                nv.VAITRO = model.VAITRO;
                nv.SODIENTHOAI = model.SODIENTHOAI;

                db.SaveChanges();

                return Json(new { success = true, message = "Cập nhật thành công!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Cập nhật thất bại: " + ex.Message });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult AccountManagement()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpdateReader(string ten, string taikhoan, string matkhau, string email)
        {
            var reader = db.DOCGIAs.FirstOrDefault(r => r.TAIKHOAN == taikhoan);
            if (reader != null)
            {
                reader.TENTV = ten;
                reader.MATKHAU = matkhau;
                reader.EMAIL = email;
                db.SaveChanges();
            }
            return RedirectToAction("AccountManagement");
        }

        [HttpPost]
        public ActionResult UpdateStaff(string ten, string taikhoan, string matkhau, string email)
        {
            var staff = db.NHANVIENs.FirstOrDefault(s => s.TAIKHOAN == taikhoan);
            if (staff != null)
            {
                staff.TENNV = ten;
                staff.MATKHAU = matkhau;
                staff.EMAIL = email;
                db.SaveChanges();
            }
            return RedirectToAction("AccountManagement");
        }
    }
}

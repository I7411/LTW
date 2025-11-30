using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library_Management.Models;
using System.Data.Entity;

namespace Library_Management.Controllers
{
    public class InfomationController : Controller
    {
        // GET: Infomation
        //Gọi CSDL
        QUANLYTHUVIENEntities1 Database = new QUANLYTHUVIENEntities1();
        
        public ActionResult InfoReader()
        {
            return View();
        }
        public ActionResult InfoLibrarian() => View();
        public ActionResult InfoAdmin() => View();
        [HttpGet]
        public ActionResult UpdateInfoReader() => View();
        [HttpGet]
        public ActionResult UpdateInfoLibrarian() => View();
        [HttpGet]
        public ActionResult UpdateInfoAdmin() => View();
        //[HttpGet]
        //public ActionResult UpdateInfo()
        //{
        //    var userId = Session["MANGUOIDUNG"];
        //    var vaitro = Session["VAITRO"];
        //    UpdateViewModel model = new UpdateViewModel();
        //    if (userId == null)
        //    {
        //        return View("Login", "Account");
        //    }
        //    if(vaitro.ToString().ToLower() != "reader")
        //    {
        //        var yours = Database.NHANVIENs.FirstOrDefault(t => t.MANV == (string)userId);
        //        if(yours == null)
        //        {
        //            return RedirectToAction("InfoReader");
        //        }
        //        model = new UpdateViewModel
        //        {
        //            GIOITINH = string.IsNullOrEmpty(yours.GIOITINH) ? "" : yours.GIOITINH,
        //            EMAIL = string.IsNullOrEmpty(yours.EMAIL) ? "" : yours.EMAIL,
        //            NGSINH =(DateTime)yours.NGSINH,
        //            SDT = string.IsNullOrEmpty(yours.SODIENTHOAI) ? "": yours.SODIENTHOAI,
        //            DIACHI = string.IsNullOrEmpty(yours.DIACHI) ? "" : yours.DIACHI,
        //        };
        //        if(vaitro.ToString().ToLower() == "admin")
        //        {
        //            return View("UpdateInfoAdmin", model);
        //        }
        //        else
        //        {
        //            return View("UpdateInfoLibrarian", model);
        //        }
        //    }
        //    else
        //    {
        //        var yours = Database.DOCGIAs.FirstOrDefault(t => t.MATHANHVIEN == (string)userId);
        //        if (yours == null)
        //        {
        //            return RedirectToAction("InfoReader");
        //        }
        //        model = new UpdateViewModel
        //        {
        //            GIOITINH = string.IsNullOrEmpty(yours.GIOITINH) ? "" : yours.GIOITINH,
        //            EMAIL = string.IsNullOrEmpty(yours.EMAIL) ? "" : yours.EMAIL,
        //            NGSINH = (DateTime)yours.NGSINH,
        //            SDT = string.IsNullOrEmpty(yours.SODIENTHOAI) ? "" : yours.SODIENTHOAI,
        //            KHOAHOC = string.IsNullOrEmpty(yours.KHOAHOC) ? "" : yours.KHOAHOC,
        //            DIACHI = string.IsNullOrEmpty(yours.DIACHI) ? "" : yours.DIACHI,
        //        };
        //    }
        //    return View("UpdateInfoReader", model);
        //}
        [HttpPost]
        public ActionResult UpdateInfoAdmin(NHANVIEN model)
        {
            var userId = Session["MANGUOIDUNG"] as string;
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Account");

            var nhanvien = Database.NHANVIENs.FirstOrDefault(t => t.MANV == userId);
            if (nhanvien == null)
            {
                var vaitro = Session["VAITRO"]?.ToString().ToLower();
                if (vaitro == "admin") return RedirectToAction("InfoAdmin");
                else return RedirectToAction("InfoLibrarian");
            }

            if (!string.IsNullOrEmpty(model.TENNV)) nhanvien.TENNV = model.TENNV;
            if (!string.IsNullOrEmpty(model.EMAIL)) nhanvien.EMAIL = model.EMAIL;
            if (!string.IsNullOrEmpty(model.DIACHI)) nhanvien.DIACHI = model.DIACHI;
            if (!string.IsNullOrEmpty(model.SODIENTHOAI)) nhanvien.SODIENTHOAI = model.SODIENTHOAI;
            if (!string.IsNullOrEmpty(model.GIOITINH)) nhanvien.GIOITINH = model.GIOITINH;

            if (model.NGSINH != null && model.NGSINH >= new DateTime(1753, 1, 1))
                nhanvien.NGSINH = model.NGSINH;
            else
                nhanvien.NGSINH = null;

            try
            {
                Database.Entry(nhanvien).State = System.Data.Entity.EntityState.Modified;
                Database.SaveChanges();
                TempData["Notice"] = "Cập nhật thành công!";
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.InnerException?.Message ?? ex.Message;
                TempData["Error"] = "Lỗi khi cập nhật: " + message;
            }

            var role = Session["VAITRO"]?.ToString().ToLower();
            if (role == "admin")
                return RedirectToAction("InfoAdmin");
            else
                return RedirectToAction("InfoLibrarian");
        }
        [HttpPost]
        public ActionResult UpdateInfo(UpdateViewModel model)
        {
            var userId = Session["MANGUOIDUNG"];
            if (userId == null) return RedirectToAction("Login", "Account");
            var vaitro = Session["VAITRO"];

         
            {
                var docGia = Database.DOCGIAs.FirstOrDefault(d => d.MATHANHVIEN == (string)userId);
                if (docGia == null) return RedirectToAction("InfoReader");
                if (!string.IsNullOrEmpty(model.NGHENGHIEP)) docGia.NGHENGHIEP = model.NGHENGHIEP;
                if (!string.IsNullOrEmpty(model.EMAIL)) docGia.EMAIL = model.EMAIL;
                if (!string.IsNullOrEmpty(model.DIACHI)) docGia.DIACHI = model.DIACHI;
                if (!string.IsNullOrEmpty(model.GIOITINH)) docGia.GIOITINH = model.GIOITINH;

                if (model.NGSINH != null && model.NGSINH >= new DateTime(1753, 1, 1))
                    docGia.NGSINH = model.NGSINH;
                else
                    docGia.NGSINH = null;
            }

            try
            {
                Database.SaveChanges();
                TempData["Notice"] = "Cập nhật thành công!";
                return RedirectToAction("InfoReader");
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.InnerException?.Message ?? ex.Message;
                TempData["Error"] = "Lỗi khi cập nhật: " + message;

                    return View("UpdateInfoReader", model);
            }
        }


    }
}
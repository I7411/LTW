using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library_Management.Models;

namespace Library_Management.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        QUANLYTHUVIENEntities1 Database = new QUANLYTHUVIENEntities1();
        //Tạo session đăng nhập
        [HttpPost]
        public ActionResult Login(string userName, string passWord, string position)
        {
            string roleInput = position.ToLower();

            if (roleInput != "reader")
            {
                var user = Database.NHANVIENs.FirstOrDefault(d =>
                    d.TAIKHOAN == userName && d.MATKHAU == passWord);

                if (user != null)
                {
                    string actualRole = user.VAITRO.ToLower();
                    Session["MANGUOIDUNG"] = user.MANV;
                    Session["VAITRO"] = actualRole;

                    if (roleInput == "admin" && actualRole == "admin")
                    {
                        return RedirectToAction("AdminHome", "Home");
                    }
                    else if (roleInput == "librarian" && actualRole == "nhân viên")
                    {
                        return RedirectToAction("LibrarianHome", "Home");
                    }
                }
            }
            else 
            {
                var reader = Database.DOCGIAs.FirstOrDefault(d =>
                    d.TAIKHOAN == userName && d.MATKHAU == passWord);

                if (reader != null)
                {
                    Session["MANGUOIDUNG"] = reader.MATHANHVIEN;
                    Session["VAITRO"] = "Reader";
                    return RedirectToAction("ReaderHome", "Home");
                }
            }

            TempData["Notice"] = "Sai tài khoản hoặc mật khẩu";
            return RedirectToAction("Login");
        }
        [HttpGet]
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SignUpAccount() => View();
        //Tạo mã ngẫu nhiên
         private Random rand = new Random();
        private string MaThe()
        {
            string soNgauNhien = rand.Next(0, 999999).ToString("D6");
            string tienTo = "1234";
            return tienTo + soNgauNhien;
        }
        private bool KiemTraMaTheTrung(string maThe)
        {
            return Database.DOCGIAs.Any(d => d.MATHANHVIEN == maThe);
        }
        public string TaoMaTheKhongTrung()
        {
            string maThe;
            do
            {
                maThe = MaThe();
            } while (KiemTraMaTheTrung(maThe));

            return maThe;
        }
        //Kiểm tra trùng mã thành viên
         private bool KiemTraMaSoTrung(string maSo)
        {
            return Database.DOCGIAs.Any(d => d.MATHANHVIEN == maSo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUpAccount(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                if (KiemTraMaSoTrung(model.MaSo))
                {
                    TempData["Notice"] = "Mã số đã đăng ký";
                    return RedirectToAction("SignUpAccount");
                }
                var info = new DOCGIA
                {
                    
                    MATHANHVIEN = model.MaSo,
                    TENTV = model.HoTen,
                    NGSINH = model.NgaySinh.Value,
                    GIOITINH = model.GioiTinh,
                    DIACHI = model.DiaChi,
                    VAITRO = model.ChucVu,
                    NGHENGHIEP = model.ChucVu,
                    TENTRUONG = model.TenTruong,
                    KHOAHOC = model.KhoaHoc,
                    EMAIL = model.Email,
                    SODIENTHOAI = model.DienThoai,
                    GHICHU = model.GhiChu,
                    MATKHAU = model.MatKhau,
                    TAIKHOAN = model.MaSo,
                };
                var infocard = new THEBANDOC
                {
                    MATHANHVIEN = model.MaSo,
                    MASOTHE = TaoMaTheKhongTrung(),
                    TINHTRANGTHE = "Chưa có",
                    HANSUDUNG = DateTime.Today.AddYears(4).ToString("yyyy-MM-dd"),
                };
                try
                {
                    Database.DOCGIAs.Add(info);
                    Database.THEBANDOCs.Add(infocard);
                    Database.SaveChanges();
                    TempData["Notice"] = "Đăng ký thành công!";
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi lưu dữ liệu: " + ex.Message);
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult ForgotPassword() => View();
        private bool checkAccount(string taikhoan)
        {
            return Database.DOCGIAs.Any(t => t.MATHANHVIEN == taikhoan);
        }
       public ActionResult ForgotPassword(string taikhoan, string nhapmk, string nhaplaimk)
        {
            taikhoan = taikhoan?.Trim();
            if (!checkAccount(taikhoan))
            {
                TempData["Notice"] = "Tài khoản không tồn tại";
                return RedirectToAction("ForgotPassword");
            }
          var docGia = Database.DOCGIAs.FirstOrDefault(d => d.MATHANHVIEN.Trim() == taikhoan);
          if(docGia != null)
            {
                docGia.MATKHAU = nhapmk;
                Database.SaveChanges();
                TempData["Notice"] = "Đổi mật khẩu thành công";
            }
            else
            {
                TempData["Notice"] = "Không tìm thấy người dùng để cập nhật";
            }
            return RedirectToAction("ForgotPassword");

        }
        public ActionResult ChangePassWord() => View();
        private bool checkPass(string maNguoiDung, string mkcu)
        {
            return Database.NHANVIENs.Any(t => t.MANV == maNguoiDung && t.MATKHAU.Trim() == mkcu);
        }
        private bool checkPassReader(string maNguoiDung, string mkcu)
        {
            return Database.DOCGIAs.Any(t => t.MATHANHVIEN == maNguoiDung && t.MATKHAU.Trim() == mkcu);
        }
        public ActionResult ChangePass(string mkcu, string nhapmk)
        {
            var maNguoiDung = Session["MANGUOIDUNG"]?.ToString();
            var vaiTro = Session["VAITRO"]?.ToString();

            if (string.IsNullOrEmpty(maNguoiDung) || string.IsNullOrEmpty(vaiTro))
            {
                TempData["Notice"] = "Phiên đăng nhập không hợp lệ.";
                return RedirectToAction("Login", "Account");
            }

            if (vaiTro != "Reader")
            {
                var nhanvien = Database.NHANVIENs.FirstOrDefault(t => t.MANV == maNguoiDung);
                if (nhanvien != null && checkPass(maNguoiDung, mkcu))
                {
                    nhanvien.MATKHAU = nhapmk;
                    Database.SaveChanges();
                    TempData["Notice"] = "Đổi mật khẩu thành công.<br/> Vui lòng đăng nhập lại.";
                    Session.Clear();
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    TempData["Notice"] = "Mật khẩu cũ không đúng";
                }
            }
            else 
            {
                var docgia = Database.DOCGIAs.FirstOrDefault(t => t.MATHANHVIEN == maNguoiDung);
                if (docgia != null && checkPassReader(maNguoiDung, mkcu))
                {
                    docgia.MATKHAU = nhapmk;
                    Database.SaveChanges();
                    TempData["Notice"] = "Đổi mật khẩu thành công.<br/> Vui lòng đăng nhập lại.";
                    Session.Clear();
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    TempData["Notice"] = "Mật khẩu cũ không đúng";
                }
            }

            return RedirectToAction("ChangePassWord");
        }
    }
}
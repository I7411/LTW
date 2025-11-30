using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library_Management.Models;
using Library_Management.Controllers;
using System.Data.Entity.Validation;
namespace Library_Management.Controllers
{
    public class ManagementController : Controller
    {
        // GET: Management
        QUANLYTHUVIENEntities1 Database = new QUANLYTHUVIENEntities1();
        [HttpGet]
        public ActionResult ReaderManagement()
        {
            return View();
        }
        //xóa
        [HttpPost]
        public ActionResult DeleteReader(string id)
        {
            using (var Database = new Library_Management.Models.QUANLYTHUVIENEntities())
            {
                try
                {
                    // Kiểm tra id hợp lệ
                    if (string.IsNullOrEmpty(id))
                    {
                        TempData["Error"] = "Mã độc giả không hợp lệ.";
                        return RedirectToAction("ReaderManagement");
                    }

                    // Tìm thông tin độc giả
                    var reader = Database.DOCGIAs.Find(id);
                    if (reader == null)
                    {
                        TempData["Error"] = "Không tìm thấy thông tin độc giả.";
                        return RedirectToAction("ReaderManagement");
                    }

                    // Tìm thẻ bạn đọc liên quan
                    var the = Database.THEBANDOCs.FirstOrDefault(t => t.MATHANHVIEN == id);
                    if (the != null)
                    {
                        // Lấy tất cả phiếu mượn liên quan đến thẻ
                        var phieuMuons = Database.PHIEUMUONs
                            .Where(p => p.MASOTHE == the.MASOTHE)
                            .ToList();

                        foreach (var phieuMuon in phieuMuons)
                        {
                            // Xóa chi tiết phiếu mượn
                            var chiTietMuons = Database.CHITIETPHIEUMUONs
                                .Where(c => c.MAPHIEUMUON == phieuMuon.MAPHIEUMUON)
                                .ToList();
                            Database.CHITIETPHIEUMUONs.RemoveRange(chiTietMuons);

                            // Xóa phiếu phạt liên quan
                            var phieuPhats = Database.PHIEUPHATs
                                .Where(pp => pp.MAPHIEUMUON == phieuMuon.MAPHIEUMUON)
                                .ToList();
                            Database.PHIEUPHATs.RemoveRange(phieuPhats);

                            // Xóa phiếu mượn
                            Database.PHIEUMUONs.Remove(phieuMuon);
                        }

                        // Xóa thẻ bạn đọc
                        Database.THEBANDOCs.Remove(the);
                    }

                    // Xóa thông tin độc giả
                    Database.DOCGIAs.Remove(reader);

                    // Lưu thay đổi vào cơ sở dữ liệu
                    Database.SaveChanges();

                    TempData["Notice"] = "Xóa thông tin độc giả thành công!";
                    return RedirectToAction("ReaderManagement");
                }
                catch (Exception ex)
                {
                    var message = ex.InnerException?.Message ?? ex.Message;
                    TempData["Error"] = "Lỗi khi xóa: " + message;
                    return RedirectToAction("ReaderManagement");
                }
            }
        }/// <summary>
         /// Thêm độc giả
         /// </summary>
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
        private string TaoMaTheKhongTrung()
        {
            string maThe;
            do
            {
                maThe = MaThe();
            } while (KiemTraMaTheTrung(maThe));

            return maThe;
        }
        private bool KiemTraMaSoTrung(string maSo)
        {
            return Database.DOCGIAs.Any(d => d.MATHANHVIEN == maSo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMember(reader model)
        {
            if (ModelState.IsValid)
            {
                if (KiemTraMaSoTrung(model.MaSo))
                {
                    TempData["Notice"] = "Mã độc giả đã tồn tại.";
                    return RedirectToAction("ReaderManagement");
                }
                var docGia = new DOCGIA
                {
                    MATHANHVIEN = model.MaSo,
                    TENTV = model.HoTen,
                    NGSINH = model.NgaySinh.Value,
                    GIOITINH = model.GioiTinh,
                    DIACHI = model.DiaChi,
                    VAITRO = "Độc giả",
                    NGHENGHIEP = model.ChucVu,
                    GHICHU = model.GhiChu,
                    SODIENTHOAI = model.DienThoai,
                    EMAIL = model.Email,
                    MATKHAU = model.MatKhau,
                    TAIKHOAN = model.MaSo
                };

                var theBanDoc = new THEBANDOC
                {
                    MATHANHVIEN = model.MaSo,
                    MASOTHE = MaThe(),
                    TINHTRANGTHE = "Chưa cấp",
                    HANSUDUNG = DateTime.Now.AddYears(4).ToString("yyyy-MM-dd")
                };

                try
                {
                    Database.DOCGIAs.Add(docGia);
                    Database.THEBANDOCs.Add(theBanDoc);
                    Database.SaveChanges();

                    TempData["Notice"] = "Thêm độc giả thành công!";
                    return RedirectToAction("ReaderManagement");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi lưu dữ liệu: " + ex.ToString());
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(UpdatReader model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var reader = Database.DOCGIAs.Find(model.MaSo);

            if (reader == null)
            {
                return HttpNotFound();
            }

            reader.TENTV = model.HoTen;
            reader.NGSINH = DateTime.Parse(model.NgaySinh.ToString());
            reader.GIOITINH = model.GioiTinh;
            reader.NGHENGHIEP = model.ChucVu;
            reader.SODIENTHOAI = model.DienThoai;
            reader.EMAIL = model.Email;
            reader.DIACHI = model.DiaChi;
            reader.VAITRO = model.ChucVu;
            reader.GHICHU = model.GhiChu;

            Database.SaveChanges();

            return RedirectToAction("ReaderManagement");
        }

        [HttpGet]
        public JsonResult GetReaderInfo(string id)
        {
            var reader = Database.DOCGIAs.FirstOrDefault(r => r.MATHANHVIEN == id);
            if (reader == null)
                return Json(null, JsonRequestBehavior.AllowGet);

            return Json(new
            {
                MaSo = reader.MATHANHVIEN,
                HoTen = reader.TENTV,
                NgaySinh = reader.NGSINH?.ToString("yyyy-MM-dd"),
                GioiTinh = reader.GIOITINH,
                ChucVu = reader.NGHENGHIEP,
                DienThoai = reader.SODIENTHOAI,
                Email = reader.EMAIL,
                DiaChi = reader.DIACHI,
                GhiChu = reader.GHICHU,
                MatKhau = reader.MATKHAU

            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///Quản lý tài liệu
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DocumentManagement() => View();
        [HttpPost]
        public JsonResult ThemTaiLieu(TAILIEU tl)
        {
            var exists = Database.TAILIEUx.Any(x => x.MATAILIEU == tl.MATAILIEU);
            if (exists)
            {
                return Json(new { success = false, message = "Mã tài liệu đã tồn tại!" });
            }
            Database.TAILIEUx.Add(tl);
            Database.SaveChanges();
            return Json(new { success = true });
        }
        [HttpPost]
        public JsonResult XoaTaiLieu(string id)
        {
            var tl = Database.TAILIEUx.FirstOrDefault(t => t.MATAILIEU == id);
            if (tl != null)
            {
                Database.TAILIEUx.Remove(tl);
                Database.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public JsonResult CapNhatTaiLieu(TAILIEU updated)
        {
            var tl = Database.TAILIEUx.FirstOrDefault(t => t.MATAILIEU == updated.MATAILIEU);
            if (tl != null)
            {
                tl.TENSACH = updated.TENSACH;
                tl.NGONNGU = updated.NGONNGU;
                tl.PHIMUON = updated.PHIMUON;
                tl.TENTACGIA = updated.TENTACGIA;
                tl.THELOAI = updated.THELOAI;
                tl.TINHTRANG = updated.TINHTRANG;
                tl.SOLUONG = updated.SOLUONG;
                Database.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        //Quản lý thẻ
        public ActionResult CardManagement() => View();

        [HttpPost]
        public JsonResult DeleteCard(string MaThe)
        {
            try
            {
                var card = Database.THEBANDOCs.FirstOrDefault(c => c.MASOTHE == MaThe);
                if (card != null)
                {
                    Database.THEBANDOCs.Remove(card);
                    Database.SaveChanges();
                    return Json(new { success = true });
                }
                return Json(new { success = false, message = "Không tìm thấy thẻ" });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public JsonResult UpdateCard(string MaThe, string HoTen, string DiaChi, string VaiTro, string TinhTrang)
        {
            try
            {
                var card = Database.THEBANDOCs.FirstOrDefault(c => c.MASOTHE == MaThe);
                if (card != null)
                {
                    var docgia = Database.DOCGIAs.FirstOrDefault(d => d.MATHANHVIEN == card.MATHANHVIEN);
                    if (docgia != null)
                    {
                        docgia.TENTV = HoTen;
                        docgia.DIACHI = DiaChi;
                        docgia.VAITRO = VaiTro;
                    }

                    card.TINHTRANGTHE = TinhTrang;

                    Database.SaveChanges();
                    return Json(new { success = true });
                }
                return Json(new { success = false });
            }
            catch
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        public JsonResult ExtendCard(string MaThe, int ExtendYears)
        {
            try
            {
                var card = Database.THEBANDOCs.FirstOrDefault(c => c.MASOTHE == MaThe);
                if (card != null)
                {
                    if (DateTime.TryParse(card.HANSUDUNG, out DateTime oldDate))
                    {
                        DateTime newDate = oldDate.AddYears(ExtendYears);
                        card.HANSUDUNG = newDate.ToString("yyyy-MM-dd"); // hoặc "dd/MM/yyyy" theo format bạn cần
                        Database.SaveChanges();

                        return Json(new { success = true, newExpiry = card.HANSUDUNG });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Ngày hết hạn không hợp lệ." });
                    }
                }

                return Json(new { success = false, message = "Không tìm thấy thẻ." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Lỗi hệ thống: " + ex.Message });
            }
        }

        //Quản lý phòng học
        [HttpGet]
        public ActionResult DocumentsAndClassRooms() => View();
        [HttpGet]
        public JsonResult GetClassroomRequestInfo(string maPhong, string maDocGia)
        {
            using (var db = new QUANLYTHUVIENEntities())
            {
                var rawData = (from p in db.PHONGHOCs
                               join d in db.DATPHONGs on p.MAPHONG equals d.MAPHONG
                               join t in db.THEBANDOCs on d.MASOTHE equals t.MASOTHE
                               join doc in db.DOCGIAs on t.MATHANHVIEN equals doc.MATHANHVIEN
                               where p.MAPHONG == maPhong && t.MATHANHVIEN == maDocGia
                               select new
                               {
                                   MaPhieu = d.MADATPHONG,
                                   YeuCau = d.YEUCAU,
                                   MaDocGia = t.MATHANHVIEN,
                                   TenDocGia = doc.TENTV,
                                   VaiTro = doc.VAITRO,
                                   Khoa = doc.KHOAHOC,
                                   Lop = "",
                                   SoLanGiaHan = 1,
                                   ThoiGianMuon = d.THOIGIANBATDAU,
                                   ThoiGianTra = d.THOIGIANKETTHUC,
                                   MaPhong = p.MAPHONG,
                                   TenPhong = p.TENPHONG,
                                   ViTri = p.TENPHONG,
                                   TrangThai = d.TRANGTHAI
                               }).FirstOrDefault();

                if (rawData == null)
                    return Json(null, JsonRequestBehavior.AllowGet);

                var result = new
                {
                    rawData.MaPhieu,
                    rawData.YeuCau,
                    rawData.MaDocGia,
                    rawData.TenDocGia,
                    rawData.VaiTro,
                    rawData.Khoa,
                    rawData.Lop,
                    SoLanGiaHan = rawData.SoLanGiaHan.ToString(),
                    ThoiGianMuon = rawData.ThoiGianMuon.ToString("yyyy-MM-dd HH:mm"),
                    ThoiGianTra = rawData.ThoiGianTra.ToString("yyyy-MM-dd HH:mm"),
                    rawData.MaPhong,
                    rawData.TenPhong,
                    rawData.ViTri,
                    rawData.TrangThai
                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Accept(string maphong)
        {
            if (string.IsNullOrEmpty(maphong))
            {
                return HttpNotFound();
            }

            // Lấy bản ghi đặt phòng theo mã phòng
            var giahan = Database.DATPHONGs.FirstOrDefault(t => t.MAPHONG == maphong);

            if (giahan == null)
            {
                return HttpNotFound();
            }

            if (giahan.TRANGTHAI == "Chờ duyệt")
            {
                giahan.TRANGTHAI = "Được gia hạn";
                Database.SaveChanges();
                TempData["Notice"] = "Gia hạn thành công.";
            }
            else
            {
                TempData["Notice"] = "Yêu cầu đang không ở trạng thái chờ duyệt, không thể gia hạn.";
            }

            return RedirectToAction("DocumentsAndClassRooms");
        }
        //Vi phạm
        [HttpGet]
        public ActionResult HandleMistakes() => View();
        [HttpGet]
        public JsonResult GetInfo(string ma)
        {
            using (var db = new QUANLYTHUVIENEntities())
            {
                var rawData = (from ph in db.PHIEUPHATs
                               join m in db.PHIEUMUONs on ph.MAPHIEUMUON equals m.MAPHIEUMUON
                               join the in db.THEBANDOCs on m.MASOTHE equals the.MASOTHE
                               join doc in db.DOCGIAs on the.MATHANHVIEN equals doc.MATHANHVIEN
                               where the.MASOTHE == ma
                               select new
                               {
                                   maDocGia = the.MASOTHE,
                                   tenDocGia = doc.TENTV,
                                   liDo = ph.LYDO,
                                   soLan = ph.SOLAN,
                                   tg = ph.NGTAO,
                                   vaitro = doc.VAITRO
                               }).FirstOrDefault();

                if (rawData == null)
                    return Json(null, JsonRequestBehavior.AllowGet);

                var result = new
                {
                    rawData.maDocGia,
                    rawData.tenDocGia,
                    rawData.vaitro,
                    rawData.liDo,
                    rawData.soLan,
                };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        private string maPhat()
        {
            string tienTo = "66666";
            Random rand = new Random();
            string soNgauNhien = rand.Next(1, 99999).ToString("D5");
            return tienTo + soNgauNhien;
        }
        private bool KiemTraMa(string maThe)
        {
            return Database.PHIEUPHATs.Any(d => d.MAPHIEUPHAT == maThe);
        }
        public string TaoMaKhongTrung()
        {
            string maThe;
            do
            {
                maThe = maPhat();
            } while (KiemTraMa(maThe));

            return maThe;
        }
        bool ktrTrung(string ma)
        {
            return Database.CHITIETDATTRUOCs.Any(t => t.MATAILIEU == ma);
        }
        [HttpPost]
        public JsonResult Add(string MaThe, string LiDo, string tinhtrang)
        {
            try
            {
               var nv = Session["MANGUOIDUNG"].ToString();
                using (var db = new Library_Management.Models.QUANLYTHUVIENEntities())
                {
                    var user = (
                        from t in db.THEBANDOCs
                        join p in db.PHIEUMUONs on t.MASOTHE equals p.MASOTHE
                        join ct in db.CHITIETPHIEUMUONs on p.MAPHIEUMUON equals ct.MAPHIEUMUON
                        where t.MASOTHE == MaThe
                        select new
                        {
                            maThe = t.MASOTHE,
                            maMuon = p.MAPHIEUMUON,
                            phi = ct.PHIMUON,
                        }
                    ).FirstOrDefault();

                    if (user == null)
                    {
                        return Json(new { success = false, message = "Không tìm thấy thẻ độc giả" });
                    }

                    if (string.IsNullOrEmpty(user.maMuon))
                    {
                        return Json(new { success = false, message = "Mã phiếu mượn không hợp lệ" });
                    }
                    if (ktrTrung(user.maMuon))
                    {
                        var tl = Database.PHIEUPHATs.FirstOrDefault(t => t.MAPHIEUMUON == user.maMuon);
                        tl.SOLAN++;
                        Database.SaveChanges();
                        return Json(new { success = true });

                    }
                    var newPhat = new PHIEUPHAT
                    {
                        MAPHIEUPHAT = TaoMaKhongTrung(),
                        MAPHIEUMUON = user.maMuon,
                        PHIPHAT = user.phi,
                        TINHTRANGTAILIEU = tinhtrang,
                        LYDO = LiDo,
                        SOLAN = 1,
                        NGTAO = DateTime.Now,
                        MANV = nv

                    };

                    db.PHIEUPHATs.Add(newPhat);
                    db.SaveChanges();

                    return Json(new { success = true });
                }
            }
            catch (DbEntityValidationException ex)
            {
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => $"{x.PropertyName}: {x.ErrorMessage}");

                var fullErrorMessage = string.Join("; ", errorMessages);
                return Json(new { success = false, message = fullErrorMessage });
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException?.InnerException?.Message ?? ex.Message;
                return Json(new { success = false, message = errorMessage });
            }
        }
        [HttpPost]
        public JsonResult Delete(string MaThe, string LiDo)
        {
            try
            {
                using (var db = new Library_Management.Models.QUANLYTHUVIENEntities())
                {
                    // Tìm vi phạm cần xóa theo mã thẻ và lý do
                    var phat = (from ph in db.PHIEUPHATs
                                join m in db.PHIEUMUONs on ph.MAPHIEUMUON equals m.MAPHIEUMUON
                                where m.MASOTHE == MaThe && ph.LYDO == LiDo
                                select ph).FirstOrDefault();

                    if (phat == null)
                    {
                        return Json(new { success = false, message = "Không tìm thấy vi phạm để xóa" });
                    }

                    db.PHIEUPHATs.Remove(phat);
                    db.SaveChanges();

                    return Json(new { success = true });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}



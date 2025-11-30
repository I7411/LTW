using Library_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Library_Management.Controllers
{
    public class DocumentController : Controller
    {
        // GET: Document
        QUANLYTHUVIENEntities1 Database = new QUANLYTHUVIENEntities1();
        //Tạo mã ngẫu nhiên
        private string MaMuon()
        {
            string tienTo = "3333333333";
            Random rand = new Random();
            string soNgauNhien = rand.Next(1, 99999).ToString("D5");
            return tienTo + soNgauNhien;
        }
        public ActionResult BorrowDocuments()
        {
            var model = new BorrowDocumentsViewModel
            {
                TaiLieuList = new List<TAILIEU>(),
                DatMuonList = Database.DATMUONTRUOCs.ToList()
            };
            return View(model);
        }
        //Lấy dữ liệu tài liệu
        [HttpGet]
        public ActionResult Search(string keyword)
        {
            var taiLieuList = new List<TAILIEU>();
            if (!string.IsNullOrEmpty(keyword))
            {
                taiLieuList = Database.TAILIEUx
                    .Where(t => t.TENSACH.Contains(keyword)
                             || t.MATAILIEU.Contains(keyword)
                             || t.NXB.Contains(keyword))
                    .ToList();
            }

            var viewModel = new BorrowDocumentsViewModel
            {
                TaiLieuList = taiLieuList,
                DatMuonList = Database.DATMUONTRUOCs.ToList()
            };

            return View("BorrowDocuments", viewModel);
        }
       bool ktrTrung(string ma)
        {
            return Database.CHITIETDATTRUOCs.Any(t => t.MATAILIEU == ma);
        }
        //Đăng ký mượn tài liệu 
        [HttpPost]
        public ActionResult Borrow(string MaTL, int SL)
        {
            var user = Session["MANGUOIDUNG"];
            var theID = Database.THEBANDOCs.FirstOrDefault(t => t.MATHANHVIEN == (string)user);
            var tailieu = Database.TAILIEUx.FirstOrDefault(t => t.MATAILIEU == MaTL);
            if (SL <= 0 || MaTL == null)
            {
                TempData["Error"] = "Vui lòng chọn nhập thông tin phù hợp";
                return RedirectToAction("BorrowDocuments");
            }
            if (tailieu == null)
            {
                TempData["Error"] = "Không tồn tại tài liệu cần mượn";
                return RedirectToAction("BorrowDocuments");
            }
            if (tailieu.SOLUONG < SL)
            {
                TempData["Error"] = "Tài liệu cần mượn vượt quá tài liệu hiện có";
                return RedirectToAction("BorrowDocuments");
            }
            if (ktrTrung(MaTL))
            {
                var tl = Database.CHITIETDATTRUOCs.FirstOrDefault(t => t.MATAILIEU == MaTL);
                tl.SOLUONG += SL;
                Database.SaveChanges();
                return RedirectToAction("BorrowDocuments");
            }
            string maMuon = MaMuon();
            while (Database.DATMUONTRUOCs.Any(x => x.MAMUON == maMuon))
            {
                maMuon = MaMuon();
            }
            DATMUONTRUOC DMT = new DATMUONTRUOC
            {
                MAMUON = maMuon,
                NGAYDAT = DateTime.Today,
                HANLAYSACH = DateTime.Today.AddDays(7),
                MASOTHE = theID.MASOTHE,
                TRANGTHAI = "Chờ nhận"
            };
            CHITIETDATTRUOC CT = new CHITIETDATTRUOC
            {
                MATAILIEU = MaTL,
                MAMUON = maMuon,
                PHIMUON = tailieu.PHIMUON,
                SOLUONG = SL
            };
            Database.DATMUONTRUOCs.Add(DMT);
            Database.CHITIETDATTRUOCs.Add(CT);
            tailieu.SOLUONG -= SL;
            try
            {
                Database.SaveChanges();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Lỗi khi cập nhật dữ liệu: " + ex.Message;
                return RedirectToAction("BorrowDocuments");
            }
            TempData["Success"] = "Đặt mượn tài liệu thành công";
            return RedirectToAction("BorrowDocuments");
        }
        //Lưu vào danh sách đặt trước
        [HttpGet]
        public ActionResult DanhSachDatTruoc()
        {
            var list = (from ct in Database.CHITIETDATTRUOCs
                        join t in Database.TAILIEUx on ct.MATAILIEU equals t.MATAILIEU
                        join m in Database.DATMUONTRUOCs on ct.MAMUON equals m.MAMUON
                        select new ChiTietDatTruocViewModel
                        {
                            MaTaiLieu = ct.MATAILIEU,
                            TenSach = t.TENSACH,
                            NXB = t.NXB,
                            TRANGTHAI = m.TRANGTHAI,
                            SOLUONG = (int)ct.SOLUONG
                        }).ToList();

            var viewModel = new BorrowDocumentsViewModel
            {
                ChiTietDatTruocList = list
            };

            return View("BorrowDocuments", viewModel);
        }
        [HttpPost]
        public JsonResult HuyDatTruoc(string maTaiLieu, string maMuon)
        {
            try
            {
                var chiTiet = Database.CHITIETDATTRUOCs
                    .FirstOrDefault(ct => ct.MATAILIEU == maTaiLieu && ct.MAMUON == maMuon);

                if (chiTiet != null)
                {
                    var taiLieu = Database.TAILIEUx.FirstOrDefault(t => t.MATAILIEU == maTaiLieu);
                    if (taiLieu != null)
                    {
                        taiLieu.SOLUONG += chiTiet.SOLUONG;
                    }

                    Database.CHITIETDATTRUOCs.Remove(chiTiet);
                    Database.SaveChanges();

                    return Json(new { success = true });
                }

                return Json(new { success = false, message = "Không tìm thấy bản ghi để xóa." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
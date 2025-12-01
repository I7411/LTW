using QLTHUVIEN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity; // cần để dùng Include


namespace QLTHUVIEN.Controllers
{
    public class BorrowHistoryController : Controller
    {
        QUANLYTHUVIENEntities db = new QUANLYTHUVIENEntities();

        // Lịch sử mượn theo Mã thẻ
        public ActionResult Index(string maThe)
        {
            var data = db.PHIEUMUON
            .Include(pm => pm.CHITIETPHIEUMUON)  // load luôn navigation
            .Where(x => x.MASOTHE == maThe)
            .ToList() // dữ liệu đã về memory
            .Select(pm => new BorrowHistoryVM
            {
                MaPhieu = pm.MAPHIEUMUON,
                NgayMuon = pm.NGAYMUON,
                NgayTra = pm.NGAYTRA,
                ChiTiet = pm.CHITIETPHIEUMUON
                    .Select(ct => new CHITIETPHIEUMUON
                    {
                        MAPHIEUMUON = ct.MAPHIEUMUON,
                        MATAILIEU = ct.MATAILIEU,
                        SOLUONG = ct.SOLUONG
                    }).ToList()
            }).ToList();


            return View(data);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library_Management.Models;

namespace Library_Management.Controllers
{
    public class SearchController : Controller
    {
        QUANLYTHUVIENEntities Database = new QUANLYTHUVIENEntities();
        // GET: Search
        public ActionResult SearchReader()
        {
            return View();
        }
        [HttpGet]
        public ActionResult SearchReader(string keyword)
        {
            var docgia = (
                from d in Database.DOCGIAs
                join t in Database.THEBANDOCs on d.MATHANHVIEN equals t.MATHANHVIEN
                where string.IsNullOrEmpty(keyword) ||
                      d.MATHANHVIEN.Contains(keyword) ||
                      d.TENTV.Contains(keyword)
                select new Info
                {
                    MaThe = t.MASOTHE,
                    MaTV = d.MATHANHVIEN,
                    HoTen = d.TENTV,
                    NgSinh = d.NGSINH,
                    TenTruong = d.TENTRUONG,
                    DiaChi = d.DIACHI,
                    Sdt = d.SODIENTHOAI
                }
            ).ToList();

            return View("SearchReader", docgia);
        }
        public ActionResult SearchDocument()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Search(string keyword)
        {
            var result = (
                from t in Database.TAILIEUx
                where string.IsNullOrEmpty(keyword)
                   || t.MATAILIEU.Contains(keyword)
                   || t.TENSACH.Contains(keyword)
                   || t.TENTACGIA.Contains(keyword)
                select new InfoDocument
                {
                    MATAILIEU = t.MATAILIEU,
                    TENSACH = t.TENSACH,
                    NGONNGU = t.NGONNGU,
                    CHIPHI = (decimal)t.PHIMUON,
                    TACGIA = t.TENTACGIA,
                    THELOAI = t.THELOAI,
                    TINHTRANG = t.TINHTRANG
                }
            ).ToList();

            var viewModel = new BorrowDocumentsViewModel
            {
                TaiLieuList2 = result
            };

            return View("SearchDocuments", viewModel);
        }
    }
}
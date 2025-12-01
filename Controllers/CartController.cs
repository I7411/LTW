using QLTHUVIEN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLTHUVIEN.Controllers
{
    public class CartController : Controller
    {
        QUANLYTHUVIENEntities db = new QUANLYTHUVIENEntities();

        public ActionResult Add(string id)
        {
            var taiLieu = db.TAILIEU.Find(id);

            var cart = Session["cart"] as List<CartItem> ?? new List<CartItem>();

            var item = cart.FirstOrDefault(x => x.MaTaiLieu == id);

            if (item == null)
            {
                cart.Add(new CartItem
                {
                    MaTaiLieu = id,
                    TenSach = taiLieu.TENSACH,
                    PhiMuon = (float)taiLieu.PHIMUON,
                    SoLuong = 1
                });
            }
            else item.SoLuong++;

            Session["cart"] = cart;

            return RedirectToAction("Index");
        }

        public ActionResult Index()
        {
            var cart = Session["cart"] as List<CartItem> ?? new List<CartItem>();
            return View(cart);
        }

        public ActionResult DatHang(string maThe)
        {
            var cart = Session["cart"] as List<CartItem>;
            if (cart == null || !cart.Any()) return RedirectToAction("Index");

            string maDat = "DM" + DateTime.Now.Ticks;

            var dat = new DATMUONTRUOC
            {
                MAMUON = maDat,
                NGAYDAT = DateTime.Now,
                TRANGTHAI = "Chờ duyệt",
                MASOTHE = maThe
            };

            db.DATMUONTRUOC.Add(dat);

            foreach (var item in cart)
            {
                db.CHITIETDATTRUOC.Add(new CHITIETDATTRUOC
                {
                    MAMUON = maDat,
                    MATAILIEU = item.MaTaiLieu,
                    PHIMUON = item.PhiMuon,
                    SOLUONG = item.SoLuong
                });
            }

            db.SaveChanges();
            Session["cart"] = null;

            return RedirectToAction("ThanhCong");
        }

        public ActionResult ThanhCong()
        {
            return View();
        }
    }
}
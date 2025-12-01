using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTHUVIEN.Models
{
    public class CartItem
    {
        public string MaTaiLieu { get; set; }
        public string TenSach { get; set; }
        public int SoLuong { get; set; }
        public float PhiMuon { get; set; }
    }
}
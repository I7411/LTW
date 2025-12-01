using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTHUVIEN.Models
{
    public class BorrowHistoryVM
    {
        public string MaPhieu { get; set; }
        public DateTime NgayMuon { get; set; }
        public DateTime NgayTra { get; set; }
        public List<CHITIETPHIEUMUON> ChiTiet { get; set; }
    }
}
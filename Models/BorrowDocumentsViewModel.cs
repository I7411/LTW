using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Library_Management.Models
{
    public class BorrowDocumentsViewModel
    {
        public List<TAILIEU> TaiLieuList { get; set; }
        public List<DATMUONTRUOC> DatMuonList { get; set; }
        public List<CHITIETDATTRUOC> ChiTietList { get; set; }
        public List<ChiTietDatTruocViewModel> ChiTietDatTruocList { get; set; }
        public List<InfoDocument> TaiLieuList2 { get; set; } = new List<InfoDocument>();

    }
}
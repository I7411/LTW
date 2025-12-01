using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Library_Management.Models
{
    public class UpdateViewModel
    {
        public DateTime NGSINH { get; set; }
        public string GIOITINH { get; set; }
        public string KHOAHOC { get; set; }
        public string NGHENGHIEP { get; set; }
        public string EMAIL { get; set; }
        public string DIACHI { get; set; }
        //Quản lý thư viện
        public string TENV { get; set; }
        public string SDT { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Library_Management.Models
{
    public class reader
    {
            [Required(ErrorMessage = "Họ và tên không được để trống")]
            public string HoTen { get; set; }

            [Required(ErrorMessage = "Mã số không được để trống")]
            public string MaSo { get; set; }

            [Required(ErrorMessage = "Ngày sinh không được để trống")]
            [DataType(DataType.Date)]
            public DateTime? NgaySinh { get; set; }

            [Required(ErrorMessage = "Giới tính không được để trống")]
            public string GioiTinh { get; set; }

            public string ChucVu { get; set; }

            public string GhiChu { get; set; }

            [Required(ErrorMessage = "Mật khẩu không được để trống")]
            public string MatKhau { get; set; }

            [Required(ErrorMessage = "Vui lòng nhập lại mật khẩu")]
            [Compare("MatKhau", ErrorMessage = "Mật khẩu không khớp")]
            public string NhapLaiMatKhau { get; set; }

            [Phone]
            public string DienThoai { get; set; }

            [EmailAddress]
            public string Email { get; set; }

            public string DiaChi { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Library_Management.Models
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Mã số không được để trống")]
        [StringLength(13, MinimumLength = 10, ErrorMessage = "Mã số phải có độ dài 10 hoặc 13 ký tự")]
        public string MaSo { get; set; }

        [Required(ErrorMessage = "Họ và tên không được để trống")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }

        [Required(ErrorMessage = "Giới tính không được để trống")]
        public string GioiTinh { get; set; }

        public string DiaChi { get; set; }

        public string DoiTuong { get; set; }

        public string ChucVu { get; set; }

        public string TenTruong { get; set; }

        public string KhoaHoc { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string DienThoai { get; set; }

        public string GhiChu { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        public string MatKhau { get; set; }

        [Required(ErrorMessage = "Nhập lại mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu nhập lại không khớp")]
        public string NhapLaiMatKhau { get; set; }

    }
}

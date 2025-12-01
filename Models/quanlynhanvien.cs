using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library_Management.Models
{
    public class quanlynhanvien
    {
        [Table("NHANVIEN")]
        public class NHANVIEN
        {
            [Key]
            [StringLength(10)]
            public string MANV { get; set; }

            [Required]
            [StringLength(100)]
            public string TENNV { get; set; }

            [DataType(DataType.Date)]
            public DateTime? NGSINH { get; set; }

            [StringLength(200)]
            public string DIACHI { get; set; }

            [StringLength(50)]
            public string VAITRO { get; set; }

            [StringLength(20)]
            public string SODIENTHOAI { get; set; }
        }
    }
}
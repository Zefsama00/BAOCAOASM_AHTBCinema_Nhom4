﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace AHTBCinema_NHOM4_SD18301.Models
{
    public class Phim
    {
        [Key]
        public string IdPhim { get; set; }
        [StringLength(50, ErrorMessage = "Tên phim tối đa 50 ký tự")]
        [Required(ErrorMessage = "Không được bỏ trống tên phim")]
        public string TenPhim { get; set; }
        public string DienVien { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống dạng phim")]
        public string DangPhim { get; set; }
        [ForeignKey("LoaiPhim")]
        //IdLoaiPhim
        public string TheLoai { get; set; }
        public virtual LoaiPhim LoaiPhim { get; set; }
        [Required(ErrorMessage = "Không được bỏ trống thời lượng")]
        [Range(1, int.MaxValue, ErrorMessage = "Thời lượng phải là số nguyên dương (phút)")]
        public int ThoiLuong { get; set; }
        public string HinhAnh { get; set; }
        public ICollection<CaChieu> CaChieus { get; set; }
    }
}

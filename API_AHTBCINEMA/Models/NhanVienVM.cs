using AHTBCinema_NHOM4_SD18301.Models;
using System.Collections.Generic;
using System;

namespace API_AHTBCINEMA.Models
{
    public class NhanVienVM
    {
        public string IdNV { get; set; }
        public string TenNV { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public DateTime NamSinh { get; set; }
        public string ChucVu { get; set; }
        public string Password { get; set; }
    }
}

﻿using AHTBCinema_NHOM4_SD18301.Models;
using API_AHTBCINEMA.Models;
using System.Collections.Generic;

namespace MVC_AHTBCINEMA.Model
{
    public class DangNhap
    {
        public IEnumerable<KhachHang> KhachHangs { get; set; }
        public IEnumerable<NhanVien> NhanViens { get; set; }
        public IEnumerable<User> Users { get; set; }
    }
}

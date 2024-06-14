using AHTBCinema_NHOM4_SD18301.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace API_AHTBCINEMA.Models
{
    public class CaChieuVM
    {
        public int IdCaChieu { get; set; }
       
        public string Phong { get; set; }
   
        public string Phim { get; set; }

        public DateTime NgayChieu { get; set; }
        public string TrangThai { get; set; }
    }
}

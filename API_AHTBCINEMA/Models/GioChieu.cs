using AHTBCinema_NHOM4_SD18301.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_AHTBCINEMA.Models
{
    public class GioChieu
    {
        public int IdGioChieu { get; set; }
        public TimeSpan GioBatDau { get; set; }
        public TimeSpan GioKetThuc {  get; set; }
        [ForeignKey("CaChieus")]
        public int Cachieu { get; set; }
        public CaChieu CaChieus { get; set; }
    }
}

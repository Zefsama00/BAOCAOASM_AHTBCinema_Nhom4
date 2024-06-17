using AHTBCinema_NHOM4_SD18301.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_AHTBCINEMA.Models
{
    public class GioChieu
    {
        [Key]
        public int IdGioChieu { get; set; }
        public TimeSpan GioBatDau { get; set; }
        public TimeSpan GioKetThuc {  get; set; }
        [ForeignKey("CaChieus")]
        public int Cachieu { get; set; }
        public CaChieu CaChieus { get; set; }
        public string TrangThai { get; set; }
        public ICollection<Ve> Ves { get; set; }
    }
}

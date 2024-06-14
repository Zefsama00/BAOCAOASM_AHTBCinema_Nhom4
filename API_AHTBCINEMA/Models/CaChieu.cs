using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AHTBCinema_NHOM4_SD18301.Models
{
    public class CaChieu
    {
        [Key]
        public int IdCaChieu { get; set; }
        [ForeignKey("Phongs")]
        public string Phong { get; set; }
        public Phong Phongs { get; set; }
        [ForeignKey("Phims")]
        public string Phim { get; set; }
        public Phim Phims { get; set; }
        public DateTime NgayChieu { get; set; }
        public string TrangThai { get; set; }
        public ICollection<Ve> Ves { get; set; }

    }
}

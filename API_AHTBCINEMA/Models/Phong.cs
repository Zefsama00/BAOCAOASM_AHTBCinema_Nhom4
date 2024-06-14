using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AHTBCinema_NHOM4_SD18301.Models
{
    public class Phong
    {
        [Key]
        public string IdPhong { get; set; }
        public int SoPhong { get; set; }
        public string TrangThai { get; set; }
        public int SoLuongGhe { get; set; }
        public ICollection<Ghe> Ghes { get; set; }
        public ICollection<CaChieu> CaChieu { get; set;}
    }
}

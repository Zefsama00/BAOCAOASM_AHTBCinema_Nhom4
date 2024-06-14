using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AHTBCinema_NHOM4_SD18301.Models
{
    public class Ve
    {
        [Key]
        public int IdVe { get; set; }
        public string TenVe { get; set; }
        public float GiaVe { get; set; }
        [ForeignKey("CaChieus")]
        public int CaChieu { get; set; }
        public CaChieu CaChieus {  get; set; }
        [ForeignKey("Ghes")]
        public string Ghe { get; set; }
        public Ghe Ghes { get; set; }
        public ICollection<HoaDon> HoaDons { get; set; }
    }
}

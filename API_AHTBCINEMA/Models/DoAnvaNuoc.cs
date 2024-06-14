using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AHTBCinema_NHOM4_SD18301.Models
{
    public class DoAnvaNuoc
    {
        [Key]
        public string IdComBo { get; set; }
        public string TenCombo { get; set; }
        public string Hinhanh { get; set; }
        public float Gia { get; set;}
        public ICollection<HoaDon> HoaDons { get; set; }
    }
}

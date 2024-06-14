using AHTBCinema_NHOM4_SD18301.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_AHTBCINEMA.Models
{
    public class VeVM
    {
        public int IdVe { get; set; }
        public string TenVe { get; set; }
        public float GiaVe { get; set; }

        public int CaChieu { get; set; }
   
        public string Ghe { get; set; }
    }
}

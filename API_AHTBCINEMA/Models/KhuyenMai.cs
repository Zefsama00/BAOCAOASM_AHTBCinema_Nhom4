using AHTBCinema_NHOM4_SD18301.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API_AHTBCINEMA.Models
{
    public class KhuyenMai
    {
        [Key] 
        public int IdKM { get; set; }
        public string KhuyenMaiName { get; set; }
        public int Phantram { get; set; }
        public ICollection<HoaDon> HoaDons { get; set; }
    }
}

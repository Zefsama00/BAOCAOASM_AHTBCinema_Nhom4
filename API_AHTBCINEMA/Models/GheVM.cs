using System.ComponentModel.DataAnnotations.Schema;

namespace API_AHTBCINEMA.Models
{
    public class GheVM
    {
        public string IdGhe { get; set; }
        public string TenGhe { get; set; }
       
        public string Phong { get; set; }
        public string TrangThai { get; set; }

        public string LoaiGhe { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API_AHTBCINEMA.Model
{
    public class PhimVM
    {
        
        public string IdPhim { get; set; }
        public string TenPhim { get; set; }
        public string DienVien { get; set; }
        public string DangPhim { get; set; }
        public int ThoiLuong { get; set; }
        public string HinhAnh { get; set; }
        public string TheLoai { get; set; }
    }
}

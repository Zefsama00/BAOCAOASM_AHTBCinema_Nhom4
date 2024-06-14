using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AHTBCinema_NHOM4_SD18301.Models
{
    public class HoaDon
    {
        [Key]

        public int IdHD { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0} VNĐ", ApplyFormatInEditMode = false)]
        [ForeignKey("Ve")]
        public int IdVe { get; set; }
        public Ve Ve { get; set; }
        [ForeignKey("Combos")]
        [DisplayFormat(DataFormatString = "{0:N0} VNĐ", ApplyFormatInEditMode = false)]
        public string Combo { get; set; }
        public DoAnvaNuoc Combos { get; set; }
        [ForeignKey("NhanViens")]
        public string NhanVien { get; set; }
        public NhanVien NhanViens { get; set; }
        [ForeignKey("KhachHangs")]
        public string KhachHang { get; set; }
        public KhachHang KhachHangs { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Khuyến mãi không được nhỏ hơn không")]
        public int KhuyenMai { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Tổng tiền không được nhỏ hơn không")]
        [DisplayFormat(DataFormatString = "{0:N0} VNĐ", ApplyFormatInEditMode = false)]
        public float TongTien { get; set; }

    }
}

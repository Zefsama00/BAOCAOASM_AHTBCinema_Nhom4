using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace AHTBCinema_NHOM4_SD18301.ViewModels
{
    public class BulkCreateGheViewModel
    {
        [Required(ErrorMessage = "Vui lòng không để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng nhập một số nguyên dương.")]
        [ValidateGheQuantity(ErrorMessage = "Số lượng ghế thêm vào vượt quá số lượng ghế có sẵn trong phòng.")]
        public int SoLuongGhe { get; set; }

        [Required(ErrorMessage = "Vui lòng không để trống.")]
        public string Phong { get; set; }

        [Required(ErrorMessage = "Vui lòng không để trống.")]
        public string TrangThai { get; set; }

        [Required(ErrorMessage = "Vui lòng không để trống.")]
        public string LoaiGhe { get; set; }

        [Required(ErrorMessage = "Vui lòng không để trống.")]
        [RegularExpression("^[A-Z]$", ErrorMessage = "Vui lòng nhập một chữ cái viết hoa duy nhất.")]
        public char StartingSeatLetter { get; set; }

        [Required(ErrorMessage = "Vui lòng không để trống.")]
        [Range(1, double.MaxValue, ErrorMessage = "Vui lòng nhập một số dương.")]
        public float GiaVe { get; set; }

        public int? GioChieuId { get; set; }
        public SelectList LoaiGheList { get; set; }
        public SelectList PhongList { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace AHTBCinema_NHOM4_SD18301.ViewModels
{
    public class BulkCreateGheViewModel
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a positive number of seats.")]
        public int SoLuongGhe { get; set; }

        [Required]
        public string Phong { get; set; }

        [Required]
        public string TrangThai { get; set; }

        [Required]
        public string LoaiGhe { get; set; }

        [Required]
        [RegularExpression("[A-Z]", ErrorMessage = "Please enter a single uppercase letter.")]
        public char StartingSeatLetter { get; set; }

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Please enter a positive price.")]
        public float GiaVe { get; set; } // Add this property for ticket price

        public int? GioChieuId { get; set; } // Changed from CaChieuId
    }
}
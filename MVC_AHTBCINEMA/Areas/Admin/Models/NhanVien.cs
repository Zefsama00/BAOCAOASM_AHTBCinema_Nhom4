using System.ComponentModel.DataAnnotations;
using System;
using Newtonsoft.Json;

namespace MVC_ASM_AHTBCinema_NHOM4_SD18301.Models
{
    public class NhanVien
    {
        [Key]
        [Required(ErrorMessage = "Vui lòng không để trống tên Id Nhân Viên")]
        public string IdNV { get; set; }
        [Required(ErrorMessage = "Vui lòng không để trống tên Nhân Viên")]
        [StringLength(50, ErrorMessage = "Độ dài tối đa 50 ký tự")]
        public string TenNV { get; set; }
        [Required(ErrorMessage = "Vui lòng không để trống Số Điện Thoại")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Độ dài bắt buộc phải bằng 10")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Vui lòng nhập đúng chữ số 0-9")]
        public string SDT { get; set; }
        [Required(ErrorMessage = "Vui lòng không để trống email")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ, a-z[0-9]@gmail.com")]
        [StringLength(50, ErrorMessage = "Độ dài tối đa 50 ký tự")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng không để trống Ngày Sinh")]
        public DateTime NamSinh { get; set; }
        [Required(ErrorMessage = "Vui lòng không để trống Chức Vụ")]
        [StringLength(50, ErrorMessage = "Độ dài tối đa 50 ksy tự")]
        public string ChucVu { get; set; }
        [Required(ErrorMessage = "Vui lòng không để trống mật khẩu")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*\W).{6,}$", ErrorMessage = "Mật khẩu ít nhất 1 chữ hoa, 1 ký tự đặt biệt, 1 số, tối thiểu là 6")]
        public string Password { get; set; }
        public string NgaySinhString => NamSinh.ToString("dd-MM-yyyy");
    }
}

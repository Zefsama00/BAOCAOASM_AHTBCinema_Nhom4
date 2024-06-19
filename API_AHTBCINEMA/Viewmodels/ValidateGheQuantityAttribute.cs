using System.ComponentModel.DataAnnotations;
using System.Linq;
using ASM_AHTBCINEMA_NHOM4_SD18301.Data;
using SQLitePCL;

namespace AHTBCinema_NHOM4_SD18301.ViewModels
{
    public class ValidateGheQuantityAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (BulkCreateGheViewModel)validationContext.ObjectInstance;
            var context = (DBCinemaContext)validationContext.GetService(typeof(DBCinemaContext));

            if (context == null)
            {
                return new ValidationResult("Database context not found.");
            }

            var phong = context.Phongs.FirstOrDefault(p => p.IdPhong == model.Phong);
            if (phong == null)
            {
                return new ValidationResult("Invalid Phong Id.");
            }

            int soLuongGheHienTai = phong.SoLuongGhe;
            int soLuongGheThem = model.SoLuongGhe;

            // Count the number of existing tickets in the chosen screening time
            int soLuongVeHienTai = context.Ves
                .Count(v => v.SuatChieu == model.GioChieuId && v.Ghes.Phong == model.Phong);

            // Total number of seats after adding the new seats
            int tongSoGheSauKhiThem = soLuongVeHienTai + soLuongGheThem;

            if (tongSoGheSauKhiThem > soLuongGheHienTai)
            {
                return new ValidationResult("Số lượng ghế thêm vào vượt quá số lượng ghế có sẵn trong phòng.");
            }

            return ValidationResult.Success;
        }
    }
}
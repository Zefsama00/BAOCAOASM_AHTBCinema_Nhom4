using AHTBCinema_NHOM4_SD18301.Models;
using API_AHTBCINEMA.Models;
using ASM_AHTBCINEMA_NHOM4_SD18301.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace API_AHTBCINEMA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly DBCinemaContext _context;

        public HoaDonController(DBCinemaContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HoaDon>>> GetProducts()
        {
            return await _context.HoaDons.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HoaDon>> GetProduct(int id)
        {
            var product = await _context.HoaDons.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HoaDon>> PostProduct(HoaDonVM product)
        {
            var PRO = new HoaDon
            {
                IdHD = product.IdHD,
                IdVe = product.IdVe,
                Combo = product.Combo,
                NhanVien = product.NhanVien,
                KhachHang = product.KhachHang,
                KhuyenMai = product.KhuyenMai,
                TongTien = product.TongTien,
            };
            try
            {
                _context.HoaDons.Add(PRO);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException a)
            {
                Console.WriteLine(a.Message);
                throw;
            }


            return CreatedAtAction("GetProduct", new { id = PRO.IdHD }, product);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatehoaDon(string id, HoaDon hoaDon)
        {
            try
            {
                // Tìm kiếm Hóa đơn hiện có
                var existinghoaDon = await _context.HoaDons.FindAsync(id);
                if (existinghoaDon == null)
                {
                    return NotFound();
                }

                // Kiểm tra tồn tại liên quan
                if (!await RelatedEntitiesExist(hoaDon))
                {
                    return NotFound(new { Message = "Ve or Combo or NhanVien,KhachHang not found" });
                }

                // Cập nhật thuộc tính của Hóa đơn
                UpdatehoaDonProperties(existinghoaDon, hoaDon);

                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();

                // Trả về kết quả thành công
                return Ok(existinghoaDon);
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi và trả về mã lỗi 500
                // Có thể log lỗi vào file hoặc hệ thống logging
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Phương thức kiểm tra tồn tại của các thực thể liên quan
        private async Task<bool> RelatedEntitiesExist(HoaDon hoaDon)
        {
            var existingVe = await _context.Ves.AnyAsync(x => x.IdVe == hoaDon.IdVe);
            var existingCombo = await _context.DoAnvaNuocs.AnyAsync(x => x.IdComBo == hoaDon.Combo);
            var existingNhanVien = await _context.NhanViens.AnyAsync(x => x.IdNV == hoaDon.NhanVien);
            var existingKhachHang = await _context.KhachHangs.AnyAsync(x => x.IdKH == hoaDon.KhachHang);

            return existingVe && existingCombo && existingNhanVien && existingKhachHang;
        }

        // Phương thức cập nhật thuộc tính của Hóa đơn
        private void UpdatehoaDonProperties(HoaDon existinghoaDon, HoaDon hoaDon)
        {
            existinghoaDon.IdVe = hoaDon.IdVe;
            existinghoaDon.NhanVien = hoaDon.NhanVien;
            existinghoaDon.KhachHang = hoaDon.KhachHang;
            existinghoaDon.KhuyenMai = hoaDon.KhuyenMai;
            existinghoaDon.TongTien = hoaDon.TongTien;
        }
    }
}


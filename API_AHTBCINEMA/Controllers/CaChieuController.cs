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
    public class CaChieuController : ControllerBase
    {
        private readonly DBCinemaContext _context;

        public CaChieuController(DBCinemaContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CaChieu>>> GetProducts()
        {
            return await _context.CaChieus.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CaChieu>> GetProduct(int id)
        {
            var product = await _context.CaChieus.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CaChieu>> PostProduct(CaChieuVM product)
        {
            var PRO = new CaChieu
            {
                IdCaChieu = product.IdCaChieu,
                Phong = product.Phong,
                Phim = product.Phim,
                NgayChieu = product.NgayChieu,
                TrangThai = product.TrangThai,
            };
            try
            {
                _context.CaChieus.Add(PRO);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException a)
            {
                Console.WriteLine(a.Message);
                throw;
            }


            return CreatedAtAction("GetProduct", new { id = PRO.IdCaChieu }, product);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Updatecachieu(string id, CaChieu cachieu)
        {
            try
            {
                // Tìm kiếm Ca Chiếu hiện có
                var existingcachieu = await _context.CaChieus.FindAsync(id);
                if (existingcachieu == null)
                {
                    return NotFound();
                }

                // Kiểm tra tồn tại liên quan
                if (!await RelatedEntitiesExist(cachieu))
                {
                    return NotFound(new { Message = "Loaicachieu or Phong not found" });
                }

                // Cập nhật thuộc tính của Ca Chiếu
                UpdatecachieuProperties(existingcachieu, cachieu);

                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();

                // Trả về kết quả thành công
                return Ok(existingcachieu);
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi và trả về mã lỗi 500
                // Có thể log lỗi vào file hoặc hệ thống logging
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Phương thức kiểm tra tồn tại của các thực thể liên quan
        private async Task<bool> RelatedEntitiesExist(CaChieu cachieu)
        {
            var existingPhim = await _context.Phims.AnyAsync(x => x.IdPhim == cachieu.Phim);
            var existingPhong = await _context.Phongs.AnyAsync(x => x.IdPhong == cachieu.Phong);

            return existingPhim && existingPhong;
        }

        // Phương thức cập nhật thuộc tính của Ca Chiếu
        private void UpdatecachieuProperties(CaChieu existingcachieu, CaChieu cachieu)
        {
            existingcachieu.Phim = cachieu.Phim;
            existingcachieu.Phong = cachieu.Phong;
            existingcachieu.TrangThai = cachieu.TrangThai;
            existingcachieu.NgayChieu = cachieu.NgayChieu;
        }
    }
}

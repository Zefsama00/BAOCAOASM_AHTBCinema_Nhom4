using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

using ASM_AHTBCINEMA_NHOM4_SD18301.Data;
using AHTBCinema_NHOM4_SD18301.Models;
using API_AHTBCINEMA.Models;

namespace LoaiGhe_Phong_Ghe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GheController : ControllerBase
    {
        private readonly DBCinemaContext _context;

        public GheController(DBCinemaContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ghe>>> GetProducts()
        {
            return await _context.Ghes.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ghe>> GetProduct(int id)
        {
            var product = await _context.Ghes.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ghe>> PostProduct(GheVM product)
        {
            var PRO = new Ghe
            {
                IdGhe = product.IdGhe,
                TenGhe = product.TenGhe,
                Phong = product.Phong,
                TrangThai = product.TrangThai,
                LoaiGhe = product.LoaiGhe,
            };
            try
            {
                _context.Ghes.Add(PRO);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException a)
            {
                Console.WriteLine(a.Message);
                throw;
            }


            return CreatedAtAction("GetProduct", new { id = PRO.IdGhe }, product);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGhe(string id, Ghe ghe)
        {
            try
            {
                // Tìm kiếm Ghế hiện có
                var existingGhe = await _context.Ghes.FindAsync(id);
                if (existingGhe == null)
                {
                    return NotFound();
                }

                // Kiểm tra tồn tại của LoaiGhe và Phong liên quan
                if (!await RelatedEntitiesExist(ghe))
                {
                    return NotFound(new { Message = "LoaiGhe or Phong not found" });
                }

                // Cập nhật thuộc tính của Ghế
                UpdateGheProperties(existingGhe, ghe);

                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();

                // Trả về kết quả thành công
                return Ok(existingGhe);
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi và trả về mã lỗi 500
                // Có thể log lỗi vào file hoặc hệ thống logging
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Phương thức kiểm tra tồn tại của các thực thể liên quan
        private async Task<bool> RelatedEntitiesExist(Ghe ghe)
        {
            var existingLoaiGhe = await _context.LoaiGhes.AnyAsync(x => x.IdLoaiGhe == ghe.LoaiGhe);
            var existingPhong = await _context.Phongs.AnyAsync(x => x.IdPhong == ghe.Phong);

            return existingLoaiGhe && existingPhong;
        }

        // Phương thức cập nhật thuộc tính của Ghế
        private void UpdateGheProperties(Ghe existingGhe, Ghe ghe)
        {
            existingGhe.TenGhe = ghe.TenGhe;
            existingGhe.Phong = ghe.Phong;
            existingGhe.TrangThai = ghe.TrangThai;
            existingGhe.LoaiGhe = ghe.LoaiGhe;
        }
    }
}

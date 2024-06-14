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
    public class VeController : ControllerBase
    {
        private readonly DBCinemaContext _context;

        public VeController(DBCinemaContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ve>>> GetProducts()
        {
            return await _context.Ves.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ve>> GetProduct(int id)
        {
            var product = await _context.Ves.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ve>> PostProduct(VeVM product)
        {
            var PRO = new Ve
            {
                IdVe = product.IdVe,
                TenVe = product.TenVe,
                GiaVe = product.GiaVe,
                CaChieu = product.CaChieu,
                Ghe = product.Ghe,
            };
            try
            {
                _context.Ves.Add(PRO);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException a)
            {
                Console.WriteLine(a.Message);
                throw;
            }


            return CreatedAtAction("GetProduct", new { id = PRO.IdVe }, product);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Updateve(string id, Ve ve)
        {
            try
            {
                // Tìm kiếm Vé hiện có
                var existingve = await _context.Ves.FindAsync(id);
                if (existingve == null)
                {
                    return NotFound();
                }

                // Kiểm tra tồn tại liên quan
                if (!await RelatedEntitiesExist(ve))
                {
                    return NotFound(new { Message = "Loaive or Phong not found" });
                }

                // Cập nhật thuộc tính của Vé
                UpdateveProperties(existingve, ve);

                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();

                // Trả về kết quả thành công
                return Ok(existingve);
            }
            catch (Exception ex)
            {
                // Ghi lại lỗi và trả về mã lỗi 500
                // Có thể log lỗi vào file hoặc hệ thống logging
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Phương thức kiểm tra tồn tại của các thực thể liên quan
        private async Task<bool> RelatedEntitiesExist(Ve ve)
        {
            var existingCaChieu = await _context.CaChieus.AnyAsync(x => x.IdCaChieu == ve.CaChieu);
            var existingGhe = await _context.Ghes.AnyAsync(x => x.IdGhe == ve.Ghe);

            return existingCaChieu && existingGhe;
        }

        // Phương thức cập nhật thuộc tính của Vé
        private void UpdateveProperties(Ve existingve, Ve ve)
        {
            existingve.TenVe = ve.TenVe;
            existingve.GiaVe = ve.GiaVe;
            existingve.CaChieu = ve.CaChieu;
            existingve.Ghe = ve.Ghe;
        }
    }
}

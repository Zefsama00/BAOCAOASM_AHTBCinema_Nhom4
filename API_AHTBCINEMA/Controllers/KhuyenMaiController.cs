using AHTBCinema_NHOM4_SD18301.Models;
using API_AHTBCINEMA.Models;
using ASM_AHTBCINEMA_NHOM4_SD18301.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_AHTBCINEMA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhuyenMaiController : ControllerBase
    {
        private readonly DBCinemaContext _context;
        public KhuyenMaiController(DBCinemaContext context)
        {
            _context = context;
            
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KhuyenMai>>> GetCategory()
        {
            return await _context.KhuyenMais.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<KhuyenMai>> PostCategory(KhuyenMaiVM khuyenmai)
        {
            var khuyenmai1 = new KhuyenMai
            {
               IdKM = khuyenmai.IdKM,
               KhuyenMaiName = khuyenmai.KhuyenName,
               Phantram = khuyenmai.Phantram,
            };
            _context.KhuyenMais.Add(khuyenmai1);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = khuyenmai1.IdKM }, khuyenmai);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<KhuyenMai>> UpdateCategory(string id, KhuyenMai combo)
        {
            var existingkhuyenMai = await _context.KhuyenMais.FindAsync(id);
            if (existingkhuyenMai == null)
            {
                return NotFound();
            }

            existingkhuyenMai.IdKM = combo.IdKM;
            existingkhuyenMai.KhuyenMaiName = combo.KhuyenMaiName;
            existingkhuyenMai.Phantram = combo.Phantram;
            await _context.SaveChangesAsync();

            return Ok(existingkhuyenMai);
        }
    }
}

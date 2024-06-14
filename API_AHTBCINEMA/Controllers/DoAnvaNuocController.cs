using AHTBCinema_NHOM4_SD18301.Models;
using API_AHTBCINEMA.Models;
using ASM_AHTBCINEMA_NHOM4_SD18301.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_AHTBCINEMA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoAnvaNuocController : ControllerBase
    {
        private readonly DBCinemaContext _context;

        public DoAnvaNuocController(DBCinemaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoAnvaNuoc>>> GetCategory()
        {
            return await _context.DoAnvaNuocs.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<DoAnvaNuoc>> PostCategory(DoAnvaNuocVM doAnvaNuoc)
        {
            var DoAnvaNuoc1 = new DoAnvaNuoc
            {
                IdComBo = doAnvaNuoc.IdComBo,
                TenCombo = doAnvaNuoc.TenCombo,
                Gia = doAnvaNuoc.Gia,
                Hinhanh = doAnvaNuoc.Hinhanh,
            };
            _context.DoAnvaNuocs.Add(DoAnvaNuoc1);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = DoAnvaNuoc1.IdComBo }, doAnvaNuoc);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<DoAnvaNuoc>> UpdateCategory(string id, DoAnvaNuoc combo)
        {
            var existingdoAnvaNuoc = await _context.DoAnvaNuocs.FindAsync(id);
            if (existingdoAnvaNuoc == null)
            {
                return NotFound();
            }

            existingdoAnvaNuoc.TenCombo = combo.TenCombo;
            existingdoAnvaNuoc.Gia = combo.Gia;
            existingdoAnvaNuoc.Hinhanh = combo.Hinhanh;
            await _context.SaveChangesAsync();

            return Ok(existingdoAnvaNuoc);
        }
    }
}

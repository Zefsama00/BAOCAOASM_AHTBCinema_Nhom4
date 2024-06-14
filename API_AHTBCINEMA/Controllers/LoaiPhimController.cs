using AHTBCinema_NHOM4_SD18301.Models;
using API_AHTBCINEMA.Model;
using ASM_AHTBCINEMA_NHOM4_SD18301.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APIPhongLP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiPhimController : ControllerBase
    {
        private readonly DBCinemaContext _context;
        public LoaiPhimController(DBCinemaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoaiPhim>>> GetCategory()
        {
            return await _context.LoaiPhims.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<LoaiPhim>> PostCategory(LoaiPhimVM category)
        {
            var category1 = new LoaiPhim
            {
                IdLP = category.IdLP,
                TenLoai = category.TenLoai,
            };
            _context.LoaiPhims.Add(category1);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category1.IdLP }, category);
        }

    }
}

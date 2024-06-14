using AHTBCinema_NHOM4_SD18301.Models;
using API_AHTBCINEMA.Models;
using ASM_AHTBCINEMA_NHOM4_SD18301.Data;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoaiGhe_Phong_Ghe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhongController : ControllerBase
    {
        private readonly DBCinemaContext _context;

        public PhongController(DBCinemaContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Phong>>> GetCategory()
        {
            return await _context.Phongs.ToListAsync();
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Phong>> PostCategory(PhongVM phong)
        {
            var phong1 = new Phong
            {
                IdPhong = phong.IdPhong,
                SoPhong = phong.SoPhong,
                TrangThai = phong.TrangThai,
                SoLuongGhe = phong.SoLuongGhe,
            };
            _context.Phongs.Add(phong1);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = phong1.IdPhong }, phong);
        }
    }
}

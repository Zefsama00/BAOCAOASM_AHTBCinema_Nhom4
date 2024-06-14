using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using ASM_AHTBCINEMA_NHOM4_SD18301.Data;
using AHTBCinema_NHOM4_SD18301.Models;
using API_AHTBCINEMA.Models;

namespace LoaiGhe_Phong_Ghe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoaiGheController : ControllerBase
    {
        private readonly DBCinemaContext _context;

        public LoaiGheController(DBCinemaContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoaiGhe>>> GetCategory()
        {
            return await _context.LoaiGhes.ToListAsync();
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LoaiGhe>> PostCategory(LoaiGheVM category)
        {
            var category1 = new LoaiGhe
            {
                IdLoaiGhe = category.IdLoaiGhe,
                TenLoaiGhe = category.TenLoaiGhe,
            };
            _context.LoaiGhes.Add(category1);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category1.IdLoaiGhe }, category);
        }
    }
}


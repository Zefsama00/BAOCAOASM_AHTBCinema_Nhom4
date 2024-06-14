using AHTBCinema_NHOM4_SD18301.Models;
using API_AHTBCINEMA.Model;
using ASM_AHTBCINEMA_NHOM4_SD18301.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIPhongLP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhimController : ControllerBase
    {
        private readonly DBCinemaContext _context;
        public PhimController(DBCinemaContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Phim>>> GetProducts()
        {
            return await _context.Phims.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Phim>> GetProduct(int id)
        {
            var product = await _context.Phims.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Phim>> PostProduct(PhimVM product)
        {
            var PRO = new Phim
            {
                IdPhim = product.IdPhim,
                TenPhim = product.TenPhim,
                DienVien = product.DienVien,
                DangPhim = product.DangPhim,
                ThoiLuong = product.ThoiLuong,
                HinhAnh = product.HinhAnh,
                TheLoai= product.TheLoai

            };
            try
            {
                _context.Phims.Add(PRO);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException a)
            {
                Console.WriteLine(a.Message);
                throw;
            }


            return CreatedAtAction("GetProduct", new { id = PRO.IdPhim }, product);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhim(string id, Phim phim)
        {
            if (id != phim.IdPhim)
            {
                return BadRequest();
            }

            _context.Entry(phim).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhimExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        private bool PhimExists(string id)
        {
            return _context.Phims.Any(e => e.IdPhim == id);
        }
    }
}

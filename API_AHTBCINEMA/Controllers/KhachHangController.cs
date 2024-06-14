using AHTBCinema_NHOM4_SD18301.Models;
using API_AHTBCINEMA.Models;
using ASM_AHTBCINEMA_NHOM4_SD18301.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_AHTBCINEMA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KhachHangController : ControllerBase
    {
        private readonly DBCinemaContext _context;
        private readonly PasswordHasher<KhachHang> _passwordHasher;
        public KhachHangController(DBCinemaContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<KhachHang>();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KhachHang>>> GetCategory()
        {
            return await _context.KhachHangs.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<KhachHang>> PostCategory(KhachHangVM khachHang)
        {
            var khachHang1 = new KhachHang
            {
                IdKH = khachHang.IdKH,
                TenKH = khachHang.TenKH,
                SDT = khachHang.SDT,
                NamSinh = khachHang.NamSinh,
                Email = khachHang.Email,
                Password = khachHang.Password,
            };
            khachHang1.Password = _passwordHasher.HashPassword(khachHang1, khachHang1.Password);
            _context.KhachHangs.Add(khachHang1);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = khachHang1.IdKH }, khachHang);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<KhachHang>> UpdateCategory(string id, KhachHang combo)
        {
            var existingkhachHang = await _context.KhachHangs.FindAsync(id);
            if (existingkhachHang == null)
            {
                return NotFound();
            }

            existingkhachHang.TenKH = combo.TenKH;
            existingkhachHang.SDT = combo.SDT;
            existingkhachHang.NamSinh = combo.NamSinh;
            existingkhachHang.Email = combo.Email;
            existingkhachHang.Password = combo.Password;
            await _context.SaveChangesAsync();

            return Ok(existingkhachHang);
        }
    }
}

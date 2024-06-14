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
    public class NhanVienController : ControllerBase
    {
        private readonly DBCinemaContext _context;
        private readonly PasswordHasher<NhanVien> _passwordHasher;
        public NhanVienController(DBCinemaContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<NhanVien>();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NhanVien>>> GetCategory()
        {
            return await _context.NhanViens.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<NhanVien>> PostCategory(NhanVienVM nhanVien)
        {
            var nhanVien1 = new NhanVien
            {
                IdNV = nhanVien.IdNV,
                TenNV = nhanVien.TenNV,
                SDT = nhanVien.SDT,
                NamSinh = nhanVien.NamSinh,
                Email = nhanVien.Email,
                ChucVu = nhanVien.ChucVu,
                Password = nhanVien.Password,
            };
            nhanVien1.Password = _passwordHasher.HashPassword(nhanVien1, nhanVien1.Password);
            _context.NhanViens.Add(nhanVien1);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = nhanVien1.IdNV }, nhanVien);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<NhanVien>> UpdateCategory(string id, NhanVien combo)
        {
            var existingnhanVien = await _context.NhanViens.FindAsync(id);
            if (existingnhanVien == null)
            {
                return NotFound();
            }

            existingnhanVien.TenNV = combo.TenNV;
            existingnhanVien.SDT = combo.SDT;
            existingnhanVien.NamSinh = combo.NamSinh;
            existingnhanVien.Email = combo.Email;
            existingnhanVien.ChucVu = combo.ChucVu;
            existingnhanVien.Password = combo.Password;
            await _context.SaveChangesAsync();

            return Ok(existingnhanVien);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AHTBCinema_NHOM4_SD18301.Models;
using ASM_AHTBCINEMA_NHOM4_SD18301.Data;

namespace MVC_AHTBCINEMA.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HoaDonsController : Controller
    {
        private readonly DBCinemaContext _context;

        public HoaDonsController(DBCinemaContext context)
        {
            _context = context;
        }

        // GET: Admin/HoaDons
        public async Task<IActionResult> Index()
        {
            var dBCinemaContext = _context.HoaDons.Include(h => h.KhachHangs).Include(h => h.NhanViens).Include(h => h.Ve);
            return View(await dBCinemaContext.ToListAsync());
        }
        public async Task<IActionResult> Index1()
        {
            var dBCinemaContext = _context.HoaDons.Include(h => h.Combos).Include(h => h.KhachHangs).Include(h => h.KhuyenMais).Include(h => h.NhanViens).Include(h => h.Ve);
            return View(await dBCinemaContext.ToListAsync());
        }
        // GET: Admin/HoaDons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons
                .Include(h => h.KhachHangs)
                .Include(h => h.NhanViens)
                .Include(h => h.Ve)
                .FirstOrDefaultAsync(m => m.IdHD == id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            return View(hoaDon);
        }

        // GET: Admin/HoaDons/Create
        public IActionResult Create()
        {
            ViewData["Combo"] = new SelectList(_context.DoAnvaNuocs, "IdComBo", "TenCombo");
            ViewData["KhachHang"] = new SelectList(_context.KhachHangs, "IdKH", "TenKH");
            ViewData["NhanVien"] = new SelectList(_context.NhanViens, "IdNV", "TenNV");
            ViewData["IdVe"] = new SelectList(_context.Ves, "IdVe", "GiaVe");
            return View();
        }

        // POST: Admin/HoaDons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdHD,IdVe,Combo,NhanVien,KhachHang,KhuyenMai,TongTien")] HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hoaDon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KhachHang"] = new SelectList(_context.KhachHangs, "IdKH", "TenKH", hoaDon.KhachHang);
            ViewData["NhanVien"] = new SelectList(_context.NhanViens, "IdNV", "TenNV", hoaDon.NhanVien);
            ViewData["IdVe"] = new SelectList(_context.Ves, "IdVe", "GiaVe", hoaDon.IdVe);
            return View(hoaDon);
        }

        // GET: Admin/HoaDons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon == null)
            {
                return NotFound();
            }
    
            ViewData["KhachHang"] = new SelectList(_context.KhachHangs, "IdKH", "TenKH", hoaDon.KhachHang);
            ViewData["NhanVien"] = new SelectList(_context.NhanViens, "IdNV", "TenNV", hoaDon.NhanVien);
            ViewData["IdVe"] = new SelectList(_context.Ves, "IdVe", "GiaVe", hoaDon.IdVe);
            return View(hoaDon);
        }

        // POST: Admin/HoaDons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdHD,IdVe,Combo,NhanVien,KhachHang,KhuyenMai,TongTien")] HoaDon hoaDon)
        {
            if (id != hoaDon.IdHD)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hoaDon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HoaDonExists(hoaDon.IdHD))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
 
            ViewData["KhachHang"] = new SelectList(_context.KhachHangs, "IdKH", "TenKH", hoaDon.KhachHang);
            ViewData["NhanVien"] = new SelectList(_context.NhanViens, "IdNV", "TenNV", hoaDon.NhanVien);
            ViewData["IdVe"] = new SelectList(_context.Ves, "IdVe", "IdVe", hoaDon.IdVe);
            return View(hoaDon);
        }
        [HttpPost]
        public IActionResult Approve(int IdHD)
        {
            var hoaDon = _context.HoaDons.Find(IdHD);
            if (hoaDon != null)
            {
                hoaDon.TrangThai = "Đã xác nhận"; 
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/HoaDons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons

                .Include(h => h.KhachHangs)
                .Include(h => h.NhanViens)
                .Include(h => h.Ve)
                .FirstOrDefaultAsync(m => m.IdHD == id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            return View(hoaDon);
        }

        // POST: Admin/HoaDons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hoaDon = await _context.HoaDons.FindAsync(id);
            _context.HoaDons.Remove(hoaDon);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HoaDonExists(int id)
        {
            return _context.HoaDons.Any(e => e.IdHD == id);
        }
    }
}

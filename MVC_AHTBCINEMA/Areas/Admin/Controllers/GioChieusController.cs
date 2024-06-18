using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using API_AHTBCINEMA.Models;
using ASM_AHTBCINEMA_NHOM4_SD18301.Data;

namespace MVC_AHTBCINEMA.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class GioChieusController : Controller
    {
        private readonly DBCinemaContext _context;

        public GioChieusController(DBCinemaContext context)
        {
            _context = context;
        }

        // GET: Admin/GioChieus
        public async Task<IActionResult> Index()
        {
            var dBCinemaContext = _context.GioChieus.Include(g => g.CaChieus);
            return View(await dBCinemaContext.ToListAsync());
        }

        // GET: Admin/GioChieus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gioChieu = await _context.GioChieus
                .Include(g => g.CaChieus)
                .FirstOrDefaultAsync(m => m.IdGioChieu == id);
            if (gioChieu == null)
            {
                return NotFound();
            }

            return View(gioChieu);
        }

        // GET: Admin/GioChieus/Create
        public IActionResult Create()
        {
            ViewData["Cachieu"] = new SelectList(_context.CaChieus, "IdCaChieu", "IdCaChieu");
            return View();
        }

        // POST: Admin/GioChieus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdGioChieu,GioBatDau,GioKetThuc,Cachieu,TrangThai")] GioChieu gioChieu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gioChieu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Cachieu"] = new SelectList(_context.CaChieus, "IdCaChieu", "IdCaChieu", gioChieu.Cachieu);
            return View(gioChieu);
        }

        // GET: Admin/GioChieus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gioChieu = await _context.GioChieus.FindAsync(id);
            if (gioChieu == null)
            {
                return NotFound();
            }
            ViewData["Cachieu"] = new SelectList(_context.CaChieus, "IdCaChieu", "IdCaChieu", gioChieu.Cachieu);
            return View(gioChieu);
        }

        // POST: Admin/GioChieus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdGioChieu,GioBatDau,GioKetThuc,Cachieu,TrangThai")] GioChieu gioChieu)
        {
            if (id != gioChieu.IdGioChieu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gioChieu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GioChieuExists(gioChieu.IdGioChieu))
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
            ViewData["Cachieu"] = new SelectList(_context.CaChieus, "IdCaChieu", "IdCaChieu", gioChieu.Cachieu);
            return View(gioChieu);
        }

        // GET: Admin/GioChieus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gioChieu = await _context.GioChieus
                .Include(g => g.CaChieus)
                .FirstOrDefaultAsync(m => m.IdGioChieu == id);
            if (gioChieu == null)
            {
                return NotFound();
            }

            return View(gioChieu);
        }

        // POST: Admin/GioChieus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gioChieu = await _context.GioChieus.FindAsync(id);
            _context.GioChieus.Remove(gioChieu);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GioChieuExists(int id)
        {
            return _context.GioChieus.Any(e => e.IdGioChieu == id);
        }
    }
}

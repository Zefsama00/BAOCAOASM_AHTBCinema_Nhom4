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
    public class GhesController : Controller
    {
        private readonly DBCinemaContext _context;

        public GhesController(DBCinemaContext context)
        {
            _context = context;
        }

        // GET: Admin/Ghes
        public async Task<IActionResult> Index()
        {
            var dBCinemaContext = _context.Ghes.Include(g => g.LoaiGhes).Include(g => g.Phongs);
            return View(await dBCinemaContext.ToListAsync());
        }

        // GET: Admin/Ghes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ghe = await _context.Ghes
                .Include(g => g.LoaiGhes)
                .Include(g => g.Phongs)
                .FirstOrDefaultAsync(m => m.IdGhe == id);
            if (ghe == null)
            {
                return NotFound();
            }

            return View(ghe);
        }

        // GET: Admin/Ghes/Create
        public IActionResult Create()
        {
            ViewData["LoaiGhe"] = new SelectList(_context.LoaiGhes, "IdLoaiGhe", "IdLoaiGhe");
            ViewData["Phong"] = new SelectList(_context.Phongs, "IdPhong", "IdPhong");
            return View();
        }

        // POST: Admin/Ghes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdGhe,TenGhe,Phong,TrangThai,LoaiGhe")] Ghe ghe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ghe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LoaiGhe"] = new SelectList(_context.LoaiGhes, "IdLoaiGhe", "IdLoaiGhe", ghe.LoaiGhe);
            ViewData["Phong"] = new SelectList(_context.Phongs, "IdPhong", "IdPhong", ghe.Phong);
            return View(ghe);
        }

        // GET: Admin/Ghes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ghe = await _context.Ghes.FindAsync(id);
            if (ghe == null)
            {
                return NotFound();
            }
            ViewData["LoaiGhe"] = new SelectList(_context.LoaiGhes, "IdLoaiGhe", "IdLoaiGhe", ghe.LoaiGhe);
            ViewData["Phong"] = new SelectList(_context.Phongs, "IdPhong", "IdPhong", ghe.Phong);
            return View(ghe);
        }

        // POST: Admin/Ghes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdGhe,TenGhe,Phong,TrangThai,LoaiGhe")] Ghe ghe)
        {
            if (id != ghe.IdGhe)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ghe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GheExists(ghe.IdGhe))
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
            ViewData["LoaiGhe"] = new SelectList(_context.LoaiGhes, "IdLoaiGhe", "IdLoaiGhe", ghe.LoaiGhe);
            ViewData["Phong"] = new SelectList(_context.Phongs, "IdPhong", "IdPhong", ghe.Phong);
            return View(ghe);
        }

        // GET: Admin/Ghes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ghe = await _context.Ghes
                .Include(g => g.LoaiGhes)
                .Include(g => g.Phongs)
                .FirstOrDefaultAsync(m => m.IdGhe == id);
            if (ghe == null)
            {
                return NotFound();
            }

            return View(ghe);
        }

        // POST: Admin/Ghes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var ghe = await _context.Ghes.FindAsync(id);
            _context.Ghes.Remove(ghe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GheExists(string id)
        {
            return _context.Ghes.Any(e => e.IdGhe == id);
        }
    }
}

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
    public class VesController : Controller
    {
        private readonly DBCinemaContext _context;

        public VesController(DBCinemaContext context)
        {
            _context = context;
        }

        // GET: Admin/Ves
        public async Task<IActionResult> Index()
        {
            var dBCinemaContext = _context.Ves.Include(v => v.CaChieus).Include(v => v.Ghes);
            return View(await dBCinemaContext.ToListAsync());
        }

        // GET: Admin/Ves/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ve = await _context.Ves
                .Include(v => v.CaChieus)
                .Include(v => v.Ghes)
                .FirstOrDefaultAsync(m => m.IdVe == id);
            if (ve == null)
            {
                return NotFound();
            }

            return View(ve);
        }

        // GET: Admin/Ves/Create
        public IActionResult Create()
        {
            ViewData["CaChieu"] = new SelectList(_context.CaChieus, "IdCaChieu", "IdCaChieu");
            ViewData["Ghe"] = new SelectList(_context.Ghes, "IdGhe", "TenGhe");
            return View();
        }

        // POST: Admin/Ves/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdVe,TenVe,GiaVe,CaChieu,Ghe")] Ve ve)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ve);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CaChieu"] = new SelectList(_context.CaChieus, "IdCaChieu", "IdCaChieu", ve.CaChieu);
            ViewData["Ghe"] = new SelectList(_context.Ghes, "IdGhe", "TenGhe", ve.Ghe);
            return View(ve);
        }

        // GET: Admin/Ves/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ve = await _context.Ves.FindAsync(id);
            if (ve == null)
            {
                return NotFound();
            }
            ViewData["CaChieu"] = new SelectList(_context.CaChieus, "IdCaChieu", "IdCaChieu", ve.CaChieu);
            ViewData["Ghe"] = new SelectList(_context.Ghes, "IdGhe", "TenGhe", ve.Ghe);
            return View(ve);
        }

        // POST: Admin/Ves/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdVe,TenVe,GiaVe,CaChieu,Ghe")] Ve ve)
        {
            if (id != ve.IdVe)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ve);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VeExists(ve.IdVe))
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
            ViewData["CaChieu"] = new SelectList(_context.CaChieus, "IdCaChieu", "IdCaChieu", ve.CaChieu);
            ViewData["Ghe"] = new SelectList(_context.Ghes, "IdGhe", "TenGhe", ve.Ghe);
            return View(ve);
        }

        // GET: Admin/Ves/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ve = await _context.Ves
                .Include(v => v.CaChieus)
                .Include(v => v.Ghes)
                .FirstOrDefaultAsync(m => m.IdVe == id);
            if (ve == null)
            {
                return NotFound();
            }

            return View(ve);
        }

        // POST: Admin/Ves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ve = await _context.Ves.FindAsync(id);
            _context.Ves.Remove(ve);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VeExists(int id)
        {
            return _context.Ves.Any(e => e.IdVe == id);
        }
    }
}

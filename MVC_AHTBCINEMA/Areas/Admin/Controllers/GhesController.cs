using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AHTBCinema_NHOM4_SD18301.Models;
using ASM_AHTBCINEMA_NHOM4_SD18301.Data;
using AHTBCinema_NHOM4_SD18301.ViewModels;

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
        [HttpGet]
        public async Task<IActionResult> GetGioChieusByPhong(string phongId)
        {
            if (string.IsNullOrEmpty(phongId))
            {
                return Json(new List<SelectListItem>());
            }

            var gioChieus = await _context.GioChieus
                .Where(gc => gc.CaChieus.Phong == phongId)
                .Select(gc => new SelectListItem
                {
                    Value = gc.IdGioChieu.ToString(),
                    Text = gc.GioBatDau.ToString("hh\\:mm") + " - " + gc.GioKetThuc.ToString("hh\\:mm") + " - " + gc.CaChieus.NgayChieu.ToString("dd/MM/yyyy") + " - " + gc.CaChieus.Phims.TenPhim
                })
                .ToListAsync();

            return Json(gioChieus);
        }
        public IActionResult BulkCreate()
        {
            ViewBag.LoaiGhe = new SelectList(_context.LoaiGhes, "IdLoaiGhe", "TenLoaiGhe");
            ViewBag.Phongs = new SelectList(_context.Phongs, "IdPhong", "SoPhong");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkCreate(BulkCreateGheViewModel model)
        {
            if (ModelState.IsValid)
            {
                var lastGhe = await _context.Ghes
                    .OrderByDescending(g => g.IdGhe)
                    .FirstOrDefaultAsync();

                int seatNumber = 1;
                char seatLetter = model.StartingSeatLetter;

                if (lastGhe != null)
                {
                    string lastIdGhe = lastGhe.IdGhe;
                    // Extract the last seat number
                    int lastSeatNumber;
                    if (int.TryParse(lastIdGhe.Substring(1), out lastSeatNumber))
                    {
                        seatNumber = lastSeatNumber + 1;
                    }
                }

                for (int i = 1; i <= model.SoLuongGhe; i++)
                {
                    string idGhe = seatLetter + seatNumber.ToString("D6");
                    string tenGhe = seatLetter + seatNumber.ToString();

                    var ghe = new Ghe
                    {
                        IdGhe = idGhe,
                        TenGhe = tenGhe,
                        Phong = model.Phong,
                        TrangThai = model.TrangThai,
                        LoaiGhe = model.LoaiGhe
                    };

                    _context.Add(ghe);

                    string tenVe = "Ve " + model.Phong + " " + model.LoaiGhe;
                    float giaVe = model.GiaVe;

                    if (model.GioChieuId.HasValue)
                    {
                        var ve = new Ve
                        {
                            TenVe = tenVe,
                            GiaVe = giaVe,
                            SuatChieu = model.GioChieuId.Value, // Save GioChieuId to Ve
                            Ghe = ghe.IdGhe,
                        };

                        _context.Add(ve);
                    }

                    seatNumber++;
                    if (seatNumber > 999999)
                    {
                        seatNumber = 1;
                        seatLetter++;
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["LoaiGhe"] = new SelectList(_context.LoaiGhes, "IdLoaiGhe", "TenLoaiGhe", model.LoaiGhe);
            ViewData["Phong"] = new SelectList(_context.Phongs, "IdPhong", "SoPhong", model.Phong);
            return View(model);
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

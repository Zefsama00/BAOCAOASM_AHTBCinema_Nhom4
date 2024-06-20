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
    public class CaChieusController : Controller
    {
        private readonly DBCinemaContext _context;

        public CaChieusController(DBCinemaContext context)
        {
            _context = context;
        }

        // GET: Admin/CaChieus
        public async Task<IActionResult> Index()
        {
            var dBCinemaContext = _context.CaChieus.Include(c => c.Phims).Include(c => c.Phongs).ToList();

            // Update the status of each CaChieu based on the current date
            foreach (var caChieu in dBCinemaContext)
            {
                DateTime currentDate = DateTime.Now.Date;
                if (caChieu.NgayChieu.Date < currentDate)
                {
                    caChieu.TrangThai = "Đã chiếu";
                }
                else if (caChieu.NgayChieu.Date == currentDate)
                {
                    caChieu.TrangThai = "Đang chiếu";
                }
                else
                {
                    caChieu.TrangThai = "Chưa chiếu";
                }
                _context.Update(caChieu);
            }
            await _context.SaveChangesAsync();

            return View(dBCinemaContext);
        }

        // GET: Admin/CaChieus/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caChieu = await _context.CaChieus
                .Include(c => c.Phims)
                .Include(c => c.Phongs)
                .FirstOrDefaultAsync(m => m.IdCaChieu == id);
            if (caChieu == null)
            {
                return NotFound();
            }

            return View(caChieu);
        }

        // GET: Admin/CaChieus/Create
        public IActionResult Create()
        {
            ViewData["Phim"] = new SelectList(_context.Phims, "IdPhim", "TenPhim");
            ViewData["Phong"] = new SelectList(_context.Phongs, "IdPhong", "SoPhong");
            return View();
        }

        // POST: Admin/CaChieus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCaChieu,Phong,Phim,NgayChieu")] CaChieu caChieu)
        {
            if (ModelState.IsValid)
            {
                // Compare NgayChieu with the current date
                if (caChieu.NgayChieu.Date >= DateTime.Now.Date)
                {
                    caChieu.TrangThai = "Chưa chiếu";
                }
                else
                {
                    caChieu.TrangThai = "Hết hạn";
                }
                _context.Add(caChieu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Phim"] = new SelectList(_context.Phims, "IdPhim", "TenPhim", caChieu.Phim);
            ViewData["Phong"] = new SelectList(_context.Phongs, "IdPhong", "SoPhong", caChieu.Phong);
            return View(caChieu);
        }

        // GET: Admin/CaChieus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caChieu = await _context.CaChieus.FindAsync(id);
            if (caChieu == null)
            {
                return NotFound();
            }
            ViewData["Phim"] = new SelectList(_context.Phims, "IdPhim", "TenPhim", caChieu.Phim);
            ViewData["Phong"] = new SelectList(_context.Phongs, "IdPhong", "SoPhong", caChieu.Phong);
            return View(caChieu);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCaChieu,Phong,Phim,TrangThai")] CaChieu caChieu)
        {
            if (id != caChieu.IdCaChieu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the current CaChieu from the database
                    var currentCaChieu = await _context.CaChieus.FindAsync(id);

                    if (currentCaChieu == null)
                    {
                        return NotFound();
                    }

                    // Update only the editable fields
                    currentCaChieu.Phong = caChieu.Phong;
                    currentCaChieu.Phim = caChieu.Phim;

                    // Save changes to the database
                    _context.Update(currentCaChieu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaChieuExists(caChieu.IdCaChieu))
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

            // If ModelState is not valid, reload data and return to the view
            ViewData["Phim"] = new SelectList(_context.Phims, "IdPhim", "TenPhim", caChieu.Phim);
            ViewData["Phong"] = new SelectList(_context.Phongs, "IdPhong", "SoPhong", caChieu.Phong);
            return View(caChieu);
        }


        private bool CaChieuExists(int id)
        {
            return _context.CaChieus.Any(e => e.IdCaChieu == id);
        }
    }
}

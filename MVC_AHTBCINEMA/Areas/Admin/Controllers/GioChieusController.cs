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
            var dBCinemaContext = _context.GioChieus
                .Include(g => g.CaChieus)
                    .ThenInclude(c => c.Phongs) // Include Phong related to CaChieus
                .Include(g => g.CaChieus)
                    .ThenInclude(c => c.Phims); // Include Phim related to CaChieus

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
              .ThenInclude(c => c.Phongs)
              .Include(g => g.CaChieus)
              .ThenInclude(c => c.Phims)
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
            var caChieus = _context.CaChieus
            .Include(c => c.Phongs) // Nạp thông tin từ bảng Phongs
            .Include(c => c.Phims)  // Nạp thông tin từ bảng Phims
            .Where(c => c.TrangThai != "Hết hạn")
            .ToList()
            .Select(c => new {
                IdCaChieu = c.IdCaChieu,
                NgayChieu = $"{c.NgayChieu.ToString("dd/MM/yyyy")} - {c.Phongs.SoPhong} - {c.Phims.TenPhim}"
            })
            .ToList();

            ViewData["Cachieu"] = new SelectList(caChieus, "IdCaChieu", "NgayChieu");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdGioChieu,GioBatDau,GioKetThuc,Cachieu,TrangThai")] GioChieu gioChieu)
        {
            if (ModelState.IsValid)
            {
                var caChieu = await _context.CaChieus.FindAsync(gioChieu.Cachieu);
                if (caChieu == null)
                {
                    ModelState.AddModelError("", "Không tìm thấy CaChieu tương ứng.");
                    ViewData["Cachieu"] = new SelectList(_context.CaChieus, "IdCaChieu", "NgayChieu", gioChieu.Cachieu);
                    return View(gioChieu);
                }

                // Check if there is any GioChieu in the CaChieu that is currently screening
                bool isAnyGioChieuInCaChieuScreening = await _context.GioChieus
                    .AnyAsync(gc => gc.Cachieu == gioChieu.Cachieu && gc.TrangThai == "Đang chiếu");

                // Update CaChieu TrangThai based on GioChieu TrangThai
                if (isAnyGioChieuInCaChieuScreening)
                {
                    caChieu.TrangThai = "Đang chiếu";
                    _context.Update(caChieu);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Check if there are only "Chưa chiếu" and "Hết hạn" GioChieu in the CaChieu
                    bool hasOnlyNotStartedOrExpiredGioChieu = await _context.GioChieus
                        .AllAsync(gc => gc.Cachieu == gioChieu.Cachieu &&
                                        (gc.TrangThai == "Chưa chiếu" || gc.TrangThai == "Hết hạn"));

                    // Update CaChieu TrangThai based on GioChieu TrangThai
                    if (hasOnlyNotStartedOrExpiredGioChieu)
                    {
                        caChieu.TrangThai = "Chưa chiếu";
                        _context.Update(caChieu);
                        await _context.SaveChangesAsync();
                    }
                }

                // Get the screening start and end times
                DateTime screeningStartDateTime = caChieu.NgayChieu.Date + gioChieu.GioBatDau;
                DateTime screeningEndDateTime = caChieu.NgayChieu.Date + gioChieu.GioKetThuc;
                DateTime currentDateTime = DateTime.Now;

                // Determine GioChieu TrangThai based on current time and screening times
                if (currentDateTime >= screeningStartDateTime && currentDateTime <= screeningEndDateTime)
                {
                    gioChieu.TrangThai = "Đang chiếu";
                }
                else if (currentDateTime < screeningStartDateTime)
                {
                    gioChieu.TrangThai = "Chưa chiếu";
                }
                else
                {
                    gioChieu.TrangThai = "Hết hạn";
                }

                // Save GioChieu
                _context.Add(gioChieu);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            var caChieus = _context.CaChieus
            .Include(c => c.Phongs) // Nạp thông tin từ bảng Phongs
            .Include(c => c.Phims)  // Nạp thông tin từ bảng Phims
            .Where(c => c.TrangThai != "Hết hạn")
            .ToList()
            .Select(c => new {
                IdCaChieu = c.IdCaChieu,
                NgayChieu = $"{c.NgayChieu.ToString("dd/MM/yyyy")} - {c.Phongs.SoPhong} - {c.Phims.TenPhim}"
            })
            .ToList();

            ViewData["Cachieu"] = new SelectList(caChieus, "IdCaChieu", "NgayChieu");
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
            var caChieus = _context.CaChieus
             .Include(c => c.Phongs) // Nạp thông tin từ bảng Phongs
             .Include(c => c.Phims)  // Nạp thông tin từ bảng Phims
             .Where(c => c.TrangThai != "Hết hạn")
             .ToList()
             .Select(c => new {
                 IdCaChieu = c.IdCaChieu,
                 NgayChieu = $"{c.NgayChieu.ToString("dd/MM/yyyy")} - {c.Phongs.SoPhong} - {c.Phims.TenPhim}"
             })
             .ToList();

            ViewData["Cachieu"] = new SelectList(caChieus, "IdCaChieu", "NgayChieu");
            return View(gioChieu);
        }

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
                    // Retrieve current GioChieu and associated CaChieu from database
                    var currentGioChieu = await _context.GioChieus
                        .Include(gc => gc.CaChieus)
                        .FirstOrDefaultAsync(gc => gc.IdGioChieu == gioChieu.IdGioChieu);

                    if (currentGioChieu == null)
                    {
                        return NotFound();
                    }

                    // Update properties of current GioChieu with new values
                    currentGioChieu.GioBatDau = gioChieu.GioBatDau;
                    currentGioChieu.GioKetThuc = gioChieu.GioKetThuc;

                    // Calculate screening times
                    DateTime screeningStartDateTime = currentGioChieu.CaChieus.NgayChieu.Date + gioChieu.GioBatDau;
                    DateTime screeningEndDateTime = currentGioChieu.CaChieus.NgayChieu.Date + gioChieu.GioKetThuc;
                    DateTime currentDateTime = DateTime.Now;

                    // Determine TrangThai based on current time and screening times
                    if (currentDateTime >= screeningStartDateTime && currentDateTime <= screeningEndDateTime)
                    {
                        gioChieu.TrangThai = "Đang chiếu";
                    }
                    else if (currentDateTime > screeningEndDateTime)
                    {
                        gioChieu.TrangThai = "Hết hạn";
                    }
                    else
                    {
                        gioChieu.TrangThai = "Chưa chiếu";
                    }

                    // Update TrangThai for CaChieu based on GioChieu
                    var caChieu = currentGioChieu.CaChieus;

                    // Check if there is any GioChieu in the CaChieu that is currently screening
                    bool isAnyGioChieuInCaChieuScreening = await _context.GioChieus
                        .AnyAsync(gc => gc.Cachieu == gioChieu.Cachieu && gc.TrangThai == "Đang chiếu");

                    // Update CaChieu TrangThai based on GioChieu TrangThai
                    if (isAnyGioChieuInCaChieuScreening)
                    {
                        caChieu.TrangThai = "Đang chiếu";
                    }
                    else
                    {
                        // Check if there are only "Chưa chiếu" and "Hết hạn" GioChieu in the CaChieu
                        bool hasOnlyNotStartedOrExpiredGioChieu = await _context.GioChieus
                            .AllAsync(gc => gc.Cachieu == gioChieu.Cachieu &&
                                            (gc.TrangThai == "Chưa chiếu" || gc.TrangThai == "Hết hạn"));

                        // Update CaChieu TrangThai based on GioChieu TrangThai
                        if (hasOnlyNotStartedOrExpiredGioChieu)
                        {
                            caChieu.TrangThai = "Chưa chiếu";
                        }
                        else
                        {
                            caChieu.TrangThai = gioChieu.TrangThai;
                        }
                    }

                    // Save changes to database
                    _context.Update(caChieu);
                    _context.Update(currentGioChieu);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
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
            }

            // If ModelState is not valid, reload Cachieu dropdown
            var caChieus = _context.CaChieus
             .Include(c => c.Phongs) // Nạp thông tin từ bảng Phongs
             .Include(c => c.Phims)  // Nạp thông tin từ bảng Phims
             .Where(c => c.TrangThai != "Hết hạn")
             .ToList()
             .Select(c => new {
                 IdCaChieu = c.IdCaChieu,
                 NgayChieu = $"{c.NgayChieu.ToString("dd/MM/yyyy")} - {c.Phongs.SoPhong} - {c.Phims.TenPhim}"
             })
             .ToList();

            ViewData["Cachieu"] = new SelectList(caChieus, "IdCaChieu", "NgayChieu");
            return View(gioChieu);
        }


        private bool GioChieuExists(int id)
        {
            return _context.GioChieus.Any(e => e.IdGioChieu == id);
        }
    }
}

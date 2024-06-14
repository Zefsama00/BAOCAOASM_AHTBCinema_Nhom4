using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AHTBCinema_NHOM4_SD18301.Models;
using ASM_AHTBCINEMA_NHOM4_SD18301.Data;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace MVC_AHTBCINEMA.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PhimsController : Controller
    {
        private readonly DBCinemaContext _context;

        public PhimsController(DBCinemaContext context)
        {
            _context = context;
        }

        // GET: Admin/Phims
        public async Task<IActionResult> Index()
        {
            var dBCinemaContext = _context.Phims.Include(p => p.LoaiPhim);
            return View(await dBCinemaContext.ToListAsync());
        }

        // GET: Admin/Phims/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phim = await _context.Phims
                .Include(p => p.LoaiPhim)
                .FirstOrDefaultAsync(m => m.IdPhim == id);
            if (phim == null)
            {
                return NotFound();
            }

            return View(phim);
        }

        // GET: Admin/Phims/Create
        public IActionResult Create()
        {
            ViewData["TheLoai"] = new SelectList(_context.LoaiPhims, "IdLP", "TenLoai");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Phim phim, IFormFile hinhAnhFile)
        {
            if (ModelState.IsValid)
            {
                if (hinhAnhFile != null && hinhAnhFile.Length > 0)
                {
                    // Kiểm tra loại và kích thước tệp (nếu cần)
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(hinhAnhFile.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError(string.Empty, "Only image files (.jpg, .jpeg, .png, .gif) are allowed.");
                        ViewData["TheLoai"] = new SelectList(_context.LoaiPhims, "IdLP", "TenLoai", phim.TheLoai);
                        return View(phim);
                    }

                    // Lưu tệp vào thư mục (ví dụ: wwwroot/uploads)
                    var fileName = Path.GetRandomFileName() + Path.GetExtension(hinhAnhFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await hinhAnhFile.CopyToAsync(fileStream);
                    }

                    // Lưu tên tệp vào đối tượng phim
                    phim.HinhAnh = fileName;
                }

                _context.Add(phim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["TheLoai"] = new SelectList(_context.LoaiPhims, "IdLP", "TenLoai", phim.TheLoai);
            return View(phim);
        }


        // GET: Admin/Phims/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phim = await _context.Phims.FindAsync(id);
            if (phim == null)
            {
                return NotFound();
            }

            ViewData["TheLoai"] = new SelectList(_context.LoaiPhims, "IdLP", "TenLoai", phim.TheLoai);
            return View(phim);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Phim phim, IFormFile hinhAnhFile)
        {
            if (id != phim.IdPhim)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy thực thể từ cơ sở dữ liệu
                    var existingPhim = await _context.Phims.FindAsync(id);

                    if (existingPhim != null)
                    {
                        // Cập nhật các thuộc tính của thực thể hiện có
                        existingPhim.TenPhim = phim.TenPhim;
                        existingPhim.DienVien = phim.DienVien;
                        existingPhim.DangPhim = phim.DangPhim;
                        existingPhim.TheLoai = phim.TheLoai;
                        existingPhim.ThoiLuong = phim.ThoiLuong;

                        if (hinhAnhFile != null && hinhAnhFile.Length > 0)
                        {
                            // Kiểm tra và lưu tệp hình ảnh
                            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                            var fileExtension = Path.GetExtension(hinhAnhFile.FileName).ToLowerInvariant();

                            if (!allowedExtensions.Contains(fileExtension))
                            {
                                ModelState.AddModelError(string.Empty, "Only image files (.jpg, .jpeg, .png, .gif) are allowed.");
                                ViewData["TheLoai"] = new SelectList(_context.LoaiPhims, "IdLP", "TenLoai", phim.TheLoai);
                                return View(phim);
                            }

                            // Lưu tệp vào thư mục (ví dụ: wwwroot/uploads)
                            var fileName = Path.GetRandomFileName() + fileExtension;
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await hinhAnhFile.CopyToAsync(fileStream);
                            }

                            // Cập nhật giá trị HinhAnh của thực thể
                            existingPhim.HinhAnh = fileName;
                        }

                        // Cập nhật thực thể trong cơ sở dữ liệu
                        _context.Update(existingPhim);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhimExists(phim.IdPhim))
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

            ViewData["TheLoai"] = new SelectList(_context.LoaiPhims, "IdLP", "TenLoai", phim.TheLoai);
            return View(phim);
        }

        private bool PhimExists(string id)
        {
            return _context.Phims.Any(e => e.IdPhim == id);
        }

        // GET: Admin/Phims/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var phim = await _context.Phims
                .Include(p => p.LoaiPhim)
                .FirstOrDefaultAsync(m => m.IdPhim == id);
            if (phim == null)
            {
                return NotFound();
            }

            return View(phim);
        }

        // POST: Admin/Phims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var phim = await _context.Phims.FindAsync(id);
            _context.Phims.Remove(phim);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

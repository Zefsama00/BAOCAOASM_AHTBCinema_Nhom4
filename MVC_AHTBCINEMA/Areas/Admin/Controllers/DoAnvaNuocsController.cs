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
    public class DoAnvaNuocsController : Controller
    {
        private readonly DBCinemaContext _context;

        public DoAnvaNuocsController(DBCinemaContext context)
        {
            _context = context;
        }

        // GET: Admin/DoAnvaNuocs
        public async Task<IActionResult> Index()
        {
            return View(await _context.DoAnvaNuocs.ToListAsync());
        }

        // GET: Admin/DoAnvaNuocs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doAnvaNuoc = await _context.DoAnvaNuocs
                .FirstOrDefaultAsync(m => m.IdComBo == id);
            if (doAnvaNuoc == null)
            {
                return NotFound();
            }

            return View(doAnvaNuoc);
        }

        // GET: Admin/DoAnvaNuocs/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DoAnvaNuoc doAnvaNuoc, IFormFile hinhanhFile)
        {
            if (ModelState.IsValid)
            {
                if (hinhanhFile != null && hinhanhFile.Length > 0)
                {
                    // Kiểm tra loại và kích thước tệp (nếu cần)
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var fileExtension = Path.GetExtension(hinhanhFile.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        ModelState.AddModelError(string.Empty, "Only image files (.jpg, .jpeg, .png, .gif) are allowed.");
                        return View(doAnvaNuoc);
                    }

                    // Lưu tệp vào thư mục (ví dụ: wwwroot/uploads)
                    var fileName = Path.GetRandomFileName() + Path.GetExtension(hinhanhFile.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await hinhanhFile.CopyToAsync(fileStream);
                    }

                    // Lưu tên tệp vào đối tượng doAnvaNuoc
                    doAnvaNuoc.Hinhanh = fileName;
                }

                _context.Add(doAnvaNuoc);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(doAnvaNuoc);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doAnvaNuoc = await _context.DoAnvaNuocs.FindAsync(id);
            if (doAnvaNuoc == null)
            {
                return NotFound();
            }
            return View(doAnvaNuoc);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdComBo,TenCombo,Hinhanh,Gia")] DoAnvaNuoc doAnvaNuoc, IFormFile hinhanhFile)
        {
            if (id != doAnvaNuoc.IdComBo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy thực thể từ cơ sở dữ liệu
                    var existingDoAnvaNuoc = await _context.DoAnvaNuocs.FindAsync(id);

                    if (existingDoAnvaNuoc != null)
                    {
                        // Cập nhật các thuộc tính của thực thể hiện có
                        existingDoAnvaNuoc.TenCombo = doAnvaNuoc.TenCombo;
                        existingDoAnvaNuoc.Gia = doAnvaNuoc.Gia;

                        if (hinhanhFile != null && hinhanhFile.Length > 0)
                        {
                            // Kiểm tra và lưu tệp hình ảnh
                            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                            var fileExtension = Path.GetExtension(hinhanhFile.FileName).ToLowerInvariant();

                            if (!allowedExtensions.Contains(fileExtension))
                            {
                                ModelState.AddModelError(string.Empty, "Only image files (.jpg, .jpeg, .png, .gif) are allowed.");
                                return View(doAnvaNuoc);
                            }

                            // Lưu tệp vào thư mục (ví dụ: wwwroot/uploads)
                            var fileName = Path.GetRandomFileName() + fileExtension;
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                await hinhanhFile.CopyToAsync(fileStream);
                            }

                            // Cập nhật giá trị Hinhanh của thực thể
                            existingDoAnvaNuoc.Hinhanh = fileName;
                        }

                        // Cập nhật thực thể trong cơ sở dữ liệu
                        _context.Update(existingDoAnvaNuoc);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoAnvaNuocExists(doAnvaNuoc.IdComBo))
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
            return View(doAnvaNuoc);
        }

        // GET: Admin/DoAnvaNuocs/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var item = await _context.DoAnvaNuocs.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.DoAnvaNuocs.Remove(item);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private bool DoAnvaNuocExists(string id)
        {
            return _context.DoAnvaNuocs.Any(e => e.IdComBo == id);
        }
    }
}

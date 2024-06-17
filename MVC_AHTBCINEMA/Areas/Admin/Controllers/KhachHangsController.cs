using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AHTBCinema_NHOM4_SD18301.Models;
using ASM_AHTBCINEMA_NHOM4_SD18301.Data;
using API_AHTBCINEMA.Models;

namespace MVC_AHTBCINEMA.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class KhachHangsController : Controller
    {
        private readonly DBCinemaContext _context;

        public KhachHangsController(DBCinemaContext context)
        {
            _context = context;
        }

        // GET: Admin/KhachHangs
        public async Task<IActionResult> Index()
        {
            return View(await _context.KhachHangs.ToListAsync());
        }

        // GET: Admin/KhachHangs/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(m => m.IdKH == id);
            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // GET: Admin/KhachHangs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/KhachHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdKH,TenKH,SDT,NamSinh,Email,Password")] KhachHang khachHang)
        {
            if (ModelState.IsValid)
            {
                _context.Add(khachHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(khachHang);
        }

        // GET: Admin/KhachHangs/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs.FindAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }
            return View(khachHang);
        }

        // POST: Admin/KhachHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdKH,TenKH,SDT,NamSinh,Email,Password")] KhachHang khachHang, [Bind("IdUser,Username,PassWord,Role")] User user)
        {
            if (id != khachHang.IdKH)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Find the existing customer
                    var existingKhachHang = await _context.KhachHangs.FindAsync(id);
                    if (existingKhachHang == null)
                    {
                        return NotFound();
                    }

                    // Update the customer's details
                    existingKhachHang.TenKH = khachHang.TenKH;
                    existingKhachHang.SDT = khachHang.SDT;
                    existingKhachHang.NamSinh = khachHang.NamSinh;
                    existingKhachHang.Email = khachHang.Email;
                    existingKhachHang.Password = khachHang.Password;

                    _context.Update(existingKhachHang);

                    // Find the existing user
                    var existingUser = await _context.Users.FindAsync(existingKhachHang.IdKH);
                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    // Update the user's details
                    string usernameFromEmail = khachHang.Email.Substring(0, khachHang.Email.IndexOf("@"));
                    existingUser.Username = usernameFromEmail;
                    existingUser.PassWord = khachHang.Password;
                    existingUser.Role = user.Role; // Assuming you might want to update the role as well

                    _context.Update(existingUser);

                    // Save changes to the database
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Handle exceptions if necessary
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu dữ liệu. Vui lòng thử lại sau.");
                    return View(khachHang); // Return to the view to display the information and errors
                }
            }
            return View(khachHang);
        }


        // GET: Admin/KhachHangs/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(m => m.IdKH == id);
            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // POST: Admin/KhachHangs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var khachHang = await _context.KhachHangs.FindAsync(id);
            _context.KhachHangs.Remove(khachHang);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KhachHangExists(string id)
        {
            return _context.KhachHangs.Any(e => e.IdKH == id);
        }
    }
}

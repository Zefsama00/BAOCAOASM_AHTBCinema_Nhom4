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
    public class NhanViensController : Controller
    {
        private readonly DBCinemaContext _context;

        public NhanViensController(DBCinemaContext context)
        {
            _context = context;
        }

        // GET: Admin/NhanViens
        public async Task<IActionResult> Index()
        {
            return View(await _context.NhanViens.ToListAsync());
        }

        // GET: Admin/NhanViens/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens
                .FirstOrDefaultAsync(m => m.IdNV == id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            return View(nhanVien);
        }

        // GET: Admin/NhanViens/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/NhanViens/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdNV,TenNV,SDT,NamSinh,Email,Password,ChucVu,TrangThai")] NhanVien nhanVien, [Bind("IdUser,Username,PassWord,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var demnv = _context.NhanViens.Count() + 1;
                    nhanVien.IdNV = "NV" + demnv.ToString();
                    nhanVien.TrangThai = "Hoạt động";
                    // Lưu nhân viên vào database
                    _context.NhanViens.Add(nhanVien);
                    int add = await _context.SaveChangesAsync();
                    string usernameFromEmail = nhanVien.Email.Substring(0, nhanVien.Email.IndexOf("@"));

                    // Sau khi lưu thành công nhân viên, tạo người dùng
                    if (add > 0)
                    {
                        user.IdUser = nhanVien.IdNV;
                        user.Username = usernameFromEmail;
                        user.PassWord = nhanVien.Password;
                        user.Role = "nhanvien"; // Assuming the role is "nhanvien"
                        _context.Users.Add(user);
                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ nếu cần thiết
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu dữ liệu. Vui lòng thử lại sau.");
                    return View(nhanVien); // Quay lại view để hiển thị thông tin và lỗi
                }
            }
            return View(nhanVien);
        }
        // GET: Admin/NhanViens/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            return View(nhanVien);
        }

        // POST: Admin/NhanViens/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdNV,TenNV,SDT,NamSinh,Email,Password,ChucVu,TrangThai")] NhanVien nhanVien, [Bind("IdUser,Username,PassWord,Role")] User user)
        {
            if (id != nhanVien.IdNV)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Find the existing employee
                    var existingNhanVien = await _context.NhanViens.FindAsync(id);
                    if (existingNhanVien == null)
                    {
                        return NotFound();
                    }

                    // Update the employee's details
                    existingNhanVien.TenNV = nhanVien.TenNV;
                    existingNhanVien.SDT = nhanVien.SDT;
                    existingNhanVien.NamSinh = nhanVien.NamSinh;
                    existingNhanVien.Email = nhanVien.Email;
                    existingNhanVien.Password = nhanVien.Password;
                    existingNhanVien.ChucVu = nhanVien.ChucVu;
                    existingNhanVien.TrangThai = nhanVien.TrangThai;

                    _context.Update(existingNhanVien);

                    // Find the existing user
                    var existingUser = await _context.Users.FindAsync(existingNhanVien.IdNV);
                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    // Update the user's details
                    string usernameFromEmail = nhanVien.Email.Substring(0, nhanVien.Email.IndexOf("@"));
                    existingUser.Username = usernameFromEmail;
                    existingUser.PassWord = nhanVien.Password;
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
                    return View(nhanVien); // Return to the view to display the information and errors
                }
            }
            return View(nhanVien);
        }

        // GET: Admin/NhanViens/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens
                .FirstOrDefaultAsync(m => m.IdNV == id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            return View(nhanVien);
        }

        // POST: Admin/NhanViens/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var nhanVien = await _context.NhanViens.FindAsync(id);
            _context.NhanViens.Remove(nhanVien);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NhanVienExists(string id)
        {
            return _context.NhanViens.Any(e => e.IdNV == id);
        }
    }
}

using AHTBCinema_NHOM4_SD18301.Models;
using ASM_AHTBCINEMA_NHOM4_SD18301.Data;
using API_AHTBCINEMA.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC_AHTBCINEMA.Model;
using System.Linq;
using System.Threading.Tasks;
using API_AHTBCINEMA.Models;
using System;
using MVC_ASM_AHTBCinema_NHOM4_SD18301.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;



namespace MVC_AHTBCINEMA.Controllers
{
    public class DangNhapController : Controller
    {
        private readonly DBCinemaContext _context;

        public DangNhapController(DBCinemaContext context)
        {
            _context = context;
        }
        private string GetMd5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Username") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Multimodel");
            }
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            string hashedPassword = "";
            if (user.PassWord != null)
            {
                 hashedPassword = GetMd5Hash(user.PassWord);
            }
         
            var u = _context.Users.FirstOrDefault(x => x.Username == user.Username && x.PassWord == hashedPassword);

            if (u != null)
            {
                if (u.Role == "nhanvien" || u.Role == "admin")
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }

                HttpContext.Session.SetString("Username", u.Username.ToString());
                return RedirectToAction("Index", "Multimodel");
            }
            else
            {
                TempData["ErroLoginMessage"] = "Tài khoản hoặc mật khẩu không chính xác.";
                ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không chính xác.");
            }

            return View();
        }



        public ActionResult Logout()
        {
            HttpContext.Session.Clear(); // Xóa session
            return RedirectToAction("Login");
        }

        // GET: Admin/KhachHangs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/KhachHangs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdKH,TenKH,SDT,NamSinh,Email,Password")] KhachHang khachHang, [Bind("IdUser,Username,PassWord,Role")] User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var demkh = _context.KhachHangs.Count() + 1;
                    khachHang.IdKH = "KH" + demkh.ToString();
                    khachHang.TrangThai = "Hoạt động";

                    // Mã hóa mật khẩu trước khi lưu vào database
                    khachHang.Password = GetMd5Hash(khachHang.Password);

                    _context.KhachHangs.Add(khachHang);
                    int add = await _context.SaveChangesAsync();

                    string catchuoi = khachHang.Email.Substring(0, khachHang.Email.IndexOf("@"));

                    if (add > 0)
                    {
                        user.IdUser = khachHang.IdKH;
                        user.Username = catchuoi;
                        user.PassWord = khachHang.Password;
                        user.Role = "User";
                        _context.Users.Add(user);
                        await _context.SaveChangesAsync();
                    }

                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu dữ liệu. Vui lòng thử lại sau.");
                    return View(khachHang);
                }
            }
            HttpContext.Session.Clear();
            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmNewPassword, string usernameweb)
        {
            try
            {
                // Lấy thông tin người dùng từ session hoặc principal
                string username = usernameweb;

                var user = await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
                if (user == null)
                {
                    ModelState.AddModelError("", "Không tìm thấy người dùng.");
                    return View();
                }

                // Kiểm tra mật khẩu hiện tại
                if (user.PassWord != GetMd5Hash(currentPassword))
                {
                    ModelState.AddModelError("", "Mật khẩu hiện tại không đúng.");
                    return View();
                }

                // Kiểm tra mật khẩu mới và xác nhận mật khẩu mới
                if (newPassword != confirmNewPassword)
                {
                    ModelState.AddModelError("", "Mật khẩu mới và xác nhận mật khẩu không khớp.");
                    return View();
                }

                // Cập nhật mật khẩu cho User
                user.PassWord = GetMd5Hash(newPassword);
                _context.Users.Update(user);

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";

                return RedirectToAction("Index", "Multimodel");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi khi thay đổi mật khẩu. Vui lòng thử lại sau.");
                return View();
            }
        }


    }
}

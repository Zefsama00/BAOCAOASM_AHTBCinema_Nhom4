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

namespace MVC_AHTBCINEMA.Controllers
{
    public class DangNhapController : Controller
    {
        private readonly DBCinemaContext _context;
        public  PasswordHasher _passwordHasher;
        public DangNhapController(DBCinemaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Email") == null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Multimodel");
            }
        }
        [HttpPost]
        public IActionResult Login(KhachHang user)
        {
          
            if (HttpContext.Session.GetString("Email") == null)
            {
              

                var u = _context.KhachHangs.Where(x => x.Email.Equals(user.Email)
                && x.Password.Equals(user.Password)).FirstOrDefault();

                if(user.Password == "admin" && user.Email == "admin")
                {
                    if(u.Role == "nhanvien" || u.Role == "admin")
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    HttpContext.Session.SetString("Username", u.Username.ToString());
                   
                     return RedirectToAction("Index", "Multimodel");
                }
                else
                {
                    return RedirectToAction("Index", "Multimodel");
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();//remove session
            return RedirectToAction("Login");
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
                try
                {
                    var demkh = _context.KhachHangs.Count() + 1;
                    khachHang.IdKH = "KH" + demkh.ToString();
                    khachHang.TrangThai = "Hoạt động";
                    // Lưu khách hàng vào database
                    _context.KhachHangs.Add(khachHang);
                    int add = await _context.SaveChangesAsync();
                    string catchuoi = khachHang.Email.Substring(0, khachHang.Email.IndexOf("@"));
                    // Sau khi lưu thành công khách hàng, tạo người dùng
                    if(add > 0) 
                    {
                        user.IdUser = khachHang.IdKH;
                        user.Username = catchuoi;
                        user.PassWord = khachHang.Password;
                        user.Role = "User";
                        _context.Users.Add(user);
                        await _context.SaveChangesAsync();
                    }
                    // Thêm người dùng vào database
                   

                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    // Xử lý ngoại lệ nếu cần thiết
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi lưu dữ liệu. Vui lòng thử lại sau.");
                    return View(khachHang); // Quay lại view để hiển thị thông tin và lỗi
                }
            }
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Multimodel");
        }
    }
}

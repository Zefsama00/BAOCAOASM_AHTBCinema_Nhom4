using AHTBCinema_NHOM4_SD18301.Models;
using ASM_AHTBCINEMA_NHOM4_SD18301.Data;
using API_AHTBCINEMA.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC_AHTBCINEMA.Model;
using System.Linq;
using System.Threading.Tasks;

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
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                if (user.Password == "844265" && user.Email == "Duydeptrai@gmail.com")
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                if (u != null)
                {
                    HttpContext.Session.SetString("Email", u.Email.ToString());
                   
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
         
                _context.KhachHangs.Add(khachHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Multimodel");
        }
    }
}

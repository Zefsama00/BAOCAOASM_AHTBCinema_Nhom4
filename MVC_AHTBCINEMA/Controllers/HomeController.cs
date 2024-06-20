using AHTBCinema_NHOM4_SD18301.Models;
using ASM_AHTBCINEMA_NHOM4_SD18301.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MVC_AHTBCINEMA.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MVC_AHTBCINEMA.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DBCinemaContext _context;
        public HomeController(ILogger<HomeController> logger, DBCinemaContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DetailsFilm()
        {
            return View();
        }

        public IActionResult GioiThieu()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Pay()
        {
            return View();
        }

        public IActionResult PhimDangChieu()
        {
            var phimDangChieu = _context.CaChieus
                .Where(x => x.TrangThai == "Chưa chiếu")
                .Include(x => x.Phims)
                .Select(x => x.Phims) 
                .ToList();

            return View(phimDangChieu);
        }



        public IActionResult Resgister()
        {
            return View();
        }

        public IActionResult Uudai()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

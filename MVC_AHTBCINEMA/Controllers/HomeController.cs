using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC_AHTBCINEMA.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_AHTBCINEMA.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
            return View();
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

using Microsoft.AspNetCore.Mvc;

namespace MVC_ASM_AHTBCinema_NHOM4_SD18301.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}

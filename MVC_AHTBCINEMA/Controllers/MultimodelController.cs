using AHTBCinema_NHOM4_SD18301.Models;
using ASM_AHTBCINEMA_NHOM4_SD18301.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_AHTBCINEMA.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_AHTBCINEMA.Controllers
{
    public class MultimodelController : Controller
    {
        private readonly DBCinemaContext _context;

        public MultimodelController(DBCinemaContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var loaiphim = _context.Phims.Select(x => x.TheLoai).ToString();
            var phimlist = _context.Phims.ToList();
            var theloailist = _context.LoaiPhims.Where(x => x.IdLP == loaiphim);
            var cachieulist = _context.CaChieus.ToList();
            var ghelist = _context.Ghes.ToList();
            var viewModel = new Multimodel
            {
                Phim = phimlist,
                CaChieu = cachieulist,
                Ghe = ghelist
            };
            return View(viewModel);
        }
      
        public IActionResult Details(string id) 
        {

            var phimlist =  _context.Phims.Where(m => m.IdPhim == id);
            var cachieulist = _context.CaChieus.ToList();
            var ghelist = _context.Ghes.ToList();
            var viewModel = new Multimodel
            {
                Phim = phimlist,
                CaChieu = cachieulist,
                Ghe = ghelist
            };
            return View(viewModel);
        }
    }
}

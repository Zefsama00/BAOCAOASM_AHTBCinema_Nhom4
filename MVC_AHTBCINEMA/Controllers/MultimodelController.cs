using AHTBCinema_NHOM4_SD18301.Models;
using ASM_AHTBCINEMA_NHOM4_SD18301.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_AHTBCINEMA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
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
        [HttpGet("ThanhToan/{id}")]
        public IActionResult ThanhToan(string id, string idphim)
        {
            // Tìm vé dựa trên id ghế
            var ve = _context.Ves.FirstOrDefault(x => x.Ghe == id);

            // Kiểm tra nếu không tìm thấy vé
            if (ve == null)
            {
                return NotFound(); // Trả về mã lỗi HTTP 404 (Not Found)
            }

            // Lấy thông tin ca chiếu dựa trên vé vừa tìm được
            var cachieu = _context.CaChieus
                .Include(c => c.Phims) // Đảm bảo load thông tin phim
                .FirstOrDefault(x => x.IdCaChieu == ve.CaChieu);

            // Kiểm tra xem idphim từ URL có khớp với Phim của ca chiếu không
            if (idphim == cachieu.Phim)
            {
                var viewModel = new Multimodel
                {
                    CaChieu = new List<CaChieu> { cachieu }, // Gán trực tiếp cachieu vào CaChieu
                    Ghe = _context.Ghes.Where(g => g.IdGhe == id).ToList(),
                    Ve = new List<Ve> { ve }
                };

                return View(viewModel); // Trả về view với viewModel đã tạo
            }

            // Nếu không khớp idphim, trả về lỗi Not Found
            return NotFound(); // Hoặc xử lý lỗi khác tùy vào logic của bạn
        }
    }
}

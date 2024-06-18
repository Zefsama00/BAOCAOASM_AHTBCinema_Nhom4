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
        [HttpGet]
        public JsonResult LoadSeats(int id)
        {
            // Lấy danh sách các vé có SuatChieuId trùng với id
            var veList = _context.Ves.Where(v => v.SuatChieu == id).ToList();

            // Lấy danh sách các IdGhe từ danh sách vé
            var gheIds = veList.Select(v => v.Ghe).ToList();

            // Lấy danh sách tên ghế dựa trên các IdGhe
            var seatNames = _context.Ghes
                .Where(g => gheIds.Contains(g.IdGhe))
                .Select(g => g.TenGhe)
                .ToList();

            return Json(seatNames);
        }
        public IActionResult Index()
        {
            var phimlist = _context.Phims.ToList();
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
            var phim = _context.Phims
                .Include(p => p.LoaiPhim)
                .SingleOrDefault(m => m.IdPhim == id);

            if (phim == null)
            {
                return NotFound();
            }

            var cachieulist = _context.CaChieus
                .Include(c => c.Phongs)
                .Include(c => c.GioChieus)
                .Where(c => c.Phim == id)
                .ToList();

            var ghelist = _context.Ghes.ToList();

            var suggestedMovies = _context.Phims
                .Where(p => p.TheLoai == phim.TheLoai && p.IdPhim != phim.IdPhim)
                .Take(4) // Limiting to 4 suggestions
                .ToList();

            var viewModel = new Multimodel
            {
                Phim = new List<Phim> { phim },
                CaChieu = cachieulist,
                Ghe = ghelist,
                SuggestedMovies = suggestedMovies
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
            var cachieu = _context.GioChieus
                .Include(c => c.CaChieus) // Đảm bảo load thông tin phim
                .FirstOrDefault(x => x.IdGioChieu == ve.SuatChieu);

            // Kiểm tra xem idphim từ URL có khớp với Phim của ca chiếu không
            if (idphim == cachieu.CaChieus.Phim)
            {
                var viewModel = new Multimodel
                {
                    CaChieu = new List<CaChieu> { cachieu.CaChieus }, // Gán trực tiếp cachieu vào CaChieu
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
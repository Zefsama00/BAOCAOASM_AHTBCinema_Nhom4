using AHTBCinema_NHOM4_SD18301.Models;
using API_AHTBCINEMA.Models;
using ASM_AHTBCINEMA_NHOM4_SD18301.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using MVC_AHTBCINEMA.Model;
using System.Collections.Generic;
using System.Linq;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

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
                .Select(g => new { Id = g.IdGhe, Name = g.TenGhe }) // Chỉ lấy Id và Name của ghế
                .ToList();

            return Json(seatNames);
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

            var suggestedMovies = _context.Phims
                .Where(p => p.TheLoai == phim.TheLoai && p.IdPhim != phim.IdPhim)
                .Take(4) // Limiting to 4 suggestions
                .ToList();

            var viewModel = new Multimodel
            {
                Phim = phimlist,
                CaChieu = cachieulist,
                Ghe = ghelist
            };
            return View(viewModel);
        }
        public IActionResult ThanhToan(string id, string idphim, int gioChieuId , string username)
        {
            // Tìm vé dựa trên id ghế
            var ve = _context.Ves.FirstOrDefault(x => x.Ghe == id);
            var phim = _context.Phims.FirstOrDefault(f => f.IdPhim == idphim);
            // Lấy thông tin ca chiếu dựa trên gioChieuId
            var cachieu = _context.GioChieus
                .Include(c => c.CaChieus) // Đảm bảo load thông tin phim
                .FirstOrDefault(x => x.IdGioChieu == gioChieuId);
            var userid = _context.Users.Where(x=>x.Username == username).FirstOrDefault();

            //GioChieu
            var giochieu = _context.GioChieus.FirstOrDefault(n => n.IdGioChieu == gioChieuId);
            var khuyenmai = 1;
            var ngaykhuyenmai = cachieu.CaChieus.NgayChieu.DayOfWeek;
            if(ngaykhuyenmai == DayOfWeek.Thursday || ngaykhuyenmai == DayOfWeek.Tuesday) 
            {
                khuyenmai = 1;
            }
            else
            {
                khuyenmai = 0;
            }
            var phantramkm = _context.KhuyenMais.FirstOrDefault(x=>x.IdKM == khuyenmai);
            var phantram = 0;
            if(phantramkm != null)
            {
                phantram = phantramkm.Phantram;
            }
            var taohoadon = new HoaDon
            {
                
                IdVe = ve.IdVe,
                Combo = "CB1",
                NhanVien = "NV1",
                KhachHang = userid.IdUser,
                KhuyenMai = khuyenmai,
                TongTien = ve.GiaVe - (ve.GiaVe * phantram/100),
                TrangThai = "Đang chờ duyệt"
            };
           
            // Chuẩn bị ViewModel để gửi tới view
            var viewModel = new Multimodel
            {
                CaChieu = new List<CaChieu> { cachieu.CaChieus },
                Ghe = _context.Ghes.Where(g => g.IdGhe == id).ToList(),
                GioChieu = new List<GioChieu> {giochieu },
                Ve = new List<Ve> { ve },
                Phim = new List<Phim> { phim },
            };

            return View(viewModel); // Trả về view với viewModel đã tạo
        }
    }
}

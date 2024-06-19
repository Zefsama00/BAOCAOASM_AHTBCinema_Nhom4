using AHTBCinema_NHOM4_SD18301.Models;
using API_AHTBCINEMA.Models;
using ASM_AHTBCINEMA_NHOM4_SD18301.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using MVC_AHTBCINEMA.Model;
using System;
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
                .Select(g => new { Id = g.IdGhe, Name = g.TenGhe, trangThai = g.TrangThai }) // Đảm bảo trangThai viết đúng
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
                .Take(4)
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
            int khuyenmai = 2;
            DayOfWeek ngaykhuyenmai = cachieu.CaChieus.NgayChieu.DayOfWeek;

            if (ngaykhuyenmai == DayOfWeek.Thursday || ngaykhuyenmai == DayOfWeek.Tuesday)
            {
                khuyenmai = 1; // Giảm giá 1
            }
      
            // Chuẩn bị ViewModel để gửi tới view
            var viewModel = new Multimodel
            {
                CaChieu = new List<CaChieu> { cachieu.CaChieus },
                Ghe = _context.Ghes.Where(g => g.IdGhe == id).ToList(),
                GioChieu = new List<GioChieu> {giochieu },
                Ve = new List<Ve> { ve },
                Phim = new List<Phim> { phim },
                KhuyenMai = _context.KhuyenMais.Where(g => g.IdKM == khuyenmai).ToList(),
               
            };

            return View(viewModel); // Trả về view với viewModel đã tạo
        }
        [HttpPost]
        public IActionResult CreateInvoice(string id, string idphim, int gioChieuId, string username)
        {
            // Tìm vé dựa trên id ghế
            var ve = _context.Ves.FirstOrDefault(x => x.Ghe == id);
            var phim = _context.Phims.FirstOrDefault(f => f.IdPhim == idphim);
            // Lấy thông tin ca chiếu dựa trên gioChieuId
            var cachieu = _context.GioChieus
                .Include(c => c.CaChieus) // Đảm bảo load thông tin phim
                .FirstOrDefault(x => x.IdGioChieu == gioChieuId);
            var userid = _context.Users.Where(x => x.Username == username).FirstOrDefault();

            // Giả sử logic tính khuyến mãi ở đây, bạn có thể thay đổi logic tùy vào yêu cầu thực tế
            int khuyenmai = 2;

            DayOfWeek ngaykhuyenmai = cachieu.CaChieus.NgayChieu.DayOfWeek;

            if (ngaykhuyenmai == DayOfWeek.Thursday || ngaykhuyenmai == DayOfWeek.Tuesday)
            {
                khuyenmai = 1; // Giảm giá 1
            }
            var phantramkm = _context.KhuyenMais.FirstOrDefault(x => x.IdKM == khuyenmai);
            var phantram = 0;
            if (phantramkm != null)
            {
                phantram = phantramkm.Phantram;
            }

            // Tạo hóa đơn mới
            var taohoadon = new HoaDon
            {
                IdVe = ve.IdVe,
                NhanVien = "NV1", // Giả sử bạn có mã nhân viên cố định
                KhachHang = userid.IdUser,
                KhuyenMai = khuyenmai,
                TongTien = ve.GiaVe - (ve.GiaVe * phantram / 100), // Tính tổng tiền sau khi áp dụng khuyến mãi
                TrangThai = "Đang chờ duyệt"
            };

            // Lưu hóa đơn vào cơ sở dữ liệu
            _context.HoaDons.Add(taohoadon);
            _context.SaveChanges();

            // Chuẩn bị ViewModel để gửi tới view
            var viewModel = new Multimodel
            {
                CaChieu = new List<CaChieu> { cachieu.CaChieus },
                Ghe = _context.Ghes.Where(g => g.IdGhe == id).ToList(),
                GioChieu = new List<GioChieu> { cachieu },
                Ve = new List<Ve> { ve },
                Phim = new List<Phim> { phim },
                KhuyenMai = _context.KhuyenMais.Where(g => g.IdKM == khuyenmai).ToList(),
            };

            return View("ThanhToan", viewModel); // Trả về view "ThanhToan" với viewModel đã tạo
        }

    }
}

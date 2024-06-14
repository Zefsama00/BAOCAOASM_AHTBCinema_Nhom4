﻿using AHTBCinema_NHOM4_SD18301.Models;
using API_AHTBCINEMA.Models;
using Microsoft.EntityFrameworkCore;

namespace ASM_AHTBCINEMA_NHOM4_SD18301.Data
{
    public class DBCinemaContext  : DbContext
    {
        public DbSet<CaChieu> CaChieus { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<GioChieu> GioChieus { get; set; }
        public DbSet<DoAnvaNuoc> DoAnvaNuocs { get; set; }
        public DbSet<Ghe> Ghes { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<KhuyenMai> KhuyenMais { get; set; }
        public DbSet<KhachHang> KhachHangs { get; set; }
        public DbSet<LoaiGhe> LoaiGhes { get; set; }
        public DbSet<LoaiPhim> LoaiPhims { get; set; }
        public DbSet<NhanVien> NhanViens { get; set; }
        public DbSet<Phim> Phims { get; set; }
        public DbSet<Phong> Phongs { get; set; }
        public DbSet<Ve> Ves { get; set; }
        public DBCinemaContext(DbContextOptions<DBCinemaContext> options) : base(options)
        { 

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CaChieu>().HasKey(p => p.IdCaChieu);
            modelBuilder.Entity<User>().HasKey(p => p.IdUser);
            modelBuilder.Entity<GioChieu>().HasKey(p => p.IdGioChieu);
            modelBuilder.Entity<DoAnvaNuoc>().HasKey(c => c.IdComBo);
            modelBuilder.Entity<Ghe>().HasKey(p => p.IdGhe);
            modelBuilder.Entity<HoaDon>().HasKey(c => c.IdHD);
            modelBuilder.Entity<KhachHang>().HasKey(p => p.IdKH);
            modelBuilder.Entity<LoaiGhe>().HasKey(c => c.IdLoaiGhe);
            modelBuilder.Entity<LoaiPhim>().HasKey(p => p.IdLP);
            modelBuilder.Entity<NhanVien>().HasKey(c => c.IdNV);
            modelBuilder.Entity<Phim>().HasKey(p => p.IdPhim);
            modelBuilder.Entity<Phong>().HasKey(c => c.IdPhong);
            modelBuilder.Entity<Ve>().HasKey(p => p.IdVe);
            modelBuilder.Entity<KhuyenMai>().HasKey(p => p.IdKM);

            modelBuilder.Entity<Ghe>()
                .HasOne(p => p.Phongs)
                .WithMany(c => c.Ghes)
                .HasForeignKey(p => p.Phong);
            modelBuilder.Entity<Ghe>()
             .HasOne(p => p.LoaiGhes)
             .WithMany(c => c.Ghes)
             .HasForeignKey(p => p.LoaiGhe);
            modelBuilder.Entity<Phim>()
              .HasOne(p => p.LoaiPhim)
              .WithMany(c => c.Phims)
              .HasForeignKey(p => p.TheLoai);
            modelBuilder.Entity<CaChieu>()
              .HasOne(p => p.Phims)
              .WithMany(c => c.CaChieus)
              .HasForeignKey(p => p.Phim);
            modelBuilder.Entity<GioChieu>()
          .HasOne(p => p.CaChieus)
          .WithMany(c => c.GioChieus)
          .HasForeignKey(p => p.Cachieu);
            modelBuilder.Entity<CaChieu>()
              .HasOne(p => p.Phongs)
              .WithMany(c => c.CaChieu)
              .HasForeignKey(p => p.Phong);
            modelBuilder.Entity<Ve>()
              .HasOne(p => p.CaChieus)
              .WithMany(c => c.Ves)
              .HasForeignKey(p => p.CaChieu);
            modelBuilder.Entity<HoaDon>()
              .HasOne(p => p.KhachHangs)
              .WithMany(c => c.HoaDons)
              .HasForeignKey(p => p.KhachHang);
            modelBuilder.Entity<HoaDon>()
           .HasOne(p => p.KhuyenMais)
           .WithMany(c => c.HoaDons)
           .HasForeignKey(p => p.KhuyenMai);
            modelBuilder.Entity<HoaDon>()
              .HasOne(p => p.NhanViens)
              .WithMany(c => c.HoaDons)
              .HasForeignKey(p => p.NhanVien);
            modelBuilder.Entity<HoaDon>()
              .HasOne<Ve>(p => p.Ve)
              .WithMany(c => c.HoaDons)
              .HasForeignKey(p => p.IdVe);
            modelBuilder.Entity<HoaDon>()
              .HasOne<DoAnvaNuoc>(p => p.Combos)
              .WithMany(c => c.HoaDons)
              .HasForeignKey(p => p.Combo);
            modelBuilder.Entity<Ve>()
              .HasOne<Ghe>(p=>p.Ghes)
              .WithOne(c => c.Ves);
              
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System;

namespace API_AHTBCINEMA.Models
{
    public class KhachHangVM
    {
        public string IdKH { get; set; }
      
        public string TenKH { get; set; }
      
        public string SDT { get; set; }
       
        public DateTime NamSinh { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
    }
}

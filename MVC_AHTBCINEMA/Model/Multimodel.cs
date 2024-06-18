using AHTBCinema_NHOM4_SD18301.Models;
using System.Collections.Generic;

namespace MVC_AHTBCINEMA.Model
{
    public class Multimodel
    {
        public IEnumerable<Phim> Phim { get; set; }
        public IEnumerable<Ghe> Ghe { get; set; }
        public IEnumerable<CaChieu> CaChieu { get; set; }
        public List<Phim> SuggestedMovies { get; internal set; }
    }
}

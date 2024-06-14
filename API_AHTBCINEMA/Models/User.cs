using System.ComponentModel.DataAnnotations;

namespace API_AHTBCINEMA.Models
{
    public class User
    {
        [Key]
        public string IdUser { get; set; }
        public string Email { get; set; }
        public string PassWord { get; set; }
    }
}

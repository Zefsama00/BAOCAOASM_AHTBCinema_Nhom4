using System.ComponentModel.DataAnnotations;

namespace API_AHTBCINEMA.Models
{
    public class User
    {
        [Key]
        public string IdUser { get; set; }
        public string Username { get; set; }
        public string PassWord { get; set; }
        public string Role { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Presentation.Models
{
    public class LoginInfoModel
    {
        [Required]
        [MaxLength(15)]
        public string Login { get; set; }
        [Required]
        [MinLength(8)]
        [MaxLength(64)]
        public string Password { get; set; }
    }
}

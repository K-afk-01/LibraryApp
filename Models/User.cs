using System.ComponentModel.DataAnnotations;

namespace LibraryMS.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [Display(Name = "Kullanıcı Adı")]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Şifre")]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "E-posta")]
        [EmailAddress]
        public string? Email { get; set; }

        [Display(Name = "Rol")]
        public string Role { get; set; } = "Admin";
    }
}

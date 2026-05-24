using System.ComponentModel.DataAnnotations;

namespace LibraryMS.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        [Display(Name = "Kategori Adı")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Açıklama")]
        [StringLength(500)]
        public string? Description { get; set; }

        public ICollection<Book>? Books { get; set; }
    }
}

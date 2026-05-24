using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryMS.Models
{
    public class Book
    {
        public int BookId { get; set; }

        [Required(ErrorMessage = "Kitap adı zorunludur.")]
        [Display(Name = "Kitap Adı")]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "ISBN zorunludur.")]
        [Display(Name = "ISBN")]
        [StringLength(20)]
        public string ISBN { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fiyat zorunludur.")]
        [Display(Name = "Fiyat (₺)")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 99999.99, ErrorMessage = "Fiyat 0'dan büyük olmalıdır.")]
        public decimal Price { get; set; }

        [Display(Name = "Yayın Tarihi")]
        [DataType(DataType.Date)]
        public DateTime? PublishDate { get; set; }

        [Display(Name = "Stok Adedi")]
        [Range(0, int.MaxValue)]
        public int Stock { get; set; } = 0;

        [Required(ErrorMessage = "Yazar seçimi zorunludur.")]
        [Display(Name = "Yazar")]
        public int AuthorId { get; set; }

        [Required(ErrorMessage = "Kategori seçimi zorunludur.")]
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }

        public Author? Author { get; set; }
        public Category? Category { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryMS.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        [Required(ErrorMessage = "Müşteri adı zorunludur.")]
        [Display(Name = "Müşteri Adı")]
        [StringLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [Display(Name = "E-posta")]
        [EmailAddress]
        public string CustomerEmail { get; set; } = string.Empty;

        [Display(Name = "Sipariş Tarihi")]
        [DataType(DataType.Date)]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Kitap seçimi zorunludur.")]
        [Display(Name = "Kitap")]
        public int BookId { get; set; }

        [Display(Name = "Adet")]
        [Range(1, 100, ErrorMessage = "Adet 1 ile 100 arasında olmalıdır.")]
        public int Quantity { get; set; } = 1;

        [Display(Name = "Toplam Tutar (₺)")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Durum")]
        public string Status { get; set; } = "Beklemede";

        public Book? Book { get; set; }
    }
}

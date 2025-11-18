using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBanHang.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        // 🔹 Khóa ngoại đến Categories
        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Categories? Categories { get; set; }
    }
}

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHang.Data.Entities
{
    public class Categories
    {
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        // 🔹 Một danh mục có thể có nhiều sản phẩm
        public ICollection<Product>? Products { get; set; }
    }
}

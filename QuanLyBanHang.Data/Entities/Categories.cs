using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHang.Data.Entities
{
    public class Categories
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        // 🔹 Mỗi danh mục có thể chứa nhiều sản phẩm
        public ICollection<Product>? Products { get; set; }
    }
}

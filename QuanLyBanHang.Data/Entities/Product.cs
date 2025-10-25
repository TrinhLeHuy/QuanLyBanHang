using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHang.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }
    }
}

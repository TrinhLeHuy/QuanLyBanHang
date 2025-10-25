using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHang.Data.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }
    }
}

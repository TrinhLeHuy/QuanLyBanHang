using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHang.Data.Entities
{
    public class Employee
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(50)]
        public string Position { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(20)]
        public string Phone { get; set; }
    }
}

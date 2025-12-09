using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBanHang.Data.Entities
{
    [Table("Employees")]
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }

        [Required, StringLength(100)]
        public string FullName { get; set; }

        [StringLength(15)]
        public string Phone { get; set; }

        // 🔹 Email đăng nhập
        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; }

        // 🔹 Mật khẩu (mã hóa)
        [Required]
        public string PasswordHash { get; set; }

        // 🔹 Vai trò (Admin hoặc Staff)
        [Required, StringLength(20)]
        public string Role { get; set; } = "Staff";

        // 🔹 Quan hệ với bảng Order
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}

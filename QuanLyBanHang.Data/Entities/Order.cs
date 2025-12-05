using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBanHang.Data.Entities
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }  // 👈 Đặt tên chuẩn theo convention EF

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal DiscountAmount { get; set; }

        [MaxLength(50)]
        public string? VoucherCode { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal CashGiven { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal ChangeAmount { get; set; }

        [MaxLength(50)]
        public string PaymentMethod { get; set; } = "Cash";

        [MaxLength(50)]
        public string PaymentStatus { get; set; } = "Completed";

        // 🔹 Khóa ngoại khách hàng
        [ForeignKey(nameof(Customer))]
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }

        // 🔹 Khóa ngoại nhân viên
        [ForeignKey(nameof(Employee))]
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        // 🔹 Danh sách chi tiết đơn hàng
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();


    }
}

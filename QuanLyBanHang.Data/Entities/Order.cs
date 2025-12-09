using QuanLyBanHang.Data.Enums;
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
        public int OrderId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }

        [Column(TypeName = "ENUM('Pending','Paid','Canceled')")]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        
        [ForeignKey(nameof(Employee))]
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        [ForeignKey(nameof(Voucher))]
        public int? VoucherId { get; set; }
        public Voucher? Voucher { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    }
}

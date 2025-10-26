using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBanHang.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }  // hoặc đổi lại OrderId cho thống nhất

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        // Khóa ngoại khách hàng
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        // 🔹 Thêm khóa ngoại nhân viên
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        // 🔹 Danh sách chi tiết đơn hàng
        public List<OrderDetail> OrderDetails { get; set; }
    }
}

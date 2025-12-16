using QuanLyBanHang.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHang.Web.Models
{
    public class POSViewModel
    {
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public List<POSItem> Items { get; set; } = new();
        
        // Dữ liệu cho UI mới
        public List<Product> Products { get; set; } = new();
        public List<Categories> Categories { get; set; } = new();
        public Employee? CurrentEmployee { get; set; }
        public List<Customer> Customers { get; set; } = new();
    }

    public class POSItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total => Quantity * UnitPrice;
    }
}
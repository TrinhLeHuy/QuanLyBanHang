using System.Collections.Generic;

namespace QuanLyBanHang.Web.DTO
{
    public class CheckoutFormRequest
    {
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public string ItemsJson { get; set; } = string.Empty;
        public string? VoucherCode { get; set; }
        public decimal CashGiven { get; set; }
        public string? PaymentMethod { get; set; }
    }

    public class CartItemDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
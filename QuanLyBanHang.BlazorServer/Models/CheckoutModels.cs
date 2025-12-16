using System.Collections.Generic;

namespace QuanLyBanHang.BlazorServer.Models
{
    public class CheckoutRequest
    {
        public int CustomerId { get; set; } // ID khách hàng (0 nếu là khách vãng lai)
        public string VoucherCode { get; set; } // Mã giảm giá (nếu có)
        public List<CartItemDTO> CartItems { get; set; }
    }

    public class CartItemDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
using QuanLyBanHang.Data.Enums;

namespace QuanLyBanHang.BlazorServer.Models
{
    public class OrderUpdateViewModel
    {
        public int OrderId { get; set; }
        public OrderStatus Status { get; set; }
    }
}

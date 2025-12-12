using QuanLyBanHang.Data.Enums;

namespace QuanLyBanHang.Web.Models
{
    public class OrderUpdateViewModel
    {
        public int OrderId { get; set; }
        public OrderStatus Status { get; set; }
    }
}

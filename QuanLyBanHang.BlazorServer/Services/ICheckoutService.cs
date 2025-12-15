using QuanLyBanHang.Data.Entities;

namespace QuanLyBanHang.BlazorServer.Services
{
    public interface ICheckoutService
    {
        public Task<Order?> PCCreateOrderAsync(int customerId, Order order, List<int> selectedProductIds);
        public Task<List<Product>?> PCGetAllProductAsync();
    }
}

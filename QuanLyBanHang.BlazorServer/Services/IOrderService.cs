using QuanLyBanHang.Data.Entities;

namespace QuanLyBanHang.BlazorServer.Services
{
    public interface IOrderService
    {
        public Task<List<Order>> GetAllAsync();
        public Task<Order?> GetByIdAsync(int id);

        public Task<List<Order>?> GetByFilterNameAsync(string filterName);

        public Task<Order?> CreateAsync(Order order);

        public Task<Order?> UpdateAsync(Order order);

        public Task<bool> DeleteAsync(Order order);
    }
}

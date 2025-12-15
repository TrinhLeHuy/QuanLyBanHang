using QuanLyBanHang.Data.Entities;

namespace QuanLyBanHang.BlazorServer.Services
{
    public interface IOrderService
    {
        public Task<List<Order>> GetAllAsync();
        public Task<Order?> GetByIdAsync(int id);

        public Task<List<Order>?> GetByFilterNameAsync(string status, string filterName);

        public Task<Order?> CreateAsync(Order order);

        public Task<Order?> UpdateAsync(Order order);

        public Task<bool> DeleteAsync(Order order);

        public Task<List<Order>?> GetByFilterStatusAsync(string Status);
        public Task<List<Order>?> GetByFilterDateAsync(string Status, DateTime startTime, DateTime endTime);
        public Task<List<Order>?> GetAllWithUserIdAsync(int userId);
        public Task<List<Order>?> GetByFilterStatusWithUserIdAsync(string Status, int userId);
        public Task<List<Order>?> GetByFilterDateWithUserIdAsync(string status, DateTime startTime, DateTime endTime, int userId);
    }
}

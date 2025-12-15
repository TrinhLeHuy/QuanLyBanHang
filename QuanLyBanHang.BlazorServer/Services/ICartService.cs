
using QuanLyBanHang.Data.Entities;

namespace QuanLyBanHang.BlazorServer.Services
{
    public interface ICartService
    {
        public Task<List<CartItems>> GetAllByCustomerIdAsync(int customerId);
        public Task<CartItems?> CreateAsync(CartItems cartItem);
        public Task<CartItems?> UpdpateAsync(int cartItemId, int quantity);
        public Task<bool> DeleteByIdAsync(int cartItemId);
        public Task<bool> DeleteByListIdAsync(int customerId, List<int> listItemId);
    }
}

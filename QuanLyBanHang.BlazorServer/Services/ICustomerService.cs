using QuanLyBanHang.Data.Entities;

namespace QuanLyBanHang.BlazorServer.Services
{
    public interface ICustomerService
    {
        public Task<List<Customer>> GetAllAsync();
    }
}

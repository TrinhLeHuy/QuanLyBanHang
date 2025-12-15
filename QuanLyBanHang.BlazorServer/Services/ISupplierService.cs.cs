using QuanLyBanHang.Data.Entities;

namespace QuanLyBanHang.BlazorServer.Services
{
    public interface ISupplierService
    {
        Task<List<Supplier>> GetAllAsync();
        Task<Supplier?> GetByIdAsync(int id);
        Task<bool> CreateAsync(Supplier supplier);
        Task UpdateAsync(Supplier supplier);
        Task DeleteAsync(int id);
    }
}

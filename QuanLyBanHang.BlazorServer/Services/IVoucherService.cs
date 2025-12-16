using QuanLyBanHang.Data.Entities;

namespace QuanLyBanHang.BlazorServer.Services
{
    public interface IVoucherService
    {
        public Task<List<Voucher>> GetAllAvailableAsync();
        Task<List<Voucher>> GetAllAsync(string? search = null, bool? active = null);
        Task<Voucher?> GetByIdAsync(int id);
        Task CreateAsync(Voucher voucher);
        Task UpdateAsync(Voucher voucher);
        Task DeleteAsync(int id);
    }
}

using QuanLyBanHang.Data.Entities;

namespace QuanLyBanHang.BlazorServer.Services
{
    public interface IVoucherService
    {
        public Task<List<Voucher>> GetAllAvailableAsync();
    }
}

using Microsoft.EntityFrameworkCore;
using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Data.Entities;

namespace QuanLyBanHang.BlazorServer.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly ApplicationDbContext _context;
        public VoucherService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Voucher>> GetAllAsync()
        {
            return await _context.Vouchers.ToListAsync();
        }
    }
}

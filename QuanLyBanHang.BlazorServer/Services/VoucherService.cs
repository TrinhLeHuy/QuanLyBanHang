using Microsoft.EntityFrameworkCore;
using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Data.Entities;
using QuanLyBanHang.Data.Enums;

namespace QuanLyBanHang.BlazorServer.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly ApplicationDbContext _context;
        public VoucherService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Voucher>> GetAllAvailableAsync()
        {
            return await _context.Vouchers
                .Where(v => v.StartDate <= DateTime.Now && v.EndDate>=DateTime.Now && v.Status == VoucherStatus.Active)
                .ToListAsync();
        }
    }
}

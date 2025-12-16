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
         public async Task<List<Voucher>> GetAllAsync(string? search = null, bool? active = null)
        {
            var query = _context.Vouchers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(v =>
                    v.Code.Contains(search) ||
                    (v.Description != null && v.Description.Contains(search)));
            }

            if (active.HasValue)
            {
                query = query.Where(v => v.IsActive == active.Value);
            }

            return await query
                .OrderByDescending(v => v.Id)
                .ToListAsync();
        }

        public async Task<Voucher?> GetByIdAsync(int id)
        {
            return await _context.Vouchers.FindAsync(id);
        }

        public async Task CreateAsync(Voucher voucher)
        {
            _context.Vouchers.Add(voucher);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Voucher voucher)
        {
            _context.Vouchers.Update(voucher);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var voucher = await _context.Vouchers.FindAsync(id);
            if (voucher == null) return;

            _context.Vouchers.Remove(voucher);
            await _context.SaveChangesAsync();
        }
    }
    
}

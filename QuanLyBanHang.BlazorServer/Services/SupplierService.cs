using Microsoft.EntityFrameworkCore;
using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Data.Entities;

namespace QuanLyBanHang.BlazorServer.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ApplicationDbContext _context;

        public SupplierService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Supplier>> GetAllAsync()
        {
            return await _context.Suppliers.ToListAsync();
        }

        public async Task<Supplier?> GetByIdAsync(int id)
        {
            return await _context.Suppliers.FindAsync(id);
        }

        public async Task<bool> CreateAsync(Supplier supplier)
        {
            var isDuplicate = await _context.Suppliers.AnyAsync(s =>
                s.SupplierName.ToLower() == supplier.SupplierName.ToLower()
                || (supplier.Email != null && s.Email == supplier.Email)
                || (supplier.Phone != null && s.Phone == supplier.Phone)
            );

            if (isDuplicate) return false;

            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateAsync(Supplier supplier)
        {
            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null) return;

            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
        }
    }
}

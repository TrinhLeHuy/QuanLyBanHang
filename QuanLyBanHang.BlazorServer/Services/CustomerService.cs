using Microsoft.EntityFrameworkCore;
using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Data.Entities;

namespace QuanLyBanHang.BlazorServer.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;

        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===================== GET ALL =====================
        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Customers
                .AsNoTracking()
                .ToListAsync();
        }

        // ===================== GET BY ID =====================
        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        // ===================== CREATE =====================
        public async Task CreateAsync(Customer customer)
        {
            // ✅ Check trùng Email
            if (await _context.Customers.AnyAsync(c => c.Email == customer.Email))
            {
                throw new Exception("Email này đã được đăng ký");
            }

            // ✅ Check trùng Phone
            if (await _context.Customers.AnyAsync(c => c.Phone == customer.Phone))
            {
                throw new Exception("Số điện thoại này đã tồn tại");
            }

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }

        // ===================== UPDATE =====================
        public async Task UpdateAsync(Customer customer)
        {
            // Check trùng Email (trừ chính nó)
            if (await _context.Customers.AnyAsync(c =>
                    c.Email == customer.Email &&
                    c.CustomerId != customer.CustomerId))
            {
                throw new Exception("Email đã được sử dụng");
            }

            // Check trùng Phone (trừ chính nó)
            if (await _context.Customers.AnyAsync(c =>
                    c.Phone == customer.Phone &&
                    c.CustomerId != customer.CustomerId))
            {
                throw new Exception("Số điện thoại đã tồn tại");
            }

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        // ===================== DELETE =====================
        public async Task DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return;

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
        // ===================== SEARCH =====================
        public async Task<List<Customer>> SearchAsync(string keyword)
        {
            var query = _context.Customers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
query = query.Where(c =>
                    c.FullName.Contains(keyword)||
                    c.Email.Contains(keyword) ||
                    c.Phone.Contains(keyword));
            }

            return await query
                .AsNoTracking()
                .ToListAsync();
        }

    }
}

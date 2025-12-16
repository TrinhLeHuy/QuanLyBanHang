using Microsoft.EntityFrameworkCore;
using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Data.Entities;
using System.Security.Cryptography;
using System.Text;

namespace QuanLyBanHang.BlazorServer.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllAsync()
            => await _context.Employees.ToListAsync();

        public async Task<Employee?> GetByIdAsync(int id)
            => await _context.Employees.FindAsync(id);

        public async Task<bool> EmailExistsAsync(string email)
            => await _context.Employees.AnyAsync(e => e.Email == email);

        // ✅ KHỚP INTERFACE
        public async Task CreateAsync(Employee employee, string password)
        {
            employee.PasswordHash = HashPassword(password);
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp != null && emp.Role != "Admin")
            {
                _context.Employees.Remove(emp);
                await _context.SaveChangesAsync();
            }
        }

        private string HashPassword(string raw)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
            return Convert.ToHexString(bytes).ToLower();
        }
    }
}

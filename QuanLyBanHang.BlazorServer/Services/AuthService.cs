using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Data.Entities;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace QuanLyBanHang.BlazorServer.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Employee?> LoginAsync(string email, string password)
        {
            string hashedPassword = ComputeSha256Hash(password);

            return await _context.Employees
                .FirstOrDefaultAsync(e => e.Email == email && e.PasswordHash == hashedPassword);
        }

        private string ComputeSha256Hash(string rawData)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            var builder = new StringBuilder();
            foreach (var b in bytes)
                builder.Append(b.ToString("x2"));
            return builder.ToString();
        }
    }
}

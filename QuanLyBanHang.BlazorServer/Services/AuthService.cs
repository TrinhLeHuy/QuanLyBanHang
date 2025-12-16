using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHang.BlazorServer.Models;
using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Data.Entities;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace QuanLyBanHang.BlazorServer.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ClaimsPrincipal?> LoginAsync(LoginViewModel loginModel)
        {
            string hashedPassword = ComputeSha256Hash(loginModel.Password);
            var user = await _context.Employees.FirstOrDefaultAsync(e => e.Email == loginModel.Email && e.PasswordHash == hashedPassword);

            if (user == null) return null;

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.EmployeeId.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Role, user.Role),
            // Thêm bất kỳ thông tin cần thiết nào khác
        };

            var claimsIdentity = new ClaimsIdentity(claims, IdentityConstants.ApplicationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return claimsPrincipal;
        }

        public async Task LogoutAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                await httpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            }
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

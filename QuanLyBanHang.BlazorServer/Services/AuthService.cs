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

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> LoginAsync(LoginViewModel loginModel)
        {
            string hashedPassword = ComputeSha256Hash(loginModel.Password);
            var user = await _context.Employees.FirstOrDefaultAsync(e => e.Email == loginModel.Email && e.PasswordHash == hashedPassword);

            if (user == null)
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(e => e.Email == loginModel.Email && e.Password == hashedPassword);
                if (customer == null)
                {
                    return false;
                }
                Session.CurrentUserId = customer.CustomerId;
                Session.CurrentUserRole = "Customer";
                return true;
            }
            Session.CurrentUserId = user.EmployeeId;
            Session.CurrentUserRole = user.Role;
            return true;


        }

        public async Task LogoutAsync()
        {
            await Task.CompletedTask;
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

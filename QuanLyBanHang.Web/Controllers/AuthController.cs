using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Web.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace QuanLyBanHang.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string hashedPassword = ComputeSha256Hash(model.Password);

            var employee = _context.Employees
                .FirstOrDefault(e => e.Email == model.Email && e.PasswordHash == hashedPassword);

            if (employee == null)
            {
                ViewBag.Error = "Email hoặc mật khẩu không chính xác!";
                return View(model);
            }

            // Lưu Session
            HttpContext.Session.SetString("EmployeeName", employee.FullName);
            HttpContext.Session.SetString("EmployeeRole", employee.Role);

            // 🟢 Tạo Claims để Identity nhận dạng
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, employee.FullName),
                new Claim(ClaimTypes.Role, employee.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // 🟢 Tạo Cookie đăng nhập
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

           
            if (employee.Role == "Admin")
                return RedirectToAction("Index", "Dashboard");
            else
                return RedirectToAction("Index", "Home");
        
        }

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

        private string ComputeSha256Hash(string rawData)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Web.Models;
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

        // 🟢 Hiển thị trang đăng nhập
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // 🟢 Xử lý đăng nhập
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
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

            HttpContext.Session.SetString("EmployeeName", employee.FullName);
            HttpContext.Session.SetString("EmployeeRole", employee.Role);
            HttpContext.Session.SetInt32("EmployeeId", employee.EmployeeId);

            if (employee.Role == "Admin")
                return RedirectToAction("Index", "Dashboard");
            else
                return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
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

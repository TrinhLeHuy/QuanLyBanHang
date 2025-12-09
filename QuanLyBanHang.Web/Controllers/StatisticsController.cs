using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHang.Data.DataContext;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyBanHang.Web.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Trang tổng quan có filter ngày (tuỳ chọn)
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            if (startDate == null)
                startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (endDate == null)
                endDate = DateTime.Now;

            var ordersInRange = await _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .Include(o => o.OrderDetails)
                .ToListAsync();

            var totalRevenue = ordersInRange.Sum(o => o.OrderDetails.Sum(d => d.UnitPrice * d.Quantity));
            var totalOrders = ordersInRange.Count;
            var totalProducts = ordersInRange.Sum(o => o.OrderDetails.Sum(d => d.Quantity));

            ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.TotalOrders = totalOrders;
            ViewBag.TotalProducts = totalProducts;

            return View();
        }

        // Doanh thu theo ngày (dùng cho view biểu bảng / chart)
        public IActionResult RevenueByDate()
        {
            var data = _context.Orders
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new {
                    Date = g.Key,
                    TotalRevenue = g.Sum(o => o.TotalAmount)
                })
                .OrderBy(x => x.Date)
                .ToList();

            return View(data);
        }

        // Doanh thu theo tháng
        public IActionResult RevenueByMonth()
        {
            var data = _context.Orders
                .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                .Select(g => new {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalRevenue = g.Sum(o => o.TotalAmount)
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToList();

            return View(data);
        }

        // Doanh thu theo nhân viên
        public IActionResult RevenueByEmployee()
        {
            var data = _context.Orders
                .Include(o => o.Employee)
                .GroupBy(o => o.Employee.FullName)
                .Select(g => new {
                    EmployeeName = g.Key,
                    OrdersCount = g.Count(),
                    TotalRevenue = g.Sum(o => o.TotalAmount)
                })
                .OrderByDescending(x => x.TotalRevenue)
                .ToList();

            return View(data);
        }
    }
}

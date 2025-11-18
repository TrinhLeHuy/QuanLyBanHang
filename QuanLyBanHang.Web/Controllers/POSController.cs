using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyBanHang.Web.Controllers
{
    public class POSController : Controller
    {
        private readonly ApplicationDbContext _context;

        public POSController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===================== Giao diện POS =====================
        public IActionResult Index()
        {
            LoadViewBags();
            return View(new POSViewModel());
        }

        // ===================== Thanh toán =====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(POSViewModel model, string ItemsJson)
        {
            if (string.IsNullOrEmpty(ItemsJson))
            {
                ModelState.AddModelError("", "Chưa có sản phẩm nào trong đơn.");
                LoadViewBags();
                return View("Index", model);
            }

            // Chuyển JSON sang danh sách item
            model.Items = System.Text.Json.JsonSerializer.Deserialize<List<POSItem>>(ItemsJson);

            if (model.Items == null || !model.Items.Any())
            {
                ModelState.AddModelError("", "Vui lòng chọn ít nhất một sản phẩm.");
                LoadViewBags();
                return View("Index", model);
            }

            // Tạo đơn hàng
            var order = new Order
            {
                CustomerId = model.CustomerId,
                EmployeeId = model.EmployeeId,
                OrderDate = DateTime.Now,
                OrderDetails = new List<OrderDetail>(),
                TotalAmount = model.Items.Sum(i => i.Quantity * i.UnitPrice)
            };

            foreach (var item in model.Items)
            {
                var product = _context.Products.Find(item.ProductId);

                if (product == null || product.Stock < item.Quantity)
                {
                    ModelState.AddModelError("", $"Sản phẩm '{item.ProductName}' không đủ hàng tồn kho.");
                    LoadViewBags();
                    return View("Index", model);
                }

                product.Stock -= item.Quantity;

                order.OrderDetails.Add(new OrderDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                });
            }

            _context.Orders.Add(order);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Thanh toán thành công!";
            return RedirectToAction(nameof(Index));
        }

        // ===================== Load dữ liệu chống NULL =====================
        private void LoadViewBags()
        {
            ViewBag.Products = _context.Products?.ToList() ?? new List<Product>();
            ViewBag.Customers = _context.Customers?.ToList() ?? new List<Customer>();
            ViewBag.Employees = _context.Employees?.ToList() ?? new List<Employee>();
        }
    }

    // ===================== ViewModel =====================
    public class POSViewModel
    {
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
        public List<POSItem> Items { get; set; } = new();
    }

    public class POSItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total => Quantity * UnitPrice;
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        // ------------------ INDEX (Giao diện POS chính) ------------------
        public IActionResult Index()
        {
            ViewBag.Products = _context.Products.ToList();
            ViewBag.Customers = _context.Customers.ToList();
            ViewBag.Employees = _context.Employees.ToList();

            return View(new POSViewModel());
        }

        // ------------------ Thanh toán ------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(POSViewModel model)
        {
            if (model == null || model.Items == null || !model.Items.Any())
            {
                ModelState.AddModelError("", "Vui lòng chọn ít nhất một sản phẩm.");
                ViewBag.Products = _context.Products.ToList();
                ViewBag.Customers = _context.Customers.ToList();
                ViewBag.Employees = _context.Employees.ToList();
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
                    ModelState.AddModelError("", $"Sản phẩm '{item.ProductName}' không đủ hàng tồn.");
                    ViewBag.Products = _context.Products.ToList();
                    ViewBag.Customers = _context.Customers.ToList();
                    ViewBag.Employees = _context.Employees.ToList();
                    return View("Index", model);
                }

                // Trừ tồn kho
                product.Stock -= item.Quantity;

                // Thêm vào chi tiết đơn hàng
                order.OrderDetails.Add(new OrderDetail
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                });
            }

            _context.Orders.Add(order);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Thanh toán thành công! Hóa đơn đã được lưu.";
            return RedirectToAction(nameof(Index));
        }
    }

    // ------------------ ViewModel cho POS ------------------
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

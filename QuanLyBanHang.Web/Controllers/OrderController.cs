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
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // -------------------- INDEX --------------------
        public IActionResult Index()
        {
            var orders = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .ToList();
            return View(orders);
        }

        // -------------------- CREATE (GET) --------------------
        [HttpGet]
        public IActionResult Create()
        {
            var model = new Order
            {
                OrderDetails = new List<OrderDetail>
                {
                    new OrderDetail(),
                    new OrderDetail()
                }
            };

            ViewBag.Customers = _context.Customers.ToList();
            ViewBag.Products = _context.Products.ToList();
            ViewBag.Employees = _context.Employees.ToList();

            return View(model);
        }

        // -------------------- CREATE (POST) --------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                order.OrderDate = DateTime.Now;
                order.TotalAmount = order.OrderDetails.Sum(d => d.Quantity * d.UnitPrice);

                _context.Orders.Add(order);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            // Nếu lỗi -> nạp lại dropdown
            ViewBag.Customers = _context.Customers.ToList();
            ViewBag.Products = _context.Products.ToList();
            ViewBag.Employees = _context.Employees.ToList();

            return View(order);
        }
    }
}

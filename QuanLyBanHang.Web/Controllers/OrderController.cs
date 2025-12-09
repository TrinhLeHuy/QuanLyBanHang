using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Data.Entities;
using QuanLyBanHang.Data.Enums;
using QuanLyBanHang.Web.Models;
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
                .ToList();
            return View(orders);
        }

        public IActionResult LoadTable()
        {
            var orders = _context.Orders
                .Include(o => o.Customer)
                .ToList();
            return PartialView("OrderTable", orders);
        }
        public IActionResult LoadFilter(string filter, string status)
        {
            var orders = new List<Order>();
            if(status !="Tất cả")
            {
                orders = _context.Orders
                            .Include(o => o.Customer)
                            .Where(o => o.Customer != null && o.Customer.FullName.ToLower().Contains(filter.ToLower()) && o.Status.ToString() == status)
                            .ToList();
            }
            else
            {
                orders = _context.Orders
                            .Include(o => o.Customer)
                            .Where(o => o.Customer != null && o.Customer.FullName.ToLower().Contains(filter.ToLower()))
                            .ToList();
            }

            return PartialView("OrderTable", orders);
        }

        public IActionResult LoadFilterDate(DateTime start, DateTime end, string status)
        {
            var orders = new List<Order>();
            if (status != "Tất cả")
            {
                orders = _context.Orders
                            .Include(o => o.Customer)
                            .Where(o => o.OrderDate>= start && o.OrderDate <= end && o.Status.ToString() == status)
                            .ToList();
            }
            else
            {
                orders = _context.Orders
                            .Include(o => o.Customer)
                            .Where(o => o.OrderDate >= start && o.OrderDate <= end)
                            .ToList();
            }

            return PartialView("OrderTable", orders);
        }

        public IActionResult LoadFilterStatus(string status)
        {
            if (status != "Tất cả")
            {
                var orders = _context.Orders
                        .Where(o => o.Status.ToString() == status)
                        .ToList();
                return PartialView("OrderTable", orders);
            }
            else
            {
                return LoadTable();
            }
        }

        // -------------------- CREATE (GET) --------------------
        [HttpGet]
        public IActionResult Create()
        {
            var model = new Order
            {
                OrderDetails = new List<OrderDetail>()
                {
                    
                }
            };

            ViewBag.Customers = _context.Customers.ToList();
            ViewBag.Products = _context.Products.ToList();
            ViewBag.Employees = _context.Employees.ToList();
            var now = DateTime.Now;
            ViewBag.Vouchers = _context.Vouchers.Where(v => v.StartDate <= now && v.EndDate >= now).ToList();

            return View(model);
        }

        // -------------------- CREATE (POST) --------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Order order)
        {
            if (!ModelState.IsValid)
            {
                //var errors = ModelState
                //.Where(ms => ms.Value.Errors.Count > 0)
                //.Select(ms => new {
                //    Property = ms.Key,
                //    Messages = ms.Value.Errors.Select(e => e.ErrorMessage).ToList()
                //})
                //.ToList();

                //// Trả về PartialView kèm errors để hiển thị trực quan
                //ViewBag.Errors = errors;
                ViewBag.Customers = _context.Customers.ToList();
                ViewBag.Products = _context.Products.ToList();
                ViewBag.Employees = _context.Employees.ToList();
                return PartialView("Create", order);
            }
            order.OrderDetails = order.OrderDetails.Where(d => d != null).ToList();
            order.OrderDate = DateTime.Now;
            order.TotalAmount = order.OrderDetails.Sum(d => d.Quantity * d.UnitPrice);

            _context.Orders.Add(order);
            _context.SaveChanges();

            return Content("success");
        }

        [HttpGet]
        public IActionResult Detail(int orderID)
        {
            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.OrderId == orderID);
            ViewBag.Customers = _context.Customers.ToList();
            ViewBag.Products = _context.Products.ToList();
            ViewBag.Employees = _context.Employees.ToList();
            ViewBag.Vouchers = _context.Vouchers.ToList();

            return PartialView("Detail", order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(OrderUpdateViewModel vm)
        {
            if (!ModelState.IsValid)
                return PartialView("_OrderUpdate", vm);

            var target = _context.Orders.FirstOrDefault(o => o.OrderId == vm.OrderId);

            if (target == null)
                return NotFound();

            target.Status = vm.Status;

            _context.SaveChanges();

            return Content("Update success!");
        }
    }
}

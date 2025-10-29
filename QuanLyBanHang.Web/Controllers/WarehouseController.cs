using Microsoft.AspNetCore.Mvc;
using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Data.Entities;

namespace QuanLyBanHang.Web.Controllers
{
    public class WarehouseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WarehouseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Danh sách kho
        public IActionResult Index()
        {
            var warehouses = _context.Warehouses.ToList();
            return View(warehouses);
        }

        // Thêm mới
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                _context.Warehouses.Add(warehouse);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(warehouse);
        }

        // Sửa kho
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var warehouse = _context.Warehouses.Find(id);
            if (warehouse == null) return NotFound();
            return View(warehouse);
        }

        [HttpPost]
        public IActionResult Edit(Warehouse warehouse)
        {
            if (ModelState.IsValid)
            {
                _context.Warehouses.Update(warehouse);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(warehouse);
        }

        // Xóa kho
        public IActionResult Delete(int id)
        {
            var warehouse = _context.Warehouses.Find(id);
            if (warehouse == null) return NotFound();

            _context.Warehouses.Remove(warehouse);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}

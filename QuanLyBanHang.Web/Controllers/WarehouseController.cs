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

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Warehouse warehouse)
        {
            if (!ModelState.IsValid)
                return View(warehouse);

            // Kiểm tra trùng: Name OR Location OR Phone
            var isDuplicate = _context.Warehouses.Any(w =>
                w.WarehouseName.Trim().ToLower() == warehouse.WarehouseName.Trim().ToLower()
                || w.Location.Trim().ToLower() == warehouse.Location.Trim().ToLower()
                || w.Phone.Trim() == warehouse.Phone.Trim()
            );

            if (isDuplicate)
            {
                ModelState.AddModelError(string.Empty,
                    "Kho hàng này đã tồn tại (trùng tên, địa chỉ hoặc số điện thoại).");

                return View(warehouse);
            }

            // Nếu không trùng → lưu
            _context.Warehouses.Add(warehouse);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
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

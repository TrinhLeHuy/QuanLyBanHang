    using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Data.Repositories;
using QuanLyBanHang.Web.Models;

namespace QuanLyBanHang.Web.Controllers
{
    public class InventoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly InventoryRepository _inventoryRepo;

        public InventoryController(ApplicationDbContext context, InventoryRepository inventoryRepo)
        {
            _context = context;
            _inventoryRepo = inventoryRepo;
        }

        // Hàm load dropdown vào ViewModel
        private void LoadDropdowns(InventoryInputViewModel model)
        {
            model.Products = _context.Products
                .Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList();

            model.Suppliers = _context.Suppliers
                .Select(s => new SelectListItem
                {
                    Value = s.SupplierId.ToString(),
                    Text = s.SupplierName
                }).ToList();

            model.Warehouses = _context.Warehouses
                .Select(w => new SelectListItem
                {
                    Value = w.WarehouseId.ToString(),
                    Text = w.WarehouseName
                }).ToList();
        }

        // =============== NHẬP KHO ===============

        [HttpGet]
        public IActionResult Import()
        {
            var vm = new InventoryInputViewModel();
            LoadDropdowns(vm);
            return View(vm);
        }

        [HttpPost]
        public IActionResult Import(InventoryInputViewModel model)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdowns(model); // nếu sai validation phải load lại dropdown
                return View(model);
            }
            if (!ModelState.IsValid)
            {
                // Có lỗi validate (Note trống, v.v.) -> quay lại View, hiển thị lỗi
                return View(model);
            }

            _inventoryRepo.ImportStock(
                model.ProductId,
                model.WarehouseId,
                model.SupplierId ?? 0,
                model.Quantity,
                model.Note
            );

            TempData["Success"] = "Nhập kho thành công!";
            return RedirectToAction(nameof(Import));
        }

        // =============== XUẤT KHO ===============

        [HttpGet]
        public IActionResult Export()
        {
            var vm = new InventoryInputViewModel();
            LoadDropdowns(vm);
            return View(vm);
        }

        [HttpPost]
        public IActionResult Export(InventoryInputViewModel model)
        {
            if (!ModelState.IsValid)
            {
                LoadDropdowns(model);
                return View(model);
            }

            var ok = _inventoryRepo.ExportStock(
                model.ProductId,
                model.WarehouseId,
                model.Quantity,
                model.Note
            );

            if (!ok)
            {
                ModelState.AddModelError("", "Không đủ tồn kho để xuất!");
                LoadDropdowns(model);
                return View(model);
            }

            TempData["Success"] = "Xuất kho thành công!";
            return RedirectToAction(nameof(Export));
        }
        public IActionResult Stock()
        {
            var stock = _context.InventoryTransactions
                .GroupBy(t => new { t.ProductId, t.WarehouseId })
                .Select(g => new StockViewModel
                {
                    ProductId = g.Key.ProductId,
                    WarehouseId = g.Key.WarehouseId,

                    ProductName = g.First().Product.Name,
                    WarehouseName = g.First().Warehouse.WarehouseName,

                    Quantity = g
                        .Where(t => t.TransactionType == "IN").Sum(t => t.Quantity)
                        - g.Where(t => t.TransactionType == "OUT").Sum(t => t.Quantity)
                })
                .Where(x => x.Quantity != 0) // hide zero stock (optional)
                .ToList();

            return View(stock);
        }
        public IActionResult Transactions()
        {
            var list = _context.InventoryTransactions
                .OrderByDescending(t => t.TransactionDate)
                .Select(t => new
                {
                    t.TransactionId,
                    t.TransactionDate,
                    t.TransactionType,
                    ProductName = t.Product.Name,
                    WarehouseName = t.Warehouse.WarehouseName,
                    SupplierName = t.Supplier != null ? t.Supplier.SupplierName : "(N/A)",
                    t.Quantity,
                    t.Note
                })
                .ToList();

            return View(list);
        }



        // =============== TỒN KHO, LỊCH SỬ ===============
        // Tùy DB, anh/chị có thể thêm:
        // public IActionResult Stock()  => xem tồn kho
        // public IActionResult Transactions() => lịch sử nhập/xuất
    }
}

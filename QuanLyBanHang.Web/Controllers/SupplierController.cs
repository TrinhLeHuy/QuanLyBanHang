using Microsoft.AspNetCore.Mvc;
using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Data.Entities;
using System.Linq;

namespace QuanLyBanHang.Web.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupplierController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Danh sách nhà cung cấp
        public IActionResult Index()
        {
            var suppliers = _context.Suppliers.ToList();
            return View(suppliers);
        }

        // Thêm mới
        [HttpGet]
        // GET: Suppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Suppliers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Supplier supplier)
        {
            // Nếu có lỗi validate (Required, Email, v.v) thì trả lại luôn
            if (!ModelState.IsValid)
            {
                return View(supplier);
            }

            // CHECK TRÙNG: TÊN + EMAIL + PHONE (tất cả giống nhau)
            var isDuplicate = _context.Suppliers.Any(s =>
            s.SupplierName.Trim().ToLower() == supplier.SupplierName.Trim().ToLower()
            || s.Email.Trim().ToLower() == supplier.Email.Trim().ToLower()
            || s.Phone.Trim() == supplier.Phone.Trim()
);

            if (isDuplicate)
            {
                ModelState.AddModelError(string.Empty,
                    "Tên, Email hoặc Số điện thoại đã tồn tại trong hệ thống.");
                return View(supplier);
            }

            // Không trùng -> lưu
            _context.Suppliers.Add(supplier);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // Sửa
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var supplier = _context.Suppliers.Find(id);
            if (supplier == null) return NotFound();
            return View(supplier);
        }

        [HttpPost]
        public IActionResult Edit(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _context.Suppliers.Update(supplier);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // Xóa
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var supplier = _context.Suppliers.Find(id);
            if (supplier == null) return NotFound();

            return View(supplier); // 👈 TRẢ VỀ VIEW XÁC NHẬN
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var supplier = _context.Suppliers.Find(id);
            if (supplier == null) return NotFound();

            _context.Suppliers.Remove(supplier);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}

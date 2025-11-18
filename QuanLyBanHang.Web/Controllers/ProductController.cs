using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Data.Entities;
using QuanLyBanHang.Web.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuanLyBanHang.Web.Controllers
{
    [AdminAuthorize] // Chỉ admin mới quản lý Product
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Product
        public IActionResult Index()
        {
            var products = _context.Products
                                   .Include(p => p.Categories)
                                   .ToList();
            return View(products);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            // Validate CategoryId
            if (!_context.Categories.Any(c => c.Id == product.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Danh mục không tồn tại.");
            }

            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }

        // GET: Product/Edit/5
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product product)
        {
            // Validate CategoryId
            if (!_context.Categories.Any(c => c.Id == product.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Danh mục không tồn tại.");
            }

            if (ModelState.IsValid)
            {
                _context.Products.Update(product);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }

        // GET: Product/Delete/5
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

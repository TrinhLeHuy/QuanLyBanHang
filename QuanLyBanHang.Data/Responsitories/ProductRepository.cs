using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace QuanLyBanHang.Data.Repositories
{
    public class ProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAll(string keyword = "")
        {
            if (string.IsNullOrEmpty(keyword))
                return _context.Products.ToList();

            return _context.Products
                .Where(p => p.Name.Contains(keyword) || p.Categories.Name.Contains(keyword))
                .ToList();
        }

        public Product GetById(int id) => _context.Products.Find(id);

        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }
    }
}

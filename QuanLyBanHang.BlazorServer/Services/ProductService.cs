using Microsoft.EntityFrameworkCore;
using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Data.Entities;

namespace QuanLyBanHang.BlazorServer.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        public ProductService(ApplicationDbContext context) 
        {
            _context=context;
        }
        public async Task<List<Product>> GetAllProductAsync()
        {
            return await _context.Products
                .Include(p => p.Categories)
                .ToListAsync();
        }
        public async Task<List<Product>> GetAllProductInStockAsync()
        {
            return await _context.Products
                .Include(p => p.Categories)
                .Where(p => p.Stock >0 )
                .ToListAsync();
        }
           // Lấy tất cả sản phẩm, bỏ ImageUrl để tránh lỗi
        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Categories)
                .Select(p => new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    Categories = new Categories
                    {
                        Id = p.Categories.Id,
                        Name = p.Categories.Name
                    }
                    // 🔹 Bỏ ImageUrl
                }).ToListAsync();
        }

        // Lấy sản phẩm theo Id
        public async Task<Product?> GetByIdAsync(int id)
        {
            var p = await _context.Products
                .Include(p => p.Categories)
                .Where(p => p.Id == id)
                .Select(p => new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    Description = p.Description,
                    CategoryId = p.CategoryId,
                    Categories = new Categories
                    {
                        Id = p.Categories.Id,
                        Name = p.Categories.Name
                    }
                }).FirstOrDefaultAsync();

            return p;
        }

        // Thêm sản phẩm
        public async Task AddAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        // Cập nhật sản phẩm
        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        // Xóa sản phẩm
        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        // Lấy danh sách danh mục cho Select
        public async Task<List<Categories>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }
    }
    }

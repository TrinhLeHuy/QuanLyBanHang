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
    }
}

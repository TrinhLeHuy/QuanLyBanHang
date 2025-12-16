using Microsoft.EntityFrameworkCore;
using QuanLyBanHang.BlazorServer.Models;
using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Data.Entities;

namespace QuanLyBanHang.BlazorServer.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly ApplicationDbContext _context;

        public InventoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        // 🔹 Lấy sản phẩm (KHÔNG ImageUrl, KHÔNG Inventory)
        public async Task<List<Product>> GetProductsAsync()
        {
            return await _context.Products
                .Select(p => new Product
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToListAsync();
        }

        // 🔹 Nhập kho (GHI TRANSACTION, KHÔNG ĐỤNG INVENTORY TABLE)
        public async Task ImportAsync(
            int productId,
            int warehouseId,
            int quantity,
            string note
        )
        {
            var tran = new InventoryTransaction
            {
                ProductId = productId,
                WarehouseId = warehouseId,
                Quantity = quantity,
                TransactionType = "IN",
                Note = note
            };

            _context.InventoryTransactions.Add(tran);
            await _context.SaveChangesAsync();
        }

        // 🔹 Tồn kho (TÍNH TỪ TRANSACTION)
        public async Task<List<StockViewModel>> GetStockAsync()
        {
            return await _context.InventoryTransactions
                .GroupBy(t => new { t.ProductId, t.WarehouseId })
                .Select(g => new StockViewModel
                {
                    ProductId = g.Key.ProductId,
                    WarehouseId = g.Key.WarehouseId,
                    ProductName = g.First().Product.Name,
                    Quantity =
                        g.Where(x => x.TransactionType == "IN").Sum(x => x.Quantity)
                      - g.Where(x => x.TransactionType == "OUT").Sum(x => x.Quantity)
                })
                .Where(x => x.Quantity != 0)
                .ToListAsync();
        }

        // 🔹 Lịch sử nhập / xuất
        public async Task<List<InventoryTransaction>> GetTransactionsAsync()
        {
            return await _context.InventoryTransactions
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }
    }
}

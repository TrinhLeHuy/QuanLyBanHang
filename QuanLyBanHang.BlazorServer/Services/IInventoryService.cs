using QuanLyBanHang.BlazorServer.Models;
using QuanLyBanHang.Data.Entities;

namespace QuanLyBanHang.BlazorServer.Services
{
    public interface IInventoryService
    {
        Task<List<Product>> GetProductsAsync();

        Task ImportAsync(
            int productId,
            int warehouseId,
            int quantity,
            string note
        );

        Task<List<StockViewModel>> GetStockAsync();

        Task<List<InventoryTransaction>> GetTransactionsAsync();
    }
}

using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Data.Entities;

namespace QuanLyBanHang.Data.Repositories
{
    public class InventoryRepository
    {
        private readonly ApplicationDbContext _context;

        public InventoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void ImportStock(int productId, int warehouseId, int? supplierId, int quantity, string note = null)
        {
            // 1. Tạo transaction
            var transaction = new InventoryTransaction
            {
                ProductId = productId,
                WarehouseId = warehouseId,
                SupplierId = supplierId,
                Quantity = quantity,
                TransactionType = "IN",
                Note = note
            };
            _context.InventoryTransactions.Add(transaction);

            // 2. Cập nhật tồn kho
            var pw = _context.ProductWarehouses
                .FirstOrDefault(x => x.ProductId == productId && x.WarehouseId == warehouseId);

            if (pw == null)
            {
                pw = new ProductWarehouse
                {
                    ProductId = productId,
                    WarehouseId = warehouseId,
                    QuantityOnHand = quantity
                };
                _context.ProductWarehouses.Add(pw);
            }
            else
            {
                pw.QuantityOnHand += quantity;
                _context.ProductWarehouses.Update(pw);
            }
            var productTarget = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (productTarget != null)
            {
                productTarget.Stock += quantity;
            }
            _context.SaveChanges();
        }

        public bool ExportStock(int productId, int warehouseId, int quantity, string note = null)
        {
            var pw = _context.ProductWarehouses
                .FirstOrDefault(x => x.ProductId == productId && x.WarehouseId == warehouseId);

            if (pw == null || pw.QuantityOnHand < quantity)
            {
                return false; // không đủ hàng
            }

            // 1. Tạo transaction
            var transaction = new InventoryTransaction
            {
                ProductId = productId,
                WarehouseId = warehouseId,
                Quantity = quantity,
                TransactionType = "OUT",
                Note = note
            };
            _context.InventoryTransactions.Add(transaction);

            // 2. Trừ tồn kho
            pw.QuantityOnHand -= quantity;
            _context.ProductWarehouses.Update(pw);

            _context.SaveChanges();
            return true;
        }
    }
}

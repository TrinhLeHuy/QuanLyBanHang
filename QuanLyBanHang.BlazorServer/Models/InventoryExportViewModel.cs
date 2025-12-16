using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuanLyBanHang.Web.ViewModels
{
    public class ExportViewModel
    {
        public int WarehouseId { get; set; }
        public int ProductId { get; set; }
        public int? SupplierId { get; set; }  // nếu cần
        public int Quantity { get; set; }
        public string? Note { get; set; }

        public IEnumerable<SelectListItem> Warehouses { get; set; }
        public IEnumerable<SelectListItem> Products { get; set; }
        public IEnumerable<SelectListItem>? Suppliers { get; set; }
    }

    public class StockItemViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string WarehouseName { get; set; }
        public int QuantityOnHand { get; set; }
        public string? SupplierName { get; set; }
    }

    public class InventoryTransactionViewModel
    {
        public int TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; } // "Import" / "Export"
        public string ProductName { get; set; }
        public string WarehouseName { get; set; }
        public string? SupplierName { get; set; }
        public int Quantity { get; set; }
        public string? Note { get; set; }
        public string? CreatedBy { get; set; } // nhân viên thực hiện
    }
}

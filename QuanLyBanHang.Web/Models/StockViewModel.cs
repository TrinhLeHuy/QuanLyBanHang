namespace QuanLyBanHang.Web.Models
{
    public class StockViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }

        public int Quantity { get; set; }
    }
}

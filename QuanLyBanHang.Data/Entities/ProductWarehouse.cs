using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBanHang.Data.Entities
{
    [Table("ProductWarehouses")]
    public class ProductWarehouse
    {
        [Key]
        public int Id { get; set; }

        // FK Product
        public int ProductId { get; set; }

        // FK Warehouse
        public int WarehouseId { get; set; }

        public int QuantityOnHand { get; set; }

        // Navigation
        public Product Product { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}

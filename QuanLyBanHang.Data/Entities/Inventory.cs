using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBanHang.Data.Entities
{
    [Table("Inventories")]
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }

        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        [ForeignKey(nameof(Warehouse))]
        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
    }
}

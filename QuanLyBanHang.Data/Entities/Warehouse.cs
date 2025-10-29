
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBanHang.Data.Entities
{
    [Table("Warehouses")]
    public class Warehouse
    {
        [Key]
        public int WarehouseId { get; set; }

        [Required, StringLength(100)]
        public string WarehouseName { get; set; }

        [StringLength(200)]
        public string Location { get; set; }

        [StringLength(15)]
        public string Phone { get; set; }
    }
}

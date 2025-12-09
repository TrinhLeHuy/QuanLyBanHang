using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBanHang.Data.Entities
{
    [Table("InventoryTransactions")]
    public class InventoryTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        // Nhập / Xuất
        [Required]
        [StringLength(10)]
        public string TransactionType { get; set; }  // "IN" hoặc "OUT"

        // Product - bắt buộc
        [Required]
        public int ProductId { get; set; }

        // Supplier - có thể null khi xuất
        public int? SupplierId { get; set; }

        // Warehouse - bắt buộc
        [Required]
        public int WarehouseId { get; set; }

        [Required]
        public int Quantity { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.Now;

        [StringLength(200)]
        public string Note { get; set; }

        // Navigation
        public Product Product { get; set; }
        public Supplier Supplier { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}

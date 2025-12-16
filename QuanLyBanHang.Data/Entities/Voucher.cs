using QuanLyBanHang.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyBanHang.Data.Entities
{
    [Table("vouchers")]
    public class Voucher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Column(TypeName = "ENUM('Percent','Fixed')")]
        public DiscountType DiscountType { get; set; } = DiscountType.Percent;
        public decimal DiscountValue { get; set; }
        public decimal DiscountPercent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Quantity { get; set; }
        public int UsedCount { get; set; }
        [Column(TypeName = "ENUM('Active','Inactive')")]
        public VoucherStatus Status { get; set; } = VoucherStatus.Active;
        public bool IsActive { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHang.Data.Entities
{
   public class Voucher
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal DiscountPercent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Quantity { get; set; }
        public int UsedCount { get; set; }
        public bool IsActive { get; set; }
    }
}

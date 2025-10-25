using System;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHang.Data.Entities
{
    public class Voucher
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Code { get; set; }

        public decimal DiscountPercent { get; set; }

        public DateTime ExpiryDate { get; set; }
    }
}

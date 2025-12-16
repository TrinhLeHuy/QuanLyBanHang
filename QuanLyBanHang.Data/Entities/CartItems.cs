using QuanLyBanHang.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyBanHang.Data.Entities
{
    [Table("cart_item")]
    public class CartItems
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [ForeignKey(nameof(Customer))]
        public int CustomerId {  get; set; }
        public Customer? Customer { get; set; }

        [Required]
        [ForeignKey(nameof(Product))]
        public int ProductId {  get; set; }
        public Product? Product { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity {  get; set; }
        public CartItemStatus Status { get; set; } = CartItemStatus.InStock;
    }
}

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace QuanLyBanHang.Web.Models
{
    public class InventoryInputViewModel
    {
        // Dùng chung cho Import / Export
        [Required]
        [Display(Name = "Sản phẩm")]
        public int ProductId { get; set; }

        [Display(Name = "Nhà cung cấp")]
        public int? SupplierId { get; set; }   // Export có thể không cần

        [Required]
        [Display(Name = "Kho")]
        public int WarehouseId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải > 0")]
        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }

        [Display(Name = "Ghi chú")]
        [Required(ErrorMessage = "Ghi chú không được để trống")]
        public string? Note { get; set; }

        // Dropdown
        public IEnumerable<SelectListItem>? Products { get; set; }
        public IEnumerable<SelectListItem>? Suppliers { get; set; }
        public IEnumerable<SelectListItem>? Warehouses { get; set; }
    }
}

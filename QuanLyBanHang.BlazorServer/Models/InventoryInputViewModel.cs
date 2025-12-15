using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace QuanLyBanHang.Web.Models
{
    public class InventoryInputViewModel
    {
        [Required(ErrorMessage = "Vui lòng chọn sản phẩm")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn kho")]
        public int WarehouseId { get; set; }

        public int? SupplierId { get; set; }

        [Required(ErrorMessage = "Số lượng phải > 0")]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        public string? Note { get; set; }

        // Dropdown
        public List<SelectListItem> Products { get; set; } = new();
        public List<SelectListItem> Suppliers { get; set; } = new();
        public List<SelectListItem> Warehouses { get; set; } = new();
    }
}

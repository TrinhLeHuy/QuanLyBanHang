using QuanLyBanHang.Data.Entities;

namespace QuanLyBanHang.BlazorServer.Services
{
    public class CheckoutService : ICheckoutService
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartItemService;
        private readonly IProductService _productService;
        public CheckoutService(IOrderService orderService, ICartService cartItemService, IProductService productService) 
        {
            _cartItemService = cartItemService;
            _orderService = orderService;
            _productService = productService;
        }

        public async Task<Order?> PCCreateOrderAsync(int customerId, Order order, List<int> selectedProductIds)
        {
            var newOrder = await _orderService.CreateAsync(order);
            if (newOrder != null)
            {
                if (await _cartItemService.DeleteByListIdAsync(customerId, selectedProductIds)) return newOrder;
            }
            return null;
        }

        public async Task<List<Product>?> PCGetAllProductAsync()
        {
            return await _productService.GetAllProductAsync();
        }

    }
}

using Microsoft.EntityFrameworkCore;
using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Data.Entities;
using QuanLyBanHang.BlazorServer.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace QuanLyBanHang.BlazorServer.Services
{
    public class PosService
    {
        private readonly ApplicationDbContext db;
        private readonly ProtectedLocalStorage localStorage;

        public PosService(ApplicationDbContext db, ProtectedLocalStorage localStorage)
        {
            this.db = db;
            this.localStorage = localStorage;
        }

        public async Task<POSViewModel> LoadAsync()
        {
            Employee? currentEmployee = null;
            
            // Lấy thông tin nhân viên từ localStorage
            try
            {
                var employeeNameResult = await localStorage.GetAsync<string>("EmployeeName");
                if (employeeNameResult.Success && !string.IsNullOrEmpty(employeeNameResult.Value))
                {
                    currentEmployee = db.Employees.FirstOrDefault(e => e.FullName == employeeNameResult.Value);
                }
            }
            catch { }

            // Nếu không tìm thấy, lấy nhân viên đầu tiên (fallback)
            if (currentEmployee == null)
            {
                currentEmployee = db.Employees.FirstOrDefault();
            }

            return new POSViewModel
            {
                Products = db.Products.Include(p => p.Categories).ToList(),
                Categories = db.Categories.ToList(),
                Customers = db.Customers.ToList(),
                CurrentEmployee = currentEmployee,
                EmployeeId = currentEmployee?.EmployeeId ?? 0
            };
        }

        public (bool ok, string msg, decimal percent) ValidateVoucher(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                return (false, "Vui lòng nhập mã voucher.", 0m);
            }

            var voucher = db.Vouchers.FirstOrDefault(v => v.Code == code);
            if (voucher == null)
            {
                return (false, "Mã voucher không tồn tại.", 0m);
            }

            var now = DateTime.UtcNow;
            if (!voucher.IsActive || 
                voucher.StartDate.ToUniversalTime() > now || 
                voucher.EndDate.ToUniversalTime() < now)
            {
                return (false, "Voucher đã hết hạn hoặc chưa được kích hoạt.", 0m);
            }

            if (voucher.UsedCount >= voucher.Quantity)
            {
                return (false, "Voucher đã hết lượt sử dụng.", 0m);
            }

            return (true, $"Áp dụng giảm {voucher.DiscountPercent}%", voucher.DiscountPercent);
        }

        public Order Checkout(int employeeId, int customerId, string? voucherCode,
            string paymentMethod, decimal cashGiven, List<CartItemDTO> items)
        {
            if (items == null || !items.Any())
            {
                throw new Exception("Giỏ hàng không được để trống!");
            }

            using var tran = db.Database.BeginTransaction();
            try
            {
                // 1. Tạo Order
                var order = new Order
                {
                    OrderDate = DateTime.Now,
                    EmployeeId = employeeId,
                    CustomerId = customerId == 0 ? null : customerId,
                    PaymentMethod = string.IsNullOrWhiteSpace(paymentMethod) ? "Cash" : paymentMethod,
                    CashGiven = Math.Max(0, cashGiven),
                    PaymentStatus = "Completed"
                };

                db.Orders.Add(order);
                db.SaveChanges();

                // 2. Xử lý OrderDetails và trừ kho
                decimal calculatedTotal = 0;
                foreach (var item in items)
                {
                    var product = db.Products.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product == null)
                        throw new Exception($"Sản phẩm ID {item.ProductId} không tồn tại.");

                    if (product.Stock < item.Quantity)
                        throw new Exception($"Sản phẩm '{product.Name}' không đủ hàng trong kho (Còn: {product.Stock}).");

                    var detail = new OrderDetail
                    {
                        OrderId = order.OrderId,
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price
                    };

                    calculatedTotal += (detail.Quantity * detail.UnitPrice);
                    db.OrderDetails.Add(detail);

                    // Trừ kho tổng
                    product.Stock -= item.Quantity;

                    // Trừ kho chi tiết (WarehouseId = 1)
                    var inventory = db.Inventories
                        .FirstOrDefault(i => i.ProductId == item.ProductId && i.WarehouseId == 1);
                    if (inventory != null)
                    {
                        if (inventory.Quantity < item.Quantity)
                            throw new Exception($"Lỗi kho: Sản phẩm '{product.Name}' không đủ hàng trong kho 1.");
                        inventory.Quantity -= item.Quantity;
                    }
                }

                // 3. Áp dụng Voucher (nếu có)
                decimal discountAmount = 0;
                string? appliedVoucher = null;
                if (!string.IsNullOrWhiteSpace(voucherCode))
                {
                    var voucher = db.Vouchers.FirstOrDefault(v => v.Code == voucherCode);
                    var now = DateTime.UtcNow;

                    if (voucher != null &&
                        voucher.IsActive &&
                        voucher.StartDate.ToUniversalTime() <= now &&
                        voucher.EndDate.ToUniversalTime() >= now &&
                        voucher.UsedCount < voucher.Quantity)
                    {
                        appliedVoucher = voucher.Code;
                        var percent = voucher.DiscountPercent;
                        discountAmount = Math.Min(calculatedTotal, Math.Floor(calculatedTotal * percent / 100));
                        voucher.UsedCount += 1;
                    }
                }

                // 4. Cập nhật tổng tiền và kết thúc
                var finalTotal = Math.Max(0, calculatedTotal - discountAmount);
                var change = Math.Max(0, cashGiven - finalTotal);

                order.DiscountAmount = discountAmount;
                order.VoucherCode = appliedVoucher;
                order.TotalAmount = finalTotal;
                order.ChangeAmount = change;

                db.SaveChanges();
                tran.Commit();
                return order;
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }

        public Order GetReceipt(int id)
        {
            return db.Orders
                .Include(o => o.OrderDetails).ThenInclude(d => d.Product)
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .FirstOrDefault(o => o.OrderId == id) 
                ?? throw new Exception("Không tìm thấy hóa đơn.");
        }
    }
}

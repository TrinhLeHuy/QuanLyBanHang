using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using QuanLyBanHang.Web.Models;
using QuanLyBanHang.Data.Entities;
using System;
using Microsoft.EntityFrameworkCore;
using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Web.DTO;

namespace QuanLyBanHang.Web.Controllers
{
    public class POSController : Controller
{
    private readonly ApplicationDbContext db; // Thay bằng tên Context thực tế

    public POSController(ApplicationDbContext db)
    {
        this.db = db;
    }

    // [HttpGet] Index - Action load dữ liệu ban đầu cho View
    public IActionResult Index()
    {
        // Lấy thông tin nhân viên hiện tại từ session
        var employeeName = HttpContext.Session.GetString("EmployeeName");
        Employee? currentEmployee = null;
        
        if (!string.IsNullOrEmpty(employeeName))
        {
            currentEmployee = db.Employees.FirstOrDefault(e => e.FullName == employeeName);
        }
        
        // Nếu không tìm thấy, lấy nhân viên đầu tiên (fallback)
        if (currentEmployee == null)
        {
            currentEmployee = db.Employees.FirstOrDefault();
        }

        var viewModel = new POSViewModel
        {
            Products = db.Products.Include(p => p.Categories).ToList(),
            Categories = db.Categories.ToList(),
            CurrentEmployee = currentEmployee,
            Customers = db.Customers.ToList(),
            EmployeeId = currentEmployee?.EmployeeId ?? 0
        };

        return View(viewModel);
    }

    // [HttpPost] Checkout - Xử lý lưu hóa đơn
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Checkout(CheckoutFormRequest request)
    {
        if (string.IsNullOrEmpty(request.ItemsJson))
        {
            TempData["ErrorMessage"] = "Giỏ hàng không được để trống!";
            return RedirectToAction("Index");
        }

        // 1. DESERIALIZE DỮ LIỆU
        // Chuyển chuỗi JSON thành List<CartItemDTO>
        var cartItems = JsonSerializer.Deserialize<List<QuanLyBanHang.Web.DTO.CartItemDTO>>(request.ItemsJson);

        if (cartItems == null || !cartItems.Any())
        {
            TempData["ErrorMessage"] = "Giỏ hàng không được để trống!";
            return RedirectToAction("Index");
        }

        // Bắt đầu Transaction để đảm bảo tính toàn vẹn dữ liệu
        using (var transaction = db.Database.BeginTransaction())
        {
            try
            {
                // 2. KHỞI TẠO VÀ LƯU ORDER
                var newOrder = new Order
                {
                    OrderDate = DateTime.Now,
                    EmployeeId = request.EmployeeId,
                    CustomerId = request.CustomerId == 0 ? null : request.CustomerId,
                    TotalAmount = 0, // Sẽ tính toán lại
                    DiscountAmount = 0,
                    VoucherCode = null,
                    CashGiven = Math.Max(0, request.CashGiven),
                    ChangeAmount = 0,
                    PaymentMethod = string.IsNullOrWhiteSpace(request.PaymentMethod) ? "Cash" : request.PaymentMethod,
                    PaymentStatus = "Completed"
                    // Các field khác: PaymentMethodId, Status, v.v.
                };

                db.Orders.Add(newOrder);
                db.SaveChanges(); // Lưu để lấy OrderId

                decimal calculatedTotal = 0;
                decimal discountAmount = 0;
                string? appliedVoucher = null;
                
                // 3. XỬ LÝ ORDER DETAILS VÀ TRỪ KHO
                foreach (var item in cartItems)
                {
                    // Lấy thông tin sản phẩm TỪ DATABASE để đảm bảo giá và tồn kho chính xác
                    var product = db.Products.FirstOrDefault(p => p.Id == item.ProductId);
                    
                    if (product == null) throw new Exception($"Sản phẩm ID {item.ProductId} không tồn tại.");
                    
                    // Kiểm tra tồn kho tổng (theo cấu trúc DB trong file SQL của bạn)
                    if (product.Stock < item.Quantity)
                    {
                        throw new Exception($"Sản phẩm '{product.Name}' không đủ hàng trong kho (Còn: {product.Stock}).");
                    }
                    
                    // a) Tạo OrderDetail
                    var detail = new OrderDetail
                    {
                        OrderId = newOrder.OrderId,
                        ProductId = product.Id,
                        Quantity = item.Quantity,
                        UnitPrice = product.Price // Lấy giá từ DB
                    };

                    calculatedTotal += (detail.Quantity * detail.UnitPrice);
                    db.OrderDetails.Add(detail);

                    // b) Trừ kho tổng (Bảng Products)
                    product.Stock -= item.Quantity;

                    // c) TRỪ KHO CHI TIẾT (Bảng Inventories/ProductWarehouses)
                    // Giả sử bán từ WarehouseId = 1
                    var inventory = db.Inventories
                                      .FirstOrDefault(i => i.ProductId == item.ProductId && i.WarehouseId == 1); 
                    
                    if (inventory != null)
                    {
                        if (inventory.Quantity < item.Quantity)
                        {
                            // Lỗi này cần được xử lý sớm hơn ở client, nhưng vẫn check tại server
                            throw new Exception($"Lỗi kho: Sản phẩm '{product.Name}' không đủ hàng trong kho 1.");
                        }
                        inventory.Quantity -= item.Quantity;
                    }
                    // db.Entry(product).State = EntityState.Modified; // Cần thiết trong một số ORM
                }

                // 3.5 ÁP DỤNG VOUCHER (nếu có)
                if (!string.IsNullOrWhiteSpace(request.VoucherCode))
                {
                    var voucher = db.Vouchers.FirstOrDefault(v => v.Code == request.VoucherCode);
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
                
                // 4. CẬP NHẬT TỔNG TIỀN VÀ KẾT THÚC
                var finalTotal = Math.Max(0, calculatedTotal - discountAmount);
                var cashGiven = Math.Max(0, request.CashGiven);
                var change = Math.Max(0, cashGiven - finalTotal);

                newOrder.DiscountAmount = discountAmount;
                newOrder.VoucherCode = appliedVoucher;
                newOrder.TotalAmount = finalTotal;
                newOrder.CashGiven = cashGiven;
                newOrder.ChangeAmount = change;
                
                db.SaveChanges();
                transaction.Commit();

                return RedirectToAction("Receipt", new { id = newOrder.OrderId });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                TempData["ErrorMessage"] = "Lỗi thanh toán: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
    }
    
    // [HttpGet] Validate voucher code
    [HttpGet]
    public IActionResult ValidateVoucher(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return Json(new { success = false, message = "Vui lòng nhập mã voucher." });
        }

        var voucher = db.Vouchers.FirstOrDefault(v => v.Code == code);
        if (voucher == null)
        {
            return Json(new { success = false, message = "Mã voucher không tồn tại." });
        }

        var now = DateTime.UtcNow;
        if (!voucher.IsActive || voucher.StartDate.ToUniversalTime() > now || voucher.EndDate.ToUniversalTime() < now)
        {
            return Json(new { success = false, message = "Voucher đã hết hạn hoặc chưa được kích hoạt." });
        }

        if (voucher.UsedCount >= voucher.Quantity)
        {
            return Json(new { success = false, message = "Voucher đã hết lượt sử dụng." });
        }

        return Json(new
        {
            success = true,
            discountPercent = voucher.DiscountPercent,
            message = $"Áp dụng giảm {voucher.DiscountPercent}%"
        });
    }

    [HttpGet]
    public IActionResult Receipt(int id)
    {
        var order = db.Orders
            .Include(o => o.OrderDetails).ThenInclude(od => od.Product)
            .Include(o => o.Customer)
            .Include(o => o.Employee)
            .FirstOrDefault(o => o.OrderId == id);

        if (order == null)
        {
            TempData["ErrorMessage"] = "Không tìm thấy hóa đơn.";
            return RedirectToAction("Index");
        }

        return View(order);
    }
}
}
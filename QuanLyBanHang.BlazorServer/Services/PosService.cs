using Microsoft.EntityFrameworkCore;
using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Data.Entities;
using QuanLyBanHang.Web.Models;

public class PosService
{
    private readonly ApplicationDbContext db;

    public PosService(ApplicationDbContext db)
    {
        this.db = db;
    }

    public POSViewModel Load()
    {
        var emp = db.Employees.First();
        return new POSViewModel
        {
            Products = db.Products.Include(p => p.Categories).ToList(),
            Categories = db.Categories.ToList(),
            Customers = db.Customers.ToList(),
            CurrentEmployee = emp,
            EmployeeId = emp.EmployeeId
        };
    }

    public (bool ok, string msg, decimal percent) ValidateVoucher(string code)
    {
        var v = db.Vouchers.FirstOrDefault(x => x.Code == code);

        if (v == null)
            return (false, "Voucher không tồn tại", 0m);

        if (!v.IsActive)
            return (false, "Voucher không hợp lệ", 0m);

        return (true, $"Giảm {v.DiscountPercent}%", v.DiscountPercent);
    }

    public Order Checkout(int employeeId, int customerId, string? voucher,
        string paymentMethod, decimal cashGiven, List<CartItemDTO> items)
    {
        using var tran = db.Database.BeginTransaction();
        try
        {
            var order = new Order
            {
                OrderDate = DateTime.Now,
                EmployeeId = employeeId,
                CustomerId = customerId == 0 ? null : customerId,
                PaymentMethod = paymentMethod,
                CashGiven = cashGiven,
                PaymentStatus = "Completed"
            };

            db.Orders.Add(order);
            db.SaveChanges();

            decimal total = 0;
            foreach (var i in items)
            {
                var p = db.Products.First(x => x.Id == i.ProductId);
                if (p.Stock < i.Quantity) throw new Exception("Không đủ kho");

                db.OrderDetails.Add(new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = p.Id,
                    Quantity = i.Quantity,
                    UnitPrice = p.Price
                });

                p.Stock -= i.Quantity;
                total += i.Quantity * p.Price;
            }

            order.TotalAmount = total;
            order.ChangeAmount = Math.Max(0, cashGiven - total);
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
            .First(o => o.OrderId == id);
    }
}
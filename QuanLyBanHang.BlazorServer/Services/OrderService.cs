using Microsoft.EntityFrameworkCore;
using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Data.Entities;
using QuanLyBanHang.Data.Enums;

namespace QuanLyBanHang.BlazorServer.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        public OrderService(ApplicationDbContext context) 
        {
            _context=context;
        }
        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.Orders.Include(o => o.Customer).ToListAsync();
        }
        public async Task<List<Order>?> GetAllWithUserIdAsync(int userId)
        {
            return await _context.Orders.Include(o => o.Customer).Where(o => o.CustomerId == userId ).ToListAsync();
        }
        public async Task<Order?> GetByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .Include(o => o.Employee)
                .Include(o => o.Customer)
                .Include(o => o.Voucher)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
            
        }

        public async Task<List<Order>?> GetByFilterNameAsync(string status, string filterName)
        {
            if (filterName == null) return null;
            if (status  == "Tất cả")
                return await _context.Orders
                    .Include(o => o.Customer)
                    .Where(o => o.Customer != null && o.Customer.FullName.Contains(filterName))
                    .ToListAsync();
            else
                return await _context.Orders
                    .Include(o => o.Customer)
                    .Where(o => o.Customer != null && o.Customer.FullName.Contains(filterName) && o.Status.ToString() == status)
                    .ToListAsync();
        }

        public async Task<Order?> CreateAsync(Order order)
        {
            if(order == null) return null;
            order.OrderDetails = order.OrderDetails.Where(d => d != null).ToList();
            order.OrderDate = DateTime.Now;
            var temp = _context.Vouchers.FirstOrDefault(v => v.Id == order.VoucherId);
            order.TotalAmount = order.OrderDetails.Sum(d => d.Quantity * d.UnitPrice);
            if (temp != null)
            {
                order.DiscountAmount = temp.DiscountType == DiscountType.Percent ? order.TotalAmount * (temp.DiscountValue) : temp.DiscountValue;
                order.TotalAmount -= order.DiscountAmount;
            }
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> UpdateAsync(Order order)
        {
            if (order == null) return null;
            var target = await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == order.OrderId);
            if(target == null) return null;
            target.Status = order.Status;
            if(order.Status == OrderStatus.Paid)
            {
                foreach (var item in order.OrderDetails)
                {
                    item.Product.Stock-=item.Quantity;
                }
            }
            await _context.SaveChangesAsync();
            return target;
        }

        public async Task<bool> DeleteAsync(Order order)
        {
            if (order == null) return false;
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Order>?> GetByFilterStatusAsync(string Status)
        {
            if (Status == "Tất cả") return await GetAllAsync();
            return await _context.Orders
                .Include(o => o.Customer)
                .Where( o => o.Status.ToString() == Status)
                .ToListAsync();
        }
        public async Task<List<Order>?> GetByFilterStatusWithUserIdAsync(string Status, int userId)
        {
            if (Status == "Tất cả") return await GetAllWithUserIdAsync(userId);
            return await _context.Orders
                .Include(o => o.Customer)
                .Where(o => o.Status.ToString() == Status && o.CustomerId == userId)
                .ToListAsync();
        }

        public async Task<List<Order>?> GetByFilterDateAsync(string status, DateTime startTime, DateTime endTime)
        {
            if (status == "Tất cả")
                return await _context.Orders
                    .Include(o => o.Customer)
                    .Where(o => o.OrderDate >= startTime && o.OrderDate <= endTime)
                    .ToListAsync();
            else
                return await _context.Orders
                    .Include(o => o.Customer)
                    .Where(o => o.OrderDate >= startTime && o.OrderDate <= endTime && o.Status.ToString() == status)
                    .ToListAsync();
        }

        public async Task<List<Order>?> GetByFilterDateWithUserIdAsync(string status, DateTime startTime, DateTime endTime, int userId)
        {
            if (status == "Tất cả")
                return await _context.Orders
                    .Include(o => o.Customer)
                    .Where(o => o.OrderDate >= startTime && o.OrderDate <= endTime && o.CustomerId == userId)
                    .ToListAsync();
            else
                return await _context.Orders
                    .Include(o => o.Customer)
                    .Where(o => o.OrderDate >= startTime && o.OrderDate <= endTime && o.Status.ToString() == status && o.CustomerId == userId)
                    .ToListAsync();
        }

    }
}

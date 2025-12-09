using Microsoft.EntityFrameworkCore;
using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Data.Entities;

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
        public async Task<Order?> GetByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderDetails)
                .Include(o => o.Employee)
                .Include(o => o.Customer)
                .Include(o => o.Voucher)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
            
        }

        public async Task<List<Order>?> GetByFilterNameAsync(string filterName)
        {
            if (filterName == null) return null;
            return await _context.Orders
                .Include(o => o.Customer)
                .Where(o => o.Customer != null && o.Customer.FullName.Contains(filterName))
                .ToListAsync();
        }

        public async Task<Order?> CreateAsync(Order order)
        {
            if(order == null) return null;
            order.OrderDetails= order.OrderDetails.Where(d => d != null).ToList();
            order.OrderDate = DateTime.Now;
            order.TotalAmount = order.OrderDetails.Sum(d => d.Quantity * d.UnitPrice);
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> UpdateAsync(Order order)
        {
            if (order == null) return null;
            var target = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == order.OrderId);
            if(target == null) return null;
            target.Status = order.Status;
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

    }
}

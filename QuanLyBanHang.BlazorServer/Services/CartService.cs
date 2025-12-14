using Microsoft.EntityFrameworkCore;
using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Data.Entities;
using System.Linq;

namespace QuanLyBanHang.BlazorServer.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;
        public CartService(ApplicationDbContext context) 
        {
            _context= context;
        }
        public async Task<List<CartItems>> GetAllByCustomerIdAsync(int customerId)
        {
            return await _context.CartItems
                .Where(i => i.CustomerId == customerId)
                .Include(i => i.Product)
                .ToListAsync();
        }
        public async Task<CartItems?> CreateAsync(CartItems cartItem)
        {
            var existsItem = await _context.CartItems.FirstOrDefaultAsync(i => i.CustomerId == cartItem.CustomerId && i.ProductId == cartItem.ProductId);
            if (existsItem!=null)
            {
                existsItem.Quantity += cartItem.Quantity;
                await _context.SaveChangesAsync();
                return existsItem;
            }
            else
            {
                _context.CartItems.Add(cartItem);
                await _context.SaveChangesAsync();
                return cartItem;
            }
        }
        public async Task<CartItems?> UpdpateAsync(int cartItemId, int quantity)
        {
            var existsItem = await _context.CartItems.FirstOrDefaultAsync(i => i.Id == cartItemId);
            if(existsItem==null) return null;
            else
            {
                existsItem.Quantity = quantity;
                await _context.SaveChangesAsync();
                return existsItem;
            }
        }
        public async Task<bool> DeleteByIdAsync(int cartItemId)
        {
            var existsItem = await _context.CartItems.FirstOrDefaultAsync(i => i.Id == cartItemId);
            if (existsItem == null) return false;
            else
            {
                _context.Remove(existsItem);
                await _context.SaveChangesAsync();
                return true;
            }
        }
        public async Task<bool> DeleteByListIdAsync(int customerId, List<int> listItemId)
        {
            var existsList = await _context.CartItems
                .Where(i => i.CustomerId == customerId)
                .Where(i => listItemId.Contains(i.Id))
                .ExecuteDeleteAsync();
            return existsList > 0;
        }
    }
}

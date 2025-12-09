using Microsoft.EntityFrameworkCore;
using QuanLyBanHang.Data.Entities;

namespace QuanLyBanHang.BlazorServer.Services
{
    public interface IProductService
    {
        public Task<List<Product>> GetAllProductAsync();
    }
}

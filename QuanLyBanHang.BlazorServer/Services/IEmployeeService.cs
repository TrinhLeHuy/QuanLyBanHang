using QuanLyBanHang.Data.Entities;

namespace QuanLyBanHang.BlazorServer.Services
{
    public interface IEmployeeService
    {
        public Task<List<Employee>> GetAllAsync();
    }
}

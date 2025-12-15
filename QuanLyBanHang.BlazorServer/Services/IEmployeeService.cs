using QuanLyBanHang.Data.Entities;

namespace QuanLyBanHang.BlazorServer.Services
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllAsync();

        Task<Employee?> GetByIdAsync(int id);

        Task<bool> EmailExistsAsync(string email);

        Task CreateAsync(Employee employee, string password); 

        Task UpdateAsync(Employee employee);

        Task DeleteAsync(int id);
    }
}

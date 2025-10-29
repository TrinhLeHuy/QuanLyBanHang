using Microsoft.EntityFrameworkCore;
using QuanLyBanHang.Data.Entities;
using System.Security.Cryptography;
using System.Text;

namespace QuanLyBanHang.Data.DataContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = "server=localhost;database=QuanLyBanHang;user=root;password=;";
                optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36)));
            }
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Inventory> Inventories { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 🔐 Hash mật khẩu admin bằng SHA256
            string hashedPassword = ComputeSha256Hash("123456");

            // Seed admin mặc định
            modelBuilder.Entity<Employee>().HasData(new Employee
            {
                EmployeeId = 1,
                FullName = "Admin",
                Email = "admin@shop.com",
                PasswordHash = hashedPassword,
                Role = "Admin",
                Phone = "0900000000"
            });
        }

        // 🧩 Hàm mã hóa SHA256 (giống AuthController)
        private static string ComputeSha256Hash(string rawData)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                    builder.Append(b.ToString("x2"));
                return builder.ToString();
            }
        }
    }
}

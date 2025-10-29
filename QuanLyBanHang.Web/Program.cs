using Microsoft.EntityFrameworkCore;
using QuanLyBanHang.Data.DataContext;
using QuanLyBanHang.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 🟢 Đăng ký các dịch vụ cần thiết
builder.Services.AddControllersWithViews();

// 🟢 Đăng ký Repository
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<VoucherRepository>();
builder.Services.AddScoped<OrderRepository>();

// 🟢 Thêm Session để lưu thông tin đăng nhập
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

// ✅ Cấu hình MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// ====================================================

var app = builder.Build();

// ====================================================
// 🔥 Cấu hình pipeline
// ====================================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 🟢 Bắt buộc thêm dòng này TRƯỚC Authorization
app.UseSession();

app.UseAuthorization();

// ✅ Đặt trang đăng nhập là mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();

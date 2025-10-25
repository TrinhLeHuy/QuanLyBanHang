# 🛒 Quản Lý Bán Hàng (.NET + MySQL + XAMPP)

## 📘 Giới thiệu

Dự án **Quản Lý Bán Hàng** là một ứng dụng web được xây dựng bằng **ASP.NET Core MVC** và **Entity Framework Core**, giúp quản lý thông tin **khách hàng, sản phẩm, đơn hàng, nhân viên và phiếu giảm giá (voucher)**.  
Dự án sử dụng **MySQL (XAMPP)** làm cơ sở dữ liệu và có thể mở rộng cho các mục đích học tập hoặc thương mại nhỏ.

---

## 🚀 Công nghệ sử dụng

- **.NET 8 / ASP.NET Core MVC**
- **Entity Framework Core**
- **Pomelo.EntityFrameworkCore.MySql**
- **XAMPP (MySQL Server)**
- **Visual Studio / VS Code**
- **GitHub**

---

## ⚙️ Cấu trúc thư mục
QuanLyBanHang/
│
├── QuanLyBanHang.sln
├── QuanLyBanHang.Web/ → Web layer (Controllers, Views, Program.cs)
└── QuanLyBanHang.Data/ → Data layer (DbContext, Entities, Migrations)

---

## 🧩 Hướng dẫn cài đặt

### 1️⃣ Cài đặt công cụ cần thiết
- Cài **.NET SDK 8.0+**  
  👉 [https://dotnet.microsoft.com/download](https://dotnet.microsoft.com/download)
- Cài **XAMPP** (chạy Apache + MySQL)  
  👉 [https://www.apachefriends.org/download.html](https://www.apachefriends.org/download.html)
- Cài **Visual Studio** hoặc **VS Code**
- Đảm bảo MySQL đã **chạy** trong XAMPP Control Panel

---

### 2️⃣ Tạo cơ sở dữ liệu MySQL

Mở **phpMyAdmin** tại:  
👉 [http://localhost/phpmyadmin](http://localhost/phpmyadmin)

Chạy lệnh SQL sau để tạo database:

```sql
CREATE DATABASE QuanLyBanHang CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
```
3️⃣ Cập nhật chuỗi kết nối

"ConnectionStrings": {
  "DefaultConnection": "server=localhost;port=3306;database=QuanLyBanHang;user=root;password="
}
4️⃣ Chạy migration (tạo bảng tự động)

D:\.Net\QuanLyBanHang\QuanLyBanHang.Web
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate --project ../QuanLyBanHang.Data
dotnet ef database update --project ../QuanLyBanHang.Data
Applying migration 'InitialCreate'
Done.

5️⃣ Chạy ứng dụng web

dotnet run
https://localhost:5001
hoặc
http://localhost:5000

🧱 Các bảng trong cơ sở dữ liệu

Customers — Thông tin khách hàng

Products — Sản phẩm

Orders — Đơn hàng

OrderDetails — Chi tiết đơn hàng

Vouchers — Phiếu giảm giá

Employees — Nhân viên

💻 Góp ý & Phát triển

Clone dự án về máy:

git clone https://github.com/TrinhLeHuy/QuanLyBanHang.git
cd QuanLyBanHang


Tạo nhánh riêng để phát triển thêm:

git checkout -b feature/add-product-ui

👨‍💻 Tác giả

Trịnh Lê Huy
📧 Email: huytrinh.22032004@gmail.com
📂 GitHub: @TrinhLeHuy


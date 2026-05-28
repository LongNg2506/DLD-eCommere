# ChronoStore - Đồng Hồ Cao Cấp

Dự án e-commerce ASP.NET Core MVC .NET 9.0 cho cửa hàng đồng hồ cao cấp.

## Yêu cầu hệ thống

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (LocalDB cũng được)
- [Git](https://git-scm.com/)

## Cách clone và chạy

### 1. Clone dự án

```bash
git clone https://github.com/LongNg2506/DLD-eCommere.git
cd DLD-eCommere
```

### 2. Cấu hình Database

Mở file `src/appsettings.json`, sửa connection string phù hợp với SQL Server của bạn:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=MyProjectDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

**Các lựa chọn connection string:**

- **Windows Authentication (khuyên dùng):**
  ```json
  "Server=localhost;Database=MyProjectDb;Trusted_Connection=True;TrustServerCertificate=True;"
  ```

- **SQL Server Authentication:**
  ```json
  "Server=localhost;Database=MyProjectDb;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
  ```

- **SQL Server LocalDB:**
  ```json
  "Server=(localdb)\\mssqllocaldb;Database=MyProjectDb;Trusted_Connection=True;TrustServerCertificate=True;"
  ```

> **Lưu ý:** Nếu dùng SQL Server Authentication, hãy tạo database `MyProjectDb` trước trong SQL Server Management Studio.

### 3. Tạo database và chạy migration

```bash
cd src
dotnet ef database update
```

Lệnh này sẽ:
- Tạo database `MyProjectDb` trong SQL Server
- Tạo toàn bộ bảng cần thiết
- Seed dữ liệu mẫu (users, categories, products)

### 4. Chạy ứng dụng

```bash
cd src
dotnet run
```

Ứng dụng sẽ khởi động tại **http://localhost:5192**

---

## Dữ liệu mẫu có sẵn

Sau khi chạy `dotnet ef database update`, database sẽ tự động có:

### Tài khoản đăng nhập

| Role | Email | Password |
|------|-------|----------|
| **Admin** | admin@chronostore.com | `Admin123!` |
| **Staff** | staff@chronostore.com | `Staff123!` |
| **Customer** | customer@chronostore.com | `Customer123!` |

### Danh mục (3 danh mục)

- Men's Watches
- Women's Watches
- Couple Watches

### Sản phẩm (36 sản phẩm)

- Đã có hình ảnh từ Unsplash
- Đã có giá và % giảm giá
- Đã có số lượng tồn kho

---

## Cấu trúc trang

### Khách hàng (Customer)
- `/` - Trang chủ
- `/Shop` - Danh sách sản phẩm
- `/Shop/ProductDetail/{id}` - Chi tiết sản phẩm
- `/Cart` - Giỏ hàng
- `/Checkout` - Thanh toán
- `/Checkout/Confirmation?orderId={id}` - Xác nhận đơn hàng
- `/Account/Profile` - Tài khoản & đơn hàng của tôi

### Quản trị (Admin/Staff)
- `/Admin` - Dashboard
- `/Admin/Products` - Quản lý sản phẩm
- `/Admin/Categories` - Quản lý danh mục
- `/Admin/Orders` - Quản lý đơn hàng
- `/Admin/Users` - Quản lý khách hàng
- `/Admin/Reports` - Báo cáo doanh thu
- `/Admin/Settings` - Cấu hình cửa hàng

---

## Các câu lệnh hữu ích

```bash
# Tạo migration mới (sau khi sửa model)
dotnet ef migrations add TênMigration

# Cập nhật database
dotnet ef database update

# Reset database (xóa và tạo lại từ đầu)
dotnet ef database drop --force
dotnet ef database update

# Xem danh sách migration
dotnet ef migrations list

# Build dự án
dotnet build

# Chạy tests (nếu có)
dotnet test
```

---

## Kiến trúc kỹ thuật

| Thành phần | Công nghệ |
|---|---|
| Framework | ASP.NET Core MVC .NET 9.0 |
| Database | SQL Server + Entity Framework Core |
| Authentication | Cookie-based (ChronoAuth) |
| UI | Razor View + Custom CSS |
| Password Hashing | BCrypt |
| Image Storage | Unsplash URLs (external) |
| Session | ASP.NET Core Distributed Memory Cache |

---

## Các bước reset dữ liệu

Nếu muốn xóa toàn bộ data và seed lại:

```bash
cd src
dotnet ef database drop --force
dotnet ef database update
```

---

## Xử lý lỗi thường gặp

**Lỗi "address already in use" (port 5192 đang bị chiếm)**
```bash
# Tìm và kill process đang dùng port 5192
netstat -ano | findstr 5192
taskkill //F //PID <PID_NUMBER>
```

**Lỗi "Cannot open database"**
- Kiểm tra SQL Server đang chạy
- Kiểm tra connection string đúng chưa
- Đảm bảo database `MyProjectDb` đã được tạo

**Lỗi build**
```bash
dotnet restore
dotnet build
```

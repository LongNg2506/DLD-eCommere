CLAUDE.md
Tổng quan dự án
Đây là template ASP.NET Core .NET 10 cho dự án web/app quản trị hoặc e-commerce, sử dụng C#, SQL Server, Entity Framework Core, ASP.NET Core MVC hoặc Web API, Razor View nếu làm MVC, và kiến trúc theo pattern nhiều feature.
Mỗi feature nên có cấu trúc riêng gồm:
Entity / Model
DTO hoặc ViewModel
Service layer
Controller
Razor View nếu là MVC
API endpoint nếu là Web API
Validation
Migration nếu có thay đổi database
Mục tiêu của dự án là giữ code rõ ràng, dễ bảo trì, dễ mở rộng và không nhồi toàn bộ logic vào Controller.
---
Tech Stack
Category	Choice
Framework	ASP.NET Core .NET 10
Language	C#
Web Pattern	ASP.NET Core MVC hoặc ASP.NET Core Web API
View Engine	Razor View / Razor Pages nếu cần
Database	SQL Server
ORM	Entity Framework Core
Validation	Data Annotations / FluentValidation nếu có
Auth	ASP.NET Core Identity hoặc JWT Bearer
UI	Bootstrap / Tailwind / theme HTML tùy dự án
API Docs	Swagger / Scalar nếu là Web API
Config	appsettings.json + User Secrets / Env Vars
Logging	Built-in Logging / Serilog nếu có
Testing	xUnit / NUnit / MSTest nếu có
---
## UI Theme / Color System

Dự án sử dụng 2 bộ theme chính: **Light Mode** và **Dark Mode**. Khi xây dựng giao diện, cần ưu tiên phong cách hiện đại, tối giản, rõ ràng và đồng bộ màu sắc theo hệ thống dưới đây.

---

### 1. Light Theme

| Vai trò màu | Mã màu | Cách dùng gợi ý |
|---|---:|---|
| Background chính | `#FFFFFF` | Nền chính của website |
| Text chính / Heading | `#5F5C6D` | Tiêu đề, nội dung quan trọng |
| Text phụ / Muted | `#ACA9BB` | Mô tả phụ, placeholder, metadata |
| Accent nhạt | `#E1FFFA` | Nền section phụ, badge, hover nhẹ |
| Primary / CTA | `#A9CEC2` | Button chính, icon nhấn mạnh, link active |

**Gợi ý sử dụng Light Theme:**
- Nền tổng thể dùng `#FFFFFF`.
- Tiêu đề và nội dung chính dùng `#5F5C6D`.
- Nội dung phụ dùng `#ACA9BB`.
- Các khối nổi bật nhẹ dùng nền `#E1FFFA`.
- Nút chính, trạng thái active hoặc điểm nhấn dùng `#A9CEC2`.

---

### 2. Dark Theme

| Vai trò màu | Mã màu | Cách dùng gợi ý |
|---|---:|---|
| Background chính | `#111111` | Nền chính dark mode |
| Surface / Card | `#474554` | Card, header, footer, block phụ |
| Text phụ / Muted | `#ACA9BB` | Mô tả phụ, thông tin ít quan trọng |
| Accent đậm | `#0A2B22` | Section nền tối có điểm nhấn |
| Primary / CTA | `#38594E` | Button chính, icon nhấn mạnh, link active |

**Gợi ý sử dụng Dark Theme:**
- Nền tổng thể dùng `#111111`.
- Card, navbar, footer hoặc khối nội dung dùng `#474554`.
- Text phụ dùng `#ACA9BB`.
- Section đặc biệt có thể dùng `#0A2B22`.
- Button chính và trạng thái active dùng `#38594E`.

---

## Responsive Design Guidelines

Dự án cần hỗ trợ giao diện responsive cho nhiều kích thước màn hình khác nhau, bao gồm: mobile, tablet, laptop và desktop. Khi xây dựng giao diện, luôn ưu tiên thiết kế theo hướng **mobile-first**, sau đó mở rộng dần cho màn hình lớn hơn.

### Breakpoint tham khảo

| Thiết bị | Kích thước tham khảo | Cách xử lý giao diện |
|---|---:|---|
| Mobile nhỏ | `< 576px` | 1 cột, nội dung gọn, font vừa phải |
| Mobile lớn | `576px - 767px` | 1 cột, có thể tăng spacing nhẹ |
| Tablet | `768px - 991px` | 2 cột nếu phù hợp |
| Laptop | `992px - 1199px` | 3 cột hoặc layout đầy đủ hơn |
| Desktop lớn | `>= 1200px` | Layout rộng, tối ưu khoảng trắng |

---

### Quy tắc responsive chung

- Giao diện phải hiển thị tốt trên cả mobile, tablet và desktop.
- Không để nội dung bị tràn ngang màn hình.
- Không dùng width cố định quá lớn như `width: 1200px`; ưu tiên `max-width`.
- Ưu tiên dùng `%`, `rem`, `em`, `fr`, `auto`, `minmax()` thay vì hard-code px quá nhiều.
- Hình ảnh sản phẩm, logo, banner phải dùng `max-width: 100%` và `height: auto`.
- Header/navbar trên mobile cần chuyển sang dạng menu gọn, hamburger hoặc layout xếp dọc.
- Các grid sản phẩm cần tự co giãn theo màn hình.
- Button, input, card phải đủ lớn để dễ thao tác trên mobile.
- Font chữ trên mobile không được quá nhỏ.
- Khoảng cách giữa các section cần giảm nhẹ trên mobile để tránh giao diện quá dài.

---

### Quy tắc cho eCommerce đồng hồ

- Trang sản phẩm trên mobile nên hiển thị 1 sản phẩm mỗi hàng.
- Tablet có thể hiển thị 2 sản phẩm mỗi hàng.
- Laptop/Desktop có thể hiển thị 3-4 sản phẩm mỗi hàng.
- Ảnh đồng hồ cần giữ tỉ lệ đẹp, không bị méo.
- Card sản phẩm phải gọn, rõ tên sản phẩm, giá, nút xem chi tiết hoặc mua hàng.
- Bộ lọc sản phẩm trên mobile có thể chuyển thành sidebar ẩn, accordion hoặc dropdown.
- Gallery/collection nên có layout đẹp trên desktop nhưng vẫn xếp gọn thành 1 cột trên mobile.
- Logo trong header cần co giãn theo chiều cao header, không bị vỡ bố cục.

---

### 3. Quy tắc thiết kế chung

- Không dùng màu gold làm màu chủ đạo.
- Không tự ý thêm quá nhiều màu ngoài hệ theme.
- Chỉ dùng thêm màu cảnh báo/lỗi khi thật sự cần thiết.
- Giao diện cần giữ cảm giác sang trọng, tối giản, sạch sẽ.
- Ưu tiên khoảng trắng rộng, bố cục dễ nhìn.
- Button, card, input nên có bo góc nhẹ và hiệu ứng hover mượt.
- Khi làm trang eCommerce đồng hồ, ưu tiên hình ảnh sản phẩm lớn, rõ nét, bố cục cao cấp.

Cấu trúc thư mục
```text
src/
  Controllers/
    HomeController.cs
    ProductsController.cs
    CustomersController.cs
    OrdersController.cs
    Admin/
      DashboardController.cs
      ProductsController.cs
      OrdersController.cs

  Models/
    Product.cs
    Customer.cs
    Order.cs
    OrderItem.cs
    Category.cs
    Brand.cs

  Data/
    AppDbContext.cs
    SeedData.cs

  DTOs/
    Products/
      ProductCreateDto.cs
      ProductUpdateDto.cs
      ProductDetailDto.cs
      ProductListItemDto.cs
    Orders/
      OrderCreateDto.cs
      OrderUpdateStatusDto.cs
      OrderDetailDto.cs

  ViewModels/
    Products/
      ProductIndexViewModel.cs
      ProductFormViewModel.cs
    Orders/
      OrderIndexViewModel.cs
      OrderDetailViewModel.cs

  Services/
    Products/
      IProductService.cs
      ProductService.cs
    Orders/
      IOrderService.cs
      OrderService.cs
    Customers/
      ICustomerService.cs
      CustomerService.cs

  Repositories/
    Products/
      IProductRepository.cs
      ProductRepository.cs

  Views/
    Shared/
      _Layout.cshtml
      _ValidationScriptsPartial.cshtml
    Home/
      Index.cshtml
    Products/
      Index.cshtml
      Details.cshtml
      Create.cshtml
      Edit.cshtml
      Delete.cshtml
    Orders/
      Index.cshtml
      Details.cshtml

  wwwroot/
    css/
    js/
    images/
    assets/

  Migrations/
```
> Nếu dự án là Web API thuần thì có thể không cần thư mục `Views/` và `wwwroot/`.
---
Feature Pattern
Mỗi feature nên theo cùng một cấu trúc để dễ mở rộng.
Ví dụ feature `Products`:
```text
Models/
  Product.cs

DTOs/
  Products/
    ProductCreateDto.cs
    ProductUpdateDto.cs
    ProductDetailDto.cs
    ProductListItemDto.cs

ViewModels/
  Products/
    ProductIndexViewModel.cs
    ProductFormViewModel.cs

Services/
  Products/
    IProductService.cs
    ProductService.cs

Controllers/
  ProductsController.cs

Views/
  Products/
    Index.cshtml
    Details.cshtml
    Create.cshtml
    Edit.cshtml
    Delete.cshtml
```
Nếu dùng Web API:
```text
Models/
  Product.cs

DTOs/
  Products/
    ProductCreateDto.cs
    ProductUpdateDto.cs
    ProductDetailDto.cs
    ProductListItemDto.cs

Services/
  Products/
    IProductService.cs
    ProductService.cs

Controllers/
  ProductsController.cs
```
---
Module / Feature hiện có
Các feature thường gặp trong dự án C# .NET + SQL Server:
Products
Categories
Brands
Suppliers
Customers
Orders
OrderItems
Payments
ShippingMethods
Shipments
Stocks
Users
Roles
Dashboard
Reports
Settings
Feature đầy đủ nên có:
Danh sách
Chi tiết
Thêm mới
Cập nhật
Xóa
Tìm kiếm
Lọc
Sắp xếp
Phân trang
Validation
Xử lý lỗi
Phân quyền nếu cần
---
Quy ước quan trọng
Entity / Model
Entity đặt trong thư mục `Models/`
Entity dùng số ít, PascalCase
Tên bảng trong database thường dùng số nhiều
Primary key dùng `Id`
Chuỗi tiếng Việt dùng `nvarchar`
Tiền tệ dùng `decimal(18,2)`
Ngày giờ dùng `datetime2`
Trạng thái dùng `bit`
Nên có `CreatedAt`
Nên có `UpdatedAt`
Nếu cần xóa mềm, dùng `IsDeleted`
Ví dụ:
```csharp
public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public int CategoryId { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? UpdatedAt { get; set; }

    public Category? Category { get; set; }
}
```
---
DbContext
DbContext đặt trong `Data/AppDbContext.cs`
Mỗi Entity cần có `DbSet`
Cấu hình quan hệ và kiểu dữ liệu trong `OnModelCreating`
Không đặt connection string trực tiếp trong DbContext
Ví dụ:
```csharp
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.Price)
                .HasColumnType("decimal(18,2)");
        });
    }
}
```
---
DTO / ViewModel
Không nên dùng Entity trực tiếp cho form hoặc API phức tạp
DTO dùng cho request/response API
ViewModel dùng cho Razor View
DTO nên tách theo mục đích: Create, Update, Detail, ListItem
Ví dụ:
```text
ProductCreateDto
ProductUpdateDto
ProductDetailDto
ProductListItemDto
ProductIndexViewModel
ProductFormViewModel
```
---
Service Layer
Service xử lý logic nghiệp vụ
Controller chỉ nhận request, gọi service và trả response
Không nhồi truy vấn phức tạp vào Controller
Service nên dùng async/await khi thao tác database
Service cần được đăng ký trong `Program.cs`
Ví dụ:
```csharp
builder.Services.AddScoped<IProductService, ProductService>();
```
---
Repository Pattern
Chỉ dùng Repository Pattern nếu dự án đã dùng hoặc người dùng yêu cầu.
Không tự ý thêm Repository Pattern vào dự án đơn giản.
Nếu có Repository:
```text
Repositories/
  Products/
    IProductRepository.cs
    ProductRepository.cs
```
Controller không nên gọi trực tiếp Repository nếu đã có Service.
Luồng nên là:
```text
Controller → Service → Repository → DbContext → SQL Server
```
Nếu không dùng Repository:
```text
Controller → Service → DbContext → SQL Server
```
---
ASP.NET Core MVC conventions
Khi dùng MVC:
Controller nằm trong `Controllers/`
View nằm trong `Views/{ControllerNameWithoutController}/{Action}.cshtml`
Shared layout nằm trong `Views/Shared/_Layout.cshtml`
Static files nằm trong `wwwroot/`
Form POST phải dùng `[ValidateAntiForgeryToken]`
View nên dùng ViewModel hoặc DTO phù hợp
Link nội bộ nên dùng Tag Helpers: `asp-controller`, `asp-action`, `asp-route-id`
Ví dụ route:
```text
ProductsController.Index()
→ Views/Products/Index.cshtml
```
Ví dụ form POST:
```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(ProductCreateDto dto)
{
    if (!ModelState.IsValid)
    {
        return View(dto);
    }

    await _productService.CreateAsync(dto);
    return RedirectToAction(nameof(Index));
}
```
---
ASP.NET Core Web API conventions
Khi dùng Web API:
Controller kế thừa `ControllerBase`
Dùng `[ApiController]`
Dùng `[Route("api/[controller]")]`
Trả dữ liệu bằng JSON
Dùng DTO cho request và response
Không trả Entity có navigation phức tạp trực tiếp
Dùng status code đúng: `Ok`, `CreatedAtAction`, `NoContent`, `BadRequest`, `NotFound`
Ví dụ:
```csharp
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }
}
```
---
Entity Framework Core conventions
Với .NET 10, ưu tiên EF Core 10.x
Dùng `Microsoft.EntityFrameworkCore.SqlServer`
Dùng `Microsoft.EntityFrameworkCore.Design` để tạo migration
Không dùng `DateTime.Now` trong `HasData`
Nếu seed data bằng `HasData`, dùng ngày cố định
Dùng `Include` khi cần navigation property
Dùng projection `.Select(...)` để trả DTO
Tránh trả object có vòng lặp navigation
Lệnh thường dùng:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```
Nếu chưa có EF tool:
```bash
dotnet tool install --global dotnet-ef
```
Nếu cần cập nhật EF tool:
```bash
dotnet tool update --global dotnet-ef
```
---
SQL Server conventions
Connection string đặt trong `appsettings.json`, User Secrets hoặc biến môi trường
Không hardcode connection string trong code
Database local có thể dùng `localhost`
Khi dùng Windows Authentication:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=YourDatabaseName;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```
Khi dùng SQL Authentication:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=YourDatabaseName;User Id=sa;Password=YourPassword;TrustServerCertificate=True;"
  }
}
```
Không commit mật khẩu thật lên Git.
---
Authentication / Authorization
Tùy dự án, có thể dùng:
ASP.NET Core Identity
JWT Bearer Authentication
Cookie Authentication
Custom login đơn giản cho project học tập
Quy ước:
Không lưu mật khẩu plain text
Mật khẩu phải được hash
Chức năng admin phải có `[Authorize]`
Chức năng theo role dùng `[Authorize(Roles = "Admin")]`
Không cho user thường gọi API hoặc action của admin
Không trả thông tin nhạy cảm trong lỗi đăng nhập
Ví dụ:
```csharp
[Authorize(Roles = "Admin")]
public async Task<IActionResult> Delete(int id)
{
    await _productService.DeleteAsync(id);
    return RedirectToAction(nameof(Index));
}
```
---
Validation
Validation phải có ở backend, không chỉ ở frontend.
Các rule thường gặp:
Tên không được rỗng
Email đúng định dạng
Số điện thoại đúng định dạng nếu có
Giá không được âm
Số lượng không được âm
Ngày bắt đầu không lớn hơn ngày kết thúc
Khóa ngoại phải tồn tại
Không cho xóa dữ liệu đang được sử dụng nếu có ràng buộc
Dùng Data Annotations:
```csharp
public class ProductCreateDto
{
    [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "Giá sản phẩm không được âm.")]
    public decimal Price { get; set; }
}
```
---
Error Handling
MVC
Nếu dữ liệu không hợp lệ, trả lại View cùng ModelState
Dùng `ModelState.AddModelError`
Thông báo lỗi phải dễ hiểu
Không hiển thị stack trace cho người dùng cuối
Ví dụ:
```csharp
ModelState.AddModelError("", "Không thể lưu dữ liệu. Vui lòng kiểm tra lại thông tin.");
return View(dto);
```
Web API
Response lỗi nên nhất quán:
```csharp
return BadRequest(new
{
    message = "Dữ liệu không hợp lệ.",
    errors = ModelState
});
```
Không trả lỗi quá chi tiết làm lộ hệ thống.
---
Search / Filter / Sort / Pagination
Danh sách dữ liệu nên hỗ trợ:
Tìm kiếm từ khóa
Lọc theo trạng thái
Lọc theo danh mục
Sắp xếp
Phân trang
DTO gợi ý:
```csharp
public class QueryRequestDto
{
    public string? Keyword { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? SortBy { get; set; }

    public string? SortDirection { get; set; } = "desc";
}
```
Kết quả phân trang gợi ý:
```csharp
public class PagedResultDto<T>
{
    public List<T> Items { get; set; } = new();

    public int TotalItems { get; set; }

    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
}
```
---
Static files / Razor assets
Nếu dùng MVC:
CSS, JS, ảnh phải nằm trong `wwwroot/`
Đường dẫn asset nên dùng `~/`
Với Razor inline style, dùng `Url.Content`
Ví dụ:
```html
<img src="~/images/logo.png" alt="Logo" />
```
Với background image:
```cshtml
<div style="background-image: url('@Url.Content("~/assets/images/banner.jpg")')"></div>
```
Không dùng đường dẫn sai kiểu:
```html
style="background-image: url(~/assets/img.jpg)"
```
vì trong inline CSS, `~` không tự resolve nếu không qua Razor helper.
---
Environment / Configuration
Các cấu hình quan trọng nên đặt ở:
`appsettings.json`
`appsettings.Development.json`
User Secrets
Environment Variables
Không hardcode:
Connection string thật
API key
Secret key
JWT secret
Password database
Ví dụ config:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=MyAppDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Issuer": "MyApp",
    "Audience": "MyAppUsers",
    "Key": "UseUserSecretsOrEnvironmentVariable"
  }
}
```
---
Thêm feature mới
Khi thêm feature mới, tham khảo skill:
```text
.claude/skills/dotnet-sqlserver-feature/SKILL.md
```
Các bước chính:
Xác định tên feature
Xác định danh sách fields
Xác định fields bắt buộc
Xác định fields tìm kiếm, lọc, sắp xếp
Xác định feature dùng MVC hay Web API
Tạo Entity trong `Models/`
Khai báo `DbSet` trong `AppDbContext`
Cấu hình kiểu dữ liệu và quan hệ trong `OnModelCreating`
Tạo DTO hoặc ViewModel
Tạo Service Interface
Tạo Service Implementation
Đăng ký Service trong `Program.cs`
Tạo Controller
Nếu là MVC, tạo Razor Views
Nếu là API, tạo endpoint và response format
Tạo migration
Cập nhật database
Chạy `dotnet build`
Chạy `dotnet run`
Kiểm tra route hoặc API
---
Lệnh thường dùng
Build project:
```bash
dotnet build
```
Run project:
```bash
dotnet run
```
Tạo migration:
```bash
dotnet ef migrations add AddFeatureName
```
Cập nhật database:
```bash
dotnet ef database update
```
Chạy test:
```bash
dotnet test
```
Cài EF tool:
```bash
dotnet tool install --global dotnet-ef
```
Cập nhật EF tool:
```bash
dotnet tool update --global dotnet-ef
```
---
Conventions
Dùng C# async/await cho database operations
Controller không nên chứa quá nhiều logic nghiệp vụ
Service xử lý nghiệp vụ chính
DbContext chỉ làm việc với database
DTO/ViewModel dùng để truyền dữ liệu
Không trả Entity phức tạp trực tiếp qua API nếu có navigation property
Không tạo kiến trúc phức tạp nếu project đang đơn giản
Không tự ý thêm AutoMapper, MediatR, CQRS nếu chưa được yêu cầu
Không tự ý đổi MVC sang API hoặc ngược lại
Nếu dự án đã có pattern riêng, ưu tiên theo pattern hiện có
---
Lưu ý khi làm việc
Luôn kiểm tra file hiện có trước khi sửa
Không sửa file không liên quan
Không xóa file nếu không có lý do rõ ràng
Không đổi target framework nếu người dùng không yêu cầu
Không dùng package không tương thích với target framework
Không commit secret hoặc connection string thật
Không nói đã test nếu chưa chạy test hoặc chưa có bằng chứng kiểm tra
Nếu thêm feature mới, đọc `SKILL.md` tương ứng trước khi code
Sau khi sửa, luôn nêu rõ file đã sửa và cách kiểm tra lại
Nếu có lỗi build, ưu tiên sửa lỗi build trước khi thêm chức năng mới
---
Checklist trước khi hoàn thành task
Trước khi kết luận hoàn thành, kiểm tra:
Project có build được không
Có cần migration không
Database đã update chưa
Controller route có đúng không
View có đúng thư mục không
API có trả status code đúng không
Form có validation chưa
POST form MVC có AntiForgeryToken chưa
Có xử lý NotFound chưa
Có xử lý dữ liệu không hợp lệ chưa
Có phân quyền cho action nhạy cảm chưa
Có tránh hardcode secret chưa
Có hướng dẫn cách kiểm tra lại chưa
---
name: dotnet-sqlserver-feature
description: Dùng skill này khi tạo hoặc mở rộng một tính năng trong dự án C# .NET với SQL Server, bao gồm Entity, DTO, ViewModel, DbContext, Service, Controller, Razor View hoặc Web API, validation, migration, kiểm thử và bảo mật cơ bản.
Skill tạo Feature trong C# .NET + SQL Server
1. Mục đích
Skill này dùng để hướng dẫn Claude tạo một tính năng mới trong dự án C# .NET + SQL Server theo cách có cấu trúc, dễ bảo trì và dễ kiểm tra.
Một tính năng có thể là:
Quản lý sản phẩm
Quản lý khách hàng
Quản lý đơn hàng
Quản lý danh mục
Quản lý thương hiệu
Quản lý nhà cung cấp
Quản lý nhân viên
Quản lý kho
Quản lý thanh toán
Quản lý báo cáo
Mục tiêu của skill này là:
Không tạo file lung tung
Không viết lại toàn bộ dự án khi không cần
Không nhồi toàn bộ logic vào Controller
Không bỏ qua validation
Không bỏ qua bảo mật cơ bản
Không bỏ qua bước kiểm tra build
Tạo feature theo cùng một chuẩn trong toàn bộ dự án
---
2. Ngữ cảnh dự án
Dự án sử dụng:
C#
.NET 9
ASP.NET Core MVC hoặc ASP.NET Core Web API
SQL Server
Entity Framework Core
Dependency Injection mặc định của ASP.NET Core
DTO hoặc ViewModel để truyền dữ liệu
Data Annotations hoặc validation thủ công
Async / await cho thao tác database
LINQ để truy vấn dữ liệu
Razor View nếu là MVC
JSON response nếu là Web API
Không dùng:
React class component
Công nghệ ngoài .NET nếu người dùng không yêu cầu
EF Core version không tương thích với target framework
Connection string hardcode trực tiếp trong code
---
3. Khi nào dùng skill này
Dùng skill này khi người dùng yêu cầu:
Tạo một tính năng mới trong dự án .NET
Tạo CRUD cho một bảng dữ liệu
Tạo Entity / Model
Tạo DTO
Tạo ViewModel
Tạo Controller
Tạo Service
Tạo Repository nếu dự án đang dùng Repository Pattern
Tạo Razor View
Tạo API endpoint
Tạo migration EF Core
Cập nhật database SQL Server
Viết truy vấn LINQ
Viết validation cho form hoặc API
Kết nối feature với database
Kiểm tra lỗi build hoặc lỗi Entity Framework
---
4. Khi nào không dùng skill này
Không dùng skill này khi:
Dự án không liên quan đến C# .NET
Người dùng chỉ hỏi một khái niệm đơn giản
Người dùng chỉ yêu cầu viết SQL thuần
Người dùng chỉ yêu cầu thiết kế giao diện HTML/CSS tĩnh
Người dùng yêu cầu công nghệ khác như Node.js, Laravel, Django, Next.js
Feature cần kiến trúc nâng cao như CQRS, microservice, message queue nhưng người dùng chưa yêu cầu
Dự án không dùng SQL Server
---
5. Tham số đầu vào cần xác định
Trước khi tạo feature, cần xác định các thông tin sau:
5.1. Tên feature
Ví dụ:
Product
Customer
Order
Category
Brand
Supplier
Employee
Payment
Stock
5.2. Danh sách fields
Ví dụ với Product:
Name
Price
Discount
StockQuantity
CategoryId
BrandId
Description
ImageUrl
IsActive
5.3. Fields bắt buộc
Ví dụ:
Name
Price
CategoryId
5.4. Fields dùng để tìm kiếm, lọc, sắp xếp
Ví dụ:
Tìm kiếm theo Name
Lọc theo CategoryId
Lọc theo BrandId
Lọc theo IsActive
Sắp xếp theo Price
Sắp xếp theo CreatedAt
5.5. Loại giao diện cần tạo
Chọn một trong hai:
ASP.NET Core MVC với Razor View
ASP.NET Core Web API
5.6. Phạm vi CRUD
Xác định có cần đầy đủ các chức năng sau không:
Danh sách
Chi tiết
Thêm mới
Cập nhật
Xóa
Tìm kiếm
Lọc
Sắp xếp
Phân trang
---
6. Quy tắc đặt tên
6.1. Entity
Entity dùng số ít và PascalCase.
Ví dụ:
```csharp
public class Product
public class Customer
public class Order
```
6.2. Controller
Controller dùng số nhiều và kết thúc bằng `Controller`.
Ví dụ:
```csharp
ProductsController
CustomersController
OrdersController
```
6.3. DbSet
DbSet dùng số nhiều.
```csharp
public DbSet<Product> Products { get; set; }
public DbSet<Customer> Customers { get; set; }
```
6.4. DTO
DTO phải có hậu tố rõ ràng.
```txt
ProductCreateDto
ProductUpdateDto
ProductDetailDto
ProductListItemDto
```
6.5. ViewModel
Nếu là MVC, ViewModel nên có hậu tố rõ ràng.
```txt
ProductIndexViewModel
ProductFormViewModel
ProductDetailViewModel
```
6.6. Service
Service đặt theo tên feature.
```txt
IProductService
ProductService
ICustomerService
CustomerService
```
---
7. Cấu trúc thư mục đề xuất
7.1. Cấu trúc chung
```txt
Controllers/
  ProductsController.cs

Models/
  Product.cs

Data/
  AppDbContext.cs

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

ViewModels/
  Products/
    ProductIndexViewModel.cs
    ProductFormViewModel.cs

Views/
  Products/
    Index.cshtml
    Details.cshtml
    Create.cshtml
    Edit.cshtml
    Delete.cshtml
```
7.2. Nếu dùng ASP.NET Core MVC
```txt
Controllers/
  ProductsController.cs

Models/
  Product.cs

DTOs/
  Products/
    ProductCreateDto.cs
    ProductUpdateDto.cs

ViewModels/
  Products/
    ProductIndexViewModel.cs
    ProductFormViewModel.cs

Services/
  Products/
    IProductService.cs
    ProductService.cs

Views/
  Products/
    Index.cshtml
    Details.cshtml
    Create.cshtml
    Edit.cshtml
    Delete.cshtml
```
7.3. Nếu dùng ASP.NET Core Web API
```txt
Controllers/
  ProductsController.cs

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

Data/
  AppDbContext.cs
```
Nếu là Web API thì không cần thư mục `Views/`.
---
8. Quy ước SQL Server
Khi tạo bảng hoặc Entity, tuân thủ các quy tắc sau:
Khóa chính dùng `Id`
Kiểu tiền dùng `decimal(18,2)`
Kiểu phần trăm có thể dùng `decimal(5,2)`
Chuỗi tiếng Việt dùng `nvarchar`
Ngày giờ dùng `datetime2`
Trạng thái đúng/sai dùng `bit`
Nên có `CreatedAt`
Nên có `UpdatedAt`
Nếu cần xóa mềm, dùng `IsDeleted`
Không hardcode dữ liệu quan trọng trong code
Không lưu mật khẩu dạng plain text
---
9. Entity / Model
Mỗi feature cần có Entity đại diện cho bảng trong SQL Server.
Ví dụ với Product:
```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourProject.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(5,2)")]
    public decimal Discount { get; set; }

    public int StockQuantity { get; set; }

    public int CategoryId { get; set; }

    public string? Description { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? UpdatedAt { get; set; }

    public Category? Category { get; set; }
}
```
---
10. DbContext
Mỗi Entity cần được khai báo trong `AppDbContext`.
```csharp
using Microsoft.EntityFrameworkCore;
using YourProject.Models;

namespace YourProject.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.Price)
                .HasColumnType("decimal(18,2)");

            entity.Property(x => x.Discount)
                .HasColumnType("decimal(5,2)");

            entity.Property(x => x.ImageUrl)
                .HasMaxLength(500);

            entity.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
        });
    }
}
```
---
11. DTO
Không nên nhận trực tiếp Entity từ form hoặc API nếu feature có logic phức tạp.
Nên tạo DTO riêng cho thêm mới, cập nhật, chi tiết và danh sách.
11.1. ProductCreateDto
```csharp
using System.ComponentModel.DataAnnotations;

namespace YourProject.DTOs.Products;

public class ProductCreateDto
{
    [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
    [MaxLength(200, ErrorMessage = "Tên sản phẩm không được vượt quá 200 ký tự.")]
    public string Name { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn hoặc bằng 0.")]
    public decimal Price { get; set; }

    [Range(0, 100, ErrorMessage = "Giảm giá phải nằm trong khoảng từ 0 đến 100.")]
    public decimal Discount { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho không được âm.")]
    public int StockQuantity { get; set; }

    [Required(ErrorMessage = "Danh mục là bắt buộc.")]
    public int CategoryId { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; } = true;
}
```
11.2. ProductUpdateDto
```csharp
using System.ComponentModel.DataAnnotations;

namespace YourProject.DTOs.Products;

public class ProductUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(0, 100)]
    public decimal Discount { get; set; }

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    [Required]
    public int CategoryId { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; }
}
```
11.3. ProductListItemDto
```csharp
namespace YourProject.DTOs.Products;

public class ProductListItemDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public decimal Discount { get; set; }

    public decimal SalePrice => Price - (Price * Discount / 100);

    public int StockQuantity { get; set; }

    public string? CategoryName { get; set; }

    public bool IsActive { get; set; }
}
```
---
12. Service Layer
Service chịu trách nhiệm xử lý nghiệp vụ.
Không nên nhồi toàn bộ logic vào Controller.
12.1. Interface
```csharp
using YourProject.DTOs.Products;

namespace YourProject.Services.Products;

public interface IProductService
{
    Task<List<ProductListItemDto>> GetAllAsync(string? keyword = null, int? categoryId = null);
    Task<ProductListItemDto?> GetByIdAsync(int id);
    Task<ProductListItemDto> CreateAsync(ProductCreateDto dto);
    Task<bool> UpdateAsync(ProductUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
```
12.2. Service implementation
```csharp
using Microsoft.EntityFrameworkCore;
using YourProject.Data;
using YourProject.DTOs.Products;
using YourProject.Models;

namespace YourProject.Services.Products;

public class ProductService : IProductService
{
    private readonly AppDbContext _db;

    public ProductService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<ProductListItemDto>> GetAllAsync(string? keyword = null, int? categoryId = null)
    {
        var query = _db.Products
            .Include(x => x.Category)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(x => x.Name.Contains(keyword));
        }

        if (categoryId.HasValue)
        {
            query = query.Where(x => x.CategoryId == categoryId.Value);
        }

        return await query
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new ProductListItemDto
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Discount = x.Discount,
                StockQuantity = x.StockQuantity,
                CategoryName = x.Category != null ? x.Category.Name : null,
                IsActive = x.IsActive
            })
            .ToListAsync();
    }

    public async Task<ProductListItemDto?> GetByIdAsync(int id)
    {
        return await _db.Products
            .Include(x => x.Category)
            .Where(x => x.Id == id)
            .Select(x => new ProductListItemDto
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Discount = x.Discount,
                StockQuantity = x.StockQuantity,
                CategoryName = x.Category != null ? x.Category.Name : null,
                IsActive = x.IsActive
            })
            .FirstOrDefaultAsync();
    }

    public async Task<ProductListItemDto> CreateAsync(ProductCreateDto dto)
    {
        var product = new Product
        {
            Name = dto.Name.Trim(),
            Price = dto.Price,
            Discount = dto.Discount,
            StockQuantity = dto.StockQuantity,
            CategoryId = dto.CategoryId,
            Description = dto.Description,
            ImageUrl = dto.ImageUrl,
            IsActive = dto.IsActive,
            CreatedAt = DateTime.Now
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        return new ProductListItemDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Discount = product.Discount,
            StockQuantity = product.StockQuantity,
            IsActive = product.IsActive
        };
    }

    public async Task<bool> UpdateAsync(ProductUpdateDto dto)
    {
        var product = await _db.Products.FindAsync(dto.Id);

        if (product == null)
        {
            return false;
        }

        product.Name = dto.Name.Trim();
        product.Price = dto.Price;
        product.Discount = dto.Discount;
        product.StockQuantity = dto.StockQuantity;
        product.CategoryId = dto.CategoryId;
        product.Description = dto.Description;
        product.ImageUrl = dto.ImageUrl;
        product.IsActive = dto.IsActive;
        product.UpdatedAt = DateTime.Now;

        await _db.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var product = await _db.Products.FindAsync(id);

        if (product == null)
        {
            return false;
        }

        _db.Products.Remove(product);
        await _db.SaveChangesAsync();

        return true;
    }
}
```
---
13. Đăng ký Service trong Program.cs
Mỗi service cần được đăng ký trong Dependency Injection.
```csharp
using Microsoft.EntityFrameworkCore;
using YourProject.Data;
using YourProject.Services.Products;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();
```
Nếu là Web API, có thể dùng:
```csharp
builder.Services.AddControllers();
```
Nếu là MVC, dùng:
```csharp
builder.Services.AddControllersWithViews();
```
---
14. Connection String
Connection string đặt trong `appsettings.json`.
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=YourDatabaseName;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```
Không hardcode connection string trực tiếp trong Controller hoặc Service.
---
15. Controller cho MVC
Nếu dự án dùng ASP.NET Core MVC, Controller trả về View.
```csharp
using Microsoft.AspNetCore.Mvc;
using YourProject.DTOs.Products;
using YourProject.Services.Products;

namespace YourProject.Controllers;

public class ProductsController : Controller
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index(string? keyword, int? categoryId)
    {
        var products = await _productService.GetAllAsync(keyword, categoryId);
        return View(products);
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _productService.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new ProductCreateDto());
    }

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

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _productService.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        var dto = new ProductUpdateDto
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Discount = product.Discount,
            StockQuantity = product.StockQuantity,
            IsActive = product.IsActive
        };

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(ProductUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var updated = await _productService.UpdateAsync(dto);

        if (!updated)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _productService.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
```
---
16. Controller cho Web API
Nếu dự án dùng Web API, Controller trả về JSON.
```csharp
using Microsoft.AspNetCore.Mvc;
using YourProject.DTOs.Products;
using YourProject.Services.Products;

namespace YourProject.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? keyword, [FromQuery] int? categoryId)
    {
        var products = await _productService.GetAllAsync(keyword, categoryId);
        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy sản phẩm."
            });
        }

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var product = await _productService.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, ProductUpdateDto dto)
    {
        if (id != dto.Id)
        {
            return BadRequest(new
            {
                message = "Id không khớp."
            });
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updated = await _productService.UpdateAsync(dto);

        if (!updated)
        {
            return NotFound(new
            {
                message = "Không tìm thấy sản phẩm."
            });
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _productService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound(new
            {
                message = "Không tìm thấy sản phẩm."
            });
        }

        return NoContent();
    }
}
```
---
17. Razor View: Index.cshtml
Nếu dùng MVC, trang danh sách có thể theo mẫu:
```cshtml
@model List<YourProject.DTOs.Products.ProductListItemDto>

@{
    ViewData["Title"] = "Danh sách sản phẩm";
}

<h1>Danh sách sản phẩm</h1>

<form method="get" asp-action="Index" class="mb-3">
    <input type="text" name="keyword" placeholder="Tìm kiếm sản phẩm..." class="form-control" />
    <button type="submit" class="btn btn-primary mt-2">Tìm kiếm</button>
</form>

<a asp-action="Create" class="btn btn-success mb-3">Thêm sản phẩm</a>

<table class="table table-bordered table-striped">
    <thead>
        <tr>
            <th>Tên sản phẩm</th>
            <th>Giá</th>
            <th>Giảm giá</th>
            <th>Giá sau giảm</th>
            <th>Tồn kho</th>
            <th>Danh mục</th>
            <th>Trạng thái</th>
            <th>Thao tác</th>
        </tr>
    </thead>
    <tbody>
        @foreach (var product in Model)
        {
            <tr>
                <td>@product.Name</td>
                <td>@product.Price.ToString("N0")</td>
                <td>@product.Discount%</td>
                <td>@product.SalePrice.ToString("N0")</td>
                <td>@product.StockQuantity</td>
                <td>@product.CategoryName</td>
                <td>
                    @(product.IsActive ? "Đang bán" : "Ngừng bán")
                </td>
                <td>
                    <a asp-action="Details" asp-route-id="@product.Id" class="btn btn-sm btn-info">Xem</a>
                    <a asp-action="Edit" asp-route-id="@product.Id" class="btn btn-sm btn-warning">Sửa</a>

                    <form asp-action="Delete" asp-route-id="@product.Id" method="post" style="display:inline">
                        @Html.AntiForgeryToken()
                        <button type="submit" class="btn btn-sm btn-danger"
                                onclick="return confirm('Bạn có chắc muốn xóa sản phẩm này?')">
                            Xóa
                        </button>
                    </form>
                </td>
            </tr>
        }
    </tbody>
</table>
```
---
18. Razor View: Create.cshtml
```cshtml
@model YourProject.DTOs.Products.ProductCreateDto

@{
    ViewData["Title"] = "Thêm sản phẩm";
}

<h1>Thêm sản phẩm</h1>

<form asp-action="Create" method="post">
    @Html.AntiForgeryToken()

    <div class="mb-3">
        <label asp-for="Name" class="form-label">Tên sản phẩm</label>
        <input asp-for="Name" class="form-control" />
        <span asp-validation-for="Name" class="text-danger"></span>
    </div>

    <div class="mb-3">
        <label asp-for="Price" class="form-label">Giá</label>
        <input asp-for="Price" class="form-control" />
        <span asp-validation-for="Price" class="text-danger"></span>
    </div>

    <div class="mb-3">
        <label asp-for="Discount" class="form-label">Giảm giá (%)</label>
        <input asp-for="Discount" class="form-control" />
        <span asp-validation-for="Discount" class="text-danger"></span>
    </div>

    <div class="mb-3">
        <label asp-for="StockQuantity" class="form-label">Số lượng tồn kho</label>
        <input asp-for="StockQuantity" class="form-control" />
        <span asp-validation-for="StockQuantity" class="text-danger"></span>
    </div>

    <div class="mb-3">
        <label asp-for="CategoryId" class="form-label">Mã danh mục</label>
        <input asp-for="CategoryId" class="form-control" />
        <span asp-validation-for="CategoryId" class="text-danger"></span>
    </div>

    <div class="mb-3">
        <label asp-for="Description" class="form-label">Mô tả</label>
        <textarea asp-for="Description" class="form-control"></textarea>
    </div>

    <div class="mb-3">
        <label asp-for="ImageUrl" class="form-label">Đường dẫn ảnh</label>
        <input asp-for="ImageUrl" class="form-control" />
    </div>

    <div class="form-check mb-3">
        <input asp-for="IsActive" class="form-check-input" />
        <label asp-for="IsActive" class="form-check-label">Đang bán</label>
    </div>

    <button type="submit" class="btn btn-primary">Lưu</button>
    <a asp-action="Index" class="btn btn-secondary">Quay lại</a>
</form>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```
---
19. Migration và cập nhật database
Khi thêm Entity mới hoặc thay đổi DbContext, chạy:
```bash
dotnet ef migrations add AddProductFeature
dotnet ef database update
```
Nếu chưa cài EF Tool:
```bash
dotnet tool install --global dotnet-ef
```
Nếu đã cài nhưng cần cập nhật:
```bash
dotnet tool update --global dotnet-ef
```
---
20. Seed data mẫu
Có thể tạo dữ liệu mẫu trong `OnModelCreating`.
```csharp
modelBuilder.Entity<Product>().HasData(
    new Product
    {
        Id = 1,
        Name = "Đồng hồ mẫu 1",
        Price = 2500000,
        Discount = 10,
        StockQuantity = 20,
        CategoryId = 1,
        IsActive = true,
        CreatedAt = new DateTime(2026, 1, 1)
    },
    new Product
    {
        Id = 2,
        Name = "Đồng hồ mẫu 2",
        Price = 3500000,
        Discount = 5,
        StockQuantity = 15,
        CategoryId = 1,
        IsActive = true,
        CreatedAt = new DateTime(2026, 1, 1)
    }
);
```
Lưu ý:
Khi dùng `HasData`, nên dùng ngày cố định
Không dùng `DateTime.Now` trong `HasData`
Cần đảm bảo khóa ngoại như `CategoryId` đã tồn tại
---
21. Tìm kiếm, lọc, sắp xếp và phân trang
Nếu feature có danh sách dữ liệu lớn, nên hỗ trợ:
Tìm kiếm theo từ khóa
Lọc theo trạng thái
Lọc theo danh mục
Sắp xếp
Phân trang
21.1. QueryRequestDto
```csharp
namespace YourProject.DTOs.Common;

public class QueryRequestDto
{
    public string? Keyword { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? SortBy { get; set; }

    public string? SortDirection { get; set; } = "desc";
}
```
21.2. PagedResultDto
```csharp
namespace YourProject.DTOs.Common;

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
22. Validation
Mỗi feature cần kiểm tra dữ liệu ở backend.
Các rule thường dùng:
Tên không được rỗng
Email phải đúng định dạng
Số điện thoại phải đúng định dạng nếu có
Giá không được âm
Số lượng không được âm
Ngày bắt đầu không được lớn hơn ngày kết thúc
Mã khóa ngoại phải tồn tại
Không cho xóa dữ liệu đang được sử dụng nếu có ràng buộc
Ví dụ kiểm tra khóa ngoại:
```csharp
var categoryExists = await _db.Categories.AnyAsync(x => x.Id == dto.CategoryId);

if (!categoryExists)
{
    throw new InvalidOperationException("Danh mục không tồn tại.");
}
```
---
23. Error Handling
Khi có lỗi, không nên trả lỗi quá kỹ làm lộ thông tin hệ thống.
23.1. Với MVC
Dùng `ModelState.AddModelError`
Trả lại View kèm dữ liệu người dùng đã nhập
Hiển thị thông báo dễ hiểu
Ví dụ:
```csharp
ModelState.AddModelError("", "Không thể tạo sản phẩm. Vui lòng kiểm tra lại thông tin.");
return View(dto);
```
23.2. Với Web API
Nên trả lỗi theo format rõ ràng:
```csharp
return BadRequest(new
{
    message = "Dữ liệu không hợp lệ.",
    errors = ModelState
});
```
---
24. Security
Các quy tắc bảo mật bắt buộc:
Không tin dữ liệu từ client
Luôn validate ở backend
Không hardcode connection string
Không lưu mật khẩu plain text
Không trả thông tin lỗi hệ thống quá chi tiết cho user
Các action thêm/sửa/xóa nên có phân quyền
Với MVC form POST, phải dùng `[ValidateAntiForgeryToken]`
Với API, cần kiểm tra authentication/authorization nếu là chức năng nội bộ
Không cho user thường gọi chức năng admin
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
25. Các bước thực hiện khi tạo feature mới
Khi tạo một feature mới, thực hiện theo thứ tự:
Xác định tên feature
Xác định danh sách fields
Xác định fields bắt buộc
Xác định fields tìm kiếm, lọc, sắp xếp
Xác định feature dùng MVC hay Web API
Tạo Entity trong `Models/`
Khai báo `DbSet` trong `AppDbContext`
Cấu hình quan hệ và kiểu dữ liệu trong `OnModelCreating`
Tạo DTO hoặc ViewModel
Tạo Service Interface
Tạo Service Implementation
Đăng ký Service trong `Program.cs`
Tạo Controller
Nếu là MVC, tạo Razor Views
Nếu là API, tạo endpoint và response format
Tạo migration
Cập nhật database
Chạy build kiểm tra lỗi
Chạy project kiểm tra chức năng
Kiểm tra thêm/sửa/xóa/tìm kiếm/lọc
Báo cáo lại các file đã tạo hoặc chỉnh sửa
---
26. Lệnh kiểm tra
Sau khi tạo feature, chạy:
```bash
dotnet build
```
Nếu có migration:
```bash
dotnet ef migrations add Add<Feature>Feature
dotnet ef database update
```
Nếu chạy project:
```bash
dotnet run
```
Nếu có test:
```bash
dotnet test
```
---
27. Reference thực tế
Khi làm việc trong một dự án có sẵn, ưu tiên tham khảo:
Feature tương tự đã tồn tại
Controller đã có sẵn
Service đã có sẵn
Cách đặt tên Entity hiện tại
Cách tổ chức DTO/ViewModel hiện tại
Cấu hình `AppDbContext`
Cấu hình `Program.cs`
Cách dự án xử lý authentication/authorization
Cách dự án xử lý validation và lỗi
Nếu dự án đã có pattern riêng, phải ưu tiên theo pattern hiện có thay vì tạo pattern mới.
---
28. Định dạng phản hồi
Sau khi tạo hoặc hướng dẫn tạo feature, phản hồi theo mẫu:
28.1. Đã tạo hoặc cần tạo
Liệt kê các file:
```txt
Models/Product.cs
DTOs/Products/ProductCreateDto.cs
DTOs/Products/ProductUpdateDto.cs
DTOs/Products/ProductListItemDto.cs
Services/Products/IProductService.cs
Services/Products/ProductService.cs
Controllers/ProductsController.cs
Views/Products/Index.cshtml
Views/Products/Create.cshtml
```
28.2. Luồng hoạt động
Mô tả ngắn gọn:
```txt
Controller nhận request → gọi Service → Service xử lý với DbContext → lưu/lấy dữ liệu từ SQL Server → trả DTO/ViewModel về View hoặc API response.
```
28.3. Cách kiểm tra
```bash
dotnet build
dotnet ef database update
dotnet run
```
Sau đó mở route MVC:
```txt
/products
```
Hoặc API:
```txt
GET /api/products
POST /api/products
PUT /api/products/{id}
DELETE /api/products/{id}
```
28.4. Lưu ý
Nêu rõ:
Có cần migration không
Có cần seed data không
Có cần phân quyền không
Có phần nào chưa làm không
Có rủi ro gì không
---
29. Loại trừ
Không làm các việc sau nếu người dùng chưa yêu cầu:
Không đổi framework của dự án
Không đổi từ MVC sang API hoặc ngược lại
Không tự ý thêm Repository Pattern nếu dự án chưa dùng
Không tự ý thêm AutoMapper nếu dự án chưa dùng
Không tự ý thêm MediatR
Không tự ý thêm CQRS
Không tự ý chuyển sang Clean Architecture
Không hardcode connection string
Không lưu mật khẩu plain text
Không xóa dữ liệu thật nếu chưa xác nhận
Không viết lại toàn bộ project
Không tạo README hoặc tài liệu dài nếu người dùng không yêu cầu
Không tạo chức năng đăng nhập/phân quyền nếu feature không cần
Không bỏ qua bước `dotnet build`
Không nói đã test nếu chưa chạy test hoặc chưa có bằng chứng kiểm tra
---
30. Checklist hoàn thành feature
Trước khi kết luận hoàn thành, kiểm tra:
Đã tạo Entity chưa
Đã khai báo DbSet chưa
Đã cấu hình kiểu dữ liệu decimal/date/string chưa
Đã tạo DTO/ViewModel chưa
Đã tạo Service chưa
Đã đăng ký Service trong Program.cs chưa
Đã tạo Controller chưa
Nếu MVC, đã tạo View chưa
Nếu API, đã tạo đủ endpoint chưa
Đã có validation chưa
Đã xử lý NotFound chưa
Đã xử lý lỗi dữ liệu không hợp lệ chưa
Đã tạo migration chưa nếu có thay đổi database
Đã update database chưa
Đã chạy `dotnet build` chưa
Đã có cách kiểm tra route/API chưa
Đã nêu rõ phần nào chưa hoàn thành chưa
---
31. Ví dụ yêu cầu đầu vào chuẩn
Người dùng có thể yêu cầu theo mẫu:
```txt
Tạo feature Product cho dự án ASP.NET Core MVC + SQL Server.

Fields:
- Name: string, bắt buộc
- Price: decimal, bắt buộc
- Discount: decimal
- StockQuantity: int
- CategoryId: int, bắt buộc
- Description: string
- ImageUrl: string
- IsActive: bool

Cần:
- Danh sách sản phẩm
- Thêm sản phẩm
- Sửa sản phẩm
- Xóa sản phẩm
- Tìm kiếm theo tên
- Lọc theo danh mục
```
## Quy tắc sử dụng theme

Khi xây dựng giao diện, luôn kiểm tra và tuân thủ hệ màu trong `CLAUDE.md`.

- Không tự ý thay đổi tone màu chính.
- Không sử dụng màu gold làm màu chủ đạo.
- Light Theme và Dark Theme phải dùng đúng theo bảng màu đã định nghĩa.
- Các component như button, card, navbar, footer, badge, hover state phải bám theo theme chung.
Claude cần xử lý theo đúng các bước trong skill này.

```md
## Kỹ năng xử lý Responsive Design

Khi xây dựng hoặc chỉnh sửa giao diện, luôn kiểm tra yếu tố responsive để đảm bảo website hiển thị tốt trên nhiều thiết bị.

### Quy trình thực hiện

1. Xác định màn hình chính cần hỗ trợ:
   - Mobile
   - Tablet
   - Laptop
   - Desktop

2. Thiết kế theo hướng mobile-first:
   - Làm giao diện mobile trước.
   - Sau đó mở rộng layout cho tablet và desktop.

3. Kiểm tra các thành phần quan trọng:
   - Header/navbar
   - Logo
   - Banner/hero section
   - Product grid
   - Product card
   - Filter/sidebar
   - Footer
   - Button và form input

4. Đảm bảo không có lỗi giao diện:
   - Không bị tràn ngang.
   - Không bị vỡ layout.
   - Không bị méo ảnh.
   - Không bị chữ quá nhỏ trên mobile.
   - Không để button quá nhỏ hoặc khó bấm.

---

### Quy tắc khi code responsive

- Ưu tiên dùng `max-width` thay vì `width` cố định.
- Hình ảnh luôn cần có:

```css
img {
  max-width: 100%;
  height: auto;
}
```

Container nên dùng dạng co giãn:
```css
container {
  width: min(100% - 32px, 1200px);
  margin-inline: auto;
}
```

Grid sản phẩm nên tự thay đổi số cột theo màn hình:
```css
.product-grid {
  display: grid;
  grid-template-columns: 1fr;
  gap: 24px;
}

@media (min-width: 768px) {
  .product-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (min-width: 992px) {
  .product-grid {
    grid-template-columns: repeat(3, 1fr);
  }
}

@media (min-width: 1200px) {
  .product-grid {
    grid-template-columns: repeat(4, 1fr);
  }
}
```
- Checklist trước khi hoàn thành giao diện
 Mobile hiển thị tốt.
 Tablet hiển thị tốt.
 Desktop hiển thị tốt.
 Không có thanh cuộn ngang.
 Logo không bị méo hoặc vỡ bố cục.
 Ảnh sản phẩm không bị méo.
 Card sản phẩm không bị lệch.
 Navbar hoạt động tốt trên mobile.
 Button dễ bấm trên màn hình nhỏ.
 Khoảng cách section hợp lý trên từng kích thước màn hình.

 ## Role / Permission Summary

Dự án có 3 loại user chính:

- `Admin`
- `Staff`
- `Customer`

### Hướng xử lý chung

- `Customer` sử dụng website bên ngoài: xem sản phẩm, đặt hàng, xem đơn hàng cá nhân.
- `Admin` và `Staff` sử dụng khu vực quản trị.
- `Admin` có toàn quyền quản lý hệ thống.
- `Staff` chỉ được xử lý công việc vận hành hằng ngày.
- Không cho `Customer` truy cập dashboard hoặc chức năng quản trị.

---

### Quyền mặc định

| Chức năng | Admin | Staff | Customer |
|---|---:|---:|---:|
| Xem sản phẩm | Có | Có | Có |
| Thêm / sửa sản phẩm | Có | Có | Không |
| Xóa sản phẩm | Có | Không | Không |
| Xem đơn hàng | Có | Có | Chỉ đơn của mình |
| Cập nhật trạng thái đơn hàng | Có | Có | Không |
| Xóa đơn hàng | Có | Không | Không |
| Xem khách hàng | Có | Có | Không |
| Quản lý user / role | Có | Không | Không |
| Xem báo cáo | Có | Hạn chế | Không |
| Settings hệ thống | Có | Không | Không |

---

### Dashboard theo role

Admin Dashboard:

- Tổng doanh thu
- Tổng đơn hàng
- Tổng sản phẩm
- Tổng khách hàng
- Biểu đồ doanh thu
- Đơn hàng gần đây
- Sản phẩm bán chạy
- Sản phẩm sắp hết hàng
- Quản lý user
- Reports
- Settings

Staff Dashboard:

- Đơn hàng hôm nay
- Đơn hàng chờ xử lý
- Đơn hàng đang giao
- Sản phẩm sắp hết hàng
- Danh sách đơn hàng gần đây
- Thao tác nhanh: thêm sản phẩm, cập nhật đơn hàng, kiểm tra tồn kho

Customer Dashboard:

- Đơn hàng của tôi
- Trạng thái đơn hàng
- Wishlist nếu có
- Thông tin cá nhân
- Địa chỉ giao hàng

---

### Sidebar theo role

Admin:

```text
Dashboard
Products
Categories
Brands
Collections
Orders
Customers
Users
Reports
Settings
Logout
```

Staff:

```text
Dashboard
Products
Orders
Customers
Inventory
Logout
```

Customer:

```text
Home
Products
Collections
Gallery
Cart
My Orders
Profile
Logout
```

Quy tắc chung:

Chức năng xem/thêm/sửa dữ liệu vận hành: Admin,Staff
Chức năng xóa dữ liệu: chỉ Admin
Chức năng quản lý user, role, reports, settings: chỉ Admin
Chức năng đơn hàng cá nhân: chỉ Customer

Lưu ý: Ẩn menu trên giao diện chưa đủ bảo mật. Backend vẫn phải dùng [Authorize] để chặn truy cập trực tiếp bằng URL.
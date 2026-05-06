using System.ComponentModel.DataAnnotations;
using MyProject.DTOs.Products;

namespace MyProject.Areas.Admin.ViewModels;

public class ProductAdminIndexViewModel
{
    public List<ProductAdminItem> Products { get; set; } = new();
    public int TotalCount { get; set; }
    public int ActiveCount { get; set; }
    public int InactiveCount { get; set; }
    public string? SearchKeyword { get; set; }
    public int? FilterCategoryId { get; set; }
    public bool? FilterIsActive { get; set; }
    public List<CategorySelectDto> Categories { get; set; } = new();
}

public class ProductAdminItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Discount { get; set; }
    public decimal SalePrice => Discount > 0 ? Price - (Price * Discount / 100) : Price;
    public int StockQuantity { get; set; }
    public string? CategoryName { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ProductAdminFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Product name is required.")]
    [MaxLength(200, ErrorMessage = "Product name must not exceed 200 characters.")]
    public string Name { get; set; } = string.Empty;

    [Range(0, double.MaxValue, ErrorMessage = "Price must be 0 or greater.")]
    public decimal Price { get; set; }

    [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100%.")]
    public decimal Discount { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative.")]
    public int StockQuantity { get; set; }

    [Required(ErrorMessage = "Category is required.")]
    public int CategoryId { get; set; }

    public string? Description { get; set; }

    [MaxLength(500)]
    public string? ImageUrl { get; set; }

    [MaxLength(1000)]
    public string? GalleryImages { get; set; }

    public bool IsActive { get; set; } = true;

    public List<CategorySelectDto> Categories { get; set; } = new();
}
using System.ComponentModel.DataAnnotations;

namespace MyProject.Areas.Admin.ViewModels;

public class CategoryAdminIndexViewModel
{
    public List<CategoryAdminItem> Categories { get; set; } = new();
    public int TotalCount { get; set; }
}

public class CategoryAdminItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int ProductCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CategoryAdminFormViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Tên danh mục là bắt buộc.")]
    [MaxLength(100, ErrorMessage = "Name must not exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Description { get; set; }
}
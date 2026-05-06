using System.ComponentModel.DataAnnotations;

namespace MyProject.Areas.Admin.ViewModels;

public class SettingsAdminViewModel
{
    [MaxLength(200)]
    public string StoreName { get; set; } = "DLD Watch Store";

    [MaxLength(500)]
    public string StoreAddress { get; set; } = string.Empty;

    [MaxLength(20)]
    public string StorePhone { get; set; } = string.Empty;

    [MaxLength(200)]
    [EmailAddress]
    public string StoreEmail { get; set; } = string.Empty;

    [MaxLength(200)]
    [EmailAddress]
    public string ContactEmail { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? StoreDescription { get; set; }

    public string? FacebookUrl { get; set; }
    public string? InstagramUrl { get; set; }
    public string? ZaloUrl { get; set; }
}
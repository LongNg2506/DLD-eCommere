using MyProject.Areas.Admin.ViewModels;

namespace MyProject.Areas.Admin.Services;

public interface IOrderAdminService
{
    Task<OrderAdminIndexViewModel> GetIndexAsync(string? status, string? keyword);
    Task<OrderAdminDetailViewModel?> GetDetailAsync(int id);
    Task<bool> UpdateStatusAsync(int id, string status, string? updatedBy, string? note);
    Task<bool> ConfirmPaymentAsync(int id, string? transactionId, string? note, string? updatedBy);
    Task<bool> DeductStockAsync(int id);
}
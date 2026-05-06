using MyProject.Areas.Admin.ViewModels;

namespace MyProject.Areas.Admin.Services;

public interface IReportAdminService
{
    Task<ReportAdminViewModel> GetReportAsync();
}
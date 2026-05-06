using MyProject.Areas.Admin.ViewModels;

namespace MyProject.Areas.Admin.Services;

public interface ISettingsAdminService
{
    SettingsAdminViewModel GetSettings();
    void SaveSettings(SettingsAdminViewModel vm);
}
using System.Text.Json;
using MyProject.Areas.Admin.ViewModels;

namespace MyProject.Areas.Admin.Services;

public class SettingsAdminService : ISettingsAdminService
{
    private readonly string _settingsPath;

    public SettingsAdminService(IWebHostEnvironment env)
    {
        _settingsPath = Path.Combine(env.ContentRootPath, "..", "wwwroot", "data", "settings.json");
    }

    public SettingsAdminViewModel GetSettings()
    {
        if (!File.Exists(_settingsPath))
            return new SettingsAdminViewModel();

        try
        {
            var json = File.ReadAllText(_settingsPath);
            return JsonSerializer.Deserialize<SettingsAdminViewModel>(json) ?? new SettingsAdminViewModel();
        }
        catch
        {
            return new SettingsAdminViewModel();
        }
    }

    public void SaveSettings(SettingsAdminViewModel vm)
    {
        var dir = Path.GetDirectoryName(_settingsPath);
        if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        var json = JsonSerializer.Serialize(vm, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_settingsPath, json);
    }
}
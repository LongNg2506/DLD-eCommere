using MyProject.DTOs.Users;

namespace MyProject.Services.Auth;

public interface IAuthService
{
    Task<UserDto?> LoginAsync(LoginDto dto);
    Task<UserDto?> RegisterAsync(RegisterDto dto);
    Task<UserDto?> GetByIdAsync(int id);
    Task<bool> EmailExistsAsync(string email);
    Task<UserDto?> UpdateProfileAsync(int userId, string fullName, string? phone, string? address);
    Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    Task<string?> GenerateResetTokenAsync(string email);
    Task<bool> ResetPasswordAsync(string token, string newPassword);
    Task<List<UserOrderDto>> GetOrdersByUserIdAsync(int userId);
}

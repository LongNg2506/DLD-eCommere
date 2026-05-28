using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Models;
using MyProject.Services.Auth;
using Xunit;

namespace MyProject.Tests;

public class AuthServiceTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task GenerateResetTokenAsync_ShouldCreateTokenForExistingUser()
    {
        // Arrange
        using var db = GetDbContext();
        var user = new User { Id = 1, Email = "test@example.com", FullName = "Test User", PasswordHash = "hash", Role = "Customer" };
        db.Users.Add(user);
        await db.SaveChangesAsync();

        var service = new AuthService(db);

        // Act
        var token = await service.GenerateResetTokenAsync("test@example.com");

        // Assert
        Assert.NotNull(token);
        var updatedUser = await db.Users.FindAsync(1);
        Assert.Equal(token, updatedUser?.ResetToken);
        Assert.NotNull(updatedUser?.ResetTokenExpiry);
    }

    [Fact]
    public async Task GenerateResetTokenAsync_ShouldReturnNullForNonExistingUser()
    {
        // Arrange
        using var db = GetDbContext();
        var service = new AuthService(db);

        // Act
        var token = await service.GenerateResetTokenAsync("nonexistent@example.com");

        // Assert
        Assert.Null(token);
    }
}

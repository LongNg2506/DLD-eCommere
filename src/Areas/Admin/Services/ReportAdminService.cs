using Microsoft.EntityFrameworkCore;
using MyProject.Areas.Admin.ViewModels;
using MyProject.Data;

namespace MyProject.Areas.Admin.Services;

public class ReportAdminService : IReportAdminService
{
    private readonly AppDbContext _db;

    public ReportAdminService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<ReportAdminViewModel> GetReportAsync()
    {
        var totalOrders = await _db.Orders.CountAsync();
        var pendingOrders = await _db.Orders.CountAsync(o => o.Status == "Pending");
        var completedOrders = await _db.Orders.CountAsync(o => o.Status == "Delivered");
        var totalRevenue = completedOrders > 0
            ? await _db.Orders.Where(o => o.Status == "Delivered").SumAsync(o => o.TotalAmount)
            : 0;
        var avgOrderValue = completedOrders > 0 ? totalRevenue / completedOrders : 0;

        var orderItems = await _db.OrderItems
            .Include(i => i.Product)
            .ToListAsync();

        var topByQuantity = orderItems
            .GroupBy(i => new { i.ProductId, i.ProductName, i.Product?.ImageUrl })
            .Select(g => new TopProductReportItem
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.ProductName,
                ImageUrl = g.Key.ImageUrl,
                TotalQuantity = g.Sum(x => x.Quantity),
                TotalRevenue = g.Sum(x => x.Quantity * x.UnitPrice)
            })
            .OrderByDescending(x => x.TotalQuantity)
            .Take(5)
            .ToList();

        var topByRevenue = orderItems
            .GroupBy(i => new { i.ProductId, i.ProductName, i.Product?.ImageUrl })
            .Select(g => new TopProductReportItem
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.ProductName,
                ImageUrl = g.Key.ImageUrl,
                TotalQuantity = g.Sum(x => x.Quantity),
                TotalRevenue = g.Sum(x => x.Quantity * x.UnitPrice)
            })
            .OrderByDescending(x => x.TotalRevenue)
            .Take(5)
            .ToList();

        var monthlyData = await _db.Orders
            .Where(o => o.Status == "Delivered")
            .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
            .OrderByDescending(g => g.Key.Year)
            .ThenByDescending(g => g.Key.Month)
            .Take(12)
            .Select(g => new MonthlyRevenueItem
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Revenue = g.Sum(x => x.TotalAmount),
                OrderCount = g.Count()
            })
            .ToListAsync();

        return new ReportAdminViewModel
        {
            TotalOrders = totalOrders,
            PendingOrders = pendingOrders,
            CompletedOrders = completedOrders,
            TotalRevenue = totalRevenue,
            AverageOrderValue = avgOrderValue,
            TopProductsByQuantity = topByQuantity,
            TopProductsByRevenue = topByRevenue,
            MonthlyRevenue = monthlyData.OrderBy(x => x.Year).ThenBy(x => x.Month).ToList()
        };
    }
}
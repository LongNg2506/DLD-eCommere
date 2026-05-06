namespace MyProject.Areas.Admin.ViewModels;

public class ReportAdminViewModel
{
    public int TotalOrders { get; set; }
    public int PendingOrders { get; set; }
    public int CompletedOrders { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal AverageOrderValue { get; set; }
    public List<TopProductReportItem> TopProductsByQuantity { get; set; } = new();
    public List<TopProductReportItem> TopProductsByRevenue { get; set; } = new();
    public List<MonthlyRevenueItem> MonthlyRevenue { get; set; } = new();
}

public class TopProductReportItem
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public int TotalQuantity { get; set; }
    public decimal TotalRevenue { get; set; }
}

public class MonthlyRevenueItem
{
    public int Year { get; set; }
    public int Month { get; set; }
    public string MonthLabel => new DateTime(Year, Month, 1).ToString("MMM yyyy");
    public decimal Revenue { get; set; }
    public int OrderCount { get; set; }
}
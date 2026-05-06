namespace MyProject.Areas.Admin.ViewModels;

public class OrderAdminIndexViewModel
{
    public List<OrderAdminItem> Orders { get; set; } = new();
    public int TotalCount { get; set; }
    public int PendingCount { get; set; }
    public int ConfirmedCount { get; set; }
    public int ShippedCount { get; set; }
    public int DeliveredCount { get; set; }
    public int CancelledCount { get; set; }
    public string? FilterStatus { get; set; }
    public string? SearchKeyword { get; set; }
}

public class OrderAdminItem
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string? ShippingAddress { get; set; }
    public string? Phone { get; set; }
}

public class OrderAdminDetailViewModel
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? ShippingAddress { get; set; }
    public string? Note { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public List<OrderItemAdminItem> Items { get; set; } = new();
    public List<OrderStatusHistoryAdminItem> StatusHistory { get; set; } = new();
}

public class OrderStatusHistoryAdminItem
{
    public string Status { get; set; } = string.Empty;
    public string? Note { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class OrderItemAdminItem
{
    public int Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Subtotal => Quantity * UnitPrice;
}
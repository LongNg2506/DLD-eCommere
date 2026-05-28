using Microsoft.EntityFrameworkCore;
using MyProject.Areas.Admin.ViewModels;
using MyProject.Data;

namespace MyProject.Areas.Admin.Services;

public class OrderAdminService : IOrderAdminService
{
    private readonly AppDbContext _db;

    public OrderAdminService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<OrderAdminIndexViewModel> GetIndexAsync(string? status, string? keyword, int page = 1, int pageSize = 10)
    {
        var query = _db.Orders.Include(o => o.User).AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(o => o.Status == status);

        if (!string.IsNullOrWhiteSpace(keyword))
            query = query.Where(o =>
                (o.User != null && o.User.FullName.Contains(keyword)) ||
                (o.User != null && o.User.Email.Contains(keyword)) ||
                (o.Phone != null && o.Phone.Contains(keyword)) ||
                (o.InvoiceNumber != null && o.InvoiceNumber.Contains(keyword)));

        var total = await query.CountAsync();
        var orders = await query
            .OrderByDescending(o => o.OrderDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(o => new OrderAdminItem
            {
                Id = o.Id,
                CustomerName = o.User != null ? o.User.FullName : "Guest",
                CustomerEmail = o.User != null ? o.User.Email : "-",
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                ShippingAddress = o.ShippingAddress,
                Phone = o.Phone
            })
            .ToListAsync();

        var pending = await _db.Orders.CountAsync(o => o.Status == "Pending");
        var confirmed = await _db.Orders.CountAsync(o => o.Status == "Confirmed");
        var shipped = await _db.Orders.CountAsync(o => o.Status == "Shipped");
        var delivered = await _db.Orders.CountAsync(o => o.Status == "Delivered");
        var cancelled = await _db.Orders.CountAsync(o => o.Status == "Cancelled");

        return new OrderAdminIndexViewModel
        {
            Orders = orders,
            TotalCount = total,
            Page = page,
            PageSize = pageSize,
            PendingCount = pending,
            ConfirmedCount = confirmed,
            ShippedCount = shipped,
            DeliveredCount = delivered,
            CancelledCount = cancelled
        };
    }

    public async Task<OrderAdminDetailViewModel?> GetDetailAsync(int id)
    {
        var order = await _db.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
            .Include(o => o.StatusHistory)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null) return null;

        return new OrderAdminDetailViewModel
        {
            Id = order.Id,
            InvoiceNumber = order.InvoiceNumber ?? $"ORD{order.Id:D5}",
            CustomerName = order.CustomerName ?? (order.User != null ? order.User.FullName : "Guest"),
            CustomerEmail = order.User?.Email ?? "-",
            Phone = order.Phone,
            ShippingAddress = order.ShippingAddress,
            Note = order.Note,
            OrderDate = order.OrderDate,
            Status = order.Status,
            PaymentMethod = order.PaymentMethod,
            PaymentStatus = order.PaymentStatus,
            TotalAmount = order.TotalAmount,
            Items = order.OrderItems.Select(i => new OrderItemAdminItem
            {
                Id = i.Id,
                ProductName = i.ProductName,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList(),
            StatusHistory = order.StatusHistory
                .OrderByDescending(h => h.CreatedAt)
                .Select(h => new OrderStatusHistoryAdminItem
                {
                    Status = h.Status,
                    Note = h.Note,
                    UpdatedBy = h.UpdatedBy,
                    CreatedAt = h.CreatedAt
                }).ToList()
        };
    }

    public async Task<bool> UpdateStatusAsync(int id, string status, string? updatedBy, string? note)
    {
        var order = await _db.Orders.FindAsync(id);
        if (order == null) return false;

        order.Status = status;
        _db.OrderStatusHistories.Add(new MyProject.Models.OrderStatusHistory
        {
            OrderId = id,
            Status = status,
            Note = note,
            UpdatedBy = updatedBy,
            CreatedAt = DateTime.Now
        });
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ConfirmPaymentAsync(int id, string? transactionId, string? note, string? updatedBy)
    {
        var order = await _db.Orders.FindAsync(id);
        if (order == null) return false;

        order.PaymentStatus = "Paid";

        var existingPayment = await _db.Payments.FirstOrDefaultAsync(p => p.OrderId == id);
        if (existingPayment != null)
        {
            existingPayment.PaymentStatus = "Paid";
            existingPayment.TransactionId = transactionId;
            existingPayment.PaymentNote = note;
            existingPayment.PaidAt = DateTime.Now;
        }
        else
        {
            _db.Payments.Add(new MyProject.Models.Payment
            {
                OrderId = id,
                PaymentMethod = order.PaymentMethod,
                PaymentStatus = "Paid",
                Amount = order.TotalAmount,
                TransactionId = transactionId,
                PaymentNote = note,
                PaidAt = DateTime.Now,
                CreatedAt = DateTime.Now
            });
        }

        _db.OrderStatusHistories.Add(new MyProject.Models.OrderStatusHistory
        {
            OrderId = id,
            Status = order.Status,
            Note = note ?? "Payment confirmed",
            UpdatedBy = updatedBy,
            CreatedAt = DateTime.Now
        });

        await _db.SaveChangesAsync();
        return true;
    }
}

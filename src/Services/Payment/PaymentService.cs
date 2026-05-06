using Microsoft.EntityFrameworkCore;
using MyProject.Data;
using MyProject.Models;

using OrderModel = MyProject.Models.Order;
using PaymentModel = MyProject.Models.Payment;

namespace MyProject.Services.Payment;

public class PaymentService : IPaymentService
{
    private readonly AppDbContext _db;

    public PaymentService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<PaymentResult> RecordPaymentAsync(int orderId, string method, string? transactionId, string? note)
    {
        var order = await _db.Orders.FindAsync(orderId);
        if (order == null)
            return new PaymentResult { Success = false, Message = "Order not found." };

        var existing = await _db.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
        if (existing != null)
        {
            existing.PaymentMethod = method;
            existing.TransactionId = transactionId;
            existing.PaymentNote = note;
            existing.Amount = order.TotalAmount;
        }
        else
        {
            _db.Payments.Add(new PaymentModel
            {
                OrderId = orderId,
                PaymentMethod = method,
                PaymentStatus = "Pending",
                Amount = order.TotalAmount,
                TransactionId = transactionId,
                PaymentNote = note,
                CreatedAt = DateTime.Now
            });
        }

        await _db.SaveChangesAsync();
        return new PaymentResult { Success = true, Message = "Payment recorded." };
    }

    public async Task<PaymentResult> ConfirmPaymentAsync(int orderId, string? transactionId, string? note)
    {
        var order = await _db.Orders.FindAsync(orderId);
        if (order == null)
            return new PaymentResult { Success = false, Message = "Order not found." };

        var payment = await _db.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
        if (payment == null)
        {
            _db.Payments.Add(new PaymentModel
            {
                OrderId = orderId,
                PaymentMethod = order.PaymentMethod,
                PaymentStatus = "Paid",
                Amount = order.TotalAmount,
                TransactionId = transactionId,
                PaymentNote = note,
                PaidAt = DateTime.Now,
                CreatedAt = DateTime.Now
            });
        }
        else
        {
            payment.PaymentStatus = "Paid";
            payment.TransactionId = transactionId;
            payment.PaymentNote = note;
            payment.PaidAt = DateTime.Now;
        }

        order.PaymentStatus = "Paid";
        _db.OrderStatusHistories.Add(new OrderStatusHistory
        {
            OrderId = orderId,
            Status = order.Status,
            Note = note ?? "Payment confirmed",
            UpdatedBy = "Admin/Staff",
            CreatedAt = DateTime.Now
        });

        await _db.SaveChangesAsync();
        return new PaymentResult { Success = true, Message = "Payment confirmed." };
    }

    public async Task<PaymentResult> RefundPaymentAsync(int orderId, string? note)
    {
        var order = await _db.Orders.FindAsync(orderId);
        if (order == null)
            return new PaymentResult { Success = false, Message = "Order not found." };

        var payment = await _db.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
        if (payment != null)
        {
            payment.PaymentStatus = "Refunded";
            payment.PaymentNote = note;
        }

        order.PaymentStatus = "Refunded";
        _db.OrderStatusHistories.Add(new OrderStatusHistory
        {
            OrderId = orderId,
            Status = order.Status,
            Note = note ?? "Payment refunded",
            UpdatedBy = "Admin/Staff",
            CreatedAt = DateTime.Now
        });

        await _db.SaveChangesAsync();
        return new PaymentResult { Success = true, Message = "Payment refunded." };
    }
}

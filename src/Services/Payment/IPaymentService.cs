namespace MyProject.Services.Payment;

public interface IPaymentService
{
    Task<PaymentResult> RecordPaymentAsync(int orderId, string method, string? transactionId, string? note);
    Task<PaymentResult> ConfirmPaymentAsync(int orderId, string? transactionId, string? note);
    Task<PaymentResult> RefundPaymentAsync(int orderId, string? note);
}

public class PaymentResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

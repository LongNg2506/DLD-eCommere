using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models;

public class Payment
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    [Required]
    [MaxLength(50)]
    public string PaymentMethod { get; set; } = "COD";
    // COD | OnlinePayment

    [Required]
    [MaxLength(20)]
    public string PaymentStatus { get; set; } = "Unpaid";
    // Unpaid | Paid | Refunded

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [MaxLength(200)]
    public string? TransactionId { get; set; }

    [MaxLength(500)]
    public string? PaymentNote { get; set; }

    public DateTime? PaidAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Order? Order { get; set; }
}


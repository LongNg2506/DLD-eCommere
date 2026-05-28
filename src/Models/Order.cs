using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models;

public class Order
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.Now;

    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = "Pending";

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    [MaxLength(300)]
    public string? ShippingAddress { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(500)]
    public string? Note { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public User? User { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    public ICollection<OrderStatusHistory> StatusHistory { get; set; } = new List<OrderStatusHistory>();

    [MaxLength(100)]
    public string? CustomerName { get; set; }

    [MaxLength(20)]
    public string PaymentMethod { get; set; } = "COD";

    [MaxLength(20)]
    public string PaymentStatus { get; set; } = "Pending";

    [MaxLength(50)]
    public string? InvoiceNumber { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models;

public class CartSession
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    [MaxLength(100)]
    public string? SessionId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    public User? User { get; set; }

    public ICollection<CartSessionItem> Items { get; set; } = new List<CartSessionItem>();
}

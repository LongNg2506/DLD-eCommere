using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyProject.Models;

public class CartSessionItem
{
    public int Id { get; set; }

    public int CartSessionId { get; set; }

    public int ProductId { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    public DateTime AddedAt { get; set; } = DateTime.Now;

    public CartSession? CartSession { get; set; }

    public Product? Product { get; set; }
}

namespace MyProject.DTOs.Users;

public class UserOrderDto
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string? ShippingAddress { get; set; }
    public string? Phone { get; set; }
    public int ItemCount { get; set; }
}

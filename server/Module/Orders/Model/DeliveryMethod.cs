namespace Ecommerce.Module.Orders.Model;

public class DeliveryMethod
{
    public int Id { get; set; }
    public required string DeliveryTime { get; set; }
    public string? Description { get; set; }
    public required decimal Price { get; set; }
}
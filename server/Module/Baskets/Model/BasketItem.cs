namespace Ecommerce.Module.Baskets.Model;

public class BasketItem
{
    public int Id { get; set; }
    public required string ProductName { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string? PictureUrl { get; set; }
    public required string Brand { get; set; }
    public required string Type { get; set; }
}
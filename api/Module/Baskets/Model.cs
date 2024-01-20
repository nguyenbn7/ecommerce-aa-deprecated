using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Module.Baskets;

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

public class CustomerBasket
{
    public CustomerBasket()
    {
        Id = Guid.NewGuid().ToString();
    }

    public CustomerBasket(string id)
    {
        Id = id;
    }
    public string Id { get; set; }
    public List<BasketItem> Items { get; set; } = new();
}

public class BasketItemDTO
{
    [Required]
    public int Id { get; set; }

    [Required]
    public required string ProductName { get; set; }

    [Required]
    [Range(0.1, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    [Required]
    [Range(1, double.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }

    [Required]
    public required string PictureUrl { get; set; }

    [Required]
    public required string Brand { get; set; }

    [Required]
    public required string Type { get; set; }
}

public class CustomerBasketDTO
{
    [Required]
    public required string Id { get; set; }

    [Required]
    public required List<BasketItemDTO> Items { get; set; }
}
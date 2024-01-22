using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Module.Baskets.DTO;

public class CustomerBasketItem
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
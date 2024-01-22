using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Module.Baskets.DTO;

public class CustomerBasket
{
    [Required]
    public required string Id { get; set; }

    [Required]
    public required List<CustomerBasketItem> Items { get; set; }
}
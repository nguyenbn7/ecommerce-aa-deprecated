namespace Ecommerce.Module.Orders.Model;

public class OrderDTO
{
    public required string BasketId { get; set; }
    public int DeliveryMethodId { get; set; }
    public required AddressDTO ShipToAddress { get; set; }
}
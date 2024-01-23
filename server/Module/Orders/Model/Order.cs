namespace Ecommerce.Module.Orders.Model;

public class Order
{
    public Order()
    {
    }

    public Order(IReadOnlyList<OrderItem> orderItems,
                 string buyerEmail,
                 DeliveryMethod deliveryMethod,
                 Address shipToAddress,
                 decimal subTotal)
    {
        BuyerEmail = buyerEmail;
        SubTotal = subTotal;
        ShipToAddress = shipToAddress;
        OrderItems = orderItems;
        DeliveryMethod = deliveryMethod;
    }

    public int Id { get; set; }
    public required string BuyerEmail { get; set; }
    public required decimal SubTotal { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    public required Address ShipToAddress { get; set; }
    public IReadOnlyList<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public Status OrderStatus { get; set; } = Status.Pending;
    public string? PaymentIntentId { get; set; }
    public required DeliveryMethod DeliveryMethod { get; set; }

    public decimal GetTotal()
    {
        return SubTotal + DeliveryMethod.Price;
    }
}
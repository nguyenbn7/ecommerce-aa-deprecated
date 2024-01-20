using System.Runtime.Serialization;

namespace Ecommerce.Module.Orders;

public class Address
{
    public Address()
    {
    }

    public Address(string firstName, string lastName, string street, string city, string state, string zipCode)
    {
        FirstName = firstName;
        LastName = lastName;
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
    }

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Street { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string ZipCode { get; set; }
}

public class DeliveryMethod
{
    public int Id { get; set; }
    public required string DeliveryTime { get; set; }
    public string? Description { get; set; }
    public required decimal Price { get; set; }
}

public class ProductItemOrdered
{
    public ProductItemOrdered()
    {
    }

    public ProductItemOrdered(int productItemId, string productName, string pictureUrl)
    {
        ProductItemId = productItemId;
        ProductName = productName;
        PictureUrl = pictureUrl;
    }

    public int ProductItemId { get; set; }
    public required string ProductName { get; set; }
    public required string PictureUrl { get; set; }
}

public enum OrderStatus
{
    [EnumMember(Value = "Pending")]
    Pending,
    [EnumMember(Value = "Payment Received")]
    PaymentReceived,
    [EnumMember(Value = "Payment Failed")]
    PaymentFailed
}

public class OrderItem
{
    public OrderItem()
    {
    }

    public OrderItem(int id, ProductItemOrdered itemOrdered, decimal price, int quantity)
    {
        Id = id;
        ItemOrdered = itemOrdered;
        Price = price;
        Quantity = quantity;
    }

    public int Id { get; set; }
    public required ProductItemOrdered ItemOrdered { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
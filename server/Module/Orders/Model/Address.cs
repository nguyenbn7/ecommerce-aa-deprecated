namespace Ecommerce.Module.Orders.Model;

public class Address
{
    public Address()
    {
    }

    public Address(string firstName,
                   string lastName,
                   string street,
                   string city,
                   string state,
                   string zipCode)
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
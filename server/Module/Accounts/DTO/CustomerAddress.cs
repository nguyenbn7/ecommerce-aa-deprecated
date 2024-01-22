namespace Ecommerce.Module.Accounts.DTO;

public class CustomerAddress
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Street { get; set; }
    public required string City { get; set; }
    public required string State { get; set; }
    public required string ZipCode { get; set; }
}
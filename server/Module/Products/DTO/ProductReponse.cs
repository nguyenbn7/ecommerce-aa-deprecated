namespace Ecommerce.Module.Products.DTO;

public class ProductReponse
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public string? PictureUrl { get; set; }
    public required string ProductType { get; set; }
    public required string ProductBrand { get; set; }
}
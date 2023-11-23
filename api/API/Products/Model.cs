using Ecommerce.API.ProductBrands;
using Ecommerce.API.ProductTypes;

namespace Ecommerce.API.Products;

public class Product
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public required string PictureUrl { get; set; }

    public ProductType ProductType { get; set; } = null!;
    public int ProductTypeId { get; set; }

    public ProductBrand ProductBrand { get; set; } = null!;
    public int ProductBrandId { get; set; }
}

public class ProductDTO
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public string? PictureUrl { get; set; }
    public required string ProductType { get; set; }
    public required string ProductBrand { get; set; }
}
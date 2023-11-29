using Ecommerce.Routes.ProductBrands;
using Ecommerce.Routes.ProductTypes;

namespace Ecommerce.Routes.Products;

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

public class ProductsParam
{
    private const int MaxPageSize = 50;
    private const int DefaultPageSize = 10;
    private int pageSize = DefaultPageSize;
    private int pageIndex = 0;
    private string? search;

    public int PageIndex
    {
        get => pageIndex;
        set => pageIndex = value < 0 ? 0 : value - 1;
    }

    public int PageSize
    {
        get => pageSize;
        set => pageSize = (value < 1)
            ? DefaultPageSize : (value > MaxPageSize) ? MaxPageSize : value;
    }
    public int? BrandId { get; set; }
    public int? TypeId { get; set; }
    public string? Sort { get; set; }
    public string? Search
    {
        get => search;
        set => search = value?.ToLower();
    }
}
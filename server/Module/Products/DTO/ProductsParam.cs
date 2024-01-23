namespace Ecommerce.Module.Products.DTO;

public class ProductsParam
{
    private const int MaxPageSize = 100;
    private const int DefaultPageSize = 9;
    private int pageSize = DefaultPageSize;
    private int pageNumber = 1;
    private string? search;

    public int PageNumber
    {
        get => pageNumber;
        set => pageNumber = value < 1 ? 1 : value;
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
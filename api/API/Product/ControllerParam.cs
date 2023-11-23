namespace Ecommerce.API.Product.Controller.Param;

public class ProductsParam
{
    private const int MaxPageSize = 50;
    private const int DefaultPageSize = 10;
    private int pageSize = DefaultPageSize;
    private int pageIndex = 1;
    private string? search;

    public int PageIndex
    {
        get => pageIndex;
        set => pageIndex = value < 1 ? 1 : value;
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
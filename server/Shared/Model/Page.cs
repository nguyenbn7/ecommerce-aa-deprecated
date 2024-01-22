namespace Ecommerce.Shared.Model;

public class Page<T> where T : class
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public required IReadOnlyList<T> Data { get; set; }
}
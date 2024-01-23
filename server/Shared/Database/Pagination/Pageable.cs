namespace Ecommerce.Shared.Model.Pagination;

public class Pageable
{
    public int Number { get; init; }
    public int Size { get; init; }

    public static Pageable Of(int pageNumber, int pageSize)
    {
        return new Pageable
        {
            Number = pageNumber < 0 ? 0 : pageNumber,
            Size = pageSize < 1 ? 6 : pageSize
        };
    }
}
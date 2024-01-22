namespace Ecommerce.Shared.Model;

public class Pageable
{
    public int Index { get; init; }
    public int Size { get; init; }

    public static Pageable Of(int pageIndex, int pageSize)
    {
        return new Pageable
        {
            Index = pageIndex < 0 ? 0 : pageIndex,
            Size = pageSize < 1 ? 6 : pageSize
        };
    }
}
using System.Linq.Expressions;

namespace Ecommerce.Share.Specification;

public interface ISpecification<TEntity> where TEntity : class
{
    Expression<Func<TEntity, bool>>? ToPredicate(CriteriaBuilder<TEntity> builder);
}

public enum SortDirection
{
    ASC = 1,
    DESC = 2
}

public class Sort<TEntity, TKey> where TEntity : class
{
    public Expression<Func<TEntity, TKey>> By { get; init; }
    public SortDirection Direction { get; init; }

    public Sort(Expression<Func<TEntity, TKey>> by)
    {
        By = by;
        Direction = SortDirection.ASC;
    }
}


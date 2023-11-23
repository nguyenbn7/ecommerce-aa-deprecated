using System.Linq.Expressions;
using Ecommerce.Share.Model;

namespace Ecommerce.Share.Specification;

public interface IPredicateSpecification<TEntity> where TEntity : class
{
    Expression<Func<TEntity, bool>>? ToPredicate(PredicateBuilder<TEntity> builder);
}

public interface IIncludeSpecification<TEntity> where TEntity : class
{
    Expression<Func<TEntity, object>> IncludeProperty();
}

public enum SortDirection
{
    ASC = 1,
    DESC = 2
}

public class Sort<TEntity> where TEntity : class
{
    public Expression<Func<TEntity, object>> SortExpression { get; private set; }
    public SortDirection Direction { get; private set; }

    public Sort(Expression<Func<TEntity, object>> sortExpression, SortDirection direction = SortDirection.ASC)
    {
        SortExpression = sortExpression;
        Direction = direction;
    }
}


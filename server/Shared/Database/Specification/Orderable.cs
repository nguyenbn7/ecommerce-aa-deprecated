using System.Linq.Expressions;

namespace Ecommerce.Shared.Database.Specification;

public sealed class Orderable<TEntity> where TEntity : class
{
    public Expression<Func<TEntity, object>> OrderedProperty { get; private set; }
    public OrderDirection OrderDirection { get; private set; }

    public Orderable(Expression<Func<TEntity, object>> orderedProperty,
                     OrderDirection orderDirection = OrderDirection.ASC)
    {
        OrderedProperty = orderedProperty;
        OrderDirection = orderDirection;
    }
}

public enum OrderDirection
{
    ASC = 1,
    DESC = 2
}
using System.Linq.Expressions;

namespace Ecommerce.Shared.Database.Specification;

public sealed class Includable<TEntity> where TEntity : class
{
    public Expression<Func<TEntity, object>> IncludedProperty { get; private set; }

    public Includable(Expression<Func<TEntity, object>> includedProperty)
    {
        IncludedProperty = includedProperty;
    }
}
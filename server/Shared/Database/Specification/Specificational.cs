using System.Linq.Expressions;

namespace Ecommerce.Shared.Database.Specification;

public interface Specificational<TEntity> where TEntity : class
{
    Expression<Func<TEntity, bool>> IsSatisfiedBy();
    Specificational<TEntity> And(Specificational<TEntity> other);
    Specificational<TEntity> AndNot(Specificational<TEntity> other);
    Specificational<TEntity> Or(Specificational<TEntity> other);
    Specificational<TEntity> OrNot(Specificational<TEntity> other);
    Specificational<TEntity> Not();
}
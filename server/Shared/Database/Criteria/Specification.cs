using System.Linq.Expressions;

namespace Ecommerce.Shared.Database.Criteria;

public interface Specification<TEntity> where TEntity : class
{
    Expression<Func<TEntity, bool>> IsSatisfiedBy();
    Specification<TEntity> And(Specification<TEntity> other);
    Specification<TEntity> AndNot(Specification<TEntity> other);
    Specification<TEntity> Or(Specification<TEntity> other);
    Specification<TEntity> OrNot(Specification<TEntity> other);
    Specification<TEntity> Not(Specification<TEntity> other);
}
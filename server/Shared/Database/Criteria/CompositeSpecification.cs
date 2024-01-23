using System.Linq.Expressions;

namespace Ecommerce.Shared.Database.Criteria;

public abstract class CompositeSpecification<TEntity> : Specification<TEntity> where TEntity : class
{    
    public abstract Expression<Func<TEntity, bool>> IsSatisfiedBy();

    public Specification<TEntity> And(Specification<TEntity> other)
    {
        return new AndSpecification<TEntity>(this, other);
    }

    public Specification<TEntity> AndNot(Specification<TEntity> other)
    {
        throw new NotImplementedException();
    }

    public Specification<TEntity> Not(Specification<TEntity> other)
    {
        throw new NotImplementedException();
    }

    public Specification<TEntity> Or(Specification<TEntity> other)
    {
        throw new NotImplementedException();
    }

    public Specification<TEntity> OrNot(Specification<TEntity> other)
    {
        throw new NotImplementedException();
    }
}
using System.Linq.Expressions;

namespace Ecommerce.Share;

public class CriteriaBuilder<TEntity>
{
    public Criteria<TEntity> Construct(Expression<Func<TEntity, bool>>? criteria = null)
    {
        return new Criteria<TEntity>(criteria);
    }
}

public class Criteria<TEntity>
{
    private Expression<Func<TEntity, bool>>? criterion = null;

    public Criteria(Expression<Func<TEntity, bool>>? criteria)
    {
        criterion = criteria;
    }

    public Criteria<TEntity> And(Expression<Func<TEntity, bool>> predicate)
    {
        if (criterion != null)
            criterion = Expression.Lambda<Func<TEntity, bool>>(Expression.Add(criterion.Body, predicate.Body));
        else
            criterion = predicate;
        return this;
    }

    public Criteria<TEntity> Or(Expression<Func<TEntity, bool>> predicate)
    {
        if (criterion != null)
            criterion = Expression.Lambda<Func<TEntity, bool>>(Expression.Or(criterion.Body, predicate.Body));
        else
            criterion = predicate;
        return this;
    }

    public Criteria<TEntity> Not(Expression<Func<TEntity, bool>> predicate)
    {
        if (criterion != null)
            criterion = Expression.Lambda<Func<TEntity, bool>>(Expression.Not(criterion));
        else
            criterion = Expression.Lambda<Func<TEntity, bool>>(Expression.Not(predicate));
        return this;
    }


    public Expression<Func<TEntity, bool>>? ToPredicate()
    {
        return criterion;
    }
}
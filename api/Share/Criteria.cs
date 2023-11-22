using System.Linq.Expressions;

namespace Ecommerce.Share;

public class CriteriaBuilder<TEntity>
{
    private Expression<Func<TEntity, bool>>? criterion = null;

    public Criteria<TEntity> Construct()
    {
        return new Criteria<TEntity>();
    }

    public Expression<Func<TEntity, bool>> ToPredicate()
    {
        if (criterion == null)
            return p => 1 == 1;
        return criterion;
    }
}

public class Criteria<TEntity>
{
    private Expression<Func<TEntity, bool>>? criterion = null;

    public Criteria<TEntity> Add(Expression<Func<TEntity, bool>> predicate)
    {
        criterion = predicate;
        return this;
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
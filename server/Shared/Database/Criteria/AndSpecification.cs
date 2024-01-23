using System.Linq.Expressions;

namespace Ecommerce.Shared.Database.Criteria;

public class AndSpecification<TEntity> : CompositeSpecification<TEntity> where TEntity : class
{
    private readonly Specification<TEntity> leftCondition;
    private readonly Specification<TEntity> rightCondition;

    public AndSpecification(Specification<TEntity> left, Specification<TEntity> right)
    {
        leftCondition = left;
        rightCondition = right;
    }

    public override Expression<Func<TEntity, bool>> IsSatisfiedBy()
    {
        var @param = Expression.Parameter(typeof(TEntity), "x");
        return Expression.Lambda<Func<TEntity, bool>>(
            Expression.AndAlso(
                Expression.Invoke(leftCondition.IsSatisfiedBy(), @param),
                Expression.Invoke(rightCondition.IsSatisfiedBy(), @param)),
            @param
        );
    }
}
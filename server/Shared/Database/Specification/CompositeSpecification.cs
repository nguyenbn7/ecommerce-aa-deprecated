using System.Linq.Expressions;

namespace Ecommerce.Shared.Database.Specification;

public abstract class CompositeSpecification<TEntity> : Specificational<TEntity> where TEntity : class
{
    public abstract Expression<Func<TEntity, bool>> IsSatisfiedBy();
    public Specificational<TEntity> And(Specificational<TEntity> other) => new AndExpression(this, other);
    public Specificational<TEntity> Or(Specificational<TEntity> other) => new OrExpression(this, other);
    public Specificational<TEntity> Not() => new NotExpression(this);
    public Specificational<TEntity> AndNot(Specificational<TEntity> other) => new NotExpression(new AndExpression(this, other));
    public Specificational<TEntity> OrNot(Specificational<TEntity> other) => new NotExpression(new OrExpression(this, other));

    private class AndExpression : CompositeSpecification<TEntity>
    {
        private readonly Specificational<TEntity> leftCondition;
        private readonly Specificational<TEntity> rightCondition;

        public AndExpression(Specificational<TEntity> left,
                             Specificational<TEntity> right)
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

    private class OrExpression : CompositeSpecification<TEntity>
    {
        private readonly Specificational<TEntity> leftCondition;
        private readonly Specificational<TEntity> rightCondition;

        public OrExpression(Specificational<TEntity> left,
                            Specificational<TEntity> right)
        {
            leftCondition = left;
            rightCondition = right;
        }

        public override Expression<Func<TEntity, bool>> IsSatisfiedBy()
        {
            var @param = Expression.Parameter(typeof(TEntity), "x");
            return Expression.Lambda<Func<TEntity, bool>>(
                Expression.OrElse(
                    Expression.Invoke(leftCondition.IsSatisfiedBy(), @param),
                    Expression.Invoke(rightCondition.IsSatisfiedBy(), @param)),
                @param
            );
        }
    }

    private class NotExpression : CompositeSpecification<TEntity>
    {
        private readonly Specificational<TEntity> wrapped;

        public NotExpression(Specificational<TEntity> x)
        {
            wrapped = x;
        }

        public override Expression<Func<TEntity, bool>> IsSatisfiedBy()
        {
            return Expression.Lambda<Func<TEntity, bool>>(
                Expression.Not(this.wrapped.IsSatisfiedBy()), this.wrapped.IsSatisfiedBy().Parameters[0]
            );
        }
    }
}
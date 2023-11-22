using System.Linq.Expressions;

namespace Ecommerce.Share;

public class ITokenService
{
    
}

public interface ISpecification<TEntity> where TEntity : class
{
    Expression<Func<TEntity, bool>>? ToPredicate(CriteriaBuilder<TEntity> builder);
}

public interface IRepository<TEntity, TKey> where TEntity : class
{
    Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecification<TEntity>? specification = null, List<Sort<TEntity, TKey>>? sorts = null);
    Task<Pagination<TEntity>> GetAllAsync(Pageable pageable, ISpecification<TEntity>? specification = null, List<Sort<TEntity, TKey>>? sorts = null);
    Task<TEntity?> GetOneAsync(ISpecification<TEntity>? specification = null, List<Sort<TEntity, TKey>>? sorts = null);
}
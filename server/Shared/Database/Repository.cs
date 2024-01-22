using Ecommerce.Shared.Model;

namespace Ecommerce.Shared.Database;

public interface Repository<TEntity, TKey> where TEntity : class
{
    Task<IReadOnlyList<TEntity>> GetAllAsync(
        IEnumerable<IIncludeSpecification<TEntity>>? includes = null,
        IPredicateSpecification<TEntity>? specification = null,
        IEnumerable<Sort<TEntity>>? sorts = null
    );
    Task<Page<TEntity>> GetAllAsync(
        Pageable pageable,
        IEnumerable<IIncludeSpecification<TEntity>>? includes = null,
        IPredicateSpecification<TEntity>? specification = null,
        IEnumerable<Sort<TEntity>>? sorts = null
    );
    Task<TEntity?> GetOneAsync(
        IEnumerable<IIncludeSpecification<TEntity>>? includes = null,
        IPredicateSpecification<TEntity>? specification = null,
        IEnumerable<Sort<TEntity>>? sorts = null
    );
}
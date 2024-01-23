using Ecommerce.Shared.Database.Specification;
using Ecommerce.Shared.Model.Pagination;

namespace Ecommerce.Shared.Database;

public interface Repository<TEntity, TKey> where TEntity : class
{
    Task<TEntity?> GetFirstOrDefaultAsync(Specificational<TEntity> predicates);
    Task<TEntity?> GetFirstOrDefaultAsync(IEnumerable<Includable<TEntity>> properties,
                                          Specificational<TEntity> predicates);

    Task<List<TEntity>> GetAllAsync();
    Task<Page<TEntity>> GetAllAsync(List<Includable<TEntity>> includedProperties,
                                    Specificational<TEntity> predicates,
                                    List<Orderable<TEntity>> orderedProperties,
                                    Pageable pageable);
}
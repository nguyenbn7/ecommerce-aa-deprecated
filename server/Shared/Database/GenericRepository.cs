using Ecommerce.Shared.Database.Specification;
using Ecommerce.Shared.Model.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Shared.Database;

public class GenericRepository<TEntity, TKey> : Repository<TEntity, TKey> where TEntity : class
{
    private readonly AppDbContext _context;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<TEntity?> GetFirstOrDefaultAsync(Specificational<TEntity> predicates)
    {
        var query = _context.Set<TEntity>().AsNoTracking().AsQueryable();

        if (predicates != null)
            query = query.Where(predicates.IsSatisfiedBy());

        return query.FirstOrDefaultAsync();
    }

    public Task<TEntity?> GetFirstOrDefaultAsync(IEnumerable<Includable<TEntity>> properties,
                                                 Specificational<TEntity> predicates)
    {
        var query = _context.Set<TEntity>().AsNoTracking().AsQueryable();

        if (properties != null)
            query = properties.Aggregate(query, (queryAcc, includable) => queryAcc.Include(includable.IncludedProperty));

        if (predicates != null)
            query = query.Where(predicates.IsSatisfiedBy());

        return query.FirstOrDefaultAsync();
    }

    public Task<List<TEntity>> GetAllAsync() => _context.Set<TEntity>().AsNoTracking().AsQueryable().ToListAsync();

    public async Task<Page<TEntity>> GetAllAsync(List<Includable<TEntity>> includedProperties,
                                           Specificational<TEntity> predicates,
                                           List<Orderable<TEntity>> orderedProperties,
                                           Pageable pageable)
    {
        var query = _context.Set<TEntity>().AsNoTracking().AsQueryable();

        if (includedProperties != null)
            query = includedProperties.Aggregate(query,
                                                 (queryAcc, includable) => queryAcc.Include(includable.IncludedProperty));

        if (predicates != null)
            query = query.Where(predicates.IsSatisfiedBy());

        var totalItems = await query.CountAsync();

        if (orderedProperties != null)
            query = orderedProperties.Aggregate(query, (queryAcc, orderable) =>
            {
                if (orderable.OrderDirection == OrderDirection.DESC)
                    return queryAcc.OrderByDescending(orderable.OrderedProperty);

                return queryAcc.OrderBy(orderable.OrderedProperty);
            });

        query = query.Skip((pageable.Number - 1) * pageable.Size).Take(pageable.Size);
        var data = await query.ToListAsync();

        return new Page<TEntity>
        {
            PageNumber = pageable.Number,
            PageSize = pageable.Size,
            TotalItems = totalItems,
            Data = data
        };
    }
}
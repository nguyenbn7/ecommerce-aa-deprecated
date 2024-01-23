using Ecommerce.Shared.Database.Criteria;
using Ecommerce.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Shared.Database;

public class GenericRepository<TEntity, TKey> : Repository<TEntity, TKey> where TEntity : class
{
    private readonly DbContext dbContext;

    public GenericRepository(DbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(
        IEnumerable<IIncludeSpecification<TEntity>>? includes = null,
        IPredicateSpecification<TEntity>? specification = null,
        IEnumerable<Sort<TEntity>>? sorts = null
    )
    {
        var query = dbContext.Set<TEntity>().AsNoTracking().AsQueryable();
        var predicate = specification?.ToPredicate(new PredicateBuilder<TEntity>());

        if (includes != null)
        {
            foreach (var includeExpression in includes)
            {
                query = query.Include(includeExpression.IncludeProperty());
            }
        }

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (sorts != null)
        {
            foreach (var sort in sorts)
            {
                if (sort.Direction == SortDirection.ASC)
                    query = query.OrderBy(sort.SortExpression);
                else query = query.OrderByDescending(sort.SortExpression);
            }
        }

        return await query.ToListAsync();
    }

    public async Task<Page<TEntity>> GetAllAsync(
        Pageable pageable,
        Specification<TEntity>? specification = null,
        IEnumerable<IIncludeSpecification<TEntity>>? includes = null,
        IEnumerable<Sort<TEntity>>? sorts = null
    )
    {
        var query = dbContext.Set<TEntity>().AsNoTracking().AsQueryable();

        if (includes != null)
        {
            foreach (var includeExpression in includes)
            {
                query = query.Include(includeExpression.IncludeProperty());
            }
        }

        if (specification != null)
        {
            query = query.Where(specification.IsSatisfiedBy());
        }

        var totalItems = await query.CountAsync();

        if (sorts != null)
        {
            foreach (var sort in sorts)
            {
                if (sort.Direction == SortDirection.ASC)
                    query = query.OrderBy(sort.SortExpression);
                else query = query.OrderByDescending(sort.SortExpression);
            }
        }

        query = query.Skip(pageable.Index).Take(pageable.Size);
        var data = await query.ToListAsync();

        return new Page<TEntity>
        {
            PageIndex = pageable.Index,
            PageSize = data.Count,
            TotalItems = totalItems,
            Data = data
        };
    }

    public async Task<TEntity?> GetOneAsync(
        IEnumerable<IIncludeSpecification<TEntity>>? includes = null,
        IPredicateSpecification<TEntity>? specification = null,
        IEnumerable<Sort<TEntity>>? sorts = null
    )
    {
        var query = dbContext.Set<TEntity>().AsNoTracking().AsQueryable();

        if (includes != null)
        {
            foreach (var includeExpression in includes)
            {
                query = query.Include(includeExpression.IncludeProperty());
            }
        }

        if (sorts != null)
        {
            foreach (var sort in sorts)
            {
                if (sort.Direction == SortDirection.ASC)
                    query = query.OrderBy(sort.SortExpression);
                else query = query.OrderByDescending(sort.SortExpression);
            }
        }

        var predicate = specification?.ToPredicate(new PredicateBuilder<TEntity>());

        if (predicate != null)
        {
            return await query.FirstOrDefaultAsync(predicate);
        }

        return await query.FirstOrDefaultAsync();
    }
}
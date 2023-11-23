using Ecommerce.Share.Model;
using Ecommerce.Share.Specification;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Share.GenericRepository;

public interface IRepository<TEntity, TKey> where TEntity : class
{
    Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecification<TEntity>? specification = null, List<Sort<TEntity, TKey>>? sorts = null);
    Task<Page<TEntity>> GetAllAsync(Pageable pageable, ISpecification<TEntity>? specification = null, List<Sort<TEntity, TKey>>? sorts = null);
    Task<TEntity?> GetOneAsync(ISpecification<TEntity>? specification = null, List<Sort<TEntity, TKey>>? sorts = null);
}

public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class
{
    private readonly DbContext dbContext;

    public Repository(DbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(ISpecification<TEntity>? specification = null, List<Sort<TEntity, TKey>>? sorts = null)
    {
        var query = dbContext.Set<TEntity>().AsNoTracking().AsQueryable();
        var predicate = specification?.ToPredicate(new CriteriaBuilder<TEntity>());

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        if (sorts != null)
        {
            foreach (var sort in sorts)
            {
                if (sort.Direction == SortDirection.ASC)
                    query = query.OrderBy(sort.By);
                else query = query.OrderByDescending(sort.By);
            }
        }

        return await query.ToListAsync();
    }

    public async Task<Page<TEntity>> GetAllAsync(Pageable pageable, ISpecification<TEntity>? specification = null, List<Sort<TEntity, TKey>>? sorts = null)
    {
        var query = dbContext.Set<TEntity>().AsNoTracking().AsQueryable();
        var predicate = specification?.ToPredicate(new CriteriaBuilder<TEntity>());

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        var totalItems = await query.CountAsync();

        if (sorts != null)
        {
            foreach (var sort in sorts)
            {
                if (sort.Direction == SortDirection.ASC)
                    query = query.OrderBy(sort.By);
                else query = query.OrderByDescending(sort.By);
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

    public async Task<TEntity?> GetOneAsync(ISpecification<TEntity>? specification = null, List<Sort<TEntity, TKey>>? sorts = null)
    {
        var query = dbContext.Set<TEntity>().AsNoTracking().AsQueryable();
        if (sorts != null)
        {
            foreach (var sort in sorts)
            {
                if (sort.Direction == SortDirection.ASC)
                    query = query.OrderBy(sort.By);
                else query = query.OrderByDescending(sort.By);
            }
        }

        var predicate = specification?.ToPredicate(new CriteriaBuilder<TEntity>());

        if (predicate != null)
        {
            return await query.FirstOrDefaultAsync(predicate);
        }

        return await query.FirstOrDefaultAsync();
    }
}

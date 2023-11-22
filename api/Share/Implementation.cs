using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Share;



public enum SortDirection
{
    ASC = 1,
    DESC = 2
}

public class Sort<TEntity, TKey> where TEntity : class
{
    public Expression<Func<TEntity, TKey>> By { get; init; }
    public SortDirection Direction { get; init; }

    public Sort(Expression<Func<TEntity, TKey>> by)
    {
        By = by;
        Direction = SortDirection.ASC;
    }
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

    public async Task<Pagination<TEntity>> GetAllAsync(Pageable pageable, ISpecification<TEntity>? specification = null, List<Sort<TEntity, TKey>>? sorts = null)
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

        return new Pagination<TEntity>
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

public class Pagination<T> where T : class
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public required IReadOnlyList<T> Data { get; set; }
}

public class Pageable
{
    public int Index { get; init; }
    public int Size { get; init; }

    public static Pageable Of(int pageIndex, int pageSize)
    {
        return new Pageable
        {
            Index = pageIndex < 0 ? 0 : pageIndex,
            Size = pageSize < 1 ? 6 : pageSize
        };
    }
}

public class TokenService : ITokenService
{
    
}
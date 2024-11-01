using CadastroApi.Infrastructure.Data;
using CadastroApi.Domain.IRepository;
using Microsoft.EntityFrameworkCore;

namespace CadastroApi.Infrastructure.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<T>> GetAllAsync(int? pageIndex, int? pageSize)
    {
        var query = _context.Set<T>()
            .AsNoTracking()            
            .AsQueryable();

        if (pageIndex is null || pageSize is null)
            return await query.ToListAsync();

        return await query
            .Skip((pageIndex.Value - 1) * pageSize.Value)
            .Take(pageSize.Value)
            .ToListAsync();
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);
    }
    public async Task CreateAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }
    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
        _context.SaveChanges();
    }
    public async Task Delete(Guid id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity is not null)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

}

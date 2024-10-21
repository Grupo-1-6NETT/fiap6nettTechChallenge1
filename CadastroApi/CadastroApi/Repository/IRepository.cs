namespace CadastroApi.Repository;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAllAsync(int? pageIndex, int? pageSize);
    Task<T> GetByIdAsync(Guid id);
    Task CreateAsync(T entity);
    void Update(T entity);
    Task Delete(Guid id);
}

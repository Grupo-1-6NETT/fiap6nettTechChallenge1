using CadastroApi.Models;

namespace CadastroApi.Repository;

public interface IContatoRepository : IRepository<Contato>
{
    Task<IEnumerable<Contato>> GetAllAsync(int? pageIndex, int? pageSize);
    Task<IEnumerable<Contato>> GetByDDDAsync(string ddd, int? pageIndex, int? pageSize);
    Task AddContatoAsync(Contato contato);
    Task UpdateContatoAsync(Contato contato);   
}

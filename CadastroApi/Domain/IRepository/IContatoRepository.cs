using CadastroApi.Domain.Models;

namespace CadastroApi.Domain.IRepository;

public interface IContatoRepository : IRepository<Contato>
{
    Task<IEnumerable<Contato>> GetByDDDAsync(string ddd, int? pageIndex, int? pageSize);
    Task AddContatoAsync(Contato contato);
    Task UpdateContatoAsync(Contato contato);
}

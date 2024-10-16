using CadastroApi.Models;

namespace CadastroApi.Repository;

public interface IContatoRepository : IRepository<Contato>
{
    Task<IEnumerable<Contato>> GetByDDDAsync(string ddd);
    Task AddContatoAsync(Contato contato);
}

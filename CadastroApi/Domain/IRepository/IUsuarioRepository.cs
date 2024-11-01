using CadastroApi.Domain.Models;

namespace CadastroApi.Domain.IRepository;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task AddUserAsync(Usuario usuario, string senha);
    Task<Usuario?> GetUserAsync(string nome, string senha);
}

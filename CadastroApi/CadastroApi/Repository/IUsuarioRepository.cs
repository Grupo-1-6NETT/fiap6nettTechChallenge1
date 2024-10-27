using CadastroApi.Models;

namespace CadastroApi.Repository
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task AddUserAsync(Usuario usuario, string senha);
        Task<Usuario?> GetUserAsync(string nome, string senha);
    }
}

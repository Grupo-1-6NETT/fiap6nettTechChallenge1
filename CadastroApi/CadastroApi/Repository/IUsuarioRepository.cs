using CadastroApi.Models;

namespace CadastroApi.Repository
{
    public interface IUsuarioRepository
    {
        Task AddUserAsync(Usuario usuario, string senha);
        Task<Usuario?> GetUserAsync(string nome, string senha);
    }
}

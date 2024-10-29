using CadastroApi.Models;

namespace CadastroApi.Services
{
    public interface ITokenService
    {
        string GetToken(Usuario usuario);
    }
}

using CadastroApi.Domain.Models;

namespace CadastroApi.Domain.Services
{
    public interface ITokenService
    {
        string GetToken(Usuario usuario);
    }
}

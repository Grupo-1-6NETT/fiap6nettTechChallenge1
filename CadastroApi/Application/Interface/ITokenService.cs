using CadastroApi.Domain.Models;

namespace CadastroApi.Application.Interface
{
    public interface ITokenService
    {
        string GetToken(Usuario usuario);
    }
}

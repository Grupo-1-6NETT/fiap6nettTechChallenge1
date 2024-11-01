using CadastroApi.Domain.Models;

namespace CadastroApi.Domain.IToken
{
    public interface ITokenService
    {
        string GetToken(Usuario usuario);
    }
}

using MediatR;

namespace CadastroApi.Application
{
    public class ListarTokenQuery : IRequest<string>
    {
        public string Usuario { get; }
        public string Senha { get; }

        public ListarTokenQuery(string usuario, string senha)
        {
            Usuario = usuario;
            Senha = senha;
        }
    }
}


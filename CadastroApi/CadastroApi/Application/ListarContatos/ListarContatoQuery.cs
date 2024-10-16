using CadastroApi.Models;
using MediatR;

namespace CadastroApi.Application
{
    public class ListarContatoQuery : IRequest<IEnumerable<Contato>>
    {
    }
}

using CadastroApi.Models;
using MediatR;

namespace CadastroApi.Application;

public class ListarContatoQuery : IRequest<IEnumerable<Contato>>
{
    public string? DDD { get; set; }
    public int? PageIndex { get; set; }
    public int? PageSize { get; set; }
}

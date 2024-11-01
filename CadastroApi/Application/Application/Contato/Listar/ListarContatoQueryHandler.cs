using CadastroApi.Domain.Models;
using CadastroApi.Domain.IRepository;
using MediatR;

namespace CadastroApi.Application;

public class ListarContatoQueryHandler : IRequestHandler<ListarContatoQuery, IEnumerable<Contato>>
{
    private readonly IContatoRepository _contatoRepository;

    public ListarContatoQueryHandler(IContatoRepository contatoRepository)
    {
        _contatoRepository = contatoRepository;
    }

    public async Task<IEnumerable<Contato>> Handle(ListarContatoQuery query, CancellationToken cancellationToken)
    {
        if(string.IsNullOrEmpty(query.DDD))
            return await _contatoRepository.GetAllAsync(query.PageIndex, query.PageSize);

        return await _contatoRepository.GetByDDDAsync(query.DDD, query.PageIndex, query.PageSize);
    }
}

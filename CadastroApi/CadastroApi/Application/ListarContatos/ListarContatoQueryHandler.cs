using CadastroApi.Models;
using CadastroApi.Repository;
using MediatR;

namespace CadastroApi.Application
{
    public class ListarContatoQueryHandler : IRequestHandler<ListarContatoQuery, IEnumerable<Contato>>
    {
        private readonly IContatoRepository _contatoRepository;

        public ListarContatoQueryHandler(IContatoRepository contatoRepository)
        {
            _contatoRepository = contatoRepository;
        }

        public async Task<IEnumerable<Contato>> Handle(ListarContatoQuery query, CancellationToken cancellationToken)
        {
            return await _contatoRepository.GetAllAsync();
        }
    }
}

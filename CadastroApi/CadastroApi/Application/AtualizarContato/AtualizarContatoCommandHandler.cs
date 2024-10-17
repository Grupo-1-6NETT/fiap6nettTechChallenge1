using CadastroApi.Models;
using CadastroApi.Repository;
using MediatR;

namespace CadastroApi.Application
{
    public class AtualizarContatoCommandHandler : IRequestHandler<AtualizarContatoCommand, Guid>
    {
        private readonly IContatoRepository _contatoRepository;

        public AtualizarContatoCommandHandler(IContatoRepository contatoRepository)
        {
            _contatoRepository = contatoRepository;
        }

        public async Task<Guid> Handle(AtualizarContatoCommand command, CancellationToken cancellationToken)
        {
            command.Validate();

            var contato = new Contato
            {
                Id = command.ID,
                Nome = command.Nome,
                Telefone = command.Telefone,
                DDD = command.DDD,
                Email = command.Email,
            };

            await _contatoRepository.UpdateContatoAsync(contato);

            return contato.Id;
        }
    }
}

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

            var contato = await _contatoRepository.GetByIdAsync(command.ID);

            if (contato is null)
                throw new KeyNotFoundException();

            contato.Nome = command.Nome;
            contato.Telefone = command.Telefone;
            contato.DDD = command.DDD;
            contato.Email = command.Email;

            await _contatoRepository.UpdateContatoAsync(contato);

            return contato.Id;
        }
    }
}

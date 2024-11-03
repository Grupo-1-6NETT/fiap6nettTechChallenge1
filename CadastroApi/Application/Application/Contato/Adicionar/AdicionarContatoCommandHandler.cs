using CadastroApi.Domain.Models;
using MediatR;
using FluentValidation;
using CadastroApi.Domain.IRepository;

namespace CadastroApi.Application;

public class AdicionarContatoCommandHandler : IRequestHandler<AdicionarContatoCommand, Guid>
{
    private readonly IContatoRepository _contatoRepository;

    public AdicionarContatoCommandHandler(IContatoRepository contatoRepository)
    {
        _contatoRepository = contatoRepository;
    }

    public async Task<Guid> Handle(AdicionarContatoCommand command, CancellationToken cancellationToken)
    {
        command.Validate();

        var contato = new Contato
        {
            Id = Guid.NewGuid(),
            Nome = command.Nome,
            Telefone = command.Telefone,
            DDD = command.DDD,
            Email = command.Email,
        };

        await _contatoRepository.AddContatoAsync(contato);

        return contato.Id;
    }
}

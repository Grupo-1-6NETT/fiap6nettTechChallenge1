using CadastroApi.Domain.IRepository;
using MediatR;

namespace CadastroApi.Application;

public class RemoverContatoCommandHandler : IRequestHandler<RemoverContatoCommand, Guid>
{
    private readonly IContatoRepository _contactRepository;

    public RemoverContatoCommandHandler(IContatoRepository contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task<Guid> Handle(RemoverContatoCommand request, CancellationToken cancellationToken)
    {
        var contato = await _contactRepository.GetByIdAsync(request.ContatoId);

        if (contato == null)
        {
            throw new KeyNotFoundException();
        }
        await _contactRepository.Delete(request.ContatoId);
        return request.ContatoId;
    }
}

using MediatR;

namespace CadastroApi.Application;

public class RemoverUsuarioCommand : IRequest<Guid>
{
    public Guid UsuarioId { get; }
    public RemoverUsuarioCommand(Guid id)
    {
        UsuarioId = id;
    }        
}

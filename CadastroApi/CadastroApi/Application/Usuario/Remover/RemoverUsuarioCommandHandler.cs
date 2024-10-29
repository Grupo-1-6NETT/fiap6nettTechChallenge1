using CadastroApi.Repository;
using MediatR;

namespace CadastroApi.Application;

public class RemoverUsuarioCommandHandler : IRequestHandler<RemoverUsuarioCommand, Guid>
{
    private readonly IUsuarioRepository _usuarioRepository;

    public RemoverUsuarioCommandHandler(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<Guid> Handle(RemoverUsuarioCommand command, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(command.UsuarioId);
        if(usuario is null)
            throw new KeyNotFoundException();

        await _usuarioRepository.Delete(command.UsuarioId);

        return usuario.Id;
    }
}

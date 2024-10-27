namespace CadastroApi.Application;

public class RemoverUsuarioCommand
{
    public Guid UsuarioId { get; }
    public RemoverUsuarioCommand(Guid id)
    {
        UsuarioId = id;
    }        
}

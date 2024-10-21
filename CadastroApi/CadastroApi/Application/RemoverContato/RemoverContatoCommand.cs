using MediatR;

namespace CadastroApi.Application.RemoverContato
{
    public class RemoverContatoCommand : IRequest<Guid>
    {
        public Guid ContatoId { get; }
        public RemoverContatoCommand(Guid id)
        {
            ContatoId = id;
        }
    }

}

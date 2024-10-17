using FluentValidation;
using MediatR;

namespace CadastroApi.Application
{
    public class AtualizarContatoCommand : AdicionarContatoCommand
    {
        public Guid ID { get; set; }
    }   
}

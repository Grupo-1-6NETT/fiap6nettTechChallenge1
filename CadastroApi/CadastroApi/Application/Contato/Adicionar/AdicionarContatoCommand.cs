using FluentValidation;
using MediatR;

namespace CadastroApi.Application;

public class AdicionarContatoCommand : IRequest<Guid>
{
    public string Nome { get; set; } = String.Empty;
    public string Telefone { get; set; } = String.Empty;
    public string DDD { get; set; } = String.Empty;
    public string Email { get; set; } = String.Empty;

    public void Validate()
    {
        var validator = new ModelContatoValidator();
        var validationResult = validator.Validate(this);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
    }
}   

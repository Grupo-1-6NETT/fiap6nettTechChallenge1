using CadastroApi.Domain.Enums;
using FluentValidation;
using MediatR;

namespace CadastroApi.Application;

public class AdicionarUsuarioCommand : IRequest<Guid>
{
    public string Nome { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public TipoUsuarioPermissao? Permissao { get; set; }

    public void Validate()
    {
        var validator = new ModelUsuarioValidator();
        var validationResult = validator.Validate(this);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
    }
}

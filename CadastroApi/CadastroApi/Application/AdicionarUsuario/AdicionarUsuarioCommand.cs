using CadastroApi.Application.Validators;
using CadastroApi.Enums;
using FluentValidation;

namespace CadastroApi.Application.AdicionarUsuario
{
    public class AdicionarUsuarioCommand
    {
        public string Nome { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public UsuarioPermissao? Permissao { get; set; }

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
}

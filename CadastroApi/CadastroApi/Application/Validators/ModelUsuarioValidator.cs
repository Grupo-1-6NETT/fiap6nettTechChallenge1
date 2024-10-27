using FluentValidation;

namespace CadastroApi.Application.Validators
{
    public class ModelUsuarioValidator: AbstractValidator<AdicionarUsuarioCommand>
    {
        public ModelUsuarioValidator()
        {
            RuleFor(u => u.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório.")
                .MaximumLength(100).WithMessage("O nome pode ter no máximo 100 caracteres.");

            RuleFor(u => u.Senha)
                .NotEmpty().WithMessage("Senha é obrigatório.")
                .MinimumLength(8).WithMessage("Senha deve ter no mínimo 8 caracteres.");

            RuleFor(u => u.Permissao)
                .NotNull().WithMessage("Permissão deve ser informada.");

        }
    }
}

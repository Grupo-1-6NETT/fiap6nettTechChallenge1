using FluentValidation;

namespace CadastroApi.Application
{

    public class ModelContatoValidator : AbstractValidator<AdicionarContatoCommand>
    {
        public ModelContatoValidator()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MaximumLength(100).WithMessage("O nome pode ter no máximo 100 caracteres");

            RuleFor(c => c.Telefone)
                .NotEmpty().WithMessage("Telefone é obrigatório")
                .Matches(@"^\d{8,9}$").WithMessage("Telefone deve conter entre 8 e 9 dígitos");

            RuleFor(c => c.DDD)
                .NotEmpty().WithMessage("DDD é obrigatório")
                .Length(2).WithMessage("DDD deve ter 2 dígitos");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .EmailAddress().WithMessage("Formato de email inválido");
        }
    }
}

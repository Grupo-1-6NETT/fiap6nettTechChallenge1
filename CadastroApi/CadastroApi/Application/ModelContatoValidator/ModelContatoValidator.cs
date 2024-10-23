using FluentValidation;

namespace CadastroApi.Application
{

    public class ModelContatoValidator : AbstractValidator<AdicionarContatoCommand>
    {
        public ModelContatoValidator()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .MaximumLength(100).WithMessage("O nome pode ter no máximo 100 caracteres")
                .When(c => !(c is AtualizarContatoCommand) || !string.IsNullOrWhiteSpace(c.Nome));

            RuleFor(c => c.Telefone)
                .NotEmpty().WithMessage("Telefone é obrigatório")
                .Matches(@"^\d{8,9}$").WithMessage("Telefone deve conter entre 8 e 9 dígitos")
                .When(c => !(c is AtualizarContatoCommand) || !string.IsNullOrWhiteSpace(c.Telefone));

            RuleFor(c => c.DDD)
                .NotEmpty().WithMessage("DDD é obrigatório")
                .Length(2).WithMessage("DDD deve ter 2 dígitos")
                .When(c => !(c is AtualizarContatoCommand) || !string.IsNullOrWhiteSpace(c.DDD));

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .EmailAddress().WithMessage("Formato de email inválido")
                .When(c => !(c is AtualizarContatoCommand) || !string.IsNullOrWhiteSpace(c.Email)); ;
        }
    }
}

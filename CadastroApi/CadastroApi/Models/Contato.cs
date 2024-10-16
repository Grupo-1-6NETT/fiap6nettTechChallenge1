using System.ComponentModel.DataAnnotations;

namespace CadastroApi.Models;

public class Contato
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Nome com caracteres inválidos")]
    [MaxLength(100, ErrorMessage = "Nome pode ter no máximo 100 caracteres")]
    [MinLength(2, ErrorMessage = "Nome deve ter no mínimo 2 caracteres")]
    public string Nome { get; set; } = String.Empty;

    [Required(ErrorMessage = "Telefone é obrigatório")]
    [RegularExpression(@"^\d{8,9}$", ErrorMessage = "Telefone com formato inválido")]
    public string Telefone { get; set; } = String.Empty;

    [Required(ErrorMessage = "DDD é obrigatório")]
    [RegularExpression(@"^\d{2}$", ErrorMessage = "DDD com formato inválido")]
    [MaxLength(3, ErrorMessage = "DDD pode ter no máximo 3 caracteres")]
    public string DDD { get; set; } = String.Empty;

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Formato de email inválido")]
    public string Email { get; set; } = String.Empty;
}

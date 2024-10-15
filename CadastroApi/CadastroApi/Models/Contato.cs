using System.ComponentModel.DataAnnotations;

namespace CadastroApi.Models;

public class Contato
{
    [Key]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Nome é obrigatório")]
    [MaxLength(100)]
    public string Nome { get; set; }

    [Required(ErrorMessage = "Telefone é obrigatório")]
    [RegularExpression(@"^\d{8,9}$", ErrorMessage = "Telefone com formato inválido")]
    public string Telefone { get; set; }

    [Required(ErrorMessage = "DDD é obrigatório")]
    [StringLength(2, ErrorMessage = "DDD com formato inválido")]
    public string DDD { get; set; }

    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Formato de email inválido")]
    public string Email { get; set; }
}

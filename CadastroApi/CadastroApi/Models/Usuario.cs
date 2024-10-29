using CadastroApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace CadastroApi.Models
{
    public class Usuario
    {
        [Key]
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;        
        public TipoUsuarioPermissao Permissao { get;set; }
    }
}

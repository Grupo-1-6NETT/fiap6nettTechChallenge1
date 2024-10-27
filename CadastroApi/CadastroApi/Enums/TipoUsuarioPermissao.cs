namespace CadastroApi.Enums
{
    public enum TipoUsuarioPermissao
    {
        Admin,
        ReadOnly
    }

    public static class UsuarioPermissao
    {
        public const string All = "Admin,ReadOnly";
        public const string Admin = "Admin";
        public const string ReadOnly = "ReadOnly";
    }
}

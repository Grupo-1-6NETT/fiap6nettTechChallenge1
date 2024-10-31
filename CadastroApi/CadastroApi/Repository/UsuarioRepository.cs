using CadastroApi.Data;
using CadastroApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CadastroApi.Repository;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    private readonly PasswordHasher<object> _passwordHasher = new();
    public UsuarioRepository(AppDbContext context) : base(context)
    {
    }

    public async Task AddUserAsync(Usuario usuario, string senha)
    {
        //Criptografa senha
        var senhaHash = _passwordHasher.HashPassword(null, senha);
        usuario.Senha = senhaHash;

        await _context.Usuarios.AddAsync(usuario);
        await _context.SaveChangesAsync();
    }

    public async Task<Usuario?> GetUserAsync(string nome, string senha)
    {
        var usuario = await _context.Usuarios
            .AsNoTracking()
            .Where(u => u.Nome == nome)
            .FirstOrDefaultAsync();

        if (usuario is null)
            return null;

        var result = _passwordHasher.VerifyHashedPassword(null, usuario.Senha, senha);

        if(result != PasswordVerificationResult.Success)
            return null;

        return usuario;


    }
}

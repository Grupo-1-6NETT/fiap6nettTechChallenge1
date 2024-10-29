using CadastroApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Contato> Contatos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
}

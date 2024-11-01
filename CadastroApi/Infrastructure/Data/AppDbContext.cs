using CadastroApi.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace CadastroApi.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Contato> Contatos { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
}

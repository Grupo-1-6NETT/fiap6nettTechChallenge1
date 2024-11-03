using CadastroApi.Infrastructure.Data;
using CadastroApi.Infrastructure.Repository;
using CadastroApi.Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CadastroApi.Domain.IToken;
using CadastroApi.Infrastructure.Services;
using System.Text;

namespace CadastroApi.Infrastructure
{
    public static class ServiceExtensions
    {
        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Verifica se a string de conexão está presente
            var connectionString = configuration.GetConnectionString("SQLiteConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("Connection string 'SQLiteConnection' is missing.");
            }

            // Adiciona o contexto do banco de dados
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString));

            services.AddScoped<IContatoRepository, ContatoRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddSingleton<ITokenService, TokenService>();
        }
    }
}

using CadastroApi.Domain.Models;
using CadastroApi.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTest.Integration
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        private const string SECRET = "45262B37D9B63986B437DEBD5C8EA45262B37D9B63986B437DEBD5C8EA";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Test");

            builder.ConfigureAppConfiguration((context, configBuilder) =>
            {
                var testConfig = new Dictionary<string, string?>
                {
                    { "Secret", SECRET },
                };

                configBuilder.AddInMemoryCollection(testConfig);

            });

            builder.ConfigureServices(services =>
            {
                // Remove o DbContext
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Adiciona um DbContext em memória
                services.AddEntityFrameworkSqlite();

                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseSqlite(connection);
                });

                var sp = services.BuildServiceProvider();

                SeedTestDB(sp);
            });


        }

        private static void SeedTestDB(ServiceProvider sp)
        {
            using (var scope = sp.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                //db.Database.OpenConnection();
                db.Database.EnsureCreated();                

                //Adiciona usuário padrão
                var passwordHash = new PasswordHasher<object>();

                db.Usuarios.AddRange(new List<Usuario>
                {
                    new() {Nome = "TestAdmin", Permissao = CadastroApi.Domain.Enums.TipoUsuarioPermissao.Admin, Senha = passwordHash.HashPassword(null!, "123") },
                    new() {Nome = "TestUser", Permissao = CadastroApi.Domain.Enums.TipoUsuarioPermissao.ReadOnly, Senha = passwordHash.HashPassword(null!, "123")},

                });

                //Adiciona contatos
                db.Contatos.AddRange(new List<Contato>
                {
                    new() { Nome = "Contato 1", Email = "contato1@example.com", Telefone = "111111111", DDD = "11" },
                    new() { Nome = "Contato 2", Email = "contato2@example.com", Telefone = "222222222", DDD = "22" }
                });

                db.SaveChanges();
            }
        }
    }
}

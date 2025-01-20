using CadastroApi.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
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

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseSqlite(connection);
                });

                var sp = services.BuildServiceProvider();
            });
        }
    }
}

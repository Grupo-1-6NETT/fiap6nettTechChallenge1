using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CadastroApi.Infrastructure.Data;

namespace CadastroApi.IntegrationTests.BancoDeDados
{
    public class MigrationTests
    {
        [Fact]
        public void Test_Migrations_Applied_Successfully()
        {
            // Configura o DbContext para usar SQLite em memória
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlite()  // Usando SQLite em memória
                .AddDbContext<AppDbContext>(options =>
                    options.UseSqlite("DataSource=:memory:"))  // Banco SQLite em memória
                .BuildServiceProvider();

            // Cria um escopo para executar o teste
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                // Abre uma conexão e aplica as migrações
                dbContext.Database.OpenConnection();
                dbContext.Database.Migrate();

                // Verifica se as tabelas foram criadas corretamente
                var contatosTableExists = dbContext.Database.ExecuteSqlRaw("SELECT name FROM sqlite_master WHERE type='table' AND name='Contatos'") != null;
                var usuariosTableExists = dbContext.Database.ExecuteSqlRaw("SELECT name FROM sqlite_master WHERE type='table' AND name='Usuarios'") != null;

                // Verifica se as tabelas existem
                Assert.True(contatosTableExists, "A tabela 'Contatos' não foi criada.");
                Assert.True(usuariosTableExists, "A tabela 'Usuarios' não foi criada.");
            }
        }
    }
}

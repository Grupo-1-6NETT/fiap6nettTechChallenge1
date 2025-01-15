using Microsoft.Data.Sqlite;
using System.IO;
using Xunit;
using SQLitePCL; 

namespace UnitTest.Integration.BancoDeDados
{
    public class DatabaseIntegrationTests
    {
        private readonly string _connectionString;

        public DatabaseIntegrationTests()
        {
            // Inicializa o SQLitePCL
            Batteries.Init();

            // Diretório atual (bin folder)
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Navega para o local correto do banco de dados
            var projectRoot = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\..\..\"));
            _connectionString = Path.Combine(projectRoot, "Infrastructure", "Data", "Cadastro.db");
        }

        [Fact]
        public void TestDatabaseConnection()
        {
            if (!File.Exists(_connectionString))
            {
                throw new FileNotFoundException("O banco de dados SQLite não foi encontrado.", _connectionString);
            }

            using var connection = new SqliteConnection($"Data Source={_connectionString}");
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT 1";
            var result = command.ExecuteScalar();

            Assert.Equal(1, Convert.ToInt32(result));
        }
    }
}

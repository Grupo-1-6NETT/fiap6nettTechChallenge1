using CadastroApi.Domain.Models;
using CadastroApi.Infrastructure.Data;
using FluentAssertions;
using IntegrationTest.Integration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;

namespace IntegrationTest.TokenTests
{
    public class TokenIntegrationTests : IntegrationTestsBase
    {
        public TokenIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
        {
            SeedDB();
        }

        [Theory]
        [InlineData("TestAdmin", "123", HttpStatusCode.OK)]
        [InlineData("BadUser", "NoPw", HttpStatusCode.Unauthorized)]
        public async Task Get_Token_DeveRetornarResultadoEsperado(string usuario, string senha, HttpStatusCode resultadoEsperado)
        {
            // Act
            var response = await _client.GetAsync($"/Token?usuario={usuario}&senha={senha}");

            // Assert
            response.StatusCode.Should().Be(resultadoEsperado);
        }

        private void SeedDB()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                db.Database.OpenConnection();
                db.Database.EnsureCreated();

                //Adiciona usuário padrão
                var passwordHash = new PasswordHasher<object>();

                db.Usuarios.AddRange(new List<Usuario>
                {
                    new() {Nome = "TestAdmin", Permissao = CadastroApi.Domain.Enums.TipoUsuarioPermissao.Admin, Senha = passwordHash.HashPassword(null!, "123") }

                });

                db.SaveChanges();
            }
        }
    }
}

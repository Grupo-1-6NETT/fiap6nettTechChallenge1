using CadastroApi.Domain.Enums;
using CadastroApi.Domain.Models;
using CadastroApi.Infrastructure.Data;
using FluentAssertions;
using IntegrationTest.Integration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text;
using System.Text.Json;

namespace IntegrationTest.UsuarioTests
{
    public class UsuarioIntegrationTests : IntegrationTestsBase
    {
        public UsuarioIntegrationTests(CustomWebApplicationFactory factory) : base(factory)
        {
            SeedDB();
        }

        [Fact]
        public async Task Post_AdicionarUsuario_NaoAutenticado_DeveCriarNovoUsuario()
        {
            // Arrange
            var novoUsuario = new
            {
                Nome = "NovoUsuario",
                Senha = "Password123",
                Permissao = 0,                
            };
            var content = new StringContent(JsonSerializer.Serialize(novoUsuario), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/Usuario", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var usuario = await context.Usuarios.FirstOrDefaultAsync(u => u.Nome == novoUsuario.Nome);

            usuario.Should().NotBeNull();
            usuario!.Nome.Should().Be(novoUsuario.Nome);
        }

        [Fact]
        public async Task Delete_RemoverUsuario_NaoAutenticado_DeveRetornarUnauthorized()
        {
            // Arrange
            var usuarioId = (await GetFirstUsuario()).Id;            

            // Act
            var response = await _client.DeleteAsync($"/Usuario/{usuarioId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Delete_RemoverUsuario_AutenticadoReadOnly_DeveRetornarForbidden()
        {
            // Arrange
            var usuarioId = (await GetFirstUsuario()).Id;

            AddAuthAsync(TipoUsuarioPermissao.ReadOnly);

            // Act
            var response = await _client.DeleteAsync($"/Usuario/{usuarioId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_RemoverUsuario_AutenticadoAdmin_DeveRemoverUsuario()
        {
            // Arrange
            var usuarioId = (await GetFirstUsuario()).Id;

            AddAuthAsync(TipoUsuarioPermissao.Admin);

            // Act
            var response = await _client.DeleteAsync($"/Usuario/{usuarioId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var usuario = await context.Usuarios.FirstOrDefaultAsync(u => u.Id == usuarioId);

            usuario.Should().BeNull();            
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
                    new() {Nome = "TestAdmin", Permissao = CadastroApi.Domain.Enums.TipoUsuarioPermissao.Admin, Senha = passwordHash.HashPassword(null!, "123") },
                    new() {Nome = "TestAdmin2", Permissao = CadastroApi.Domain.Enums.TipoUsuarioPermissao.Admin, Senha = passwordHash.HashPassword(null!, "123") },
                    new() {Nome = "TestUser", Permissao = CadastroApi.Domain.Enums.TipoUsuarioPermissao.ReadOnly, Senha = passwordHash.HashPassword(null!, "123")},
                    new() {Nome = "TestUser2", Permissao = CadastroApi.Domain.Enums.TipoUsuarioPermissao.ReadOnly, Senha = passwordHash.HashPassword(null!, "123")},

                });

                db.SaveChanges();
            }
        }

        private async Task<Usuario> GetFirstUsuario()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            return await db.Usuarios.FirstAsync();
        }
    }
}

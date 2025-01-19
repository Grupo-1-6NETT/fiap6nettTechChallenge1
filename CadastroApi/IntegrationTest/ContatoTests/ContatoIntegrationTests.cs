using CadastroApi.Domain.Models;
using CadastroApi.Infrastructure.Data;
using FluentAssertions;
using IntegrationTest.Integration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text;
using System.Text.Json;
using CadastroApi.Domain.Enums;

namespace IntegrationTest.ContatoTests
{
    public class ContatoIntegrationTests : IntegrationTestsBase
    {
        public ContatoIntegrationTests(CustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task Post_Contatos_UsuarioAdmin_DeveCriarNovoContato()
        {
            // Arrange
            var novoContato = new
            {
                Nome = "Novo Contato",
                Email = "novo@contato.com",
                Telefone = "333333333",
                DDD = "33"
            };


            var content = new StringContent(JsonSerializer.Serialize(novoContato), Encoding.UTF8, "application/json");

            AddAuthAsync(TipoUsuarioPermissao.Admin);

            // Act
            var response = await _client.PostAsync("/Contatos", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var contato = await context.Contatos.FirstOrDefaultAsync(c => c.Email == novoContato.Email);

            contato.Should().NotBeNull();
            contato!.Nome.Should().Be(novoContato.Nome);
        }

        [Fact]
        public async Task Post_Contatos_UsuarioReadOnly_DeveRetornarForbidden()
        {
            // Arrange
            var novoContato = new
            {
                Nome = "Novo Contato",
                Email = "novo@contato.com",
                Telefone = "333333333",
                DDD = "33"
            };


            var content = new StringContent(JsonSerializer.Serialize(novoContato), Encoding.UTF8, "application/json");

            AddAuthAsync(CadastroApi.Domain.Enums.TipoUsuarioPermissao.ReadOnly);

            // Act
            var response = await _client.PostAsync("/Contatos", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Theory]
        [InlineData(TipoUsuarioPermissao.Admin)]
        [InlineData(TipoUsuarioPermissao.ReadOnly)]
        public async Task Get_Contatos_DeveRetornarListaDeContatos(TipoUsuarioPermissao role)
        {
            AddAuthAsync(role);
            // Act
            var response = await _client.GetAsync("/Contatos");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var contatos = JsonSerializer.Deserialize<List<dynamic>>(jsonResponse);

            contatos.Should().HaveCountGreaterThanOrEqualTo(2);
        }

        [Fact]
        public async Task Patch_Contatos_UsuarioAdmin_DeveAtualizarContato()
        {
            // Arrange
            var contatoId = (await GetFirstContato()).Id;

            var contatoAtualizado = new
            {
                Id = contatoId,
                Nome = "Contato Atualizado"
            };

            var content = new StringContent(JsonSerializer.Serialize(contatoAtualizado), Encoding.UTF8, "application/json");

            AddAuthAsync(TipoUsuarioPermissao.Admin);

            // Act
            var response = await _client.PatchAsync($"/Contatos", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Verifica no banco
            using var scope = _factory.Services.CreateScope(); ;
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var contato = await db.Contatos.FindAsync(contatoId);

            contato.Should().NotBeNull();
            contato!.Nome.Should().Be("Contato Atualizado");
        }

        [Fact]
        public async Task Patch_Contatos_UsuarioReadOnly_DeveRetornarForbidden()
        {
            // Arrange
            var contatoId = (await GetFirstContato()).Id;

            var contatoAtualizado = new
            {
                Id = contatoId,
                Nome = "Contato Atualizado"
            };

            var content = new StringContent(JsonSerializer.Serialize(contatoAtualizado), Encoding.UTF8, "application/json");

            AddAuthAsync(TipoUsuarioPermissao.ReadOnly);

            // Act
            var response = await _client.PatchAsync($"/Contatos", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_Contatos_UsuarioAdmin_DeveExcluirContato()
        {
            // Act
            var contatoId = (await GetFirstContato()).Id;
            AddAuthAsync(TipoUsuarioPermissao.Admin);
            var response = await _client.DeleteAsync($"/Contatos/{contatoId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // Verifica no banco
            using var scope = _factory.Services.CreateScope(); ;
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var contato = await db.Contatos.FindAsync(contatoId);

            contato.Should().BeNull();
        }

        [Fact]
        public async Task Delete_Contatos_UsuarioReadOnly_DeveRetornarForbidden()
        {
            // Act
            var contatoId = (await GetFirstContato()).Id;
            AddAuthAsync(TipoUsuarioPermissao.ReadOnly);
            var response = await _client.DeleteAsync($"/Contatos/{contatoId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        private async Task<Contato> GetFirstContato()
        {
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            return await db.Contatos.FirstAsync();
        }
    }
}

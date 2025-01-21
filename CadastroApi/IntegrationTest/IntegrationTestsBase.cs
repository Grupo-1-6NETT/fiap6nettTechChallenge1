using CadastroApi.Domain.Enums;
using CadastroApi.Domain.Models;
using CadastroApi.Infrastructure.Data;
using IntegrationTest.Integration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace IntegrationTest
{
    public class IntegrationTestsBase(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
    {
        protected readonly HttpClient _client = factory.CreateClient();
        protected readonly CustomWebApplicationFactory _factory = factory;

        private const string SECRET = "45262B37D9B63986B437DEBD5C8EA45262B37D9B63986B437DEBD5C8EA";

        protected async void AddAuthAsync(TipoUsuarioPermissao role)
        {
            var user = await GetUsuarioByRole(role);
            var token = GenerateToken(user);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task<Usuario> GetUsuarioByRole(TipoUsuarioPermissao role)
        {
            using var scope = _factory.Services.CreateScope(); ;
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            return await db.Usuarios.FirstAsync(u => u.Permissao == role);
        }

        private string GenerateToken(Usuario usuario)
        {
            var handler = new JwtSecurityTokenHandler();

            if (string.IsNullOrEmpty(SECRET))
                return string.Empty;

            var key = Encoding.ASCII.GetBytes(SECRET);

            var props = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(
                [
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Role, usuario.Permissao.ToString()),
                ]),

                Expires = DateTime.UtcNow.AddHours(8),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = handler.CreateToken(props);
            return handler.WriteToken(token);
        }
    }
}

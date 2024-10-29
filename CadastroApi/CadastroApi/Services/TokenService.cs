using CadastroApi.Models;
using CadastroApi.Repository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CadastroApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;        

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetToken(Usuario usuario)
        {
            var handler = new JwtSecurityTokenHandler();
            var secret = _configuration.GetValue<string>("Secret");            

            if(string.IsNullOrEmpty(secret))
                return string.Empty;

            var key = Encoding.ASCII.GetBytes(secret);
            
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

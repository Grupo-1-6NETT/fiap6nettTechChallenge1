using CadastroApi.Repository;
using CadastroApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace CadastroApi.Controllers;

[Route("[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly IUsuarioRepository _usuarioRepository;

    public TokenController(ITokenService tokenService, IUsuarioRepository usuarioRepository)
    {
        _tokenService = tokenService;
        _usuarioRepository = usuarioRepository;
    }

    /// <summary>
    /// Gera um token de autenticação para o usuário e senha informados
    /// </summary>
    /// <param name="usuario">Nome do Usuário cadastrado</param>
    /// <param name="senha">Senha do Usuário</param>
    /// <returns>O token de autenticação da API</returns>
    /// <response code="200">Token gerado com sucesso</response>
    /// <response code="401">Usuário não autenticado</response>    
    /// <response code="500">Erro inesperado</response>
    [HttpGet]
    public async Task<IActionResult> GetToken(string usuario, string senha)
    {
        var user = await _usuarioRepository.GetUserAsync(usuario, senha);

        if (user is not null)
        {
            var token = _tokenService.GetToken(user);

            if (!string.IsNullOrWhiteSpace(token))
            {
                return Ok(token);
            }
        }

        return Unauthorized();
    }
}

using CadastroApi.Application;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CadastroApi.Controllers;

[Route("[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IMediator _mediator;

    public TokenController(IMediator mediator)
    {
        _mediator = mediator;
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
        // Envia a requisição pelo MediatR
        var token = await _mediator.Send(new ListarTokenQuery(usuario, senha));

        if (!string.IsNullOrWhiteSpace(token))
        {
            return Ok(token);
        }

        return Unauthorized();
    }
}

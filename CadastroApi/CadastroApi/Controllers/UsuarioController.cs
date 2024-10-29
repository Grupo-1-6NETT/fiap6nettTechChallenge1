using CadastroApi.Application;
using CadastroApi.Application.Extensions;
using CadastroApi.Enums;
using CadastroApi.Repository;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CadastroApi.Controllers;

[Route("[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IMediator _mediator;

    public UsuarioController(IUsuarioRepository usuarioRepository, IMediator mediator)
    {
        _usuarioRepository = usuarioRepository;
        _mediator = mediator;
    }

    /// <summary>
    /// Adiciona um Usuário na base de dados 
    /// </summary>
    /// <remarks>
    /// Exemplo:
    /// 
    ///  {
    ///     "nome": "batman",
    ///     "senha": "P4ssw0rd",
    ///     "permissao": "admin",    
    /// }
    /// </remarks>
    /// <param name="command">Comando com os dados do Usuário</param>
    /// <returns>O Id do Usuário adicionado</returns>
    /// <response code="201">Usuário adicionado na base de dados</response>
    /// <response code="400">Falha ao processar a requisição</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="403">Usuário não autorizado</response>
    /// <response code="500">Erro inesperado</response>
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Authorize(Roles =UsuarioPermissao.Admin)]
    [HttpPost]
    public async Task<IActionResult> AdicionarUsuario([FromBody] AdicionarUsuarioCommand command)
    {
        try
        {
            var usuarioID = await _mediator.Send(command);
            return Created("", usuarioID);
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.ToResultMessage());
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Ocorreu um erro inesperado.", Details = ex.Message });
        }
    }


    /// <summary>
    /// Remove o Usuário na base de dados com o ID informado
    /// </summary>
    /// <param name="id">O ID do Usuário a ser removido</param>
    /// <returns>Resultado da operação de remoção</returns>
    /// <response code="200">Usuário removido com sucesso</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="403">Usuário não autorizado</response>
    /// <response code="404">Usuário não encontrado</response>
    /// <response code="500">Erro inesperado</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [Authorize(Roles = UsuarioPermissao.Admin)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoverUsuario(Guid id)
    {
        try
        {
            var command = new RemoverUsuarioCommand(id);
            await _mediator.Send(command);
            return Ok($"Usuário com id {id} removido com sucesso."); 
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Usuário com id {id} não encontrado.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Ocorreu um erro inesperado.", Details = ex.Message });
        }

    }
}

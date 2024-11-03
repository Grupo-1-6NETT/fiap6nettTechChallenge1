using CadastroApi.Application;
using CadastroApi.Application.Extensions;
using CadastroApi.Domain.Enums;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CadastroApi.Controllers;

[Route("[controller]")]
[ApiController]
public class ContatosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContatosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Lista os Contatos cadastrados, ordenados por nome, que correspondem aos parâmetros informados
    /// </summary>    
    /// <param name="ddd">Filtra os contatos cujo telefone contenham o DDD informado</param>
    /// <param name="pagina">Informe o número de página para ignorar os contatos de páginas anteriores. Se não informado, todos os contatos serão exibidos</param>
    /// <param name="resultadosPorPagina">Número de contatos a serem exibidos para a <paramref name="pagina"/> informada. Se não informado, todos os contatos serão exibidos</param>
    /// <returns>A lista de Contatos correspodentes à pesquisa</returns>
    /// <response code="200">Pesquisa realizada com sucesso</response>
    /// <response code="401">Usuário não autenticado</response>    
    /// <response code="500">Erro inesperado</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(500)]
    [Authorize(Roles = UsuarioPermissao.All)]
    [HttpGet]
    public async Task<IActionResult> ListarContatos(string? ddd = null, int? pagina = null, int? resultadosPorPagina = null)
    {
        try
        {
            var query = new ListarContatoQuery { DDD = ddd, PageIndex = pagina, PageSize = resultadosPorPagina};
            var contatos = await _mediator.Send(query);
            return Ok(contatos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Ocorreu um erro inesperado.", Details = ex.Message });
        }
    }

    /// <summary>
    /// Adiciona um Contato na base de dados 
    /// </summary>
    /// <remarks>
    /// Exemplo:
    /// 
    ///  {
    ///     "nome": "João",
    ///     "telefone": "988994199",
    ///     "ddd": "11",
    ///     "email": "joao@gmail.com"
    /// }
    /// </remarks>
    /// <param name="command">Comando com os dados do Contato</param>
    /// <returns>O Id do Contato adicionado</returns>
    /// <response code="201">Contato adicionado na base de dados</response>
    /// <response code="400">Falha ao processar a requisição</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="403">Usuário não autorizado</response>
    /// <response code="500">Erro inesperado</response>
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(500)]
    [Authorize(Roles = UsuarioPermissao.Admin)]
    [HttpPost]
    public async Task<IActionResult> AdicionarContato([FromBody] AdicionarContatoCommand command)
    {
        try
        {
            var contatoId = await _mediator.Send(command);
            return Created("", contatoId);
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
    /// Atualiza um Contato na base de dados 
    /// </summary>
    /// <remarks>
    /// Exemplo:
    /// 
    ///  {
    ///     "id": "1991dcff-06a9-4b09-9e16-79f76055a21b",
    ///     "nome": "João",
    ///     "telefone": "988994199",
    ///     "ddd": "11",
    ///     "email": "joao@gmail.com"
    /// }
    /// </remarks>
    /// <param name="command">Comando com os dados do Contato</param>
    /// <returns>O Id do Contato atualizado</returns>
    /// <response code="200">Contato atualizado na base de dados</response>
    /// <response code="400">Falha ao processar a requisição</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="403">Usuário não autorizado</response>
    /// <response code="404">Contato não encontrado</response>
    /// <response code="500">Erro inesperado</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [Authorize(Roles = UsuarioPermissao.Admin)]
    [HttpPatch]
    public async Task<IActionResult> AtualizarContato([FromBody] AtualizarContatoCommand command)
    {
        try
        {
            var contatoId = await _mediator.Send(command);
            return Ok($"Contato com Id {contatoId} atualizado com sucesso.");
        }
        catch(KeyNotFoundException)
        {
            return NotFound($"Id: {command.ID} não encontrado na base de dados.");
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
    /// Remove o contato na base de dados com o ID informado
    /// </summary>
    /// <param name="id">O ID do contato a ser removido</param>
    /// <returns>Resultado da operação de remoção</returns>
    /// <response code="200">Contato removido com sucesso</response>
    /// <response code="401">Usuário não autenticado</response>
    /// <response code="403">Usuário não autorizado</response>
    /// <response code="404">Contato não encontrado</response>
    /// <response code="500">Erro inesperado</response>
    [ProducesResponseType(200)]
    [ProducesResponseType(401)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    [Authorize(Roles =UsuarioPermissao.Admin)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoverContato(Guid id)
    {
        try
        {
            var command = new RemoverContatoCommand(id);
            await _mediator.Send(command);
            return Ok($"Contato com id {id} removido com sucesso."); 
        }
        catch (KeyNotFoundException)
        {
            return NotFound($"Contato com id {id} não encontrado.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Error = "Ocorreu um erro inesperado.", Details = ex.Message });
        }

    }
}

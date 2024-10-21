using CadastroApi.Application;
using CadastroApi.Application.RemoverContato;
using CadastroApi.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CadastroApi.Controllers;

[Route("[controller]")]
[ApiController]
public class ContatosController : ControllerBase
{
    private readonly IContatoRepository _contatoRepository;
    private readonly IMediator _mediator;

    public ContatosController(IContatoRepository contatoRepository, IMediator mediator)
    {
        _contatoRepository = contatoRepository;
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
    /// <response code="500">Erro inesperado</response>
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

    [HttpPost]
    public async Task<IActionResult> AdicionarContato([FromBody] AdicionarContatoCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var contatoId = await _mediator.Send(command);
            return Ok(new { Id = contatoId });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
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
    [HttpPatch]
    public async Task<IActionResult> AtualizarContato([FromBody] AtualizarContatoCommand command)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var contatoId = await _mediator.Send(command);
            return Ok(new { Id = contatoId });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    /// <summary>
    /// Remove o contato na base de dados com o ID informado
    /// </summary>
    /// <param name="id">O ID do contato a ser removido</param>
    /// <returns>Resultado da operação de remoção</returns>
    /// <response code="200">Contato removido com sucesso</response>
    /// <response code="404">Contato não encontrado</response>
    /// <response code="500">Erro inesperado</response>
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

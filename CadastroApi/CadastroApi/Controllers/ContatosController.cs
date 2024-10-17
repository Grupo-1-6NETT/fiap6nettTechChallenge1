using CadastroApi.Application;
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

    [HttpGet]
    public async Task<IActionResult> ListarContatos()
    {
        try
        {
            var query = new ListarContatoQuery();
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
}

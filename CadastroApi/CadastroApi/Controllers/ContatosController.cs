using CadastroApi.Application;
using CadastroApi.Models;
using CadastroApi.Repository;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
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
}

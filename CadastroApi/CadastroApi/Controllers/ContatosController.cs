using CadastroApi.Models;
using CadastroApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CadastroApi.Controllers;

[Route("[controller]")]
[ApiController]
public class ContatosController : ControllerBase
{
    private readonly IContatoRepository _contatoRepository;

    public ContatosController(IContatoRepository contatoRepository)
    {
        _contatoRepository = contatoRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Contato>>> GetAllAsync()
    {
        var contatos = await _contatoRepository.GetAllAsync();

        return Ok(contatos);
        
    }

}

using CadastroApi.Application;
using CadastroApi.Controllers;
using CadastroApi.Domain.IRepository;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest;

public class AdicionarContatoTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IContatoRepository> _contatoRepositoryMock;
    private readonly ContatosController _controller;

    public AdicionarContatoTests()
    {
        _contatoRepositoryMock = new Mock<IContatoRepository>();
        _mediatorMock = new Mock<IMediator>();
        //_controller = new ContatosController(_contatoRepositoryMock.Object, _mediatorMock.Object);
        _controller = new ContatosController(_mediatorMock.Object);
        
    }

    [Fact]
    public async Task AdicionarContato_InformadosDadosValidos_DeveRetornarOk()
    {
        var guid = new Guid();

        var command = new AdicionarContatoCommand
        {
            Nome = "Felipe Dantas",
            Telefone = "999999999",
            DDD = "11",
            Email = "felipe@example.com"
        };
        _mediatorMock.Setup(m => m.Send(It.IsAny<AdicionarContatoCommand>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(guid);

        var result = await _controller.AdicionarContato(command);
        var createdResult = Assert.IsType<CreatedResult>(result);

        Assert.Equal(guid, createdResult.Value);
    }

    [Fact]
    public async Task AdicionarContato_DadosInvalidos_DeveRetornarBadRequest()
    {
        var command = new AdicionarContatoCommand
        {
            Nome = "Felipe Dantas",
            Telefone = "999999999", 
            DDD = "1111",            
            Email = "felipe@example.com"
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<AdicionarContatoCommand>(), It.IsAny<CancellationToken>()))
                     .ThrowsAsync(new ValidationException(""));

        var result = await _controller.AdicionarContato(command);

        Assert.IsType<BadRequestObjectResult>(result);
    }
}
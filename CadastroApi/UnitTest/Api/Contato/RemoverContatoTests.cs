using CadastroApi.Application;
using CadastroApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.Api.ContatoTests;

public class RemoverContatoTests
{
    private readonly Mock<IMediator> _mediatorMock;    
    private readonly ContatosController _controller;

    public RemoverContatoTests()
    {
        _mediatorMock = new Mock<IMediator>();     
        _controller = new ContatosController(_mediatorMock.Object);
    }

    [Fact]
    public async Task RemoverContato_InformadoContatoExistente_DeverRetornarOk()
    {
        var contatoId = Guid.NewGuid();
        var command = new RemoverContatoCommand(contatoId);

        _mediatorMock.Setup(m => m.Send(It.IsAny<RemoverContatoCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(contatoId);

        var result = await _controller.RemoverContato(contatoId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal($"Contato com id {contatoId} removido com sucesso.", okResult.Value);
    }

    [Fact]
    public async Task RemoverContato_InformadoContatoInexistente_DeveRetornarNotFound()
    {
        var contatoId = Guid.NewGuid();
        var command = new RemoverContatoCommand(contatoId);

        _mediatorMock.Setup(m => m.Send(It.IsAny<RemoverContatoCommand>(), It.IsAny<CancellationToken>()))
                     .ThrowsAsync(new KeyNotFoundException());

        var result = await _controller.RemoverContato(contatoId);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal($"Contato com id {contatoId} não encontrado.", notFoundResult.Value);

    }
}

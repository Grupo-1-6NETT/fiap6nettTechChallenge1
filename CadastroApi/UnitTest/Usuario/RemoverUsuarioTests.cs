using CadastroApi.Application;
using CadastroApi.Controllers;
using CadastroApi.Repository;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest;

public class RemoverUsuarioTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly UsuarioController _controller;

    public RemoverUsuarioTests()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _mediatorMock = new Mock<IMediator>();
        _controller = new UsuarioController(_usuarioRepositoryMock.Object, _mediatorMock.Object);
    }

    [Fact]
    public async Task RemoverUsuario_InformadoUsuarioExistente_DeverRetornarOk()
    {        
        var usuarioId = Guid.NewGuid();
        var command = new RemoverUsuarioCommand(usuarioId);

        _mediatorMock.Setup(m => m.Send(It.IsAny<RemoverUsuarioCommand>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync(usuarioId);
       
        var result = await _controller.RemoverUsuario(usuarioId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal($"Usuário com id {usuarioId} removido com sucesso.", okResult.Value);
    }

    [Fact]
    public async Task RemoverUsuario_InformadoUsuarioInexistente_DeveRetornarNotFound()
    {
        var usuarioId = Guid.NewGuid();
        var command = new RemoverUsuarioCommand(usuarioId);

        _mediatorMock.Setup(m => m.Send(It.IsAny<RemoverUsuarioCommand>(), It.IsAny<CancellationToken>()))
                     .ThrowsAsync(new KeyNotFoundException());

        var result = await _controller.RemoverUsuario(usuarioId);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal($"Usuário com id {usuarioId} não encontrado.", notFoundResult.Value);

    }
}

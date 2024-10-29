using CadastroApi.Application;
using CadastroApi.Controllers;
using CadastroApi.Repository;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest;

public class AtualizarContatoTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IContatoRepository> _contatoRepositoryMock;
    private readonly ContatosController _controller;

    public AtualizarContatoTests()
    {
        _contatoRepositoryMock = new Mock<IContatoRepository>();
        _mediatorMock = new Mock<IMediator>();
        _controller = new ContatosController(_contatoRepositoryMock.Object,_mediatorMock.Object);            
    }

    [Fact]
    public async Task AtualizarContato_InformadoIdExistente_DeveAtualizarContato()
    {
        var contatoId = Guid.NewGuid();

        var command = new AtualizarContatoCommand
        {
            ID = contatoId,
            Nome = "Batman",
            DDD = "99",
            Telefone = "999999999",
            Email = "batman@gotham.com"
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<AtualizarContatoCommand>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(contatoId);

        var result = await _controller.AtualizarContato(command);

        var okResult = Assert.IsType<OkObjectResult>(result);

        Assert.Equal($"Contato com Id {contatoId} atualizado com sucesso.", okResult.Value);
    }

    [Fact]
    public async Task AtualizarContato_InformadoIdInexistente_DeveRetornarNotFound()
    {
        var contatoId = Guid.NewGuid();

        var command = new AtualizarContatoCommand
        {
            ID = contatoId,
            Nome = "Batman",
            DDD = "99",
            Telefone = "999999999",
            Email = "batman@gotham.com"
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<AtualizarContatoCommand>(), It.IsAny<CancellationToken>()))
                 .ThrowsAsync(new KeyNotFoundException());

        var result = await _controller.AtualizarContato(command);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

        Assert.Equal($"Id: {command.ID} não encontrado na base de dados.", notFoundResult.Value);
    }

    [Fact]
    public async Task AtualizarContato_InformadoDadosInvalidos_DeveRetornarBadRequest()
    {
        var contatoId = Guid.NewGuid();

        var command = new AtualizarContatoCommand
        {
            ID = contatoId,
            Nome = "Batman",
            DDD = "99",
            Telefone = "999999999",
            Email = "notavalidemail"
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<AtualizarContatoCommand>(), It.IsAny<CancellationToken>()))
                 .ThrowsAsync(new ValidationException(""));

        var result = await _controller.AtualizarContato(command);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
    }
}
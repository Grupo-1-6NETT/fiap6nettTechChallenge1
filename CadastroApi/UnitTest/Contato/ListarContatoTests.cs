using CadastroApi.Application;
using CadastroApi.Domain.Models;
using CadastroApi.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using CadastroApi.Domain.IRepository;

namespace UnitTest;

public class ListarContatoTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IContatoRepository> _contatoRepositoryMock;
    private readonly ContatosController _controller;

    public ListarContatoTests()
    {
        _contatoRepositoryMock = new Mock<IContatoRepository>();
        _mediatorMock = new Mock<IMediator>();
        //_controller = new ContatosController(_contatoRepositoryMock.Object, _mediatorMock.Object);
        _controller = new ContatosController(_mediatorMock.Object);
    }

    [Fact]
    public async Task ListarContatos_ParametrosNaoInformados_DeveRetornarTodosContatos()
    {
        var contatos = new List<Contato>
        {
            new() { Nome = "Batman", Telefone = "999999999", DDD = "11", Email = "batman@gotham.com" },
            new() { Nome = "Robin", Telefone = "888888888", DDD = "21", Email = "robin@gotham.com" }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<ListarContatoQuery>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(contatos);

        var result = await _controller.ListarContatos();

        var okResultObject = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(contatos, okResultObject.Value);
        
    }

    [Theory]
    [InlineData("11", null, null)]        
    [InlineData("11", 1, null)]
    [InlineData("11", 1, 1)]
    [InlineData(null, 1, null)]
    [InlineData(null, 1, 1)]
    [InlineData(null, null, 1)]
    [InlineData(null, null, null)]
    public async Task ListarContatos_ParametrosInformados_DeveRetornarListaFiltrada(string? ddd = null, int? pageIndex = null, int? pageSize = null)
    {
        var contatos = new List<Contato>
        {
            new() { Nome = "Batman", Telefone = "999999999", DDD = "11", Email = "batman@gotham.com" },
            new() { Nome = "Robin", Telefone = "888888888", DDD = "11", Email = "robin@gotham.com" }
        };

        _mediatorMock.Setup(m => m.Send(It.IsAny<ListarContatoQuery>(), It.IsAny<CancellationToken>()))
                 .ReturnsAsync(contatos);

        var result = await _controller.ListarContatos(ddd, pageIndex, pageSize);

        var okResultObject = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(contatos, okResultObject.Value);
    }
}

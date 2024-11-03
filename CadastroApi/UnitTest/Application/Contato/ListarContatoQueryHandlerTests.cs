using CadastroApi.Application;
using CadastroApi.Domain.IRepository;
using CadastroApi.Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest.Application.ContatoTests;

public class ListarContatoQueryHandlerTests
{
    private readonly Mock<IContatoRepository> _contatoRepositoryMock;
    private readonly ListarContatoQueryHandler _handler;

    public ListarContatoQueryHandlerTests()
    {
        _contatoRepositoryMock = new Mock<IContatoRepository>();
        _handler = new ListarContatoQueryHandler(_contatoRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ParametrosNaoInformados_DeveRetornarTodosContatos()
    {
        var command = new ListarContatoQuery();

        var result = await _handler.Handle(command, CancellationToken.None);

        _contatoRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<int?>(), It.IsAny<int?>()), Times.Once);
        _contatoRepositoryMock.Verify(x => x.GetByDDDAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()), Times.Never);
    }

    [Theory]
    [InlineData("11", null, null)]
    [InlineData("11", 1, null)]
    [InlineData("11", 1, 1)]    
    public async Task Handle_DDDInformado_DeveRetornarListaFiltrada(string? ddd = null, int? pageIndex = null, int? pageSize = null)
    {
        var command = new ListarContatoQuery { DDD = ddd, PageIndex = pageIndex, PageSize = pageSize};

        var result = await _handler.Handle(command, CancellationToken.None);

        _contatoRepositoryMock.Verify(x => x.GetAllAsync(It.IsAny<int?>(), It.IsAny<int?>()), Times.Never);
        _contatoRepositoryMock.Verify(x => x.GetByDDDAsync(It.IsAny<string>(), It.IsAny<int?>(), It.IsAny<int?>()), Times.Once);
    }
}
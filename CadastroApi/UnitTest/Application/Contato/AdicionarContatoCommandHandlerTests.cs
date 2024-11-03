using CadastroApi.Application;
using CadastroApi.Domain.IRepository;
using CadastroApi.Domain.Models;
using FluentValidation;
using Moq;

namespace UnitTest.Application.ContatoTests;

public class AdicionarContatoCommandHandlerTests
{
    private readonly Mock<IContatoRepository> _contatoRepositoryMock;
    private readonly AdicionarContatoCommandHandler _handler;    

    public AdicionarContatoCommandHandlerTests()
    {
        _contatoRepositoryMock = new Mock<IContatoRepository>();
        _handler = new AdicionarContatoCommandHandler(_contatoRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_InformadosDadosValidos_DeveRetornarOk()
    {
        var command = new AdicionarContatoCommand
        {
            Nome = "Batman",
            Telefone = "999999999",
            DDD = "11",
            Email = "batman@gotham.com"
        };

        var result = await _handler.Handle(command, CancellationToken.None);

        _contatoRepositoryMock.Verify(x => x.AddContatoAsync(It.IsAny<Contato>()), Times.Once);        
    }

    [Fact]
    public async Task Handle_InformadosDadosInvalidos_ValidationException()
    {
        var command = new AdicionarContatoCommand
        {
            Nome = "Batman",
            Telefone = "99",
            DDD = "11",
            Email = "batman@gotham.com"
        };

        await Assert.ThrowsAsync<ValidationException>(async () => await _handler.Handle(command, CancellationToken.None));
    }
}
using CadastroApi.Application;
using CadastroApi.Domain.IRepository;
using CadastroApi.Domain.Models;
using FluentValidation;
using Moq;

namespace UnitTest.Application.ContatoTests;

public class AtualizarContatoCommandHandlerTests
{
    private readonly Mock<IContatoRepository> _contatoRepositoryMock;
    private readonly AtualizarContatoCommandHandler _handler;    

    public AtualizarContatoCommandHandlerTests()
    {
        _contatoRepositoryMock = new Mock<IContatoRepository>();
        _handler = new AtualizarContatoCommandHandler(_contatoRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_InformadoContatoExistente_DeveRetornarOk()
    {
        var id = new Guid();

        var command = new AtualizarContatoCommand
        {
            ID = id,
            Nome = "Batman",
            Telefone = "999999999",
            DDD = "11",
            Email = "batman@gotham.com"
        };

        var contato = new Contato
        {
            Id = id,
            Nome = "Robin",
            Telefone = "999999999",
            DDD = "11",
            Email = "robin@gotham.com"
        };

        _contatoRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(contato);


        var result = await _handler.Handle(command, CancellationToken.None);

        _contatoRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);        
        _contatoRepositoryMock.Verify(x => x.UpdateContatoAsync(It.IsAny<Contato>()), Times.Once);
    }

    [Fact]
    public async Task Handle_InformadoContatoInexistente_KeyNotFoundException()
    {
        var id = new Guid();

        var command = new AtualizarContatoCommand
        {
            ID = id,
            Nome = "Batman",
            Telefone = "999999999",
            DDD = "11",
            Email = "batman@gotham.com"
        };

        _contatoRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(default(Contato));

        await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_InformadoContatoExistenteComDadosInvalidos_ValidationException()
    {
        var id = new Guid();

        var command = new AtualizarContatoCommand
        {
            ID = id,
            Nome = "Batman",
            Telefone = "99",
            DDD = "aaa",
            Email = "invalidmail"
        };

        var contato = new Contato
        {
            Id = id,
            Nome = "Robin",
            Telefone = "999999999",
            DDD = "11",
            Email = "robin@gotham.com"
        };

        _contatoRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(contato);

        await Assert.ThrowsAsync<ValidationException>(async () => await _handler.Handle(command, CancellationToken.None));
    }
}
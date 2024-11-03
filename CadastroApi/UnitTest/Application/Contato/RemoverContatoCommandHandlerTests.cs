using CadastroApi.Application;
using CadastroApi.Domain.IRepository;
using CadastroApi.Domain.Models;
using Moq;

namespace UnitTest.Application.ContatoTests;

public class RemoverContatoCommandHandlerTests
{
    private readonly Mock<IContatoRepository> _contatoRepositoryMock;
    private readonly RemoverContatoCommandHandler _handler;    

    public RemoverContatoCommandHandlerTests()
    {
        _contatoRepositoryMock = new Mock<IContatoRepository>();
        _handler = new RemoverContatoCommandHandler(_contatoRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_InformadoContatoExistente_DeveRetornarOk()
    {
        var id = new Guid();

        var command = new RemoverContatoCommand(id);

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
        _contatoRepositoryMock.Verify(x => x.Delete(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_InformadoContatoInexistente_KeyNotFoundException()
    {
        var id = new Guid();

        var command = new RemoverContatoCommand(id);

        _contatoRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(default(Contato));

        await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));
    }
}
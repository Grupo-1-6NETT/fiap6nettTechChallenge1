using CadastroApi.Application;
using CadastroApi.Domain.IRepository;
using CadastroApi.Domain.Models;
using Moq;

namespace UnitTest.Application.UsuarioTests;

public class RemoverUsuarioCommandHandlerTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly RemoverUsuarioCommandHandler _handler;

    public RemoverUsuarioCommandHandlerTests()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _handler = new RemoverUsuarioCommandHandler(_usuarioRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_InformadoUsuarioExistente_DeveRemoverUsuario()
    {
        var id = new Guid();

        var command = new RemoverUsuarioCommand(id);

        var usuario = new Usuario { Id = id };

        _usuarioRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(usuario);

        var result = await _handler.Handle(command, CancellationToken.None);

        _usuarioRepositoryMock.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        _usuarioRepositoryMock.Verify(x => x.Delete(It.IsAny<Guid>()), Times.Once);
    }

    [Fact]
    public async Task Handle_InformadoUsuarioInexistente_KeyNotFoundException()
    {
        var id = new Guid();

        var command = new RemoverUsuarioCommand(id);

        _usuarioRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(default(Usuario));

        await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));
    }
}
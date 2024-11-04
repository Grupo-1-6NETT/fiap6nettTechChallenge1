using CadastroApi.Application;
using CadastroApi.Domain.Enums;
using CadastroApi.Domain.IRepository;
using CadastroApi.Domain.Models;
using FluentValidation;
using Moq;

namespace UnitTest.Application.UsuarioTests;

public class AdicionarUsuarioCommandHandlerTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly AdicionarUsuarioCommandHandler _handler;    

    public AdicionarUsuarioCommandHandlerTests()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _handler = new AdicionarUsuarioCommandHandler(_usuarioRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_InformadosDadosValidos_DeveAdicionarUsuario()
    {
        var command = new AdicionarUsuarioCommand
        {
            Nome = "Batman",
            Senha = "batsenha",
            Permissao = TipoUsuarioPermissao.Admin
        };

        var result = await _handler.Handle(command, CancellationToken.None);

        _usuarioRepositoryMock.Verify(x => x.AddUserAsync(It.IsAny<Usuario>(), It.IsAny<string>()), Times.Once);        
    }

    [Fact]
    public async Task Handle_InformadosDadosInvalidos_ValidationException()
    {
        var command = new AdicionarUsuarioCommand
        {
            Nome = "",
            Senha = ""
        };

        await Assert.ThrowsAsync<ValidationException>(async () => await _handler.Handle(command, CancellationToken.None));
    }
}
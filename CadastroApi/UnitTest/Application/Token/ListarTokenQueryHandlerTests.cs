using CadastroApi.Application;
using CadastroApi.Domain.IRepository;
using CadastroApi.Domain.IToken;
using CadastroApi.Domain.Models;
using Moq;

namespace UnitTest.Application.TokenTests;

public class ListarTokenQueryHandlerTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly ListarTokenQueryHandler _handler;

    public ListarTokenQueryHandlerTests()
    { 
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _tokenServiceMock = new Mock<ITokenService>();
        _handler = new ListarTokenQueryHandler(_usuarioRepositoryMock.Object, _tokenServiceMock.Object);
    }

    [Fact]
    public async Task Handle_InformadoDadosValidos_DeverRetornarToken()
    {
        var query = new ListarTokenQuery("user", "password");
        var expectedToken = "generatedToken";

        var usuario = new Usuario();

        _usuarioRepositoryMock.Setup(x => x.GetUserAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(usuario);
        _tokenServiceMock.Setup(x => x.GetToken(It.IsAny<Usuario>()))
            .Returns(expectedToken);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.Equal(expectedToken, result);
    }


    [Fact]
    public async Task Handle_InformadoDadosInvalidos_DeverRetornarStringVazio()
    {
        var query = new ListarTokenQuery("user", "password");
        var expectedToken = string.Empty;

        _usuarioRepositoryMock.Setup(x => x.GetUserAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(default(Usuario));

        var result = await _handler.Handle(query, CancellationToken.None);

        _tokenServiceMock.Verify(x => x.GetToken(It.IsAny<Usuario>()), Times.Never);

        Assert.Equal(expectedToken, result);
    }
}

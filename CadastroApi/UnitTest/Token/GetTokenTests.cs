using CadastroApi.Application;
using CadastroApi.Controllers;
using CadastroApi.Enums;
using CadastroApi.Models;
using CadastroApi.Repository;
using CadastroApi.Services;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTest;

public class GetTokenTests
{
    private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly TokenController _controller;

    public GetTokenTests()
    {
        _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
        _tokenServiceMock = new Mock<ITokenService>();        
        _controller = new TokenController(_tokenServiceMock.Object, _usuarioRepositoryMock.Object);
    }

    [Fact]
    public async Task GetToken_InformadoDadosValidos_DeverRetornarOk()
    {
        _usuarioRepositoryMock
            .Setup(x => x.GetUserAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(new Usuario());

        _tokenServiceMock
            .Setup(x => x.GetToken(It.IsAny<Usuario>()))
            .Returns("generatedToken");
       
        var result = await _controller.GetToken("user", "password");

        var okResult = Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetToken_InformadoDadosInvalidos_DeverRetornarUnauthorized()
    {
        _usuarioRepositoryMock
            .Setup(x => x.GetUserAsync(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((Usuario?)null);

        _tokenServiceMock
            .Setup(x => x.GetToken(It.IsAny<Usuario>()))
            .Returns("generatedToken");

        var result = await _controller.GetToken("user", "password");

        Assert.IsType<UnauthorizedResult>(result);
    }
}

using Moq;
using CadastroApi.Application;
using CadastroApi.Models;
using CadastroApi.Repository;
using FluentAssertions;
using FluentValidation;

namespace UnitTest;

public class AdicionarContatoTests
{
    private readonly Mock<IContatoRepository> _contatoRepositoryMock;
    private readonly AdicionarContatoCommandHandler _handler;

    public AdicionarContatoTests()
    {
        _contatoRepositoryMock = new Mock<IContatoRepository>();
        _handler = new AdicionarContatoCommandHandler(_contatoRepositoryMock.Object);
    }

    [Fact]
    public async Task AdicionarContato_DeveSalvarContatoNoRepositorio()
    {
        var command = new AdicionarContatoCommand
        {
            Nome = "Felipe Dantas",
            Telefone = "999999999",
            DDD = "11",
            Email = "felipe@example.com"
        };

        var result = await _handler.Handle(command, CancellationToken.None);

        _contatoRepositoryMock.Verify(repo => repo.AddContatoAsync(It.IsAny<Contato>()), Times.Once);
        result.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task AdicionarContato_DeveGerarErro_CPFValidatorFalha()
    {
        var command = new AdicionarContatoCommand
        {
            Nome = "Felipe Dantas",
            Telefone = "999999999", 
            DDD = "1111",            
            Email = "felipe@example.com"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));

        _contatoRepositoryMock.Verify(repo => repo.AddContatoAsync(It.IsAny<Contato>()), Times.Never);

        exception.Errors.Should().NotBeEmpty();
        exception.Errors.Should().Contain(e => e.PropertyName == "DDD");
    }

    [Fact]
    public async Task AdicionarContato_DeveGerarErro_EmailValidatorFalha()
    {
        var command = new AdicionarContatoCommand
        {
            Nome = "Felipe Dantas",
            Telefone = "999999999",
            DDD = "1111",
            Email = "felipe@@@example.com"
        };

        var exception = await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, CancellationToken.None));

        _contatoRepositoryMock.Verify(repo => repo.AddContatoAsync(It.IsAny<Contato>()), Times.Never);

        exception.Errors.Should().NotBeEmpty();
        exception.Errors.Should().Contain(e => e.PropertyName == "Email");
    }
}
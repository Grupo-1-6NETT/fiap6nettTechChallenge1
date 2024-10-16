using Moq;
using CadastroApi.Application;
using CadastroApi.Models;
using CadastroApi.Repository;
using FluentAssertions;

namespace ListarContato
{
    public class ListarContatoTests
    {
        private readonly Mock<IContatoRepository> _contatoRepositoryMock;
        private readonly ListarContatoQueryHandler _handler;

        public ListarContatoTests()
        {
            _contatoRepositoryMock = new Mock<IContatoRepository>();
            _handler = new ListarContatoQueryHandler(_contatoRepositoryMock.Object);
        }

        [Fact]
        public async Task ListarTodosContatos_DeveRetornarListaDeContatos()
        {
            var contatos = new List<Contato>
        {
            new Contato { Nome = "Felipe Dantas", Telefone = "999999999", DDD = "11", Email = "felipe@example.com" },
            new Contato { Nome = "Maria Silva", Telefone = "888888888", DDD = "21", Email = "maria@example.com" }
        };
            _contatoRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(contatos);

            var result = await _handler.Handle(new ListarContatoQuery(), CancellationToken.None);

            result.Should().BeEquivalentTo(contatos); 
            _contatoRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }
    }
}

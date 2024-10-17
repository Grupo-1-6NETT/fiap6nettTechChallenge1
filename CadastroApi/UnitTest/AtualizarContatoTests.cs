using CadastroApi.Application;
using CadastroApi.Models;
using CadastroApi.Repository;
using FluentAssertions;
using Moq;

namespace AtualizarContato
{
    public class AtualizarContatoTests
    {
        private readonly Mock<IContatoRepository> _contatoRepositoryMock;
        private readonly AtualizarContatoCommandHandler _handler;

        public AtualizarContatoTests()
        {
            _contatoRepositoryMock = new Mock<IContatoRepository>();
            _handler = new AtualizarContatoCommandHandler(_contatoRepositoryMock.Object);            
        }

        [Fact]
        public async Task AtualizarContato_DeveAtualizarContato()
        {
            var contatoId = Guid.NewGuid();

            var command = new AtualizarContatoCommand
            {
                ID = contatoId,
                Nome = "Batman",
                DDD = "99",
                Telefone = "999999999",
                Email = "batman@gotham.com"
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().Be(contatoId);

            _contatoRepositoryMock.Verify(repo => repo.UpdateContatoAsync(It.IsAny<Contato>()), Times.Once);
        }

        [Fact]
        public async Task AtualizarContato_DeveGerarErro_ArgumentException()
        {
            var command = new AtualizarContatoCommand
            {
                Nome = "Batman",
                DDD = "99",
                Telefone = "999999999",
                Email = "batman@gotham.com"
            };

            _contatoRepositoryMock.Setup(repo => repo.UpdateContatoAsync(It.IsAny<Contato>())).ThrowsAsync(new ArgumentException());

            var act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ArgumentException>();

            _contatoRepositoryMock.Verify(repo => repo.UpdateContatoAsync(It.IsAny<Contato>()), Times.Once);
        }
    }
}
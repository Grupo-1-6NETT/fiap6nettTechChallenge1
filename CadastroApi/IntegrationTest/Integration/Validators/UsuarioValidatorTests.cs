using CadastroApi.Application;

namespace CadastroApi.IntegrationTests.Validators
{
    public class UsuarioValidatorTests
    {
        private readonly ModelUsuarioValidator _validator;

        public UsuarioValidatorTests()
        {
            _validator = new ModelUsuarioValidator();
        }

        [Fact]
        public async Task Validator_Should_Return_No_Errors_When_Valid_Command()
        {
            // Arrange
            var validCommand = new AdicionarUsuarioCommand
            {
                Nome = "Maria Oliveira",
                Senha = "Senha12345",
                Permissao = Domain.Enums.TipoUsuarioPermissao.Admin
            };

            // Act
            var result = await _validator.ValidateAsync(validCommand);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Fact]
        public async Task Validator_Should_Return_Errors_When_Invalid_Command()
        {
            // Arrange
            var invalidCommand = new AdicionarUsuarioCommand
            {
                Nome = "", // Nome vazio
                Senha = "123", // Senha muito curta
                Permissao = null // Permissão não informada
            };

            // Act
            var result = await _validator.ValidateAsync(invalidCommand);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(3, result.Errors.Count); // Número de erros esperados
            Assert.Contains(result.Errors, e => e.PropertyName == "Nome" && e.ErrorMessage == "Nome é obrigatório.");
            Assert.Contains(result.Errors, e => e.PropertyName == "Senha" && e.ErrorMessage == "Senha deve ter no mínimo 8 caracteres.");
            Assert.Contains(result.Errors, e => e.PropertyName == "Permissao" && e.ErrorMessage == "Permissão deve ser informada.");
        }

        [Fact]
        public async Task Validator_Should_Return_Errors_When_Empty_Fields()
        {
            // Arrange
            var emptyFieldsCommand = new AdicionarUsuarioCommand
            {
                Nome = "",
                Senha = "",
                Permissao = null
            };

            // Act
            var result = await _validator.ValidateAsync(emptyFieldsCommand);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Nome" && e.ErrorMessage == "Nome é obrigatório.");
            Assert.Contains(result.Errors, e => e.PropertyName == "Senha" && e.ErrorMessage == "Senha é obrigatório.");
            Assert.Contains(result.Errors, e => e.PropertyName == "Permissao" && e.ErrorMessage == "Permissão deve ser informada.");
        }

        [Fact]
        public async Task Validator_Should_Return_Error_When_Nome_Exceeds_Maximum_Length()
        {
            // Arrange
            var command = new AdicionarUsuarioCommand
            {
                Nome = new string('A', 101), // Nome com 101 caracteres
                Senha = "SenhaValida123",
                Permissao = Domain.Enums.TipoUsuarioPermissao.ReadOnly
            };

            // Act
            var result = await _validator.ValidateAsync(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Nome" && e.ErrorMessage == "O nome pode ter no máximo 100 caracteres.");
        }
    }
}

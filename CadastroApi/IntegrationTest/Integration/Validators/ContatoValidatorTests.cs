using CadastroApi.Application;

namespace CadastroApi.IntegrationTests.Validators
{
    public class ContatoValidatorTests
    {
        private readonly ModelContatoValidator _validator;

        public ContatoValidatorTests()
        {
            _validator = new ModelContatoValidator();
        }

        [Fact]
        public async Task Validator_Should_Return_No_Errors_When_Valid_Command()
        {
            // Arrange
            var validCommand = new AdicionarContatoCommand
            {
                Nome = "João Silva",
                Telefone = "987654321",
                DDD = "11",
                Email = "joao.silva@example.com"
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
            var invalidCommand = new AdicionarContatoCommand
            {
                Nome = "", // Nome vazio
                Telefone = "123", // Telefone inválido
                DDD = "123", // DDD inválido
                Email = "emailinvalido" // Email inválido
            };

            // Act
            var result = await _validator.ValidateAsync(invalidCommand);

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal(5, result.Errors.Count); // Número de erros esperados
            Assert.Contains(result.Errors, e => e.PropertyName == "Nome" && e.ErrorMessage == "Nome é obrigatório");
            Assert.Contains(result.Errors, e => e.PropertyName == "Telefone" && e.ErrorMessage == "Telefone deve conter entre 8 e 9 dígitos");
            Assert.Contains(result.Errors, e => e.PropertyName == "DDD" && e.ErrorMessage == "DDD deve estar entre 11 e 99");
            Assert.Contains(result.Errors, e => e.PropertyName == "Email" && e.ErrorMessage == "Formato de email inválido");
        }

        [Fact]
        public async Task Validator_Should_Return_Errors_When_Empty_Fields()
        {
            // Arrange
            var emptyFieldsCommand = new AdicionarContatoCommand
            {
                Nome = "",
                Telefone = "",
                DDD = "",
                Email = ""
            };

            // Act
            var result = await _validator.ValidateAsync(emptyFieldsCommand);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Nome" && e.ErrorMessage == "Nome é obrigatório");
            Assert.Contains(result.Errors, e => e.PropertyName == "Telefone" && e.ErrorMessage == "Telefone é obrigatório");
            Assert.Contains(result.Errors, e => e.PropertyName == "DDD" && e.ErrorMessage == "DDD é obrigatório");
            Assert.Contains(result.Errors, e => e.PropertyName == "Email" && e.ErrorMessage == "Email é obrigatório");
        }
    }
}

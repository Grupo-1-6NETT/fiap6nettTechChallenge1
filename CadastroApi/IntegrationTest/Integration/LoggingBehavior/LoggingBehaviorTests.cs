using CadastroApi.Application;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;

namespace CadastroApi.IntegrationTests.LoggingBehavior
{
    public class LoggingBehaviorTests
    {
        private readonly List<string> _loggedMessages;
        private readonly Mock<ILogger<LoggingBehavior<TestRequest, TestResponse>>> _mockLogger;
        private readonly LoggingBehavior<TestRequest, TestResponse> _loggingBehavior;

        public LoggingBehaviorTests()
        {
            _loggedMessages = new List<string>();
            _mockLogger = new Mock<ILogger<LoggingBehavior<TestRequest, TestResponse>>>();

            // Mock the logger to capture logged messages
            _mockLogger
                .Setup(logger => logger.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()
                ))
                .Callback((LogLevel logLevel, EventId eventId, object state, Exception exception, Delegate formatter) =>
                {
                    _loggedMessages.Add(state.ToString());
                });

            _loggingBehavior = new LoggingBehavior<TestRequest, TestResponse>(_mockLogger.Object);
        }

        [Fact]
        public async Task Handle_Should_Log_Information_And_Proceed_To_Next()
        {
            // Arrange
            var request = new TestRequest { Data = "Test Data" };
            var response = new TestResponse { Result = "Success" };
            var mockNext = new Mock<RequestHandlerDelegate<TestResponse>>();
            mockNext.Setup(next => next()).ReturnsAsync(response);

            // Act
            var result = await _loggingBehavior.Handle(request, mockNext.Object, CancellationToken.None);

            // Assert
            Assert.Equal(response, result);
            Assert.Contains("Processando a requisicao: TestRequest", _loggedMessages);
            Assert.Contains("Requisicao processada: TestRequest", _loggedMessages);
        }

        [Fact]
        public async Task Handle_Should_Log_Error_When_Exception_Is_Thrown()
        {
            // Arrange
            var request = new TestRequest { Data = "Test Data" };
            var mockNext = new Mock<RequestHandlerDelegate<TestResponse>>();
            mockNext.Setup(next => next()).ThrowsAsync(new InvalidOperationException("Test Exception"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _loggingBehavior.Handle(request, mockNext.Object, CancellationToken.None));

            // Assert
            Assert.Contains("Erro na requisicao: TestRequest", _loggedMessages);
        }

        [Fact]
        public async Task Handle_Should_Write_To_Log_File()
        {
            // Arrange
            var logDirectory = Path.Combine("logs");
            var logFilePath = Path.Combine(logDirectory, $"log-{DateTime.Now:yyyy-MM-dd}.log");
            if (File.Exists(logFilePath))
                File.Delete(logFilePath);

            var request = new TestRequest { Data = "Test Data" };
            var response = new TestResponse { Result = "Success" };
            var mockNext = new Mock<RequestHandlerDelegate<TestResponse>>();
            mockNext.Setup(next => next()).ReturnsAsync(response);

            // Act
            await _loggingBehavior.Handle(request, mockNext.Object, CancellationToken.None);

            // Assert
            Assert.True(File.Exists(logFilePath));
            var logContent = await File.ReadAllTextAsync(logFilePath);
            Assert.Contains("Requisicao: TestRequest", logContent);
        }

        public class TestRequest : IRequest<TestResponse>
        {
            public string Data { get; set; }
        }

        public class TestResponse
        {
            public string Result { get; set; }
        }
    }
}

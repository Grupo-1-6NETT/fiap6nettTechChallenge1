using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CadastroApi.Application
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Processando a requisicao: {typeof(TRequest).Name}");

            await LogToFile($"Requisicao: {typeof(TRequest).Name} - {DateTime.Now}\nData: {request}\n");

            try
            {
                var response = await next();

                _logger.LogInformation($"Requisicao processada: {typeof(TRequest).Name}");
                await LogToFile($"Resposta: {typeof(TResponse).Name} - {DateTime.Now}\nData: {response}\n");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro na requisicao: {typeof(TRequest).Name}");
                await LogToFile($"Mensagem de erro: {ex.Message}\n{ex.StackTrace}\n");

                throw;
            }
        }

        private async Task LogToFile(string message)
        {
            string logFilePath = Path.Combine("logs", $"log-{DateTime.Now:yyyy-MM-dd}.log");

            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));

            await File.AppendAllTextAsync(logFilePath, message + Environment.NewLine);
        }
    }
}

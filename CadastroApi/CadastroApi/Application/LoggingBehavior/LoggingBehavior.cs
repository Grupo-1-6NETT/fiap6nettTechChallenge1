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
            // Log inicial informando que a requisição está sendo tratada
            _logger.LogInformation($"Handling {typeof(TRequest).Name}");

            // Loga os dados da requisição
            await LogToFile($"Request: {typeof(TRequest).Name} - {DateTime.Now}\nData: {request}\n");

            try
            {
                // Executa a próxima etapa do pipeline
                var response = await next();

                // Loga que a requisição foi tratada com sucesso
                _logger.LogInformation($"Handled {typeof(TRequest).Name}");
                await LogToFile($"Response: {typeof(TResponse).Name} - {DateTime.Now}\nData: {response}\n");

                return response;
            }
            catch (Exception ex)
            {
                // Loga qualquer erro que ocorra durante o processamento da requisição
                _logger.LogError(ex, $"Error handling {typeof(TRequest).Name}");
                await LogToFile($"Error: {ex.Message}\n{ex.StackTrace}\n");

                throw;
            }
        }

        private async Task LogToFile(string message)
        {
            // Define o nome do arquivo de log do dia atual no formato log-yyyy-MM-dd.log
            string logFilePath = Path.Combine("logs", $"log-{DateTime.Now:yyyy-MM-dd}.log");

            // Cria a pasta "logs" se ela não existir
            Directory.CreateDirectory(Path.GetDirectoryName(logFilePath));

            // Grava o log no arquivo do dia
            await File.AppendAllTextAsync(logFilePath, message + Environment.NewLine);
        }
    }
}

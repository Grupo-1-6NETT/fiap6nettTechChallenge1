using FluentValidation;

namespace CadastroApi.Application.Extensions
{
    public static class ValidationExceptionExtension
    {
        public static string[] ToResultMessage(this ValidationException exception) 
        {
            if (exception.Errors == null || exception.Errors.Count() == 0)
                return Array.Empty<string>();


            return exception.Errors.Select(x => x.ErrorMessage).ToArray();
        }
    }
}

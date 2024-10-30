using CadastroApi.Repository;
using CadastroApi.Services;
using MediatR;

namespace CadastroApi.Application
{
    public class GetTokenHandler : IRequestHandler<ListarTokenQuery, string>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;

        public GetTokenHandler(IUsuarioRepository usuarioRepository, ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
        }

        public async Task<string> Handle(ListarTokenQuery request, CancellationToken cancellationToken)
        {
            var user = await _usuarioRepository.GetUserAsync(request.Usuario, request.Senha);

            if (user is not null)
            {
                var token = _tokenService.GetToken(user);

                if (!string.IsNullOrWhiteSpace(token))
                {
                    return token; 
                }
            }
            return string.Empty; 
        }
    }
}
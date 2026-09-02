using MediatR;
using Modulos.Autenticacao.Aplicacao.Repositorios;
using Modulos.Autenticacao.Aplicacao.Servicos;

namespace Modulos.Autenticacao.Aplicacao.CasosDeUso.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ISenhaServico _senhaServico;
        private readonly ITokenServico _tokenServico;

        public LoginHandler(IUsuarioRepositorio usuarioRepositorio, ISenhaServico senhaServico, ITokenServico tokenServico)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _senhaServico = senhaServico;
            _tokenServico = tokenServico;
        }

        public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepositorio.ObterPorEmailAsync(request.Email);
            if (usuario is null || usuario == default || !_senhaServico.VerificarHash(request.Senha, usuario.SenhaHash))
            {
                throw new ArgumentException("E-mail ou Senha inválidos.");
            }

            return _tokenServico.GerarToken(usuario);
        }
    }
}
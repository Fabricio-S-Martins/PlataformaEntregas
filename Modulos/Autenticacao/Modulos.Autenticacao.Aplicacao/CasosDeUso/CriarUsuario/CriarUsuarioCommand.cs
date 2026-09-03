using MediatR;

namespace Modulos.Autenticacao.Aplicacao.CasosDeUso.CriarUsuario
{
    public record CriarUsuarioCommand(string Nome, string Email, string Senha, string Papel) : IRequest;
}

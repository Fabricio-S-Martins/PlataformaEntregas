using MediatR;

namespace Modulos.Autenticacao.Aplicacao.CasosDeUso.Login
{
    public record LoginCommand(string Email, string Senha) : IRequest<string>;
}
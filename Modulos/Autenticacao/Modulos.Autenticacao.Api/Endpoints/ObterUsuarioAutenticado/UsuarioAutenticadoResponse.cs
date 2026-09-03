using System;

namespace Modulos.Autenticacao.Api.Endpoints.ObterUsuarioAutenticado
{
    public record UsuarioAutenticadoResponse(Guid Id, string Papel);
}
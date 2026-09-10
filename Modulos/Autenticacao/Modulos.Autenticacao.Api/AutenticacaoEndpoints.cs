using Microsoft.AspNetCore.Routing;
using Modulos.Autenticacao.Api.Endpoints.CriarUsuario;
using Modulos.Autenticacao.Api.Endpoints.Login;
using Modulos.Autenticacao.Api.Endpoints.ObterUsuarioAutenticado;

namespace Modulos.Autenticacao.Api
{
    public static class AutenticacaoEndpoints
    {
        public static void MapAutenticacaoEndpoints(this IEndpointRouteBuilder rotas)
        {
            rotas.MapUsuariosEndpoints();
            rotas.MapLoginEndPoint();
            rotas.MapObterUsuarioAutenticadoEndpoint();
        }
    }
}
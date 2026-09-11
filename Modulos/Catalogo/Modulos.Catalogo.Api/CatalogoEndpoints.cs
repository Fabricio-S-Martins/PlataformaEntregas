using Microsoft.AspNetCore.Routing;
using Modulos.Catalogo.Api.Endpoints.CadastrarRestaurante;
using Modulos.Catalogo.Api.Endpoints.ObterRestaurante;

namespace Modulos.Catalogo.Api
{
    public static class CatalogoEndpoints
    {
        public static void MapCatalogoEndpoints(this IEndpointRouteBuilder rotas)
        {
            rotas.MapRestaurantesEndpoints();
            rotas.MapObterRestauranteEndpoint();
        }
    }
}
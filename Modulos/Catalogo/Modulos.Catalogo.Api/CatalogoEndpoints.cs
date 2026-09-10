using Microsoft.AspNetCore.Routing;
using Modulos.Catalogo.Api.Endpoints.CadastrarRestaurante;

namespace Modulos.Catalogo.Api
{
    public static class CatalogoEndpoints
    {
        public static void MapCatalogoEndpoints(this IEndpointRouteBuilder rotas)
        {
            rotas.MapRestaurantesEndpoints();
        }
    }
}
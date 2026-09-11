using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modulos.Catalogo.Aplicacao.CasosDeUso.ObterRestaurante;

namespace Modulos.Catalogo.Api.Endpoints.ObterRestaurante
{
    public static class ObterRestauranteEndpoint
    {
        public static void MapObterRestauranteEndpoint(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/restaurantes/{id}", async (Guid id, ISender sender, CancellationToken cancelationToken) =>
            {
                var restaurante = await sender.Send(new ObterRestauranteQuery(id), cancelationToken);
                if (restaurante == null)
                    return Results.NotFound();

                return Results.Ok(restaurante);
            })
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound);
        }
    }
}
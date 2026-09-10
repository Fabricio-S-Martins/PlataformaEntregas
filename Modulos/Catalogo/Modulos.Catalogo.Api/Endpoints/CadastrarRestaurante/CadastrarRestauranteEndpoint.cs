using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modulos.Catalogo.Aplicacao.CasosDeUso.CadastrarRestaurante;

namespace Modulos.Catalogo.Api.Endpoints.CadastrarRestaurante
{
    public static class CadastrarRestauranteEndpoint
    {
        public static void MapRestaurantesEndpoints(this IEndpointRouteBuilder routes)
        {
            routes.MapPost("/restaurantes", async (CadastrarRestauranteRequest request, ISender mediator, CancellationToken cancellationToken) =>
            {
                try
                {
                    await mediator.Send(new CadastrarRestauranteCommand(request.Nome, request.Cnpj), cancellationToken);
                    return Results.Created();
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { erro = ex.Message });
                }
            })
            .WithName("CadastrarRestaurante")
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
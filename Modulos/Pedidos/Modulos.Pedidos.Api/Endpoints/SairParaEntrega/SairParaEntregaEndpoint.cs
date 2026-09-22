using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modulos.Pedidos.Aplicacao.CasosDeUso.SairParaEntrega;

namespace Modulos.Pedidos.Api.Endpoints.SairParaEntrega
{
    public static class SairParaEntregaEndpoint
    {
        public static void MapSairParaEntregaEndpoint(this IEndpointRouteBuilder routes)
        {
            routes.MapPost("/pedidos/{id}/sair-para-entrega", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                try
                {
                    await sender.Send(new SairParaEntregaCommand(id), cancellationToken);
                    return Results.NoContent();
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { erro = ex.Message });
                }
                catch (InvalidOperationException ex)
                {
                    return Results.NotFound(new { erro = ex.Message });
                }
            })
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound);
        }
    }
}
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modulos.Pedidos.Aplicacao.CasosDeUso.Cancelar;

namespace Modulos.Pedidos.Api.Endpoints.Cancelar
{
    public static class CancelarEndpoint
    {
        public static void MapCancelarEndpoint(this IEndpointRouteBuilder routes)
        {
            routes.MapPost("/pedidos/{id}/cancelar", async (Guid id, ISender sender, CancellationToken cancellationToken) =>
            {
                try
                {
                    await sender.Send(new CancelarCommand(id), cancellationToken);
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
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modulos.Pedidos.Aplicacao.CasosDeUso.AdicionarItem;

namespace Modulos.Pedidos.Api.Endpoints.AdicionarItem
{
    public static class AdicionarItemEndpoint
    {
        public static void MapAdicionarItemEndpoint(this IEndpointRouteBuilder routes)
        {
            routes.MapPost("/pedidos/{id}/itens", async (Guid id, AdicionarItemRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                try
                {
                    await sender.Send(new AdicionarItemCommand(id, request.ItemCardapioId, request.Quantidade, request.PrecoUnitario), cancellationToken);
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
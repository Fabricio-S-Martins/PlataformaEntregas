using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modulos.Pedidos.Aplicacao.CasosDeUso.CriarPedido;

namespace Modulos.Pedidos.Api.Endpoints.CriarPedido
{
    public static class CriarPedidoEndpoint
    {
        public static void MapCriarPedidoEndpoint(this IEndpointRouteBuilder routes)
        {
            routes.MapPost("/pedidos", async (CriarPedidoRequest request, ISender sender, CancellationToken cancellationToken) =>
            {
                try
                {
                    var id = await sender.Send(new CriarPedidoCommand(request.ClienteId, request.RestauranteId), cancellationToken);
                    return Results.Created($"/pedidos/{id}", new { id });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { erro = ex.Message });
                }
            })
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
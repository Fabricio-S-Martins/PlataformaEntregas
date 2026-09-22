namespace Modulos.Pedidos.Api.Endpoints.CriarPedido
{
    public record CriarPedidoRequest(Guid ClienteId, Guid RestauranteId);
}
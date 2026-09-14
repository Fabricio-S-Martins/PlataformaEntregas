using MediatR;

namespace Modulos.Pedidos.Aplicacao.CasosDeUso.CriarPedido
{
    public record CriarPedidoCommand(Guid ClienteId, Guid RestauranteId) : IRequest<Guid>;
}
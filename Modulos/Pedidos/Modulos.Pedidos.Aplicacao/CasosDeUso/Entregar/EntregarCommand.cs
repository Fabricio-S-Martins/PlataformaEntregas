using MediatR;

namespace Modulos.Pedidos.Aplicacao.CasosDeUso.Entregar
{
    public record EntregarCommand(Guid PedidoId) : IRequest;
}
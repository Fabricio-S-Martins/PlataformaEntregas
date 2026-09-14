using MediatR;

namespace Modulos.Pedidos.Aplicacao.CasosDeUso.SairParaEntrega
{
    public record SairParaEntregaCommand(Guid PedidoId) : IRequest;
}
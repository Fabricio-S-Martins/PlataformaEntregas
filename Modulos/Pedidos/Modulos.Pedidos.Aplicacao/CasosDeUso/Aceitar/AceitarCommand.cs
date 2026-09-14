using MediatR;

namespace Modulos.Pedidos.Aplicacao.CasosDeUso.Aceitar
{
    public record AceitarCommand(Guid PedidoId) : IRequest;
}
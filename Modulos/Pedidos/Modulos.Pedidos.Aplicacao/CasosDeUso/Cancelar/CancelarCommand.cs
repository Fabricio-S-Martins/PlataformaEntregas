using MediatR;

namespace Modulos.Pedidos.Aplicacao.CasosDeUso.Cancelar
{
    public record CancelarCommand(Guid PedidoId) : IRequest;
}
using MediatR;

namespace Modulos.Pedidos.Aplicacao.CasosDeUso.ConfirmarPagamento
{
    public record ConfirmarPagamentoCommand(Guid PedidoId) : IRequest;
}

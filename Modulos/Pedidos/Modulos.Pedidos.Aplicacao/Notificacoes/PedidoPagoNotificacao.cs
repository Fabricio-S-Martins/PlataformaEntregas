using MediatR;

namespace Modulos.Pedidos.Aplicacao.Notificacoes
{
    public record PedidoPagoNotificacao(Guid PedidoId) : INotification;
}
using MediatR;

namespace Modulos.Pedidos.Aplicacao.Notificacoes
{
    public record PedidoEntregueNotificacao(Guid PedidoId) : INotification;
}
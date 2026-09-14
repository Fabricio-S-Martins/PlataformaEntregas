using MediatR;

namespace Modulos.Pedidos.Aplicacao.Notificacoes
{
    public record PedidoAceitoNotificacao(Guid PedidoId) : INotification;
}
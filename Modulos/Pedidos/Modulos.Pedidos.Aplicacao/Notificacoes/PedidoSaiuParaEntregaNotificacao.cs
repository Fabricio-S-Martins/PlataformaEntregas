using MediatR;

namespace Modulos.Pedidos.Aplicacao.Notificacoes
{
    public record PedidoSaiuParaEntregaNotificacao(Guid PedidoId) : INotification;
}
using MediatR;

namespace Modulos.Pedidos.Aplicacao.Notificacoes
{
    public record PedidoEmPreparoNotificacao(Guid PedidoId) : INotification;
}
using MediatR;

namespace Modulos.Pedidos.Aplicacao.Notificacoes
{
    public record PedidoItemAdicionadoNotificacao(Guid PedidoId) : INotification;
}
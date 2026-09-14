using MediatR;

namespace Modulos.Pedidos.Aplicacao.Notificacoes
{
    public record PedidoCanceladoNotificacao(Guid PedidoId) : INotification;
}
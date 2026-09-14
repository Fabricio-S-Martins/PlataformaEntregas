using MediatR;

namespace Modulos.Pedidos.Aplicacao.Notificacoes
{
    public record PedidoCriadoNotificacao(Guid PedidoId) : INotification;
}
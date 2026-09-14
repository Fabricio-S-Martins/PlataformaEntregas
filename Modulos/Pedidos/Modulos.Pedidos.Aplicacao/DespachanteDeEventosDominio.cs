using MediatR;
using Modulos.Pedidos.Aplicacao.Notificacoes;
using Modulos.Pedidos.Dominio.Eventos;

namespace Modulos.Pedidos.Aplicacao
{
    public static class DespachanteDeEventosDominio
    {
        public static async Task DespacharAsync(IReadOnlyList<IEventoDominio> eventos, IPublisher publisher, CancellationToken cancellationToken)
        {
            foreach (var evento in eventos)
            {
                INotification notificacao = evento switch
                {
                    PedidoCriadoEvento e => new PedidoCriadoNotificacao(e.PedidoId),
                    PedidoItemAdicionadoEvento e => new PedidoItemAdicionadoNotificacao(e.PedidoId),
                    PedidoPagoEvento e => new PedidoPagoNotificacao(e.PedidoId),
                    PedidoAceitoEvento e => new PedidoAceitoNotificacao(e.PedidoId),
                    PedidoEmPreparoEvento e => new PedidoEmPreparoNotificacao(e.PedidoId),
                    PedidoSaiuParaEntregaEvento e => new PedidoSaiuParaEntregaNotificacao(e.PedidoId),
                    PedidoEntregueEvento e => new PedidoEntregueNotificacao(e.PedidoId),
                    PedidoCanceladoEvento e => new PedidoCanceladoNotificacao(e.PedidoId),
                    _ => throw new NotSupportedException("Evento inválido.")
                };

                await publisher.Publish(notificacao, cancellationToken);
            }
        }
    }
}
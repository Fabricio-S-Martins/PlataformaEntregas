using MediatR;
using Modulos.Pedidos.Aplicacao.Repositorios;

namespace Modulos.Pedidos.Aplicacao.CasosDeUso.Aceitar
{
    public class AceitarHandler : IRequestHandler<AceitarCommand>
    {
        private readonly IPedidoRepositorio _pedidoRepositorio;
        private readonly IPublisher _publisher;
        public AceitarHandler(IPedidoRepositorio pedidoRepositorio, IPublisher publisher)
        {
            _pedidoRepositorio = pedidoRepositorio;
            _publisher = publisher;
        }

        public async Task Handle(AceitarCommand request, CancellationToken cancellationToken)
        {
            var pedido = await _pedidoRepositorio.ObterPorIdAsync(request.PedidoId);
            if (pedido == null)
                throw new InvalidOperationException("Pedido não encontrado.");

            var resultadoPedido = pedido.Aceitar();
            if (!resultadoPedido.Sucesso)
                throw new ArgumentException(string.Join(Environment.NewLine, resultadoPedido.Erros));

            await _pedidoRepositorio.AtualizarAsync(pedido);
            await DespachanteDeEventosDominio.DespacharAsync(pedido.Eventos, _publisher, cancellationToken);
        }
    }
}
using MediatR;
using Modulos.Pedidos.Aplicacao.Repositorios;

namespace Modulos.Pedidos.Aplicacao.CasosDeUso.AdicionarItem
{
    public class AdicionarItemHandler : IRequestHandler<AdicionarItemCommand>
    {
        private readonly IPedidoRepositorio _pedidoRepositorio;
        private readonly IPublisher _publisher;
        public AdicionarItemHandler(IPedidoRepositorio pedidoRepositorio, IPublisher publisher)
        {
            _pedidoRepositorio = pedidoRepositorio;
            _publisher = publisher;
        }

        public async Task Handle(AdicionarItemCommand request, CancellationToken cancellationToken)
        {
            var pedido = await _pedidoRepositorio.ObterPorIdAsync(request.PedidoId);
            if (pedido == null)
                throw new InvalidOperationException("Pedido não encontrado.");

            var resultadoItemPedido = pedido.AdicionarItem(request.ItemCardapioId, request.Quantidade, request.PrecoUnitario);
            if (!resultadoItemPedido.Sucesso)
                throw new ArgumentException(string.Join(Environment.NewLine, resultadoItemPedido.Erros));

            await _pedidoRepositorio.AtualizarAsync(pedido);
            await DespachanteDeEventosDominio.DespacharAsync(pedido.Eventos, _publisher, cancellationToken);
        }
    }
}
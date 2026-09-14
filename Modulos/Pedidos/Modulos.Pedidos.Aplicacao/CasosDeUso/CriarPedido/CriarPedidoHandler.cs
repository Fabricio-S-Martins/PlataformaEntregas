using MediatR;
using Modulos.Pedidos.Aplicacao.Repositorios;
using Modulos.Pedidos.Dominio.Entidades;

namespace Modulos.Pedidos.Aplicacao.CasosDeUso.CriarPedido
{
    public class CriarPedidoHandler : IRequestHandler<CriarPedidoCommand, Guid>
    {
        private readonly IPedidoRepositorio _pedidoRepositorio;
        private readonly IPublisher _publisher;
        public CriarPedidoHandler(IPedidoRepositorio pedidoRepositorio, IPublisher publisher)
        {
            _pedidoRepositorio = pedidoRepositorio;
            _publisher = publisher;
        }

        public async Task<Guid> Handle(CriarPedidoCommand request, CancellationToken cancellationToken)
        {
            var resultadoPedido = Pedido.Criar(request.ClienteId, request.RestauranteId);
            if (!resultadoPedido.Sucesso)
                throw new ArgumentException(string.Join(Environment.NewLine, resultadoPedido.Erros));

            await _pedidoRepositorio.AdicionarAsync(resultadoPedido.Valor);
            await DespachanteDeEventosDominio.DespacharAsync(resultadoPedido.Valor.Eventos, _publisher, cancellationToken);

            return resultadoPedido.Valor.Id;
        }
    }
}
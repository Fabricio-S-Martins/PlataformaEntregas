using MediatR;
using Modulos.Pedidos.Aplicacao.CasosDeUso.SairParaEntrega;
using Modulos.Pedidos.Aplicacao.Repositorios;
using Modulos.Pedidos.Dominio.Entidades;
using Moq;

namespace Modulos.Pedidos.Aplicacao.Testes.CasosDeUso.SairParaEntrega
{
    public class SairParaEntregaHandlerTestes
    {
        private readonly Mock<IPedidoRepositorio> _pedidoRepositorioMock;
        private readonly Mock<IPublisher> _publisherMock;

        public SairParaEntregaHandlerTestes()
        {
            _pedidoRepositorioMock = new Mock<IPedidoRepositorio>();
            _publisherMock = new Mock<IPublisher>();
        }

        private static Pedido CriarPedidoEmPreparo()
        {
            var pedido = Pedido.Criar(Guid.NewGuid(), Guid.NewGuid()).Valor;
            pedido.AdicionarItem(Guid.NewGuid(), 2, 10);
            pedido.ConfirmarPagamento();
            pedido.Aceitar();
            pedido.IniciarPreparo();
            return pedido;
        }

        [Fact]
        public async Task Handle_ComPedidoEmPreparo_DevePassarPeloAtualizarDoRepositorioEPublicar()
        {
            var pedido = CriarPedidoEmPreparo();
            _pedidoRepositorioMock.Setup(r => r.ObterPorIdAsync(pedido.Id)).ReturnsAsync(pedido);
            var handler = new SairParaEntregaHandler(_pedidoRepositorioMock.Object, _publisherMock.Object);
            var command = new SairParaEntregaCommand(pedido.Id);

            await handler.Handle(command, CancellationToken.None);

            _pedidoRepositorioMock.Verify(r => r.AtualizarAsync(pedido), Times.Once);
            _publisherMock.Verify(p => p.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task Handle_ComPedidoForaDoStatusEmPreparo_DeveLancarExcecaoSemChamarOAtualizarNemPublicar()
        {
            var pedido = Pedido.Criar(Guid.NewGuid(), Guid.NewGuid()).Valor;
            _pedidoRepositorioMock.Setup(r => r.ObterPorIdAsync(pedido.Id)).ReturnsAsync(pedido);
            var handler = new SairParaEntregaHandler(_pedidoRepositorioMock.Object, _publisherMock.Object);
            var command = new SairParaEntregaCommand(pedido.Id);

            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, CancellationToken.None));

            _pedidoRepositorioMock.Verify(r => r.AtualizarAsync(It.IsAny<Pedido>()), Times.Never);
            _publisherMock.Verify(p => p.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ComPedidoNaoEncontrado_DeveLancarExcecaoSemChamarOAtualizarNemPublicar()
        {
            _pedidoRepositorioMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Pedido)null);
            var handler = new SairParaEntregaHandler(_pedidoRepositorioMock.Object, _publisherMock.Object);
            var command = new SairParaEntregaCommand(Guid.NewGuid());

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));

            _pedidoRepositorioMock.Verify(r => r.AtualizarAsync(It.IsAny<Pedido>()), Times.Never);
            _publisherMock.Verify(p => p.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
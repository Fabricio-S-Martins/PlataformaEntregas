using MediatR;
using Modulos.Pedidos.Aplicacao.CasosDeUso.Aceitar;
using Modulos.Pedidos.Aplicacao.Repositorios;
using Modulos.Pedidos.Dominio.Entidades;
using Moq;

namespace Modulos.Pedidos.Aplicacao.Testes.CasosDeUso.Aceitar
{
    public class AceitarHandlerTestes
    {
        private readonly Mock<IPedidoRepositorio> _pedidoRepositorioMock;
        private readonly Mock<IPublisher> _publisherMock;

        public AceitarHandlerTestes()
        {
            _pedidoRepositorioMock = new Mock<IPedidoRepositorio>();
            _publisherMock = new Mock<IPublisher>();
        }

        private static Pedido CriarPedidoPago()
        {
            var pedido = Pedido.Criar(Guid.NewGuid(), Guid.NewGuid()).Valor;
            pedido.AdicionarItem(Guid.NewGuid(), 2, 10);
            pedido.ConfirmarPagamento();
            return pedido;
        }

        [Fact]
        public async Task Handle_ComPedidoPago_DevePassarPeloAtualizarDoRepositorioEPublicar()
        {
            var pedido = CriarPedidoPago();
            _pedidoRepositorioMock.Setup(r => r.ObterPorIdAsync(pedido.Id)).ReturnsAsync(pedido);
            var handler = new AceitarHandler(_pedidoRepositorioMock.Object, _publisherMock.Object);
            var command = new AceitarCommand(pedido.Id);

            await handler.Handle(command, CancellationToken.None);

            _pedidoRepositorioMock.Verify(r => r.AtualizarAsync(pedido), Times.Once);
            _publisherMock.Verify(p => p.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task Handle_ComPedidoForaDoStatusPago_DeveLancarExcecaoSemChamarOAtualizarNemPublicar()
        {
            var pedido = Pedido.Criar(Guid.NewGuid(), Guid.NewGuid()).Valor;
            _pedidoRepositorioMock.Setup(r => r.ObterPorIdAsync(pedido.Id)).ReturnsAsync(pedido);
            var handler = new AceitarHandler(_pedidoRepositorioMock.Object, _publisherMock.Object);
            var command = new AceitarCommand(pedido.Id);

            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, CancellationToken.None));

            _pedidoRepositorioMock.Verify(r => r.AtualizarAsync(It.IsAny<Pedido>()), Times.Never);
            _publisherMock.Verify(p => p.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ComPedidoNaoEncontrado_DeveLancarExcecaoSemChamarOAtualizarNemPublicar()
        {
            _pedidoRepositorioMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Pedido)null);
            var handler = new AceitarHandler(_pedidoRepositorioMock.Object, _publisherMock.Object);
            var command = new AceitarCommand(Guid.NewGuid());

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));

            _pedidoRepositorioMock.Verify(r => r.AtualizarAsync(It.IsAny<Pedido>()), Times.Never);
            _publisherMock.Verify(p => p.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
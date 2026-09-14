using MediatR;
using Modulos.Pedidos.Aplicacao.CasosDeUso.AdicionarItem;
using Modulos.Pedidos.Aplicacao.Repositorios;
using Modulos.Pedidos.Dominio.Entidades;
using Moq;

namespace Modulos.Pedidos.Aplicacao.Testes.CasosDeUso.AdicionarItem
{
    public class AdicionarItemHandlerTestes
    {
        private readonly Mock<IPedidoRepositorio> _pedidoRepositorioMock;
        private readonly Mock<IPublisher> _publisherMock;

        public AdicionarItemHandlerTestes()
        {
            _pedidoRepositorioMock = new Mock<IPedidoRepositorio>();
            _publisherMock = new Mock<IPublisher>();
        }

        [Fact]
        public async Task Handle_ComPedidoEncontradoEDadosValidos_DevePassarPeloAtualizarDoRepositorioEPublicar()
        {
            var pedido = Pedido.Criar(Guid.NewGuid(), Guid.NewGuid()).Valor;
            _pedidoRepositorioMock.Setup(r => r.ObterPorIdAsync(pedido.Id)).ReturnsAsync(pedido);
            var handler = new AdicionarItemHandler(_pedidoRepositorioMock.Object, _publisherMock.Object);
            var command = new AdicionarItemCommand(pedido.Id, Guid.NewGuid(), 2, 10);

            await handler.Handle(command, CancellationToken.None);

            _pedidoRepositorioMock.Verify(r => r.AtualizarAsync(pedido), Times.Once);
            _publisherMock.Verify(p => p.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task Handle_ComPedidoEncontradoEDadosInvalidos_DeveLancarExcecaoSemChamarOAtualizarNemPublicar()
        {
            var pedido = Pedido.Criar(Guid.NewGuid(), Guid.NewGuid()).Valor;
            _pedidoRepositorioMock.Setup(r => r.ObterPorIdAsync(pedido.Id)).ReturnsAsync(pedido);
            var handler = new AdicionarItemHandler(_pedidoRepositorioMock.Object, _publisherMock.Object);
            var command = new AdicionarItemCommand(pedido.Id, Guid.Empty, 0, 0);

            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, CancellationToken.None));

            _pedidoRepositorioMock.Verify(r => r.AtualizarAsync(It.IsAny<Pedido>()), Times.Never);
            _publisherMock.Verify(p => p.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ComPedidoNaoEncontrado_DeveLancarExcecaoSemChamarOAtualizarNemPublicar()
        {
            _pedidoRepositorioMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Pedido)null);
            var handler = new AdicionarItemHandler(_pedidoRepositorioMock.Object, _publisherMock.Object);
            var command = new AdicionarItemCommand(Guid.NewGuid(), Guid.NewGuid(), 2, 10);

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));

            _pedidoRepositorioMock.Verify(r => r.AtualizarAsync(It.IsAny<Pedido>()), Times.Never);
            _publisherMock.Verify(p => p.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
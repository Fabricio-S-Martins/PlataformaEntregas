using MediatR;
using Modulos.Pedidos.Aplicacao.CasosDeUso.Entregar;
using Modulos.Pedidos.Aplicacao.Repositorios;
using Modulos.Pedidos.Dominio.Entidades;
using Moq;

namespace Modulos.Pedidos.Aplicacao.Testes.CasosDeUso.Entregar
{
    public class EntregarHandlerTestes
    {
        private readonly Mock<IPedidoRepositorio> _pedidoRepositorioMock;
        private readonly Mock<IPublisher> _publisherMock;

        public EntregarHandlerTestes()
        {
            _pedidoRepositorioMock = new Mock<IPedidoRepositorio>();
            _publisherMock = new Mock<IPublisher>();
        }

        private static Pedido CriarPedidoSaiuParaEntrega()
        {
            var pedido = Pedido.Criar(Guid.NewGuid(), Guid.NewGuid()).Valor;
            pedido.AdicionarItem(Guid.NewGuid(), 2, 10);
            pedido.ConfirmarPagamento();
            pedido.Aceitar();
            pedido.IniciarPreparo();
            pedido.SairParaEntrega();
            return pedido;
        }

        [Fact]
        public async Task Handle_ComPedidoSaiuParaEntrega_DevePassarPeloAtualizarDoRepositorioEPublicar()
        {
            var pedido = CriarPedidoSaiuParaEntrega();
            _pedidoRepositorioMock.Setup(r => r.ObterPorIdAsync(pedido.Id)).ReturnsAsync(pedido);
            var handler = new EntregarHandler(_pedidoRepositorioMock.Object, _publisherMock.Object);
            var command = new EntregarCommand(pedido.Id);

            await handler.Handle(command, CancellationToken.None);

            _pedidoRepositorioMock.Verify(r => r.AtualizarAsync(pedido), Times.Once);
            _publisherMock.Verify(p => p.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task Handle_ComPedidoForaDoStatusSaiuParaEntrega_DeveLancarExcecaoSemChamarOAtualizarNemPublicar()
        {
            var pedido = Pedido.Criar(Guid.NewGuid(), Guid.NewGuid()).Valor;
            _pedidoRepositorioMock.Setup(r => r.ObterPorIdAsync(pedido.Id)).ReturnsAsync(pedido);
            var handler = new EntregarHandler(_pedidoRepositorioMock.Object, _publisherMock.Object);
            var command = new EntregarCommand(pedido.Id);

            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, CancellationToken.None));

            _pedidoRepositorioMock.Verify(r => r.AtualizarAsync(It.IsAny<Pedido>()), Times.Never);
            _publisherMock.Verify(p => p.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ComPedidoNaoEncontrado_DeveLancarExcecaoSemChamarOAtualizarNemPublicar()
        {
            _pedidoRepositorioMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Pedido)null);
            var handler = new EntregarHandler(_pedidoRepositorioMock.Object, _publisherMock.Object);
            var command = new EntregarCommand(Guid.NewGuid());

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));

            _pedidoRepositorioMock.Verify(r => r.AtualizarAsync(It.IsAny<Pedido>()), Times.Never);
            _publisherMock.Verify(p => p.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
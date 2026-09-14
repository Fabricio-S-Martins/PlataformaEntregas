using MediatR;
using Modulos.Pedidos.Aplicacao.CasosDeUso.Cancelar;
using Modulos.Pedidos.Aplicacao.Repositorios;
using Modulos.Pedidos.Dominio.Entidades;
using Moq;

namespace Modulos.Pedidos.Aplicacao.Testes.CasosDeUso.Cancelar
{
    public class CancelarHandlerTestes
    {
        private readonly Mock<IPedidoRepositorio> _pedidoRepositorioMock;
        private readonly Mock<IPublisher> _publisherMock;

        public CancelarHandlerTestes()
        {
            _pedidoRepositorioMock = new Mock<IPedidoRepositorio>();
            _publisherMock = new Mock<IPublisher>();
        }

        private static Pedido CriarPedidoAceito()
        {
            var pedido = Pedido.Criar(Guid.NewGuid(), Guid.NewGuid()).Valor;
            pedido.AdicionarItem(Guid.NewGuid(), 2, 10);
            pedido.ConfirmarPagamento();
            pedido.Aceitar();
            return pedido;
        }

        [Fact]
        public async Task Handle_ComPedidoCriado_DevePassarPeloAtualizarDoRepositorioEPublicar()
        {
            var pedido = Pedido.Criar(Guid.NewGuid(), Guid.NewGuid()).Valor;
            _pedidoRepositorioMock.Setup(r => r.ObterPorIdAsync(pedido.Id)).ReturnsAsync(pedido);
            var handler = new CancelarHandler(_pedidoRepositorioMock.Object, _publisherMock.Object);
            var command = new CancelarCommand(pedido.Id);

            await handler.Handle(command, CancellationToken.None);

            _pedidoRepositorioMock.Verify(r => r.AtualizarAsync(pedido), Times.Once);
            _publisherMock.Verify(p => p.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task Handle_ComPedidoAceitoOuPosterior_DeveLancarExcecaoSemChamarOAtualizarNemPublicar()
        {
            var pedido = CriarPedidoAceito();
            _pedidoRepositorioMock.Setup(r => r.ObterPorIdAsync(pedido.Id)).ReturnsAsync(pedido);
            var handler = new CancelarHandler(_pedidoRepositorioMock.Object, _publisherMock.Object);
            var command = new CancelarCommand(pedido.Id);

            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, CancellationToken.None));

            _pedidoRepositorioMock.Verify(r => r.AtualizarAsync(It.IsAny<Pedido>()), Times.Never);
            _publisherMock.Verify(p => p.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ComPedidoNaoEncontrado_DeveLancarExcecaoSemChamarOAtualizarNemPublicar()
        {
            _pedidoRepositorioMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>())).ReturnsAsync((Pedido)null);
            var handler = new CancelarHandler(_pedidoRepositorioMock.Object, _publisherMock.Object);
            var command = new CancelarCommand(Guid.NewGuid());

            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(command, CancellationToken.None));

            _pedidoRepositorioMock.Verify(r => r.AtualizarAsync(It.IsAny<Pedido>()), Times.Never);
            _publisherMock.Verify(p => p.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
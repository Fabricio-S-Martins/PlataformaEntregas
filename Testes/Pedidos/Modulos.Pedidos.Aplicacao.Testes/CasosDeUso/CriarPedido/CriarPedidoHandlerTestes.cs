using MediatR;
using Modulos.Pedidos.Aplicacao.CasosDeUso.CriarPedido;
using Modulos.Pedidos.Aplicacao.Repositorios;
using Modulos.Pedidos.Dominio.Entidades;
using Moq;

namespace Modulos.Pedidos.Aplicacao.Testes.CasosDeUso.CriarPedido
{
    public class CriarPedidoHandlerTestes
    {
        private readonly Mock<IPedidoRepositorio> _pedidoRepositorioMock;
        private readonly Mock<IPublisher> _publisherMock;

        public CriarPedidoHandlerTestes()
        {
            _pedidoRepositorioMock = new Mock<IPedidoRepositorio>();
            _publisherMock = new Mock<IPublisher>();
        }

        [Fact]
        public async Task Handle_ComDadosValidos_DevePassarPeloAdicionarDoRepositorioEPublicar()
        {
            var handler = new CriarPedidoHandler(_pedidoRepositorioMock.Object, _publisherMock.Object);
            var command = new CriarPedidoCommand(Guid.NewGuid(), Guid.NewGuid());

            var id = await handler.Handle(command, CancellationToken.None);

            Assert.NotEqual(Guid.Empty, id);
            _pedidoRepositorioMock.Verify(r => r.AdicionarAsync(It.IsAny<Pedido>()), Times.Once);
            _publisherMock.Verify(p => p.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
        }

        [Fact]
        public async Task Handle_ComDadosInvalidos_DeveLancarExcecaoSemChamarOAdicionarDoRepositorioNemPublicar()
        {
            var handler = new CriarPedidoHandler(_pedidoRepositorioMock.Object, _publisherMock.Object);
            var command = new CriarPedidoCommand(Guid.Empty, Guid.Empty);

            await Assert.ThrowsAsync<ArgumentException>(() => handler.Handle(command, CancellationToken.None));

            _pedidoRepositorioMock.Verify(r => r.AdicionarAsync(It.IsAny<Pedido>()), Times.Never);
            _publisherMock.Verify(p => p.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
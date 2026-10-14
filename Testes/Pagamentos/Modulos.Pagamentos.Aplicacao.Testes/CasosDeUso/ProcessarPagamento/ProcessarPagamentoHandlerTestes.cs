using Compartilhado.Contratos.Pagamentos;
using Compartilhado.Contratos.Pedidos;
using MediatR;
using Microsoft.Extensions.Logging;
using Modulos.Pagamentos.Aplicacao.CasosDeUso.ProcessarPagamento;
using Modulos.Pagamentos.Aplicacao.Repositorios;
using Modulos.Pagamentos.Aplicacao.Servicos;
using Modulos.Pagamentos.Dominio.Entidades;
using Modulos.Pagamentos.Dominio.Enums;
using Moq;

namespace Modulos.Pagamentos.Aplicacao.Testes.CasosDeUso.ProcessarPagamento;

public class ProcessarPagamentoHandlerTestes
{
    private readonly Mock<IPagamentoRepositorio> _pagamentoRepositorioMock;
    private readonly Mock<IGatewayPagamentoServico> _gatewayMock;
    private readonly Mock<IPublisher> _publisherMock;
    private readonly Mock<ILogger<ProcessarPagamentoHandler>> _loggerMock;
    private readonly ProcessarPagamentoHandler _handler;
    private Pagamento _pagamentoGravado;

    public ProcessarPagamentoHandlerTestes()
    {
        _pagamentoRepositorioMock = new Mock<IPagamentoRepositorio>();
        _gatewayMock = new Mock<IGatewayPagamentoServico>();
        _publisherMock = new Mock<IPublisher>();
        _loggerMock = new Mock<ILogger<ProcessarPagamentoHandler>>();

        _pagamentoRepositorioMock.Setup(r => r.AdicionarAsync(It.IsAny<Pagamento>()))
                                 .Callback<Pagamento>(p => _pagamentoGravado = p)
                                 .Returns(Task.CompletedTask);

        _handler = new ProcessarPagamentoHandler(_pagamentoRepositorioMock.Object, _gatewayMock.Object,
                                                 _publisherMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ComGatewayQueAprova_DeveAprovarAtualizarEPublicarPagamentoAprovado()
    {
        var notificacao = new PedidoFechadoIntegracao(Guid.NewGuid(), 50m);
        _gatewayMock.Setup(g => g.CobrarAsync(It.IsAny<Guid>(), It.IsAny<decimal>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

        await _handler.Handle(notificacao, CancellationToken.None);

        Assert.Equal(StatusPagamento.Aprovado, _pagamentoGravado.Status);
        _pagamentoRepositorioMock.Verify(r => r.AtualizarAsync(_pagamentoGravado), Times.Once);
        _publisherMock.Verify(p => p.Publish(
            It.Is<PagamentoAprovadoIntegracao>(e => e.PedidoId == notificacao.PedidoId
                                                    && e.PagamentoId == _pagamentoGravado.Id
                                                    && e.Valor == 50m),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ComGatewayQueRecusa_DeveRecusarAtualizarSemPublicar()
    {
        var notificacao = new PedidoFechadoIntegracao(Guid.NewGuid(), 50m);
        _gatewayMock.Setup(g => g.CobrarAsync(It.IsAny<Guid>(), It.IsAny<decimal>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        await _handler.Handle(notificacao, CancellationToken.None);

        Assert.Equal(StatusPagamento.Recusado, _pagamentoGravado.Status);
        _pagamentoRepositorioMock.Verify(r => r.AtualizarAsync(_pagamentoGravado), Times.Once);
        _publisherMock.Verify(p => p.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ComGatewayQueLancaExcecao_DeveMarcarFalhaENaoPublicar()
    {
        var notificacao = new PedidoFechadoIntegracao(Guid.NewGuid(), 50m);
        _gatewayMock.Setup(g => g.CobrarAsync(It.IsAny<Guid>(), It.IsAny<decimal>(), It.IsAny<CancellationToken>()))
                    .ThrowsAsync(new InvalidOperationException("gateway fora do ar"));

        await _handler.Handle(notificacao, CancellationToken.None);

        Assert.Equal(StatusPagamento.Falhou, _pagamentoGravado.Status);
        _pagamentoRepositorioMock.Verify(r => r.AtualizarAsync(_pagamentoGravado), Times.Once);
        _publisherMock.Verify(p => p.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ComPedidoJaPagoAprovado_NaoDeveCriarCobrarNemPublicar()
    {
        var notificacao = new PedidoFechadoIntegracao(Guid.NewGuid(), 50m);
        _pagamentoRepositorioMock.Setup(r => r.ExisteAprovadoPorPedidoAsync(notificacao.PedidoId)).ReturnsAsync(true);

        await _handler.Handle(notificacao, CancellationToken.None);

        _pagamentoRepositorioMock.Verify(r => r.AdicionarAsync(It.IsAny<Pagamento>()), Times.Never);
        _gatewayMock.Verify(g => g.CobrarAsync(It.IsAny<Guid>(), It.IsAny<decimal>(), It.IsAny<CancellationToken>()), Times.Never);
        _publisherMock.Verify(p => p.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ComValorZero_DeveLancarArgumentExceptionSemGravarNemCobrar()
    {
        var notificacao = new PedidoFechadoIntegracao(Guid.NewGuid(), 0m);

        await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(notificacao, CancellationToken.None));

        _pagamentoRepositorioMock.Verify(r => r.AdicionarAsync(It.IsAny<Pagamento>()), Times.Never);
        _pagamentoRepositorioMock.Verify(r => r.AtualizarAsync(It.IsAny<Pagamento>()), Times.Never);
        _gatewayMock.Verify(g => g.CobrarAsync(It.IsAny<Guid>(), It.IsAny<decimal>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}

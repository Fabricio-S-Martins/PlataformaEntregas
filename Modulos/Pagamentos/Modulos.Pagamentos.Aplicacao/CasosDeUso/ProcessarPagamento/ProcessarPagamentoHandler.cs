using Compartilhado.Contratos.Pagamentos;
using Compartilhado.Contratos.Pedidos;
using Compartilhado.Dominio;
using MediatR;
using Microsoft.Extensions.Logging;
using Modulos.Pagamentos.Aplicacao.Repositorios;
using Modulos.Pagamentos.Aplicacao.Servicos;
using Modulos.Pagamentos.Dominio.Entidades;
using Modulos.Pagamentos.Dominio.Enums;

namespace Modulos.Pagamentos.Aplicacao.CasosDeUso.ProcessarPagamento;

public class ProcessarPagamentoHandler : INotificationHandler<PedidoFechadoIntegracao>
{
    private readonly IPagamentoRepositorio _pagamentoRepositorio;
    private readonly IGatewayPagamentoServico _gateway;
    private readonly IPublisher _publisher;
    private readonly ILogger<ProcessarPagamentoHandler> _logger;
    public ProcessarPagamentoHandler(IPagamentoRepositorio pagamentoRepositorio, IGatewayPagamentoServico gateway, IPublisher publisher, ILogger<ProcessarPagamentoHandler> logger)
    {
        _pagamentoRepositorio = pagamentoRepositorio;
        _gateway = gateway;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(PedidoFechadoIntegracao notification, CancellationToken cancellationToken)
    {
        if (await _pagamentoRepositorio.ExisteAprovadoPorPedidoAsync(notification.PedidoId))
            return;

        var resultado = Pagamento.Criar(notification.PedidoId, notification.Valor);
        if (!resultado.Sucesso)
            throw new ArgumentException(string.Join(Environment.NewLine, resultado.Erros));

        var pagamento = resultado.Valor;
        await _pagamentoRepositorio.AdicionarAsync(pagamento);

        Resultado<Pagamento> resultadoStatus;
        try
        {
            var aprovado = await _gateway.CobrarAsync(pagamento.Id, pagamento.Valor, cancellationToken);
            resultadoStatus = aprovado ? pagamento.Aprovar() : pagamento.Recusar();
        }
        catch (Exception ex)
        {
            resultadoStatus = pagamento.MarcarFalha();
            _logger.LogWarning("Falha ao cobrar o Pagamento {PagamentoId} do Pedido {PedidoId}: {Mensagem}",
                               pagamento.Id, notification.PedidoId, ex.Message);
        }

        if (!resultadoStatus.Sucesso)
            throw new ArgumentException(string.Join(Environment.NewLine, resultadoStatus.Erros));

        await _pagamentoRepositorio.AtualizarAsync(pagamento);

        if (pagamento.Status == StatusPagamento.Aprovado)
            await _publisher.Publish(new PagamentoAprovadoIntegracao(notification.PedidoId, pagamento.Id, pagamento.Valor),
                                     cancellationToken);
    }
}

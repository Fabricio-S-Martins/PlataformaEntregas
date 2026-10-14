namespace Modulos.Pagamentos.Aplicacao.Servicos;

public interface IGatewayPagamentoServico
{
    Task<bool> CobrarAsync(Guid pagamentoId, decimal valor, CancellationToken cancellationToken);
}

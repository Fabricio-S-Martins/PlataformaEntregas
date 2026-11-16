using Modulos.Pagamentos.Aplicacao.Servicos;
using Polly;

namespace Modulos.Pagamentos.Infraestrutura.Gateway;

public class GatewayPagamentoFalsoServico : IGatewayPagamentoServico
{
    public const int PercentualAprovadoPadrao = 70;
    public const int PercentualRecusadoPadrao = 20;
    public const int PercentualFalhaPadrao = 10;

    private readonly int _percentualAprovado;
    private readonly int _percentualRecusado;
    private readonly ResiliencePipeline<bool> _pipeline;

    public GatewayPagamentoFalsoServico(
        int percentualAprovado,
        int percentualRecusado,
        int percentualFalha,
        ResiliencePipeline<bool> pipeline)
    {
        if (!PercentualValido(percentualAprovado) || !PercentualValido(percentualRecusado) || !PercentualValido(percentualFalha))
            throw new ArgumentException("Cada percentual deve estar entre 0 e 100.");

        if (percentualAprovado + percentualRecusado + percentualFalha != 100)
            throw new ArgumentException("A soma dos percentuais deve ser 100.");

        _percentualAprovado = percentualAprovado;
        _percentualRecusado = percentualRecusado;
        _pipeline = pipeline;
    }

    public async Task<bool> CobrarAsync(Guid pagamentoId, decimal valor, CancellationToken cancellationToken)
    {
        return await _pipeline.ExecuteAsync(Sortear, cancellationToken);
    }

    private ValueTask<bool> Sortear(CancellationToken cancellationToken)
    {
        var sorteio = Random.Shared.Next(100);

        if (sorteio < _percentualAprovado)
            return ValueTask.FromResult(true);

        if (sorteio < _percentualAprovado + _percentualRecusado)
            return ValueTask.FromResult(false);

        throw new GatewayPagamentoIndisponivelException();
    }

    private static bool PercentualValido(int percentual) => percentual is >= 0 and <= 100;
}

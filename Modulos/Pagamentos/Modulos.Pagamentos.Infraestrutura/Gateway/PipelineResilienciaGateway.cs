using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Timeout;

namespace Modulos.Pagamentos.Infraestrutura.Gateway;

public static class PipelineResilienciaGateway
{
    private static readonly PredicateBuilder<bool> TratamentoDeFalha = new PredicateBuilder<bool>()
        .Handle<GatewayPagamentoIndisponivelException>()
        .Handle<TimeoutRejectedException>();

    public static ResiliencePipeline<bool> Criar(ILogger logger)
    {
        // Ordem de fora para dentro: retry → circuit breaker → timeout.
        // O timeout vale por tentativa e o circuit breaker enxerga cada tentativa.
        return new ResiliencePipelineBuilder<bool>()
            .AddRetry(CriarOpcoesRetry(logger))
            .AddCircuitBreaker(CriarOpcoesCircuitBreaker(logger))
            .AddTimeout(TimeSpan.FromSeconds(2))
            .Build();
    }

    private static RetryStrategyOptions<bool> CriarOpcoesRetry(ILogger logger)
    {
        return new RetryStrategyOptions<bool>
        {
            ShouldHandle = TratamentoDeFalha,
            MaxRetryAttempts = 3,
            Delay = TimeSpan.FromMilliseconds(200),
            BackoffType = DelayBackoffType.Exponential,
            OnRetry = argumentos =>
            {
                logger.LogWarning(
                    argumentos.Outcome.Exception,
                    "Gateway de pagamento falhou. Nova tentativa {Tentativa} após {Espera} ms.",
                    argumentos.AttemptNumber + 1,
                    argumentos.RetryDelay.TotalMilliseconds);
                return default;
            }
        };
    }

    private static CircuitBreakerStrategyOptions<bool> CriarOpcoesCircuitBreaker(ILogger logger)
    {
        return new CircuitBreakerStrategyOptions<bool>
        {
            ShouldHandle = TratamentoDeFalha,
            FailureRatio = 0.5,
            MinimumThroughput = 5,
            BreakDuration = TimeSpan.FromSeconds(30),
            OnOpened = argumentos =>
            {
                logger.LogWarning(
                    argumentos.Outcome.Exception,
                    "Circuito do gateway de pagamento aberto por {Duracao} s.",
                    argumentos.BreakDuration.TotalSeconds);
                return default;
            },
            OnClosed = _ =>
            {
                logger.LogInformation("Circuito do gateway de pagamento fechado.");
                return default;
            }
        };
    }
}

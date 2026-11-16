using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Modulos.Pagamentos.Aplicacao.Repositorios;
using Modulos.Pagamentos.Aplicacao.Servicos;
using Modulos.Pagamentos.Infraestrutura.Gateway;
using Modulos.Pagamentos.Infraestrutura.Persistencia;
using Modulos.Pagamentos.Infraestrutura.Persistencia.Repositorios;
using Polly;

namespace Modulos.Pagamentos.Infraestrutura;

public static class InjecaoDeDependencia
{
    public static void RegistrarPagamentosInfraestrutura(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PagamentosDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("BasePlataformaEntregas")));

        services.AddScoped<IPagamentoRepositorio, PagamentoRepositorio>();

        // Singleton: o estado do circuit breaker vive no pipeline; se fosse recriado a cada requisição, o circuito nunca abriria.
        services.AddSingleton(provider => PipelineResilienciaGateway.Criar(
            provider.GetRequiredService<ILoggerFactory>().CreateLogger<GatewayPagamentoFalsoServico>()));

        services.AddScoped<IGatewayPagamentoServico>(provider => new GatewayPagamentoFalsoServico(
            GatewayPagamentoFalsoServico.PercentualAprovadoPadrao,
            GatewayPagamentoFalsoServico.PercentualRecusadoPadrao,
            GatewayPagamentoFalsoServico.PercentualFalhaPadrao,
            provider.GetRequiredService<ResiliencePipeline<bool>>()));
    }
}

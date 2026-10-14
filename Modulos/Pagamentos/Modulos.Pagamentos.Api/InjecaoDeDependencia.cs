using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modulos.Pagamentos.Aplicacao;
using Modulos.Pagamentos.Infraestrutura;

namespace Modulos.Pagamentos.Api;

public static class InjecaoDeDependencia
{
    public static void RegistrarPagamentosApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegistrarPagamentosInfraestrutura(configuration);
        services.RegistrarPagamentosAplicacao();
    }
}

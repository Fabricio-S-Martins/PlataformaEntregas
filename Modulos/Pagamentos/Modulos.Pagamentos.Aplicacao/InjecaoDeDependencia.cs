using Microsoft.Extensions.DependencyInjection;

namespace Modulos.Pagamentos.Aplicacao;

public static class InjecaoDeDependencia
{
    public static void RegistrarPagamentosAplicacao(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(InjecaoDeDependencia).Assembly));
    }
}

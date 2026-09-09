using Microsoft.Extensions.DependencyInjection;

namespace Modulos.Catalogo.Aplicacao
{
    public static class InjecaoDeDependencia
    {
        public static void RegistrarCatalogoAplicacao(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(InjecaoDeDependencia).Assembly));
        }
    }
}
using Microsoft.Extensions.DependencyInjection;

namespace Modulos.Pedidos.Aplicacao
{
    public static class InjecaoDeDependencia
    {
        public static void RegistrarPedidosAplicacao(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(InjecaoDeDependencia).Assembly));
        }
    }
}
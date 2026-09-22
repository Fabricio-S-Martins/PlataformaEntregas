using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modulos.Pedidos.Aplicacao;
using Modulos.Pedidos.Infraestrutura;

namespace Modulos.Pedidos.Api
{
    public static class InjecaoDeDependencia
    {
        public static void RegistrarPedidosApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegistrarPedidosInfraestrutura(configuration);
            services.RegistrarPedidosAplicacao();
        }
    }
}
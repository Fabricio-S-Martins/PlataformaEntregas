using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modulos.Pedidos.Aplicacao.Repositorios;
using Modulos.Pedidos.Infraestrutura.Persistencia;
using Modulos.Pedidos.Infraestrutura.Persistencia.Repositorios;

namespace Modulos.Pedidos.Infraestrutura
{
    public static class InjecaoDeDependencia
    {
        public static void RegistrarPedidosInfraestrutura(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PedidosDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("BasePlataformaEntregas")));

            services.AddScoped<IPedidoRepositorio, PedidoRepositorio>();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modulos.Catalogo.Aplicacao.Repositorios;
using Modulos.Catalogo.Infraestrutura.Persistencia;
using Modulos.Catalogo.Infraestrutura.Persistencia.Repositorios;

namespace Modulos.Catalogo.Infraestrutura
{
    public static class InjecaoDeDependencia
    {
        public static void RegistrarCatalogoInfraestrutura(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CatalogoDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("BasePlataformaEntregas")));
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("RedisPlataformaEntregas");
            });

            services.AddScoped<IRestauranteRepositorio, RestauranteRepositorio>();
        }
    }
}
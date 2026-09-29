using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modulos.Catalogo.Aplicacao.Repositorios;
using Modulos.Catalogo.Aplicacao.Servicos;
using Modulos.Catalogo.Infraestrutura.Cache;
using Modulos.Catalogo.Infraestrutura.Persistencia;
using Modulos.Catalogo.Infraestrutura.Persistencia.Repositorios;
using StackExchange.Redis;

namespace Modulos.Catalogo.Infraestrutura
{
    public static class InjecaoDeDependencia
    {
        public static void RegistrarCatalogoInfraestrutura(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CatalogoDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("BasePlataformaEntregas")));

            var connectionStringRedis = configuration.GetConnectionString("RedisPlataformaEntregas");
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connectionStringRedis;
            });
            ConnectionMultiplexer cm = ConnectionMultiplexer.Connect(connectionStringRedis);
            services.AddSingleton<IConnectionMultiplexer>(cm);

            services.AddScoped<ITravaDistribuidaServico, TravaDistribuidaServico>();

            services.AddScoped<IRestauranteRepositorio, RestauranteRepositorio>();
        }
    }
}
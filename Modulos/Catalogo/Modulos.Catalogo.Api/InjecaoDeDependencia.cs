using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modulos.Catalogo.Infraestrutura;
using Modulos.Catalogo.Aplicacao;

namespace Modulos.Catalogo.Api
{
    public static class InjecaoDeDependencia
    {
        public static void RegistrarCatalogoApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegistrarCatalogoInfraestrutura(configuration);
            services.RegistrarCatalogoAplicacao();
        }
    }
}
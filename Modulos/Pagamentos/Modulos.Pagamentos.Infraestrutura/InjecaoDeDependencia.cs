using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modulos.Pagamentos.Aplicacao.Repositorios;
using Modulos.Pagamentos.Infraestrutura.Persistencia;
using Modulos.Pagamentos.Infraestrutura.Persistencia.Repositorios;

namespace Modulos.Pagamentos.Infraestrutura;

public static class InjecaoDeDependencia
{
    public static void RegistrarPagamentosInfraestrutura(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PagamentosDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("BasePlataformaEntregas")));

        services.AddScoped<IPagamentoRepositorio, PagamentoRepositorio>();
    }
}

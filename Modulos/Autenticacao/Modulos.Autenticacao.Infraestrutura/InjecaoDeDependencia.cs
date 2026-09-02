using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modulos.Autenticacao.Aplicacao.Repositorios;
using Modulos.Autenticacao.Aplicacao.Servicos;
using Modulos.Autenticacao.Infraestrutura.Persistencia;
using Modulos.Autenticacao.Infraestrutura.Persistencia.Repositorios;
using Modulos.Autenticacao.Infraestrutura.Servicos;

namespace Modulos.Autenticacao.Infraestrutura;

public static class InjecaoDeDependencia
{
    public static void RegistrarAutenticacaoInfraestrutura(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AutenticacaoDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("BasePlataformaEntregas")));
        services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
        services.AddScoped<ITokenServico, TokenServico>();
    }
}
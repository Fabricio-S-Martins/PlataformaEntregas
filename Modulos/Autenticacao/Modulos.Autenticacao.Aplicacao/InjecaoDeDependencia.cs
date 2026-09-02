using Microsoft.Extensions.DependencyInjection;
using Modulos.Autenticacao.Aplicacao.Servicos;

namespace Modulos.Autenticacao.Aplicacao;

public static class InjecaoDeDependencia
{
    public static void RegistrarAutenticacaoAplicacao(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(InjecaoDeDependencia).Assembly));

        services.AddScoped<ISenhaServico, SenhaServico>();
    }
}
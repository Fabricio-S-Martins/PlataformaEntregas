using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Modulos.Autenticacao.Aplicacao;
using Modulos.Autenticacao.Infraestrutura;
using System.Text;

namespace Modulos.Autenticacao.Api
{
    public static class InjecaoDeDependencia
    {
        public static void RegistrarAutenticacaoApi(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegistrarAutenticacaoInfraestrutura(configuration);
            services.RegistrarAutenticacaoAplicacao();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = configuration.GetValue<string>("APIConfiguracoes:Issuer"),
                            ValidAudience = configuration.GetValue<string>("APIConfiguracoes:Audience"),
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("APIConfiguracoes:KeyJWT")))
                        };
                        options.MapInboundClaims = false;
                    });

            services.AddAuthorization();
        }
    }
}
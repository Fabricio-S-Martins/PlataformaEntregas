using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Modulos.Autenticacao.Api.Endpoints.CriarUsuario;
using Modulos.Autenticacao.Api.Endpoints.Login;
using Modulos.Autenticacao.Api.Endpoints.ObterUsuarioAutenticado;
using Modulos.Autenticacao.Aplicacao;
using Modulos.Autenticacao.Infraestrutura;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegistrarAutenticacaoInfraestrutura(builder.Configuration);
builder.Services.RegistrarAutenticacaoAplicacao();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("bearer", document)] = []
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration.GetValue<string>("APIConfiguracoes:Issuer"),
                        ValidAudience = builder.Configuration.GetValue<string>("APIConfiguracoes:Audience"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("APIConfiguracoes:KeyJWT")))
                    };
                    options.MapInboundClaims = false;
                });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

var api = app.MapGroup("/api");
api.MapUsuariosEndpoints();
api.MapLoginEndPoint();
api.MapObterUsuarioAutenticadoEndpoint();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
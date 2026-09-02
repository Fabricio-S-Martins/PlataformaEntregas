using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Modulos.Autenticacao.Api.Endpoints.CriarUsuario;
using Modulos.Autenticacao.Api.Endpoints.Login;
using Modulos.Autenticacao.Aplicacao;
using Modulos.Autenticacao.Infraestrutura;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegistrarAutenticacaoInfraestrutura(builder.Configuration);
builder.Services.RegistrarAutenticacaoAplicacao();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var api = app.MapGroup("/api");
api.MapUsuariosEndpoints();
api.MapLoginEndPoint();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
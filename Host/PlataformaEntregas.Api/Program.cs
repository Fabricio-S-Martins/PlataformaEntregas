using Microsoft.OpenApi;
using Modulos.Autenticacao.Api;
using Modulos.Catalogo.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegistrarAutenticacaoApi(builder.Configuration);
builder.Services.RegistrarCatalogoApi(builder.Configuration);
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

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

var api = app.MapGroup("/api");
api.MapAutenticacaoEndpoints();
api.MapCatalogoEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
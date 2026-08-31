using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modulos.Autenticacao.Aplicacao.CasosDeUso.CriarUsuario;
using System;
using System.Threading;

namespace Modulos.Autenticacao.Api.Endpoints.CriarUsuario;

public static class CriarUsuarioEndpoint
{
    public static void MapUsuariosEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("/usuarios", async (CriarUsuarioRequest request, ISender mediator, CancellationToken cancellationToken) =>
        {
            try
            {
                await mediator.Send(new CriarUsuarioCommand(request.Nome, request.Email, request.Senha, request.Papel), cancellationToken);
                return Results.Created();
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { erro = ex.Message });
            }
        })
        .WithName("CriarUsuario")
        .Produces(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
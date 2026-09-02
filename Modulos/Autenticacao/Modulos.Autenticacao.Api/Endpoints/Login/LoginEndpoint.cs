using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Modulos.Autenticacao.Aplicacao.CasosDeUso.Login;
using System;
using System.Threading;

namespace Modulos.Autenticacao.Api.Endpoints.Login
{
    public static class LoginEndpoint
    {
        public static void MapLoginEndPoint(this IEndpointRouteBuilder routes)
        {
            routes.MapPost("/login", async (LoginRequest loginRequest, ISender mediator, CancellationToken cancellationToken) =>
            {
                try
                {
                    var token = await mediator.Send(new LoginCommand(loginRequest.Email, loginRequest.Senha), cancellationToken);
                    return Results.Ok(new { token });
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { erro = ex.Message });
                }
            })
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest);
        }
    }
}
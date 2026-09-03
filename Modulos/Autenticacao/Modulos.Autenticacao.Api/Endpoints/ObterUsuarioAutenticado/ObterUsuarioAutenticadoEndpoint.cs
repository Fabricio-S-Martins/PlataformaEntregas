using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Modulos.Autenticacao.Api.Endpoints.ObterUsuarioAutenticado
{
    public static class ObterUsuarioAutenticadoEndpoint
    {
        public static void MapObterUsuarioAutenticadoEndpoint(this IEndpointRouteBuilder routes)
        {
            routes.MapGet("/usuarios/autenticado", (HttpContext httpContext) =>
            {
                var user = httpContext.User;

                var id = user.FindFirstValue(JwtRegisteredClaimNames.Sub);
                var papel = user.FindFirstValue(ClaimTypes.Role);

                return Results.Ok(new UsuarioAutenticadoResponse(Guid.Parse(id), papel));
            })
            .RequireAuthorization()
            .Produces(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized);
        }
    }
}
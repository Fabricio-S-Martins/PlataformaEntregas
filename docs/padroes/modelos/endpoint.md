# Endpoint

## Esqueleto
```csharp
namespace Modulos.<Modulo>.Api.Endpoints.<Caso>;

public static class <Caso>Endpoint
{
    public static void Map<Caso>Endpoint(this IEndpointRouteBuilder routes)
    {
        routes.Map<Verbo>("/<rota>", async (<parametros>, ISender sender, CancellationToken cancellationToken) =>
        {
            try
            {
                await sender.Send(new <Caso>Command(<dados>), cancellationToken);
                return Results.<Sucesso>();
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { erro = ex.Message });
            }
        })
        .Produces(StatusCodes.Status<2xx>)
        .ProducesProblem(StatusCodes.Status400BadRequest);
    }
}
```
- Verbo, rota, parâmetros (`Guid id`, `<Caso>Request`), dados do comando e status de sucesso variam.
- Um `catch` por exceção que o handler lança: `InvalidOperationException` vira `NotFound` (+ `.ProducesProblem` 404) quando o recurso pode não existir.
- Com retorno do handler: `var <x> = await sender.Send(...)` e o resultado vai no `Results.<Sucesso>`.
- Consulta (Query) pode dispensar o `try/catch`.

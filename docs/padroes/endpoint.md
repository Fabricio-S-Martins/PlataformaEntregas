# Endpoint

**Modelo base:** o alvo pode ter particularidades. | **Origem:** AceitarEndpoint, CriarPedidoEndpoint, LoginEndpoint, ObterRestauranteEndpoint

## Esqueleto
```csharp
static class <Caso>Endpoint {
  static void Map<Caso>Endpoint(this IEndpointRouteBuilder routes) {
    routes.Map<Verbo>("/<rota>", async (<Request | Guid id>, ISender sender, CancellationToken ct) => {
      try {                                    // (se houver)
        [var x =] await sender.Send(new <Caso>Command(...), ct);
        return Results.<NoContent | Ok | Created>(...);
      }
      catch (ArgumentException ex) { return Results.BadRequest(new { erro = ex.Message }); }
      catch (InvalidOperationException ex) { return Results.NotFound(...); }   // (se houver)
    }).Produces(<sucesso>).ProducesProblem(<erro>);   // um por status
  }
}
```

## Variações
- **ObterRestauranteEndpoint:** Query sem `try/catch`; 404 se o resultado for nulo.

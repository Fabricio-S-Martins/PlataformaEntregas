# Handler

**Modelo base:** o alvo pode ter particularidades. | **Origem:** AceitarHandler, AdicionarItemHandler, CriarPedidoHandler, CriarUsuarioHandler, CadastrarRestauranteHandler

## Esqueleto
```csharp
class <Caso>Handler : IRequestHandler<<Caso>Command[, <Tipo>]> {
  private readonly I<Repositorio> _repositorio;   // + demais deps
  ctor(deps) { atribui campos }

  async Task[<Tipo>] Handle(<Caso>Command request, CancellationToken ct) {
    var <entidade> = await _repositorio.ObterPorIdAsync(...);   // (se houver)
    if (<entidade> == null) throw ...;                          // (se houver)
    var resultado = <Entidade>.<Metodo>(...);                   // Criar ou transição
    if (!resultado.Sucesso) throw new ArgumentException(...);
    await _repositorio.AdicionarAsync | AtualizarAsync(...);
    await DespachanteDeEventosDominio.DespacharAsync(...);      // (se houver)
    return ...;                                                 // (se houver)
  }
}
```

## Variações
- **CadastrarRestauranteHandler:** grava no `IDistributedCache` após persistir.

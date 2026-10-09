# Entidade de domínio

**Modelo base:** o alvo pode ter particularidades. | **Origem:** Usuario, Restaurante, Pedido, Pagamento (lidos); ItemPedido, Cardapio, ItemCardapio (Grep).

## Esqueleto
```csharp
class <E> {
  private <E>() {}                                   // EF Core
  private <E>(dados) { Id = Guid.NewGuid(); atribuições }

  Guid Id { get; }  demais props { get; }            // Status: private set (se houver)

  static Resultado<<E>> Criar(dados) {
    erros = new List<string>()                       // "<Campo> inválido."
    valida primitivos; VO: new <VO>(x) → .Valido
    if (erros.Count > 0) return ComFalha(erros)
    return ComSucesso(new <E>(dados))
  }
}
```

## Variações
- **Pedido, Pagamento:** `Status` + transições `Resultado<<E>> <Ação>()`.
- **Pedido:** eventos e coleção filha.

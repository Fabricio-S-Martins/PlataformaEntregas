# Entidade de domínio

## Esqueleto
```csharp
namespace Modulos.<Modulo>.Dominio.Entidades;

public class <E>
{
    private <E>(){}
    private <E>(<dados>)
    {
        Id = Guid.NewGuid();
        <atribuições>
    }

    public Guid Id { get; }
    public <Tipo> <Prop> { get; }

    public static Resultado<<E>> Criar(<dados>)
    {
        var erros = new List<string>();
        if (<inválido>)
            erros.Add("<Campo> inválido.");

        if (erros.Count > 0)
            return Resultado<<E>>.ComFalha(erros);

        return Resultado<<E>>.ComSucesso(new <E>(<dados>));
    }
}
```
- Dados, propriedades e validações variam; um `if` + `erros.Add` por regra.
- Com estado: `Status { get; private set; }` e transições `public Resultado<<E>> <Ação>()`, que falham se o status atual não permite, senão atribuem o novo `Status` e retornam `ComSucesso(this)`.
- Coleção filha: `IReadOnlyList<<F>> <Fs> => <Fs>Interno;` + `private List<<F>> <Fs>Interno { get; } = [];`; o método de adição cria o filho por `<F>.Criar(...)`, propaga `Erros` se falhar e retorna `Resultado<<F>>`.
- Eventos de domínio: `IReadOnlyList<IEventoDominio> Eventos => EventosInterno;`, com `EventosInterno.Add(new <Evento>(Id))` no construtor ou na transição.
- Value object: validado na `Criar` (`var <vo> = new <VO>(<dado>);` + `if (!<vo>.Valido)`); o construtor recebe o VO.

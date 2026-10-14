# Handler

## Esqueleto
```csharp
namespace Modulos.<Modulo>.Aplicacao.CasosDeUso.<Caso>;

public class <Caso>Handler : IRequestHandler<<Caso>Command>
{
    private readonly I<E>Repositorio _<e>Repositorio;
    private readonly IPublisher _publisher;
    public <Caso>Handler(I<E>Repositorio <e>Repositorio, IPublisher publisher)
    {
        _<e>Repositorio = <e>Repositorio;
        _publisher = publisher;
    }

    public async Task Handle(<Caso>Command request, CancellationToken cancellationToken)
    {
        var <e> = await _<e>Repositorio.ObterPorIdAsync(request.<E>Id);
        if (<e> == null)
            throw new InvalidOperationException("<E> não encontrado.");

        var resultado<E> = <e>.<Metodo>();
        if (!resultado<E>.Sucesso)
            throw new ArgumentException(string.Join(Environment.NewLine, resultado<E>.Erros));

        await _<e>Repositorio.AtualizarAsync(<e>);
        await DespachanteDeEventosDominio.DespacharAsync(<e>.Eventos, _publisher, cancellationToken);
    }
}
```
- Entidade, método de domínio e dependências do construtor variam; dependência extra (cache, serviço, logger) vira campo e parâmetro.
- Criar: sem `ObterPorIdAsync` nem checagem de nulo; a entidade vem de `<E>.Criar(...)` e é gravada com `AdicionarAsync`. Com retorno: `IRequestHandler<<Caso>Command, Guid>`, `Task<Guid>` e `return resultado<E>.Valor.Id;`.
- Evento de integração: `INotificationHandler<<Evento>Integracao>`, sem retorno.

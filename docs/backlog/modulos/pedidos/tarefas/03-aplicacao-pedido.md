---
tags: [backlog, modulo/pedidos, fluxo/pedido-operacao, fluxo/pedido-status, tarefa/concluido]
---

# 03 — Criar casos de uso do Pedido e publicar os eventos de domínio

**Módulo:** Pedidos
**Camada:** Aplicação

## Contexto

Primeira peça da camada de Aplicação do módulo Pedidos. Segue o mesmo padrão CQRS com MediatR já usado em Autenticação e Catálogo: um Command por intenção, um Handler que aciona o agregado `Pedido` (Domínio) e persiste via o Repository Pattern, abstraído pela interface `IPedidoRepositorio`.

A peça nova aqui é fechar o **Domain Events pattern** começado na task 01: até agora `Pedido` só registra os eventos numa lista interna, sem publicar nada. Esta task publica esses eventos via MediatR `INotification`, depois de cada persistência — sem o Domínio depender de MediatR (ele continua só com a interface `IEventoDominio`, framework-free). A Aplicação faz a ponte: pra cada tipo de evento de domínio existe uma notificação MediatR correspondente, e um despachante converte um pro outro.

## Conceitos novos

### Publicar eventos de domínio via MediatR

Depois que um Handler persiste a mudança no agregado, ele passa a lista `Pedido.Eventos` pra um despachante, que publica uma notificação MediatR (`IPublisher.Publish`) equivalente a cada evento de domínio registrado. Handlers de notificação (`INotificationHandler<T>`) que reagirem a cada uma ficam pra tasks futuras (Pagamentos, Notificações) — aqui só o mecanismo de publicação é criado.

Referência: https://github.com/jbogard/MediatR#publish-notifications

## O que fazer

1. Na pasta `Modulos/Pedidos/`, criar a pasta `Modulos.Pedidos.Aplicacao/`.
2. Dentro dessa pasta, criar o projeto `Modulos.Pedidos.Aplicacao`.

### Repositório

3. Dentro desse projeto, criar a pasta `Repositorios/`. Dentro dela, criar a interface `IPedidoRepositorio`, com os métodos: `AdicionarAsync(Pedido pedido)`, `AtualizarAsync(Pedido pedido)` e `ObterPorIdAsync(Guid id)` (retornando `Task<Pedido>`).

### Notificações MediatR

4. Dentro do projeto, criar a pasta `Notificacoes/`. Dentro dela, criar, nesta ordem, os records que espelham cada evento de domínio, todos implementando `INotification` do MediatR e todos só com a prop `PedidoId` (`Guid`):
`PedidoCriadoNotificacao`, `PedidoItemAdicionadoNotificacao`, `PedidoPagoNotificacao`, `PedidoAceitoNotificacao`, `PedidoEmPreparoNotificacao`, `PedidoSaiuParaEntregaNotificacao`, `PedidoEntregueNotificacao`, `PedidoCanceladoNotificacao`.

5. Na raiz do projeto, criar a classe estática `DespachanteDeEventosDominio`, com o método estático assíncrono `DespacharAsync`, recebendo `eventos` (`IReadOnlyList<IEventoDominio>`), `publisher` (`IPublisher` do MediatR) e `cancellationToken`. Dentro dele, percorrer `eventos` e, pra cada um, usar um `switch` de pattern matching sobre o tipo concreto do evento pra montar a notificação correspondente (mesmo `PedidoId`) e chamar `publisher.Publish`, nesta correspondência exata: `PedidoCriadoEvento` → `PedidoCriadoNotificacao` e assim sucessivamente.

### Casos de uso

6. Dentro do projeto, criar a pasta `CasosDeUso/CriarPedido/`. Dentro dela, criar, nesta ordem: o record `CriarPedidoCommand`, com `ClienteId` (`Guid`) e `RestauranteId` (`Guid`), implementando `IRequest<Guid>`; a classe `CriarPedidoHandler`, implementando `IRequestHandler<CriarPedidoCommand, Guid>` — chama `Pedido.Criar`; se falha, lança `ArgumentException` com os erros; se sucesso, chama `IPedidoRepositorio.AdicionarAsync`, despacha os eventos do pedido criado, e devolve o `Id` gerado.

7. Dentro do projeto, criar a pasta `CasosDeUso/AdicionarItem/`. Dentro dela, criar, nesta ordem: o record `AdicionarItemCommand`, com `PedidoId` (`Guid`), `ItemCardapioId` (`Guid`), `Quantidade` (`int`) e `PrecoUnitario` (`decimal`), implementando `IRequest`; a classe `AdicionarItemHandler`, implementando `IRequestHandler<AdicionarItemCommand>` — busca o pedido via `ObterPorIdAsync`; se não encontrar, lança `InvalidOperationException`; chama `Pedido.AdicionarItem`; se falha, lança `ArgumentException` com os erros; se sucesso, chama `IPedidoRepositorio.AtualizarAsync` e despacha os eventos.

8. Criar seis pastas em `CasosDeUso/`, uma por transição, cada uma com um Command (só com `PedidoId` (`Guid`), implementando `IRequest`) e um Handler (implementando `IRequestHandler<TCommand>`) seguindo exatamente o mesmo formato do passo 7 — busca o pedido, chama o método de transição correspondente do agregado, lança `ArgumentException` em caso de falha, senão atualiza e despacha os eventos:
    - `CasosDeUso/ConfirmarPagamento/`: `ConfirmarPagamentoCommand`/`ConfirmarPagamentoHandler`, chamando `Pedido.ConfirmarPagamento`.
    - `CasosDeUso/Aceitar/`: `AceitarCommand`/`AceitarHandler`, chamando `Pedido.Aceitar`.
    - `CasosDeUso/IniciarPreparo/`: `IniciarPreparoCommand`/`IniciarPreparoHandler`, chamando `Pedido.IniciarPreparo`.
    - `CasosDeUso/SairParaEntrega/`: `SairParaEntregaCommand`/`SairParaEntregaHandler`, chamando `Pedido.SairParaEntrega`.
    - `CasosDeUso/Entregar/`: `EntregarCommand`/`EntregarHandler`, chamando `Pedido.Entregar`.
    - `CasosDeUso/Cancelar/`: `CancelarCommand`/`CancelarHandler`, chamando `Pedido.Cancelar`.

### Registro de DI

9. Na raiz do projeto, criar a classe estática `InjecaoDeDependencia`, com o método de extensão `RegistrarPedidosAplicacao(this IServiceCollection services)`, registrando o MediatR para o assembly deste projeto.

## Notas / decisões tomadas

- `IPedidoRepositorio.AtualizarAsync` é explícito (em vez de depender de change-tracking implícito do EF Core) — mais claro sobre quando a persistência acontece, mesmo padrão didático já adotado no resto do projeto.
- Pedido não encontrado (`ObterPorIdAsync` retornando `null`) lança `InvalidOperationException` — diferente de `ArgumentException` (reservada pra falha de validação/invariante do Domínio). A API (task futura) mapeia isso pra `404`.
- Handlers de notificação reagindo a cada evento (`INotificationHandler<T>`) ficam fora de escopo — entram quando Pagamentos/Notificações existirem.
- Testes automatizados desta camada ficam para uma task separada, mesmo padrão adotado em Autenticação e Catálogo.

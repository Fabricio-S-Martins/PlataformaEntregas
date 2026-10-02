---
tags: [backlog, modulo/pedidos, fluxo/pedido-operacao, fluxo/pedido-status, tarefa/concluido]
---

# 04 — Cobrir os casos de uso do Pedido com testes automatizados

**Módulo:** Pedidos
**Camada:** Aplicação

## Contexto

A task 03 criou os 8 casos de uso do `Pedido` (criação, adicionar item, e as 6 transições de status), todos dependendo de `IPedidoRepositorio` e do `IPublisher` do MediatR. Como ainda não existe implementação real do repositório (isso é Infraestrutura, task futura), os Handlers só podem ser testados com mocks das duas dependências, usando **Moq** — mesma técnica já usada em `CadastrarRestauranteHandlerTestes`.

## O que fazer

1. Na pasta `Testes/Pedidos/`, criar a pasta `Modulos.Pedidos.Aplicacao.Testes/`.
2. Dentro dessa pasta, criar o projeto `Modulos.Pedidos.Aplicacao.Testes`.
3. Dentro desse projeto, criar a pasta `CasosDeUso/`.

### CriarPedidoHandler

4. Dentro de `CasosDeUso/`, criar a pasta `CriarPedido/`. Dentro dela, criar a classe de teste `CriarPedidoHandlerTestes`, usando um mock de `IPedidoRepositorio` e um mock de `IPublisher`.

### AdicionarItemHandler

5. Dentro de `CasosDeUso/`, criar a pasta `AdicionarItem/`. Dentro dela, criar a classe de teste `AdicionarItemHandlerTestes`, usando um mock de `IPedidoRepositorio` e um mock de `IPublisher`.

### Handlers de transição

6. Criar seis pastas em `CasosDeUso/`, uma por transição, cada uma com uma classe de teste, seguindo exatamente o mesmo formato do passo 5 — mock de `IPedidoRepositorio` e mock de `IPublisher`:
    - `CasosDeUso/ConfirmarPagamento/ConfirmarPagamentoHandlerTestes`.
    - `CasosDeUso/Aceitar/AceitarHandlerTestes`.
    - `CasosDeUso/IniciarPreparo/IniciarPreparoHandlerTestes`.
    - `CasosDeUso/SairParaEntrega/SairParaEntregaHandlerTestes`.
    - `CasosDeUso/Entregar/EntregarHandlerTestes`.
    - `CasosDeUso/Cancelar/CancelarHandlerTestes`.

## Cenários a cobrir

**`CriarPedidoHandlerTestes` (`CriarPedidoHandler`):**
- Dados válidos chama `IPedidoRepositorio.AdicionarAsync` exatamente uma vez, chama `IPublisher.Publish` pelo menos uma vez, e devolve o `Id` do pedido criado.
- Dados inválidos lança `ArgumentException`, sem chamar `AdicionarAsync` nem `Publish`.

**`AdicionarItemHandlerTestes` (`AdicionarItemHandler`):**
- Pedido encontrado e dados válidos chama `IPedidoRepositorio.AtualizarAsync` exatamente uma vez, e chama `IPublisher.Publish` pelo menos uma vez.
- Pedido encontrado e dados inválidos lança `ArgumentException`, sem chamar `AtualizarAsync` nem `Publish`.
- Pedido não encontrado (`ObterPorIdAsync` devolvendo `null`) lança `InvalidOperationException`, sem chamar `AtualizarAsync` nem `Publish`.

**Handlers de transição (`ConfirmarPagamentoHandlerTestes`, `AceitarHandlerTestes`, `IniciarPreparoHandlerTestes`, `SairParaEntregaHandlerTestes`, `EntregarHandlerTestes`, `CancelarHandlerTestes`):** pra cada um, cobrir os mesmos três cenários do `AdicionarItemHandlerTestes` — pedido no status de origem correto chama `AtualizarAsync` e `Publish`; pedido em status que não permite a transição lança `ArgumentException` sem chamar nenhum dos dois; pedido não encontrado lança `InvalidOperationException` sem chamar nenhum dos dois. Pra `ConfirmarPagamentoHandlerTestes`, cobrir também: pedido em `Criado` mas sem nenhum item lança `ArgumentException`.

## Notas / decisões tomadas

- Nomenclatura dos métodos de teste segue o padrão já fixado: `Metodo_ComCenario_DeveResultado`.
- `Pedido` de cada teste é construído direto via `Pedido.Criar`/`AdicionarItem`/os métodos de transição (Domínio já testado na task 02) — não via mock, já que é uma classe concreta, não uma interface.

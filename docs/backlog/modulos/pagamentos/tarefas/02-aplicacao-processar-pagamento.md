---
tags: [backlog/tarefa, modulo/pagamentos, status/pendente]
---

# 02 — Processar o pagamento ao receber o Pedido fechado, publicar a aprovação e cobrir com testes automatizados

**Módulo:** Pagamentos | **Camada:** Aplicação | **Deps:** Compartilhado 03, Pagamentos 01

## O que fazer

- **Padrão:** consumidor de Integration Event; o gateway fica atrás de uma interface (Ports and Adapters), com o adaptador na Infraestrutura.
- **Duplicidade:** se já existe `Pagamento` `Aprovado` para o Pedido, o handler não faz nada.
- **Resultado da cobrança:** `true` do gateway aprova, `false` recusa, exceção marca `Falhou`. Recusa e falha são resultados, não exceções: o handler nunca lança por elas, senão o `fechar` do cliente responderia `500`.
- **Publicação:** só o `Pagamento` aprovado publica `PagamentoAprovadoIntegracao`.
- **Execução:** o handler roda dentro da requisição de `fechar` (MediatR in-process), então o tempo de retry do gateway conta na resposta ao cliente.

## Checklist

### 1. Aplicação
- [ ] **a:** Na pasta `Modulos/Pagamentos/`, criar a pasta `Modulos.Pagamentos.Aplicacao/`.
- [ ] **b:** Dentro dela, criar o projeto `Modulos.Pagamentos.Aplicacao` (biblioteca de classes), com os pacotes `MediatR` (mesma versão de `Modulos.Pedidos.Aplicacao`) e `Microsoft.Extensions.Logging.Abstractions`.
- [ ] **c:** Dentro do projeto, criar a pasta `Repositorios/`. Dentro dela, criar a interface `IPagamentoRepositorio`, com `AdicionarAsync` e `AtualizarAsync`, recebendo um `Pagamento`, e `ExisteAprovadoPorPedidoAsync`, recebendo o id do Pedido e retornando `bool`.
- [ ] **d:** Dentro do projeto, criar a pasta `Servicos/`. Dentro dela, criar a interface `IGatewayPagamentoServico`, com `CobrarAsync`, recebendo o id do Pagamento, o valor e um `CancellationToken`, retornando `bool` (`true` aprovado, `false` recusado) e lançando exceção quando o gateway falha.
- [ ] **e:** Dentro do projeto, criar a pasta `CasosDeUso/`. Dentro dela, criar a pasta `ProcessarPagamento/`.
- [ ] **f:** Dentro dela, criar a classe `ProcessarPagamentoHandler`, implementando `INotificationHandler<PedidoFechadoIntegracao>`, recebendo `IPagamentoRepositorio`, `IGatewayPagamentoServico`, `IPublisher` e `ILogger<ProcessarPagamentoHandler>`.
- [ ] **g:** No `Handle`, retornar sem fazer nada se `ExisteAprovadoPorPedidoAsync` for `true` para o `PedidoId`.
- [ ] **h:** Criar o `Pagamento` com `Pagamento.Criar` (`PedidoId` e `Valor` da notificação), lançando `ArgumentException` com os erros unidos por `Environment.NewLine` se `resultado.Sucesso == false`, e gravar com `AdicionarAsync`.
- [ ] **i:** Chamar `CobrarAsync`: `true` chama `Aprovar`, `false` chama `Recusar`, exceção chama `MarcarFalha` e registra aviso com `PedidoId`, `PagamentoId` e a mensagem da exceção.
- [ ] **j:** Gravar o novo status com `AtualizarAsync` e, só se aprovado, publicar `PagamentoAprovadoIntegracao` (`PedidoId`, `PagamentoId` e `Valor`) via `IPublisher`.
- [ ] **k:** Na raiz do projeto, criar `InjecaoDeDependencia`, com um método de extensão de `IServiceCollection` chamado `RegistrarPagamentosAplicacao`, registrando o MediatR a partir do assembly, como em `RegistrarPedidosAplicacao`.

### 2. Documentação
- [ ] **a:** Criar `docs/documentacao/modulos/pagamentos/fluxos/pagamentos-processamento.md` com o fluxo de cobrança do Pedido fechado (passos e diagrama Mermaid)
- [ ] **b:** Atualizar `docs/documentacao/modulos/pagamentos/pagamentos.md` com o link do fluxo e as regras de duplicidade, recusa e falha
- [ ] **c:** Perguntar ao Dev se quer o plano de documentação do restante do módulo/fluxo

### 3. QA & Testes
- [ ] **a:** Na pasta `Testes/Pagamentos/`, criar a pasta `Modulos.Pagamentos.Aplicacao.Testes/`. Dentro dela, criar o projeto de testes xUnit `Modulos.Pagamentos.Aplicacao.Testes`, com os mesmos pacotes de `Modulos.Pedidos.Aplicacao.Testes`.
- [ ] **b:** Dentro do projeto, criar a pasta `CasosDeUso/ProcessarPagamento/` com os testes do `ProcessarPagamentoHandler`: gateway aprova, o `Pagamento` termina `Aprovado`, é atualizado e publica `PagamentoAprovadoIntegracao` com `PedidoId`, `PagamentoId` e `Valor`.
- [ ] **c:** Testar gateway que recusa: `Pagamento` termina `Recusado`, é atualizado e nada é publicado.
- [ ] **d:** Testar gateway que lança exceção: `Pagamento` termina `Falhou`, o handler não lança e nada é publicado.
- [ ] **e:** Testar Pedido que já tem `Pagamento` `Aprovado`: nada é criado, o gateway não é chamado e nada é publicado.
- [ ] **f:** Testar valor zero na notificação: `ArgumentException`, sem gravar nem chamar o gateway.

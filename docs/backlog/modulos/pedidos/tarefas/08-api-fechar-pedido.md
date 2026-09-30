---
tags: [backlog/tarefa, modulo/pedidos, fluxo/pedido-operacao, status/pendente]
---

# 08 — Expor o fechamento do Pedido publicando o contrato de integração e exigir autenticação no confirmar-pagamento

**Módulo:** Pedidos | **Camada:** Aplicação / Infraestrutura / API | **Deps:** Compartilhado 03, Pedidos 07

## O que fazer

- **Integração:** o `PedidoFechadoEvento` é despachado como `PedidoFechadoIntegracao`, o contrato do projeto compartilhado; Pagamentos o escuta.
- **Nova tentativa:** repetir o `fechar` republica o contrato, sem lógica extra.
- **Autenticação:** `fechar` e `confirmar-pagamento` exigem token JWT válido, sem papel específico e sem conferir se o token pertence ao cliente do Pedido.
- **Persistência:** `FechadoEm` vira coluna nula; `Total` fica fora do mapeamento, por ser calculado.

## Checklist

### 1. Aplicação
- [ ] **a:** Na pasta `Modulos/Pedidos/Modulos.Pedidos.Aplicacao/CasosDeUso/`, criar a pasta `FecharPedido/`.
- [ ] **b:** Dentro dela, criar o record `FecharPedidoCommand`, implementando `IRequest`, com `PedidoId` (`Guid`).
- [ ] **c:** Ainda nela, criar a classe `FecharPedidoHandler`, no mesmo formato do `ConfirmarPagamentoHandler`: `InvalidOperationException` com `Pedido não encontrado.` se o Pedido não existir; chama `Fechar` e lança `ArgumentException` com os erros unidos por `Environment.NewLine` se `resultado.Sucesso == false`; chama `AtualizarAsync` e despacha `pedido.Eventos` pelo `DespachanteDeEventosDominio`.
- [ ] **d:** Em `DespachanteDeEventosDominio`, mapear `PedidoFechadoEvento` para `PedidoFechadoIntegracao` (`PedidoId` e `Valor`).

### 2. Infraestrutura
- [ ] **a:** Em `PedidoConfiguracao`, ignorar a propriedade `Total`, como já é feito com `Itens`.
- [ ] **b:** Gerar a Migration `AdicionarFechadoEmPedido` do `PedidosDbContext`, com `Modulos.Pedidos.Infraestrutura` como projeto alvo e `PlataformaEntregas.Api` como startup project. Ler a migration gerada e confirmar que só adiciona a coluna `FechadoEm`, nula, sem incluir `Total`.

### 3. API
- [ ] **a:** Na pasta `Modulos/Pedidos/Modulos.Pedidos.Api/Endpoints/`, criar a pasta `FecharPedido/`.
- [ ] **b:** Dentro dela, criar a classe `FecharPedidoEndpoint`, com um método de extensão de `IEndpointRouteBuilder` que mapeia `POST /pedidos/{id}/fechar`, exige autenticação, despacha `FecharPedidoCommand` via `ISender` e retorna `204`, `400` com `{ erro }` se `ArgumentException` for lançada, `404` com `{ erro }` se `InvalidOperationException` for lançada e `401` sem token, no mesmo formato do `ConfirmarPagamentoEndpoint`.
- [ ] **c:** Em `PedidosEndpoints`, registrar o mapeamento de fechar.
- [ ] **d:** Em `ConfirmarPagamentoEndpoint`, exigir autenticação e declarar o `401` entre as respostas.

### 4. Documentação
- [ ] **a:** Atualizar `docs/documentacao/modulos/pedidos/fluxos/pedido-operacao.md` com o passo de fechar o Pedido e o contrato publicado
- [ ] **b:** Atualizar `docs/documentacao/modulos/pedidos/api/pedidos-api.md` com a rota `fechar` e a exigência de autenticação em `fechar` e `confirmar-pagamento`
- [ ] **c:** Perguntar ao Dev se quer o plano de documentação do restante do módulo/fluxo

### 5. QA & Testes
- [ ] **a:** Em `Testes/Pedidos/Modulos.Pedidos.Aplicacao.Testes/CasosDeUso/`, criar a pasta `FecharPedido/` com os testes do `FecharPedidoHandler`: Pedido `Criado` com itens atualiza o repositório e publica `PedidoFechadoIntegracao` com o valor do Pedido.
- [ ] **b:** Testar Pedido inexistente: `InvalidOperationException`, sem atualizar nem publicar.
- [ ] **c:** Testar Pedido sem itens: `ArgumentException`, sem atualizar nem publicar.
- [ ] **d:** Testar Pedido fora de `Criado`: `ArgumentException`, sem atualizar nem publicar.
- [ ] **e:** Chamar `fechar` e `confirmar-pagamento` sem token, pelo `PlataformaEntregas.Api.http`, e confirmar `401` nos dois.

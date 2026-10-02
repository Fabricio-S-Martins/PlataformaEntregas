---
tags: [backlog, modulo/pedidos, fluxo/pedido-status, tarefa/concluido]
---

# 02 — Cobrir Pedido e ItemPedido com testes automatizados

**Módulo:** Pedidos
**Camada:** Domínio

## Contexto

`Pedido` e `ItemPedido` (task 01) ainda não têm nenhum teste automatizado — as invariantes de cada `Criar`, o comportamento de `AdicionarItem` e as seis transições de status só foram checados manualmente até aqui. Mesmo padrão de testes já usado em `RestauranteTestes`/`CardapioTestes`: xUnit, Bogus pra gerar dados válidos, checando `resultado.Sucesso`/`resultado.Erros`/`resultado.Valor`.

## O que fazer

1. Na pasta `Testes/`, criar a pasta `Pedidos/`. Dentro dela, criar a pasta `Modulos.Pedidos.Dominio.Testes/` e, dentro dela, o projeto `Modulos.Pedidos.Dominio.Testes`.
2. Dentro desse projeto, criar a pasta `Entidades/`.
3. Criar a classe de teste `ItemPedidoTestes`.
4. Criar a classe de teste `PedidoTestes`.

## Cenários a cobrir

**`ItemPedidoTestes` (`ItemPedido.Criar`):**
- Dados válidos retorna sucesso.
- `pedidoId` vazio retorna falha com o erro correspondente.
- `itemCardapioId` vazio retorna falha com o erro correspondente.
- Quantidade zero ou negativa retorna falha com o erro correspondente.
- Preço unitário zero ou negativo retorna falha com o erro correspondente.
- Mais de um dado inválido ao mesmo tempo retorna falha com todos os erros.

**`PedidoTestes` (`Pedido.Criar`):**
- Dados válidos retorna sucesso, com `Status` iniciando em `Criado`, `Itens` vazio, e `PedidoCriadoEvento` presente em `Eventos`.
- `clienteId` vazio retorna falha com o erro correspondente.
- `restauranteId` vazio retorna falha com o erro correspondente.
- Os dois vazios ao mesmo tempo retorna falha com os dois erros.

**`PedidoTestes` (`AdicionarItem`):**
- Pedido em `Criado` e dados válidos retorna sucesso, o item passa a constar em `Itens`, e `PedidoItemAdicionadoEvento` fica presente em `Eventos`.
- Pedido em `Criado` e dados inválidos retorna falha, e `Itens` não é alterada.
- Pedido em qualquer status diferente de `Criado` retorna falha, e `Itens` não é alterada.

**`PedidoTestes` (transições — `ConfirmarPagamento`, `Aceitar`, `IniciarPreparo`, `SairParaEntrega`, `Entregar`, `Cancelar`):**
- Para cada método, chamado a partir do status de origem correto (com pelo menos um item no pedido, quando aplicável), retorna sucesso, `Status` muda pro destino, e o evento correspondente fica presente em `Eventos`.
- Para cada método, chamado a partir de um status que não permite aquela transição, retorna falha e `Status` não muda.
- `ConfirmarPagamento` chamado num pedido sem nenhum item retorna falha, mesmo estando em `Criado`.
- `Cancelar` chamado a partir de `Aceito` (ou qualquer status posterior) retorna falha.

## Notas / decisões tomadas

- Nomenclatura dos métodos de teste segue o padrão já fixado: `Metodo_ComCenario_DeveResultado`.

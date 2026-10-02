---
tags: [documentacao, modulo/pedidos, fluxo/pedido-operacao]
---

# O que acontece a cada ação

## Objetivo
Mostrar o caminho de qualquer ação sobre o pedido (criar, adicionar item ou mudar de etapa), do pedido da API até o registro.

## Diagrama
```mermaid
flowchart TD
  Req["Ação solicitada pela API"] --> Loc["Localiza o pedido<br/>(exceto ao criar)"]
  Loc -->|"não existe"| E404["Recusa: pedido não encontrado (404)"]
  Loc --> Reg["Aplica a regra do pedido"]
  Reg -->|"regra não permite"| E400["Recusa com a mensagem do erro (400)"]
  Reg --> Sal["Salva o pedido"]
  Sal --> Eve["Registra o evento da ação"]
  Eve --> Ok["Resposta: 201 ao criar, 204 nas demais"]
```

## Regras
- Uma ação recusada não altera o pedido.
- O evento só é registrado depois que o pedido é salvo.
- Ao localizar o pedido, seus itens são carregados junto.

Ordem das chamadas e erros: [Integrar com Pedidos](../api/pedidos-api.md).

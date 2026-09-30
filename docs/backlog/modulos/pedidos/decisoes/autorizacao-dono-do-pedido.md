---
tags: [backlog/decisao, modulo/pedidos, status/adiada]
---
# Como impedir que um usuário feche ou pague o Pedido de outro cliente?

## Contexto
Os endpoints de fechar e confirmar pagamento exigem token JWT válido, sem papel específico e sem conferir se o token pertence ao cliente do Pedido. Qualquer usuário autenticado pode, hoje, fechar o Pedido de outro cliente. O `Pedido` já guarda o `ClienteId`. Adiada: reabre depois que a exigência de autenticação nesses endpoints estiver pronta.

## Opções
- **Comparar a claim `sub` do token com o `ClienteId` do Pedido e responder `403` quando não for o dono:** deixa claro o motivo da recusa.
- **Comparar do mesmo jeito e responder `404` quando não for o dono:** não revela que o Pedido existe.

## O que depende
O escopo da regra (só fechar e confirmar pagamento, ou todos os endpoints de Pedidos), quais transições cabem ao restaurante e ao entregador, e se `CriarPedidoRequest` deixa de receber o `ClienteId` no corpo e passa a usar o do token.

## Decisão

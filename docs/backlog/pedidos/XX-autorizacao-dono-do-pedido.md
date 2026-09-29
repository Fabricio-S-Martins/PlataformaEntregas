# XX — Impedir que um usuário feche ou pague o Pedido de outro cliente

**Módulo:** Pedidos
**Camada:** Aplicação / API
**Status:** adiada — revisitar depois do card 08, que já exige autenticação em `fechar` e `confirmar-pagamento`

## Contexto

O card 08 exige token JWT válido em `fechar` e `confirmar-pagamento`, sem papel específico e sem conferir se o token pertence ao cliente do Pedido. Qualquer usuário autenticado pode, hoje, fechar o Pedido de outro cliente. O `Pedido` já guarda o `ClienteId`.

## A discutir quando esta task for detalhada

- Comparar o usuário do token (claim `sub`) com o `ClienteId` do Pedido, e qual resposta devolver quando não for o dono (`403` ou `404`).
- Se a regra vale só para `fechar` e `confirmar-pagamento` ou para todos os endpoints de Pedidos, e quais transições cabem ao restaurante e ao entregador.
- Se `CriarPedidoRequest` deixa de receber o `ClienteId` no corpo e passa a usar o do token.

Esta task será detalhada (com "O que fazer" e checklist) quando chegar a vez dela no backlog — por ora é só um marcador pra não perder o contexto.

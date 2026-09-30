---
tags: [backlog/decisao, modulo/pagamentos, modulo/pedidos, status/adiada]
---
# Como estornar o pagamento aprovado depois que o Pedido foi cancelado?

## Contexto
Se o Pedido é cancelado enquanto o pagamento está em andamento, o gateway pode aprovar e cobrar o cliente. O Pedido não pode ir a `Pago`: o caso é registrado em log (`PedidoId`, `PagamentoId` e `Valor`) e não relançado. O cliente fica cobrado sem Pedido pago, e não existe estorno. Adiada: reabre quando existir a mensageria com RabbitMQ, Outbox e Polly.

## Opções
- **Saga com compensação:** o cancelamento durante um pagamento em andamento entra em uma fila; depois da cobrança, o pagamento é aprovado, o Pedido é cancelado e o valor é estornado. Depende da fila e de garantia de entrega.
- **Manter só o log:** situação atual, sem estorno nem estado consultável.

## O que depende
Como Pedidos sabe que há pagamento em andamento (o `Cancelar` hoje é imediato e não consulta Pagamentos), o novo estado do Pagamento para o estorno e a chamada de estorno no gateway falso, se o Pagamento aprovado sem Pedido pago passa a ser um estado consultável no banco, e qual fila e garantia de entrega.

## Decisão

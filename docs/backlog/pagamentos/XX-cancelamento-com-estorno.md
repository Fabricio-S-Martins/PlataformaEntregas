# XX — Cancelar o Pedido com estorno quando o pagamento é aprovado depois do cancelamento

**Módulo:** Pagamentos
**Camada:** Aplicação / Infraestrutura
**Status:** adiada — depende do card de RabbitMQ, Outbox e Polly, que ainda não existe

## Contexto

Se o Pedido é cancelado enquanto o pagamento está em andamento, o gateway pode aprovar e cobrar o cliente. O Pedido não pode ir a `Pago`: o card Pedidos 09 registra o caso em log (`PedidoId`, `PagamentoId` e `Valor`) e não relança. O cliente fica cobrado sem Pedido pago, e não existe estorno.

## Ideia

Saga com compensação: o cancelamento durante um pagamento em andamento entra em uma fila. Depois da cobrança, o pagamento é aprovado, o Pedido é cancelado e o valor é estornado.

## A discutir quando esta task for detalhada

- Como Pedidos sabe que há pagamento em andamento para o Pedido, já que o `Cancelar` hoje é imediato e não consulta Pagamentos.
- Novo estado do Pagamento para o estorno e a chamada de estorno no gateway falso.
- Se o Pagamento aprovado sem Pedido pago, hoje só em log, passa a ser um estado consultável no banco.
- Qual fila e qual garantia de entrega, junto do card de RabbitMQ e Outbox.

Esta task será detalhada (com "O que fazer" e checklist) quando chegar a vez dela no backlog — por ora é só um marcador pra não perder o contexto.

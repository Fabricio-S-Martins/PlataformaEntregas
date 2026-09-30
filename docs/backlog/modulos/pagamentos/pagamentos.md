---
tags: [backlog/modulo, modulo/pagamentos]
---

# Módulo Pagamentos

[← Backlog](../../backlog.md)

| Task |
|---|
| [01 - Modelar o agregado Pagamento com status e transições no Domínio e cobrir com testes automatizados](tarefas/01-dominio-pagamento.md) |
| [02 - Processar o pagamento ao receber o Pedido fechado, publicar a aprovação e cobrir com testes automatizados](tarefas/02-aplicacao-processar-pagamento.md) |
| [03 - Persistir o Pagamento no Postgres, registrar o módulo no host e gerar a Migration inicial](tarefas/03-infraestrutura-persistencia-pagamento.md) |
| [04 - Criar o gateway de pagamento falso configurável com retry, circuit breaker e timeout do Polly e cobrir com testes automatizados](tarefas/04-infraestrutura-gateway-falso-polly.md) |

## Decisões

- [Como estornar o pagamento aprovado depois que o Pedido foi cancelado?](decisoes/estorno-de-pagamento-apos-cancelamento.md)

# Módulo Pagamentos

Status possíveis: `todo`, `em andamento`, `feito`, `bloqueado`.

[← Backlog](../backlog.md)

| Task | Status |
|---|---|
| [01 - Modelar o agregado Pagamento com status e transições no Domínio e cobrir com testes automatizados](01-dominio-pagamento.md) | todo |
| [02 - Processar o pagamento ao receber o Pedido fechado, publicar a aprovação e cobrir com testes automatizados](02-aplicacao-processar-pagamento.md) | todo |
| [03 - Persistir o Pagamento no Postgres, registrar o módulo no host e gerar a Migration inicial](03-infraestrutura-persistencia-pagamento.md) | todo |
| [04 - Criar o gateway de pagamento falso configurável com retry, circuit breaker e timeout do Polly e cobrir com testes automatizados](04-infraestrutura-gateway-falso-polly.md) | todo |
| [XX - Cancelar o Pedido com estorno quando o pagamento é aprovado depois do cancelamento](XX-cancelamento-com-estorno.md) | adiada (depende do card de RabbitMQ e Outbox) |

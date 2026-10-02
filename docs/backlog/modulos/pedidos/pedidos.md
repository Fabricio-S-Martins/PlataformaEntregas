---
tags: [backlog, modulo/pedidos]
---

# Módulo Pedidos

[← Backlog](../../backlog.md)

| Task |
|---|
| [01 - Modelar o agregado Pedido com máquina de estados e eventos de domínio](tarefas/01-dominio-pedido-maquina-estados.md) |
| [02 - Cobrir Pedido e ItemPedido com testes automatizados](tarefas/02-testes-pedido-item-pedido.md) |
| [03 - Criar casos de uso do Pedido e publicar os eventos de domínio](tarefas/03-aplicacao-pedido.md) |
| [04 - Cobrir os casos de uso do Pedido com testes automatizados](tarefas/04-testes-aplicacao-pedido.md) |
| [05 - Implementar IPedidoRepositorio com EF Core](tarefas/05-infraestrutura-pedido-repositorio.md) |
| [06 - Expor endpoints do ciclo de vida do Pedido](tarefas/06-api-pedido.md) |
| [07 - Fechar o Pedido com total calculado, permitindo nova tentativa e bloqueando novos itens](tarefas/07-dominio-fechar-pedido.md) |
| [08 - Expor o fechamento do Pedido publicando o contrato de integração e exigir autenticação no confirmar-pagamento](tarefas/08-api-fechar-pedido.md) |
| [09 - Confirmar o pagamento do Pedido ao receber a aprovação de Pagamentos, registrando em log quando o Pedido não puder ser pago](tarefas/09-confirmar-pagamento-por-integracao.md) |

## Decisões

- [Como impedir que um usuário feche ou pague o Pedido de outro cliente?](decisoes/autorizacao-dono-do-pedido.md)
- [Como expirar Pedidos abandonados em Criado?](decisoes/expiracao-de-pedidos-abandonados.md)
- [Como estornar o pagamento aprovado depois que o Pedido foi cancelado?](../pagamentos/decisoes/estorno-de-pagamento-apos-cancelamento.md)

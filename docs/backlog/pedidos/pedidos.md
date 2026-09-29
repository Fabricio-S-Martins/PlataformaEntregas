# Módulo Pedidos

Status possíveis: `todo`, `em andamento`, `feito`, `bloqueado`.

[← Backlog](../backlog.md)

| Task | Status |
|---|---|
| [01 - Modelar o agregado Pedido com máquina de estados e eventos de domínio](01-dominio-pedido-maquina-estados.md) | feito |
| [02 - Cobrir Pedido e ItemPedido com testes automatizados](02-testes-pedido-item-pedido.md) | feito |
| [03 - Criar casos de uso do Pedido e publicar os eventos de domínio](03-aplicacao-pedido.md) | feito |
| [04 - Cobrir os casos de uso do Pedido com testes automatizados](04-testes-aplicacao-pedido.md) | feito |
| [05 - Implementar IPedidoRepositorio com EF Core](05-infraestrutura-pedido-repositorio.md) | feito |
| [06 - Expor endpoints do ciclo de vida do Pedido](06-api-pedido.md) | feito |
| [07 - Fechar o Pedido com total calculado, permitindo nova tentativa e bloqueando novos itens](07-dominio-fechar-pedido.md) | todo |
| [08 - Expor o fechamento do Pedido publicando o contrato de integração e exigir autenticação no confirmar-pagamento](08-api-fechar-pedido.md) | todo |
| [09 - Confirmar o pagamento do Pedido ao receber a aprovação de Pagamentos, registrando em log quando o Pedido não puder ser pago](09-confirmar-pagamento-por-integracao.md) | todo |

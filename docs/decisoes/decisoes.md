---
tags: [decisao]
---

# Decisões

Decisões de arquitetura difíceis de reverter, uma nota por decisão.

## Módulos
- [Autenticação](modulos/autenticacao/autenticacao.md): cadastro de usuários e login, com papéis Cliente, Restaurante e Entregador.
- [Catálogo](modulos/catalogo/catalogo.md): restaurantes e, para cada um, o cardápio com seus itens.
- [Compartilhado](modulos/compartilhado/compartilhado.md): decisões transversais a todos os módulos (monólito modular, host único, contratos de integração, Clean Architecture com CQRS, persistência e Result Pattern).
- [Pagamentos](modulos/pagamentos/pagamentos.md): cobrança de um pedido, com status e regras de transição.
- [Pedidos](modulos/pedidos/pedidos.md): pedido de um cliente a um restaurante, com itens e etapas.

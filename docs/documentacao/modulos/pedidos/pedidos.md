---
tags: [documentacao, modulo/pedidos]
---

# Pedidos

[Documentação](../../documentacao.md)

Um pedido é feito por um cliente a um restaurante, reúne itens e passa por etapas até a entrega.

## Etapas

Criado → Pago → Aceito → Em preparo → Saiu para entrega → Entregue
(ou Cancelado)

## Regras

- Itens só podem ser adicionados enquanto o pedido está Criado.
- Cada item precisa de quantidade e preço maiores que zero.
- Pedido sem itens não pode ser pago.
- As etapas seguem sempre essa ordem, sem pular.
- O pedido só pode ser cancelado enquanto está Criado ou Pago.

## Fluxos

- [Etapas do pedido](fluxos/pedido-status.md)
- [O que acontece a cada ação](fluxos/pedido-operacao.md)

## Integração

Ordem das chamadas e erros: [Integrar com Pedidos](api/pedidos-api.md)

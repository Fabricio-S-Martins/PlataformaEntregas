---
tags: [decisao, modulo/pedidos, decisao/aceita]
---
# Ciclo de vida do Pedido como máquina de estados no Domínio, com eventos de domínio

- **Decisão e porquê:** as transições do Pedido (Criado → Pago → Aceito → Em preparo → Saiu para entrega → Entregue/Cancelado) vivem no Domínio, e os casos de uso publicam os eventos de domínio via MediatR.
- **Descartado:** (a confirmar) estado como campo livre alterado pelos handlers — motivo a confirmar com o Dev.
- **Não fazer:** mudar o estado do Pedido fora dos métodos do Domínio; pular etapas da máquina de estados.

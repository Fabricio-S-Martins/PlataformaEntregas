# XX — Expirar Pedidos abandonados em Criado

**Módulo:** Pedidos
**Camada:** Domínio / Aplicação / Infraestrutura
**Status:** adiada — revisitar depois do módulo Pagamentos

## Contexto

Com o plano de Pagamentos, recusa ou falha do pagamento não cancela o Pedido: ele continua `Criado` para o cliente tentar de novo. Não existe expiração, então Pedidos que o cliente nunca volta a pagar ficam `Criado` para sempre. Hoje o `Cancelar` é manual e só vale em `Criado` ou `Pago`.

## A discutir quando esta task for detalhada

- Prazo de expiração, contado a partir de `CriadoEm` ou de `FechadoEm`.
- O que acontece ao expirar: cancelar o Pedido pelo `Cancelar` existente ou criar um status próprio.
- Quem dispara: um serviço em segundo plano (`BackgroundService`) que varre periodicamente, ou a fila, quando ela existir.
- Como não expirar um Pedido com pagamento em andamento.

Esta task será detalhada (com "O que fazer" e checklist) quando chegar a vez dela no backlog — por ora é só um marcador pra não perder o contexto.

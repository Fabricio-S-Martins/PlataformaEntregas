---
tags: [backlog, modulo/pedidos, decisao/adiada]
---
# Como expirar Pedidos abandonados em Criado?

## Contexto
Com o plano de Pagamentos, recusa ou falha do pagamento não cancela o Pedido: ele continua `Criado` para o cliente tentar de novo. Não existe expiração, então Pedidos que o cliente nunca volta a pagar ficam `Criado` para sempre. Hoje o `Cancelar` é manual e só vale em `Criado` ou `Pago`. Adiada: reabre depois do módulo Pagamentos.

## Opções
- **Expirar cancelando pelo `Cancelar` existente:** reaproveita a transição que já existe.
- **Criar um status próprio de expirado:** distingue expiração de cancelamento manual.
- **Disparo por `BackgroundService` que varre periodicamente:** funciona sem fila.
- **Disparo pela fila, quando ela existir:** depende da mensageria.

## O que depende
O prazo de expiração (contado a partir de `CriadoEm` ou de `FechadoEm`) e como não expirar um Pedido com pagamento em andamento.

## Decisão

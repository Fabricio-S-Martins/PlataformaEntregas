---
tags: [decisao, modulo/pagamentos, decisao/aceita]
---
# Pagamento processado ao reagir ao evento de pedido fechado

- **Decisão e porquê:** o módulo Pagamentos consome `PedidoFechadoIntegracao` e publica `PagamentoAprovadoIntegracao`; o Pedidos confirma o pagamento por esse evento. Hoje roda in-process dentro da requisição de fechar, então o tempo do gateway conta na resposta; a fila assíncrona é evolução futura.
- **Descartado:** (a confirmar) Pedidos chamar Pagamentos diretamente — acoplaria os módulos.
- **Não fazer:** o Pedidos chamar o Pagamentos diretamente; recusa ou falha do gateway lançar exceção no handler (são resultados).

---
tags: [decisao, modulo/pagamentos, decisao/aceita]
---
# Gateway de pagamento atrás de uma porta, com resiliência Polly na Infraestrutura

- **Decisão e porquê:** a Aplicação depende de `IGatewayPagamentoServico` (Ports and Adapters) e o adaptador na Infraestrutura aplica o pipeline Polly (retry, circuit breaker, timeout). Trocar o gateway real não toca a Aplicação.
- **Descartado:** (a confirmar) chamar o gateway direto no handler — motivo a confirmar com o Dev.
- **Não fazer:** a Aplicação referenciar o SDK ou cliente HTTP do gateway; resiliência fora do adaptador.

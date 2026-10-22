---
tags: [especificacao, modulo/pagamentos, especificacao/rascunho]
---
# Pagamentos: regras de negócio

**Para que existe:** cobrar o valor de um pedido e registrar o resultado de cada tentativa.

## Termos
| Termo | Significado |
|---|---|
| Pagamento | Cobrança de um pedido; cada tentativa gera um pagamento próprio. |
| Pendente | Estado inicial: aguardando o resultado da cobrança. |
| Aprovado | A cobrança foi aceita. |
| Recusado | A cobrança foi negada. |
| Falhou | Houve erro técnico ao processar, sem resposta sobre a cobrança. |
| Gateway | Serviço externo que processa a cobrança. |

## Regras
- **Nasce Pendente:** todo pagamento é criado Pendente.
- **Pedido e valor válidos:** o pagamento precisa de um pedido válido e de valor maior que zero. Com os dois errados, os dois erros são informados juntos.
- **Resultado único:** só um pagamento Pendente pode ser aprovado, recusado ou marcado como falho.
- **Estados finais:** Aprovado, Recusado e Falhou não mudam mais; nova tentativa cria um novo pagamento.
- **Sem cobrança duplicada:** pedido que já tem pagamento aprovado não é cobrado de novo.
- **Recusa e falha não são erro:** cobrança negada deixa o pagamento Recusado e erro técnico o deixa Falhou, sem gerar erro para quem fechou o pedido.
- **Só aprovado avisa:** só o pagamento aprovado gera o aviso de pagamento aprovado.

## Aguardando Validações
- Validação 1: o pagamento recusado ou que falhou cancela o pedido, ou o cliente pode tentar de novo? Quantas vezes?
- Validação 2: se o pedido for cancelado enquanto o pagamento está em andamento e o gateway aprovar, o cliente é estornado? Hoje o caso só é registrado em log e o cliente fica cobrado.
- Validação 3: quais formas de pagamento existem (cartão, Pix, dinheiro)?
- Validação 4: o cliente pode pedir reembolso de pedido já pago e cancelado?

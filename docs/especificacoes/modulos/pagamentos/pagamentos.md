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
| Estorno | Devolução ao cliente do valor de uma cobrança aprovada depois que o pedido já estava Cancelado. |
| Reembolso | Devolução ao cliente do valor de um pedido que estava Pago e foi Cancelado. |

## Regras
- **Nasce Pendente:** todo pagamento é criado Pendente.
- **Pedido e valor válidos:** o pagamento precisa de um pedido válido e de valor maior que zero. Com os dois errados, os dois erros são informados juntos.
- **Resultado único:** só um pagamento Pendente pode ser aprovado, recusado ou marcado como falho.
- **Estados finais:** Aprovado, Recusado e Falhou não mudam mais; nova tentativa cria um novo pagamento.
- **Sem cobrança duplicada:** pedido que já tem pagamento aprovado não é cobrado de novo.
- **Recusa e falha não são erro:** cobrança negada deixa o pagamento Recusado e erro técnico o deixa Falhou, sem gerar erro para quem fechou o pedido.
- **Só aprovado avisa:** só o pagamento aprovado gera o aviso de pagamento aprovado.
- **Recusa não cancela o pedido:** pagamento Recusado ou Falhou não cancela o pedido; o cliente pode tentar de novo.
- **Limite de tentativas:** cada pedido tem até 3 pagamentos Recusados ou Falhos; atingido o limite, não há nova cobrança desse pedido.
- **Estorno automático:** pagamento aprovado de um pedido que já estava Cancelado é estornado sem ação de ninguém, e o estorno é registrado.
- **Só cartão de crédito:** a única forma de pagamento é o cartão de crédito.
- **Reembolso automático e integral:** pedido Pago que é Cancelado tem o valor pago devolvido por inteiro, sem ação de ninguém, e a devolução é registrada.

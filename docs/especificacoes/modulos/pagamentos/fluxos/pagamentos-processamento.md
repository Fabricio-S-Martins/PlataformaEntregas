---
tags: [especificacao, modulo/pagamentos, fluxo/pagamentos-processamento, especificacao/rascunho]
---
# Cobrança do pedido fechado

[Pagamentos](../pagamentos.md)

**Objetivo:** cobrar o valor de um pedido assim que ele é fechado e avisar quando a cobrança é aprovada.

## Regras
- **Sem cobrança duplicada:** pedido com pagamento já aprovado não é cobrado de novo e nada é publicado.
- **Pendente antes da cobrança:** o pagamento é gravado como Pendente antes da cobrança e atualizado com o resultado.
- **Recusa e falha não são erro:** cobrança negada e erro técnico são resultados do fluxo e não geram erro para quem fechou o pedido.
- **Falha registrada:** o erro técnico é registrado em log com o pedido, o pagamento e a mensagem do erro.
- **Só aprovado avisa:** só o pagamento aprovado publica o aviso, com o pedido, o pagamento e o valor cobrado.
- **Dados inválidos interrompem:** pedido ou valor inválido (valor zero, por exemplo) interrompe o fluxo com erro, sem gravar nem cobrar.
- **Cobrança dentro do fechamento:** o tempo gasto no gateway conta na resposta ao cliente.

## Critérios de aceite
| Critério | Quando | Então |
|:-:|---|---|
| Critério 1: cobrança aprovada | Pedido fechado sem pagamento aprovado e gateway aceita | O pagamento fica Aprovado e o aviso de pagamento aprovado é publicado. |
| Critério 2: cobrança negada | O gateway nega a cobrança | O pagamento fica Recusado, nenhum aviso é publicado e quem fechou o pedido não recebe erro. |
| Critério 3: erro técnico | O gateway falha sem responder sobre a cobrança | O pagamento fica Falhou, o erro é registrado em log e quem fechou o pedido não recebe erro. |
| Critério 4: pedido já pago | Já existe pagamento aprovado para o pedido | Nada é cobrado nem publicado. |
| Critério 5: valor inválido | O valor é zero ou o pedido é inválido | O fluxo é interrompido com erro e nada é gravado nem cobrado. |
| Critério 6: nova tentativa | Há um pagamento Recusado ou Falhou e o pedido é fechado de novo | Um novo pagamento é criado e cobrado, e o antigo não muda. |
| Critério 7: aviso com dados | O pagamento é aprovado | O aviso traz o pedido, o pagamento e o valor cobrado. |

## Aguardando Validações
- Validação 1: o que é "pedido fechado": o cliente concluir a montagem do pedido? Em que ação do pedido isso acontece?
- Validação 2: quanto tempo o cliente espera pela cobrança antes de receber uma resposta (limite de tempo do gateway)?
- Validação 3: se o gateway falhar, o sistema tenta cobrar de novo sozinho ou só o cliente tenta?
- Validação 4: após o pagamento aprovado, o que acontece com o pedido (vai para Pago automaticamente)?
- Validação 5: se o pagamento for aprovado depois que o pedido foi cancelado, o cliente é estornado? Hoje o cliente fica cobrado, sem pedido pago, e só há registro em log.

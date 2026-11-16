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
- **Limite de espera:** a cobrança tem no máximo 10 segundos; passado esse tempo, o pagamento fica Falhou.
- **Só cartão de crédito:** a cobrança é sempre no cartão de crédito.
- **Pedido fechado:** o cliente dono do pedido conclui a montagem com a ação de fechar; é essa ação que inicia a cobrança.
- **Repetição automática:** diante de erro técnico, o sistema repete a cobrança sozinho antes de marcar o pagamento como Falhou; o cliente não precisa agir.
- **Pedido Pago sozinho:** com o pagamento aprovado, o pedido vai para Pago sem ação de ninguém.
- **Recusa não cancela o pedido:** com o pagamento Recusado ou Falhou, o pedido continua Criado e o cliente pode fechar de novo.
- **Limite de tentativas:** com 3 pagamentos Recusados ou Falhos no pedido, um novo fechamento não cobra, informa que o limite de tentativas foi atingido e o pedido continua Criado.
- **Estorno automático:** pagamento aprovado depois que o pedido foi Cancelado é estornado sem ação de ninguém, e o estorno é registrado.

## Critérios de aceite
| Critério | Quando | Então |
|:-:|---|---|
| Critério 1: cobrança aprovada | Pedido fechado sem pagamento aprovado e gateway aceita | O pagamento fica Aprovado e o aviso de pagamento aprovado é publicado. |
| Critério 2: cobrança negada | O gateway nega a cobrança | O pagamento fica Recusado, nenhum aviso é publicado e quem fechou o pedido não recebe erro. |
| Critério 3: erro técnico | O gateway falha sem responder sobre a cobrança, mesmo depois das repetições automáticas | O pagamento fica Falhou, o erro é registrado em log e quem fechou o pedido não recebe erro. |
| Critério 4: pedido já pago | Já existe pagamento aprovado para o pedido | Nada é cobrado nem publicado. |
| Critério 5: valor inválido | O valor é zero ou o pedido é inválido | O fluxo é interrompido com erro e nada é gravado nem cobrado. |
| Critério 6: nova tentativa | Há um pagamento Recusado ou Falhou, o limite de 3 não foi atingido e o pedido é fechado de novo | Um novo pagamento é criado e cobrado, e o antigo não muda. |
| Critério 7: aviso com dados | O pagamento é aprovado | O aviso traz o pedido, o pagamento e o valor cobrado. |
| Critério 8: cobrança iniciada pelo fechamento | O cliente dono fecha o pedido | A cobrança é iniciada; sem o fechamento, nada é cobrado. |
| Critério 9: repetição automática | O gateway falha numa tentativa e responde numa repetição seguinte | O pagamento recebe o resultado da resposta, sem ação do cliente. |
| Critério 10: pedido Pago sozinho | O pagamento fica Aprovado | O pedido vai para Pago sem ação de ninguém. |
| Critério 11: recusa não cancela | O pagamento fica Recusado ou Falhou | O pedido continua Criado. |
| Critério 12: limite de tentativas | O pedido já tem 3 pagamentos Recusados ou Falhos e é fechado de novo | Nada é cobrado, a resposta informa que o limite de tentativas foi atingido e o pedido continua Criado. |
| Critério 13: estorno automático | O gateway aprova a cobrança de um pedido que já estava Cancelado | O valor cobrado é estornado sem ação de ninguém e o estorno é registrado. |
| Critério 14: limite de espera | A cobrança passa de 10 segundos sem resposta do gateway | O pagamento fica Falhou e quem fechou o pedido não recebe erro. |
| Critério 15: reembolso de pedido Pago | Um pedido Pago é Cancelado | O valor pago é devolvido por inteiro, sem ação de ninguém, e a devolução é registrada. |

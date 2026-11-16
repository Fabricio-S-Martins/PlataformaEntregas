---
tags: [especificacao, modulo/pedidos, fluxo/pedido-status, especificacao/rascunho]
---
# Etapas do pedido

[Pedidos](../pedidos.md)

**Objetivo:** levar o pedido de Criado até Entregue, ou Cancelado.

## Regras
- **Fechar:** ação do cliente dono do pedido que conclui a montagem e inicia a cobrança; o pedido continua Criado até a aprovação.
- **Confirmar pagamento:** leva de Criado para Pago e exige ao menos um item; acontece sozinha quando o pagamento é aprovado.
- **Aceitar:** leva de Pago para Aceito.
- **Iniciar preparo:** leva de Aceito para Em preparo.
- **Sair para entrega:** leva de Em preparo para Saiu para entrega.
- **Entregar:** leva de Saiu para entrega para Entregue.
- **Cancelar:** leva de Criado ou Pago para Cancelado.
- **Ação fora da ordem:** é recusada com uma mensagem e o pedido continua na mesma etapa.
- **Itens só em Criado:** itens só podem ser adicionados na etapa Criado.
- **Toda ação vira evento:** toda ação bem-sucedida é registrada como evento do pedido, inclusive a criação e a adição de item.
- **Fechar e pagar só pelo dono:** só o cliente dono do pedido o fecha e paga; outro usuário, mesmo autenticado, é recusado e o pedido não muda.
- **Aceitar e preparar só pelo Restaurante:** só quem tem o papel Restaurante aceita e inicia o preparo; outro papel é recusado e o pedido não muda.
- **Entregar só pelo Entregador:** só quem tem o papel Entregador entrega; outro papel é recusado e o pedido não muda.
- **Preço do Catálogo:** ao adicionar o item, o preço é o do Catálogo naquele momento e fica gravado no item.
- **Pedido não pago expira:** pedido Criado por 24 horas, desde a criação, sem pagamento vai para Cancelado sozinho.
- **Itens do restaurante do pedido:** só entra item do restaurante do pedido; item de outro restaurante é recusado.
- **Cancelar pedido Pago reembolsa:** cancelar um pedido Pago, antes de o restaurante aceitar, devolve o valor ao cliente.

## Critérios de aceite
| Critério | Quando | Então |
|:-:|---|---|
| Critério 1: pagar pedido com itens | Pedido Criado com ao menos um item | O pedido vai para Pago. |
| Critério 2: pagar pedido sem itens | Pedido Criado sem itens | A ação é recusada e o pedido continua Criado. |
| Critério 3: percorrer as etapas | Pedido Pago | Aceitar, iniciar preparo, sair para entrega e entregar levam, nessa ordem, a Aceito, Em preparo, Saiu para entrega e Entregue. |
| Critério 4: pular etapa | Pedido Pago e alguém pede para iniciar o preparo | A ação é recusada e o pedido continua Pago. |
| Critério 5: cancelar antes do aceite | Pedido Criado ou Pago | O pedido vai para Cancelado. |
| Critério 6: cancelar depois do aceite | Pedido Aceito, Em preparo, Saiu para entrega ou Entregue | O cancelamento é recusado e o pedido continua na mesma etapa. |
| Critério 7: item fora de Criado | Pedido em qualquer etapa diferente de Criado | A adição de item é recusada e o pedido não muda. |
| Critério 8: item com valor inválido | Quantidade ou preço igual ou menor que zero | A adição é recusada e o pedido não muda. |
| Critério 9: ação gera evento | Qualquer ação bem-sucedida | Um evento do pedido é registrado. |
| Critério 10: ação recusada não gera evento | Qualquer ação recusada | Nenhum evento é registrado e o pedido não muda. |
| Critério 11: dono fecha o pedido | O cliente dono fecha o pedido Criado | A ação é aceita e a cobrança é iniciada. |
| Critério 12: outro usuário fecha ou paga | Um usuário autenticado que não é o dono tenta fechar ou pagar o pedido | A ação é recusada e o pedido não muda. |
| Critério 13: aceitar com o papel Restaurante | Pedido Pago e usuário com o papel Restaurante pede para aceitar ou iniciar o preparo | A ação é aceita. |
| Critério 14: aceitar com outro papel | Pedido Pago e usuário com outro papel pede para aceitar ou iniciar o preparo | A ação é recusada e o pedido não muda. |
| Critério 15: entregar com o papel Entregador | Pedido Saiu para entrega e usuário com o papel Entregador pede para entregar | O pedido vai para Entregue. |
| Critério 16: entregar com outro papel | Pedido Saiu para entrega e usuário com outro papel pede para entregar | A ação é recusada e o pedido não muda. |
| Critério 17: preço do Catálogo | Item adicionado a um pedido | O item guarda o preço do Catálogo daquele momento. |
| Critério 18: preço muda depois | O preço do item muda no Catálogo depois de adicionado ao pedido | O item do pedido mantém o preço de quando foi adicionado. |
| Critério 19: pedido não pago expira | Pedido Criado há 24 horas, sem pagamento | O pedido vai para Cancelado sozinho. |
| Critério 20: pedido pago não expira | Pedido que foi pago antes de completar 24 horas | O pedido não é cancelado por expiração. |
| Critério 21: item de outro restaurante | Adicionar item que não pertence ao restaurante do pedido | A adição é recusada e o pedido não muda. |
| Critério 22: cancelar pedido Pago | Pedido Pago é cancelado antes do aceite | O pedido vai para Cancelado e o valor pago é devolvido ao cliente. |

## Fora do escopo
- Prazo máximo para o restaurante aceitar o pedido.

## Aguardando Validações
- Validação 4: quem executa "sair para entrega" e quem cancela o pedido (o cliente dono, o restaurante ou os dois)? Hoje qualquer usuário autenticado executa.

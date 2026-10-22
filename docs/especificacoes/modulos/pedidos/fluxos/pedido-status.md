---
tags: [especificacao, modulo/pedidos, fluxo/pedido-status, especificacao/rascunho]
---
# Etapas do pedido

[Pedidos](../pedidos.md)

**Objetivo:** levar o pedido de Criado até Entregue, ou Cancelado.

## Regras
- **Confirmar pagamento:** leva de Criado para Pago e exige ao menos um item.
- **Aceitar:** leva de Pago para Aceito.
- **Iniciar preparo:** leva de Aceito para Em preparo.
- **Sair para entrega:** leva de Em preparo para Saiu para entrega.
- **Entregar:** leva de Saiu para entrega para Entregue.
- **Cancelar:** leva de Criado ou Pago para Cancelado.
- **Ação fora da ordem:** é recusada com uma mensagem e o pedido continua na mesma etapa.
- **Itens só em Criado:** itens só podem ser adicionados na etapa Criado.
- **Toda ação vira evento:** toda ação bem-sucedida é registrada como evento do pedido, inclusive a criação e a adição de item.

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

## Aguardando Validações
- Validação 1: quem executa cada ação (por exemplo, só o restaurante aceita e inicia o preparo, só o entregador entrega)?
- Validação 2: depois de Pago, o cliente pode cancelar até o restaurante aceitar? Há reembolso nesse caso?
- Validação 3: o pedido tem um prazo máximo para o restaurante aceitar?

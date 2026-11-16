---
tags: [especificacao, modulo/pedidos, fluxo/pedido-operacao, especificacao/rascunho]
---
# O que acontece a cada ação

[Pedidos](../pedidos.md)

**Objetivo:** definir o resultado de qualquer ação sobre o pedido (criar, adicionar item ou mudar de etapa).

## Regras
- **Pedido existente:** toda ação, exceto criar, exige um pedido existente; senão a resposta é "não encontrado".
- **Regra não permite:** a ação é recusada com a mensagem do erro.
- **Recusa não altera:** uma ação recusada não muda o pedido.
- **Evento depois de salvar:** o evento da ação só é registrado depois que o pedido foi salvo.
- **Itens junto:** ao localizar o pedido, seus itens são considerados.
- **Cliente e restaurante obrigatórios:** criar o pedido exige o cliente, que é o usuário autenticado, e um restaurante que existe.

## Critérios de aceite
| Critério | Quando | Então |
|:-:|---|---|
| Critério 1: criar pedido | Dados válidos | O pedido é criado e o evento de criação é registrado. |
| Critério 2: ação em pedido inexistente | O id não pertence a nenhum pedido | A resposta é "não encontrado". |
| Critério 3: ação que a regra não permite | Ação fora das regras do pedido | A resposta traz a mensagem do erro e o pedido não muda. |
| Critério 4: ação permitida | Ação dentro das regras do pedido | O pedido é salvo e só então o evento é registrado. |
| Critério 5: itens considerados | Pedido com itens | As regras que dependem dos itens (por exemplo, pagar exige itens) usam os itens já adicionados. |
| Critério 6: restaurante não informado | Criar pedido sem restaurante | O pedido é recusado e nada é criado. |
| Critério 7: restaurante inexistente | Criar pedido com um restaurante que não existe | O pedido é recusado e nada é criado. |
| Critério 8: cliente é o usuário autenticado | Um usuário autenticado cria um pedido | O cliente do pedido é esse usuário. |

## Aguardando Validações
- Validação 2: duas pessoas alterando o mesmo pedido ao mesmo tempo: qual alteração vale?

---
tags: [especificacao, modulo/catalogo, fluxo/catalogo-montagem-cardapio, especificacao/rascunho]
---
# Montagem de cardápio

[Catálogo](../catalogo.md)

**Objetivo:** criar o cardápio de um restaurante e adicionar itens a ele.

## Regras
- **Cardápio nasce vazio:** o cardápio começa sem itens e pertence a um restaurante.
- **Restaurante obrigatório e existente:** o id do restaurante é obrigatório e o restaurante precisa existir. Mensagem para id ausente: "Identificador do Restaurante inválido."
- **Um cardápio por restaurante:** o restaurante que já tem cardápio não ganha outro.
- **Itens um a um:** os itens são adicionados um a um, sempre por meio do cardápio.
- **Item completo:** nome e descrição são obrigatórios.
- **Nome único no cardápio:** o cardápio não aceita dois itens com o mesmo nome.
- **Preço maior que zero:** o preço do item é maior que zero.
- **Item nasce disponível:** o item adicionado começa disponível.
- **Item mantido pelo dono:** só o usuário a quem o restaurante pertence edita, remove ou marca como indisponível um item.
- **Todos os erros juntos:** com mais de um dado inválido, todos os erros são devolvidos juntos.

## Critérios de aceite
| Critério | Quando | Então |
|:-:|---|---|
| Critério 1: cardápio válido | O id é de um restaurante que existe e que ainda não tem cardápio | O cardápio é criado sem itens. |
| Critério 2: sem restaurante | O id do restaurante não é informado | O cardápio é recusado com "Identificador do Restaurante inválido." |
| Critério 3: item válido | Nome e descrição preenchidos, nome ainda não usado no cardápio e preço maior que zero | O item é adicionado ao cardápio, disponível. |
| Critério 4: item inválido | Nome ou descrição em branco, nome já usado no cardápio ou preço igual ou menor que zero | O item é recusado e o cardápio não muda. |
| Critério 5: vários erros no item | Mais de um dado do item inválido | O item é recusado com todos os erros juntos. |
| Critério 6: restaurante inexistente | O id informado não pertence a nenhum restaurante | O cardápio é recusado e nada é criado. |
| Critério 7: segundo cardápio | O restaurante já tem cardápio | O novo cardápio é recusado e o existente não muda. |
| Critério 8: dono altera item | O usuário a quem o restaurante pertence edita, remove ou marca como indisponível um item | A alteração é feita. |
| Critério 9: outro usuário altera item | Um usuário que não é o dono tenta alterar um item | A alteração é recusada e o item não muda. |

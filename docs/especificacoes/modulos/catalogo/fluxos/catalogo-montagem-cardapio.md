---
tags: [especificacao, modulo/catalogo, fluxo/catalogo-montagem-cardapio, especificacao/rascunho]
---
# Montagem de cardápio

[Catálogo](../catalogo.md)

**Objetivo:** criar o cardápio de um restaurante e adicionar itens a ele.

## Regras
- **Cardápio nasce vazio:** o cardápio começa sem itens e pertence a um restaurante.
- **Restaurante obrigatório:** o id do restaurante é obrigatório. Mensagem: "Identificador do Restaurante inválido."
- **Itens um a um:** os itens são adicionados um a um, sempre por meio do cardápio.
- **Item completo:** nome e descrição são obrigatórios.
- **Preço não negativo:** o preço do item não pode ser negativo.
- **Item nasce disponível:** o item adicionado começa disponível.
- **Todos os erros juntos:** com mais de um dado inválido, todos os erros são devolvidos juntos.

## Critérios de aceite
| Critério | Quando | Então |
|:-:|---|---|
| Critério 1: cardápio válido | O id do restaurante é informado | O cardápio é criado sem itens. |
| Critério 2: sem restaurante | O id do restaurante não é informado | O cardápio é recusado com "Identificador do Restaurante inválido." |
| Critério 3: item válido | Nome e descrição preenchidos e preço maior ou igual a zero | O item é adicionado ao cardápio, disponível. |
| Critério 4: item inválido | Nome ou descrição em branco, ou preço negativo | O item é recusado e o cardápio não muda. |
| Critério 5: vários erros no item | Mais de um dado do item inválido | O item é recusado com todos os erros juntos. |

## Aguardando Validações
- Validação 1: o restaurante informado precisa existir? Hoje não é conferido.
- Validação 2: um restaurante pode ter mais de um cardápio?
- Validação 3: o item pode ser editado, removido ou marcado como indisponível? Por quem?
- Validação 4: dois itens do mesmo cardápio podem ter o mesmo nome?

---
tags: [especificacao, modulo/catalogo, especificacao/rascunho]
---
# Catálogo: regras de negócio

**Para que existe:** cadastrar restaurantes e, para cada um, o cardápio com seus itens.

## Termos
| Termo | Significado |
|---|---|
| Restaurante | Estabelecimento que vende comida no marketplace, identificado por nome e CNPJ. |
| Cardápio | Conjunto de itens de um restaurante. |
| Item | Produto do cardápio, com nome, descrição e preço. |
| Disponível | Item que pode ser pedido. |

## Regras
- **Nome do restaurante obrigatório:** o nome não pode ficar em branco.
- **CNPJ válido:** o CNPJ tem 14 dígitos numéricos e dígitos verificadores corretos.
- **CNPJ único:** não existem dois restaurantes com o mesmo CNPJ.
- **Restaurante nasce ativo:** todo restaurante cadastrado começa ativo.
- **Restaurante do usuário Restaurante:** só quem tem o papel Restaurante cadastra um restaurante, e ele fica ligado a esse usuário.
- **Desativar restaurante:** o usuário a quem o restaurante pertence pode desativá-lo. Restaurante inativo não recebe novos pedidos e seus itens não podem ser pedidos; pedidos em andamento seguem normalmente.
- **Cardápio de um restaurante:** cada cardápio pertence a um restaurante que existe, e cada restaurante tem um só cardápio.
- **Item completo:** todo item tem nome e descrição obrigatórios.
- **Nome único no cardápio:** não existem dois itens com o mesmo nome no mesmo cardápio.
- **Preço maior que zero:** o preço do item é maior que zero.
- **Item nasce disponível:** todo item adicionado começa disponível.
- **Item mantido pelo dono:** só o usuário a quem o restaurante pertence edita, remove ou marca como indisponível um item dele.

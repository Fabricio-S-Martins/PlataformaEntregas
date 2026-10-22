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
- **CNPJ com 14 dígitos:** o CNPJ tem exatamente 14 dígitos numéricos.
- **Restaurante nasce ativo:** todo restaurante cadastrado começa ativo.
- **Cardápio de um restaurante:** cada cardápio pertence a um restaurante.
- **Item completo:** todo item tem nome e descrição obrigatórios.
- **Preço não negativo:** o preço do item não pode ser negativo.
- **Item nasce disponível:** todo item adicionado começa disponível.

## Aguardando Validações
- Validação 1: o CNPJ precisa ter dígitos verificadores válidos, ou basta ter 14 dígitos?
- Validação 2: dois restaurantes podem ter o mesmo CNPJ?
- Validação 3: um restaurante pode ser desativado, e o que acontece com os itens e os pedidos dele?
- Validação 4: o preço do item pode ser zero?

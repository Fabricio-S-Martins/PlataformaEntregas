# Catálogo

[Documentação](../../README.md)

Cadastro de restaurantes e, para cada um, o cardápio com seus itens.

## Restaurante

- O nome é obrigatório.
- O CNPJ precisa ter exatamente 14 dígitos numéricos.
- O restaurante nasce ativo.

## Cardápio e itens

Cada cardápio pertence a um restaurante e reúne itens. Hoje, o sistema ainda não tem rotas para cardápio nem para itens, apenas as regras deles:

- Todo item tem nome e descrição obrigatórios.
- O preço do item não pode ser negativo.
- O item nasce disponível.

## Fluxos

- [Cadastro de restaurante](../../fluxos/catalogo-cadastro-restaurante.md)
- [Consulta de restaurante](../../fluxos/catalogo-consulta-restaurante.md)
- [Montagem de cardápio](../../fluxos/catalogo-montagem-cardapio.md)

## Integração

Ordem das chamadas e erros: [Integrar com Catálogo](catalogo-api.md)

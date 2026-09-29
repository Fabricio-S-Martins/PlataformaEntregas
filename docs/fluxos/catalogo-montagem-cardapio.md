# Montagem de cardápio

## Objetivo
Criar o cardápio de um restaurante e adicionar itens a ele.

> ⚠️ **Atenção:** o sistema ainda não tem rotas para cardápio nem para itens. Estas são as regras já previstas para quando existirem.

## Diagrama
```mermaid
flowchart TD
  Req["Cardápio solicitado<br/>(id do restaurante)"] --> Res{"Id do restaurante informado?"}
  Res -->|"não"| ErrC["Recusado: 'Identificador do Restaurante inválido.'"]
  Res -->|"sim"| Car["Cardápio criado, sem itens"]
  Car --> Ite["Item solicitado<br/>(nome, descrição, preço)"]
  Ite --> Val{"Dados válidos?"}
  Val -->|"não"| ErrI["Recusado, com todos os erros encontrados"]
  Val -->|"sim"| Adi["Item adicionado ao cardápio, disponível"]
```

## Regras
- O cardápio nasce vazio e pertence a um restaurante.
- O id do restaurante é obrigatório. Não é conferido se o restaurante existe.
- Os itens são adicionados um a um, sempre por meio do cardápio.
- Todo item tem nome e descrição obrigatórios.
- O preço do item não pode ser negativo.
- O item nasce disponível.
- Se houver mais de um dado inválido, todos os erros são devolvidos juntos.

Regras do módulo: [Catálogo](../modulos/catalogo/catalogo.md).

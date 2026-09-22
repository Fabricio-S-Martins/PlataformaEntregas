# Consulta de restaurante

## Objetivo
Devolver os dados de um restaurante a partir do seu id, com consulta rápida quando possível.

## Diagrama
```mermaid
flowchart TD
  Req["Consulta solicitada<br/>(id do restaurante)"] --> Cac{"Há cópia em cache?"}
  Cac -->|"sim"| Dev["Devolve o restaurante (200)"]
  Cac -->|"não"| Ban["Busca no banco"]
  Ban --> Ach{"Encontrou?"}
  Ach -->|"não"| E404["Não encontrado (404)"]
  Ach -->|"sim"| Gua["Guarda uma cópia em cache por 10 minutos"]
  Gua --> Dev
```

## Regras
- O cache é consultado primeiro. Se houver cópia, ela é devolvida sem ir ao banco.
- Sem cópia, o restaurante é buscado no banco e, se existir, uma cópia é guardada por 10 minutos.
- Restaurante inexistente retorna 404, sem corpo.
- A resposta traz o id, o nome, o CNPJ e se o restaurante está ativo.
- A consulta não altera o restaurante.

Regras do módulo: [Catálogo](../modulos/catalogo/catalogo.md).

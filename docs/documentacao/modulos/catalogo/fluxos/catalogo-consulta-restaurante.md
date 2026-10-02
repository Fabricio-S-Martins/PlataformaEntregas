---
tags: [documentacao, modulo/catalogo, fluxo/catalogo-consulta-restaurante]
---

# Consulta de restaurante

## Objetivo
Devolver os dados de um restaurante a partir do seu id, com consulta rápida e sem sobrecarregar o banco quando várias consultas chegam juntas.

## Diagrama
```mermaid
flowchart TD
  Req["Consulta solicitada<br/>(id do restaurante)"] --> Cac{"Há cópia em cache?"}
  Cac -->|"sim"| Dev["Devolve o restaurante (200)"]
  Cac -->|"não"| Tra{"Conseguiu a trava<br/>deste restaurante?"}
  Tra -->|"sim"| Rec{"Outra consulta já<br/>guardou a cópia?"}
  Rec -->|"sim"| Lib["Libera a trava"]
  Rec -->|"não"| Ban["Busca no banco"]
  Ban --> Ach{"Encontrou?"}
  Ach -->|"sim"| Gua["Guarda uma cópia em cache por 10 minutos"]
  Gua --> Lib
  Ach -->|"não"| Lib
  Lib --> Fim{"Há restaurante?"}
  Fim -->|"sim"| Dev
  Fim -->|"não"| E404["Não encontrado (404)"]
  Tra -->|"não"| Esp["Espera 200 ms e consulta o cache de novo<br/>(até 5 tentativas)"]
  Esp --> Apa{"A cópia apareceu?"}
  Apa -->|"sim"| Dev
  Apa -->|"não, esgotou as tentativas"| Dir["Busca no banco<br/>sem guardar cópia"]
  Dir --> Ach2{"Encontrou?"}
  Ach2 -->|"sim"| Dev
  Ach2 -->|"não"| E404
```

## Regras
- O cache é consultado primeiro. Se houver cópia, ela é devolvida sem ir ao banco.
- Sem cópia, só uma consulta por vez busca o mesmo restaurante no banco: a que consegue a trava. A trava dura no máximo 5 segundos e é liberada ao final, com ou sem sucesso.
- Quem consegue a trava confere o cache mais uma vez antes de ir ao banco, porque outra consulta pode ter guardado a cópia nesse intervalo.
- Quem não consegue a trava espera 200 ms e consulta o cache de novo, até 5 vezes (cerca de 1 segundo no total).
- Esgotadas as 5 tentativas sem cópia, o restaurante é buscado direto no banco, sem trava e sem guardar cópia. A resposta tem prioridade sobre evitar a busca repetida.
- Quando o restaurante é encontrado com a trava, uma cópia é guardada por 10 minutos.
- Restaurante inexistente retorna 404, sem corpo, e nenhuma cópia é guardada.
- A resposta traz o id, o nome, o CNPJ e se o restaurante está ativo.
- A consulta não altera o restaurante.

Regras do módulo: [Catálogo](../catalogo.md).

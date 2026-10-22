---
tags: [especificacao, modulo/catalogo, fluxo/catalogo-consulta-restaurante, especificacao/rascunho]
---
# Consulta de restaurante

[Catálogo](../catalogo.md)

**Objetivo:** devolver os dados de um restaurante a partir do seu id.

## Regras
- **Dados devolvidos:** a resposta traz o id, o nome, o CNPJ e se o restaurante está ativo.
- **Restaurante inexistente:** quando não há restaurante com o id, a resposta é "não encontrado", sem corpo.
- **Consulta não altera:** consultar não muda o restaurante.

## Critérios de aceite
| Critério | Quando | Então |
|:-:|---|---|
| Critério 1: restaurante existente | O id pertence a um restaurante | A resposta traz id, nome, CNPJ e se está ativo. |
| Critério 2: restaurante inexistente | O id não pertence a nenhum restaurante | A resposta é "não encontrado", sem corpo. |
| Critério 3: consultas simultâneas | Várias consultas chegam juntas para o mesmo restaurante | Todas recebem a resposta correta. |

## Aguardando Validações
- Validação 1: quanto tempo a consulta pode mostrar um dado desatualizado (hoje, até 10 minutos)? Por exemplo, um restaurante desativado pode continuar aparecendo como ativo.
- Validação 2: a consulta é pública ou exige login?

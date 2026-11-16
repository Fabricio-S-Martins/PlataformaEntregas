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
- **Consulta pública:** qualquer pessoa consulta, sem login.
- **Defasagem aceita:** a resposta pode mostrar um dado com até 10 minutos de atraso.

## Critérios de aceite
| Critério | Quando | Então |
|:-:|---|---|
| Critério 1: restaurante existente | O id pertence a um restaurante | A resposta traz id, nome, CNPJ e se está ativo. |
| Critério 2: restaurante inexistente | O id não pertence a nenhum restaurante | A resposta é "não encontrado", sem corpo. |
| Critério 3: consultas simultâneas | Várias consultas chegam juntas para o mesmo restaurante | Todas recebem a resposta correta. |
| Critério 4: consulta sem login | Chamada sem usuário autenticado | A resposta traz os dados do restaurante. |
| Critério 5: restaurante desativado | O restaurante foi desativado há menos de 10 minutos | A resposta ainda pode mostrá-lo como ativo; passados 10 minutos, mostra inativo. |

---
tags: [especificacao, modulo/autenticacao, fluxo/autenticacao-aprovacao, especificacao/rascunho]
---
# Aprovação de Restaurante e Entregador

[Autenticação](../autenticacao.md)

**Objetivo:** liberar o uso do sistema por Restaurante e Entregador recém-cadastrados, depois que um administrador os aprova.

## Regras
- **Nasce pendente:** o usuário cadastrado como Restaurante ou Entregador nasce aguardando aprovação; o Cliente nasce aprovado.
- **Aprovação só por administrador:** só um administrador aprova um usuário pendente.
- **Pendente não entra:** usuário aguardando aprovação não obtém token, mesmo com e-mail e senha corretos.
- **Aprovado entra:** depois de aprovado, o usuário faz login normalmente.

## Critérios de aceite
| Critério | Quando | Então |
|:-:|---|---|
| Critério 1: Restaurante nasce pendente | Cadastro válido com papel Restaurante ou Entregador | O usuário é criado aguardando aprovação. |
| Critério 2: Cliente nasce aprovado | Cadastro válido com papel Cliente | O usuário é criado já aprovado. |
| Critério 3: pendente não entra | Usuário aguardando aprovação faz login com e-mail e senha corretos | O login é recusado e nenhum token é devolvido. |
| Critério 4: administrador aprova | Um administrador aprova um usuário pendente | O usuário passa a aprovado. |
| Critério 5: aprovado entra | Usuário aprovado faz login com e-mail e senha corretos | Os tokens são devolvidos. |
| Critério 6: outro papel aprova | Um usuário que não é administrador tenta aprovar | A aprovação é recusada e o usuário continua pendente. |

## Aguardando Validações
- Validação 1: como nasce o primeiro administrador (cadastro comum, criação direta pela equipe)? Hoje não existe o papel administrador.

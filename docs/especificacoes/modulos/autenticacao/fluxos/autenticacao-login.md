---
tags: [especificacao, modulo/autenticacao, fluxo/autenticacao-login, especificacao/rascunho]
---
# Login

[Autenticação](../autenticacao.md)

**Objetivo:** autenticar o usuário com e-mail e senha e devolver um token de acesso.

## Regras
- **Erro genérico:** e-mail inexistente e senha incorreta geram a mesma recusa, "E-mail ou Senha inválidos.", sem indicar qual dos dois errou.
- **Token com identidade:** o token identifica o usuário (id) e o papel (Cliente, Restaurante ou Entregador).
- **Token com prazo:** o token tem prazo de validade, definido na configuração do sistema.
- **Recusa não gera token:** uma recusa não devolve token.
- **Rotas protegidas:** as rotas protegidas exigem o token.

## Critérios de aceite
| Critério | Quando | Então |
|:-:|---|---|
| Critério 1: login válido | E-mail cadastrado e senha correta | É devolvido um token com o id e o papel do usuário. |
| Critério 2: e-mail inexistente | O e-mail não pertence a nenhum usuário | O login é recusado com "E-mail ou Senha inválidos." e nenhum token é devolvido. |
| Critério 3: senha incorreta | O e-mail existe e a senha não confere | O login é recusado com a mesma mensagem do Critério 2 e nenhum token é devolvido. |
| Critério 4: token vencido | O token passou do prazo de validade | A rota protegida recusa o acesso. |
| Critério 5: sem token | Chamada a uma rota protegida sem token | A rota protegida recusa o acesso. |

## Aguardando Validações
- Validação 1: quanto tempo o token dura (por exemplo, 60 minutos)? Hoje o prazo está só na configuração.
- Validação 2: depois que o token vence, a pessoa faz login de novo ou a sessão é renovada sem pedir a senha?
- Validação 3: deve haver bloqueio por tentativas erradas (por exemplo, 5 falhas bloqueiam por 15 minutos)? Hoje não há limite.
- Validação 4: o e-mail digitado no login vale com qualquer caixa e espaços nas pontas, como no cadastro?

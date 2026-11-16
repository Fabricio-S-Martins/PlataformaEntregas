---
tags: [especificacao, modulo/autenticacao, fluxo/autenticacao-login, especificacao/rascunho]
---
# Login

[Autenticação](../autenticacao.md)

**Objetivo:** autenticar o usuário com e-mail e senha e devolver um token de acesso e um token de renovação.

## Regras
- **Erro genérico:** e-mail inexistente e senha incorreta geram a mesma recusa, "E-mail ou Senha inválidos.", sem indicar qual dos dois errou.
- **E-mail normalizado:** o e-mail digitado vale com qualquer caixa e com espaços nas pontas, como no cadastro.
- **Token com identidade:** o token de acesso identifica o usuário (id) e o papel (Cliente, Restaurante ou Entregador).
- **Token com prazo:** o token de acesso vale por 30 minutos.
- **Renovação sem senha:** o login também devolve um token de renovação, válido por 7 dias. Com ele, a pessoa obtém um novo token de acesso sem digitar a senha.
- **Renovação recusada:** token de renovação vencido ou inválido não gera token de acesso; a pessoa precisa fazer login de novo.
- **Bloqueio por tentativas:** 5 falhas seguidas de login com o mesmo e-mail bloqueiam novas tentativas desse e-mail por 15 minutos, mesmo com a senha correta. Um login válido zera a contagem de falhas.
- **Pendente não entra:** usuário Restaurante ou Entregador ainda não aprovado é recusado no login, mesmo com e-mail e senha corretos.
- **Recusa não gera token:** uma recusa não devolve token.
- **Rotas protegidas:** as rotas protegidas exigem o token de acesso.

## Critérios de aceite
| Critério | Quando | Então |
|:-:|---|---|
| Critério 1: login válido | E-mail cadastrado e senha correta | É devolvido um token de acesso com o id e o papel do usuário e um token de renovação. |
| Critério 2: e-mail inexistente | O e-mail não pertence a nenhum usuário | O login é recusado com "E-mail ou Senha inválidos." e nenhum token é devolvido. |
| Critério 3: senha incorreta | O e-mail existe e a senha não confere | O login é recusado com a mesma mensagem do Critério 2 e nenhum token é devolvido. |
| Critério 4: token vencido | O token de acesso passou de 30 minutos | A rota protegida recusa o acesso. |
| Critério 5: sem token | Chamada a uma rota protegida sem token | A rota protegida recusa o acesso. |
| Critério 6: e-mail com outra caixa | Existe `ana@x.com` e o login chega com `  Ana@X.com ` e a senha correta | O login é aceito e os tokens são devolvidos. |
| Critério 7: renovação válida | Token de renovação dentro dos 7 dias | É devolvido um novo token de acesso, sem pedir a senha. |
| Critério 8: renovação vencida | Token de renovação passou de 7 dias ou é inválido | A renovação é recusada, nenhum token de acesso é devolvido e é preciso fazer login. |
| Critério 9: bloqueio | 5 falhas seguidas de login com o mesmo e-mail | Novas tentativas com esse e-mail são recusadas por 15 minutos, mesmo com a senha correta, e nenhum token é devolvido. |
| Critério 10: fim do bloqueio | Passaram 15 minutos desde o bloqueio | O login com a senha correta volta a funcionar. |
| Critério 11: contagem zerada | Houve falhas, mas menos de 5, e depois um login válido | A contagem de falhas volta a zero. |
| Critério 12: usuário pendente | Restaurante ou Entregador ainda não aprovado faz login com e-mail e senha corretos | O login é recusado e nenhum token é devolvido. |

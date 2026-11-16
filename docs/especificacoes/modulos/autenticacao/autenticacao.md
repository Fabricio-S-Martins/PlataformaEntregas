---
tags: [especificacao, modulo/autenticacao, especificacao/rascunho]
---
# Autenticação: regras de negócio

**Para que existe:** cadastrar usuários com um papel e identificá-los no sistema por e-mail e senha.

## Termos
| Termo | Significado |
|---|---|
| Papel | Função do usuário no marketplace: Cliente, Restaurante ou Entregador. |
| E-mail normalizado | E-mail sem espaços nas pontas e em letras minúsculas. |
| Administrador | Usuário que aprova Restaurante e Entregador. |
| Token de acesso | Prova de identidade enviada nas rotas protegidas; vale por 30 minutos. |
| Token de renovação | Prova usada para obter novo token de acesso sem digitar a senha; vale por 7 dias. |

## Regras
- **Um papel por usuário:** cada usuário tem exatamente um papel, definido no cadastro.
- **E-mail único:** não existem dois usuários com o mesmo e-mail normalizado.
- **Senha forte:** a senha tem no mínimo 8 caracteres, com letra maiúscula, letra minúscula, número e caractere especial.
- **Senha protegida:** a senha nunca é guardada em texto puro.
- **Login com erro genérico:** e-mail ou senha incorretos geram a mesma mensagem, sem indicar qual dos dois errou.
- **Sessão renovável:** o token de acesso vale por 30 minutos e se renova com o token de renovação, válido por 7 dias.
- **Bloqueio por tentativas:** 5 falhas seguidas de login com o mesmo e-mail bloqueiam novas tentativas desse e-mail por 15 minutos.
- **Restaurante e Entregador com aprovação:** esses papéis só usam o sistema depois de aprovados por um administrador; o Cliente entra direto. Não há confirmação de e-mail.

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

## Regras
- **Um papel por usuário:** cada usuário tem exatamente um papel, definido no cadastro.
- **E-mail único:** não existem dois usuários com o mesmo e-mail normalizado.
- **Senha forte:** a senha tem no mínimo 8 caracteres, com letra maiúscula, letra minúscula, número e caractere especial.
- **Senha protegida:** a senha nunca é guardada em texto puro.
- **Login com erro genérico:** e-mail ou senha incorretos geram a mesma mensagem, sem indicar qual dos dois errou.

## Aguardando Validações
- Validação 1: Restaurante e Entregador precisam de aprovação, ou o e-mail precisa de confirmação, antes de usar o sistema?

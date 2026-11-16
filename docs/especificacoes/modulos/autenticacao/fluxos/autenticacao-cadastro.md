---
tags: [especificacao, modulo/autenticacao, fluxo/autenticacao-cadastro, especificacao/rascunho]
---
# Cadastro de usuário

[Autenticação](../autenticacao.md)

**Objetivo:** registrar um novo usuário com nome, e-mail, senha e papel.

## Regras
- **Nome obrigatório:** o nome não pode ficar em branco. Mensagem: "Nome inválido."
- **E-mail com formato válido:** precisa ter o formato `nome@dominio.ext`. Mensagem: "E-mail inválido."
- **E-mail ainda não cadastrado:** se o e-mail normalizado já pertence a outro usuário, o cadastro é recusado. Mensagem: "E-mail já cadastrado."
- **Papel permitido:** só Cliente, Restaurante ou Entregador.
- **Senha forte:** no mínimo 8 caracteres, com maiúscula, minúscula, número e caractere especial. Mensagem: "Senha inválida."
- **Todos os erros juntos:** com mais de um erro, as mensagens vêm juntas, separadas por quebra de linha.
- **Recusa não cria usuário:** um cadastro recusado não grava nada.
- **Login depois:** o cadastro não loga a pessoa; ela entra pelo fluxo de login.
- **Aprovação depois:** Restaurante e Entregador nascem aguardando aprovação, como descrito em [Aprovação de Restaurante e Entregador](autenticacao-aprovacao.md); o e-mail não precisa de confirmação.

## Critérios de aceite
| Critério | Quando | Então |
|:-:|---|---|
| Critério 1: cadastro válido | Nome, e-mail, senha e papel válidos, e e-mail ainda não cadastrado | O usuário é criado e o cadastro é concluído sem devolver token. |
| Critério 2: nome em branco | O nome está vazio ou só com espaços | O cadastro é recusado com "Nome inválido." e nenhum usuário é criado. |
| Critério 3: e-mail inválido | O e-mail não tem o formato `nome@dominio.ext` | O cadastro é recusado com "E-mail inválido." e nenhum usuário é criado. |
| Critério 4: e-mail já cadastrado | O e-mail normalizado já pertence a outro usuário | O cadastro é recusado com "E-mail já cadastrado." e nenhum usuário é criado. |
| Critério 5: e-mail repetido com outra caixa | Já existe `ana@x.com` e chega `  Ana@X.com ` | O cadastro é recusado com "E-mail já cadastrado.". |
| Critério 6: papel inexistente | O papel não é Cliente, Restaurante nem Entregador | O cadastro é recusado e nenhum usuário é criado. |
| Critério 7: senha fraca | A senha tem menos de 8 caracteres ou falta maiúscula, minúscula, número ou caractere especial | O cadastro é recusado com "Senha inválida." e nenhum usuário é criado. |
| Critério 8: vários erros | Mais de um dado inválido | O cadastro é recusado com todas as mensagens, uma por linha. |
| Critério 9: senha protegida | Um cadastro válido é concluído | A senha não fica guardada em texto puro. |
| Critério 10: login depois do cadastro | O cadastro foi concluído | A pessoa só obtém token ao fazer login com o e-mail e a senha cadastrados. |

## Fora do escopo
- Mais de um papel para o mesmo e-mail.
- Confirmação de e-mail.

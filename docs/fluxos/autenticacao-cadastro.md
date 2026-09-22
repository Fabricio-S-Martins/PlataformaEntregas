# Cadastro de usuário

## Objetivo
Registrar um novo usuário com nome, e-mail, senha e papel.

## Diagrama
```mermaid
flowchart TD
  Req["Cadastro solicitado<br/>(nome, e-mail, senha e papel)"] --> Sen["Protege a senha"]
  Sen --> Val{"Dados válidos?<br/>(nome, e-mail e papel)"}
  Val -->|"não"| E400["Recusa com a mensagem do erro (400)"]
  Val -->|"sim"| Sal["Salva o usuário"]
  Sal --> Ok["Cadastro concluído (201)"]
```

## Regras
- O nome não pode ficar em branco: "Nome inválido."
- O e-mail precisa ter formato válido: "E-mail inválido."
- O papel precisa ser Cliente, Restaurante ou Entregador.
- Com mais de um erro, as mensagens vêm juntas, separadas por quebra de linha.
- Um cadastro recusado não cria o usuário.

Regras do módulo: [Autenticação](../modulos/autenticacao/autenticacao.md).

---
tags: [documentacao, modulo/autenticacao, fluxo/autenticacao-login]
---

# Login

## Objetivo
Autenticar o usuário com e-mail e senha e devolver um token de acesso.

## Diagrama
```mermaid
flowchart TD
  Req["Login solicitado<br/>(e-mail e senha)"] --> Loc["Busca o usuário pelo e-mail"]
  Loc --> Ver{"Usuário existe<br/>e a senha confere?"}
  Ver -->|"não"| E400["Recusa: E-mail ou Senha inválidos (400)"]
  Ver -->|"sim"| Tok["Gera o token com o id e o papel do usuário"]
  Tok --> Ok["Devolve o token (200)"]
```

## Regras
- E-mail inexistente e senha incorreta geram a mesma recusa, "E-mail ou Senha inválidos.", sem indicar qual dos dois errou.
- O token identifica o usuário (id) e o papel (Cliente, Restaurante ou Entregador).
- O token tem prazo de validade, definido na configuração do sistema.
- Uma recusa não gera token.

Regras do módulo: [Autenticação](../autenticacao.md).

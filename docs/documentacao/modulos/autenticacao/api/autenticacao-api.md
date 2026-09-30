---
tags: [documentacao/api, modulo/autenticacao]
---

# Integrar com Autenticação

O contrato completo de cada rota (campos, tipos e status de resposta) está no Swagger da API, disponível em desenvolvimento em `/swagger`. Este guia explica a ordem das chamadas e como ler as respostas e os erros.

Regras do módulo: [Autenticação](../autenticacao.md).

## Ordem das Chamadas

Todas as rotas começam com `/api`. Para usar o sistema, cadastre o usuário, faça o login e envie o token nas rotas protegidas:

| Passo | Método | Rota | Descrição |
| :---: | :---: | :--- | :--- |
| **1** | `POST` | `/api/usuarios` | Cadastra o usuário. Corpo: `nome`, `email`, `senha` e `papel` (Cliente, Restaurante ou Entregador). |
| **2** | `POST` | `/api/login` | Autentica e devolve o token. Corpo: `email` e `senha`. |
| **3** | `GET` | `/api/usuarios/autenticado` | Devolve o `id` e o `papel` do usuário do token. Exige o token no cabeçalho `Authorization: Bearer <token>`. |

> ⚠️ **Atenção:** o cadastro e o login não exigem token. A rota do passo 3 exige.

## Respostas e Erros

| Status | Tipo | Descrição / Regra |
| :---: | :---: | :--- |
| **201** | Sucesso | Usuário cadastrado, sem corpo. Aplicável na rota `POST /api/usuarios`. |
| **200** | Sucesso | Login: `{ "token": "<token>" }`. Usuário autenticado: `{ "id": "<id>", "papel": "<papel>" }`. |
| **400** | Erro | A regra recusou a ação. O corpo é `{ "erro": "<mensagem>" }` e nada é criado.<br>*(Cadastro: "Nome inválido.", "E-mail inválido." ou papel fora dos três aceitos. Login: "E-mail ou Senha inválidos." Mensagens múltiplas vêm separadas por quebra de linha.)* |
| **401** | Erro | Rota protegida chamada sem token válido. |

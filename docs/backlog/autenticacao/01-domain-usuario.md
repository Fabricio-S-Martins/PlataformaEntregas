# 01 — Domínio: entidade Usuario

**Módulo:** Autenticação
**Camada:** Domínio
**Status:** feito ✅

## Contexto

Em Clean Architecture, a camada **Domínio** é o centro — não depende de nada (nem EF Core, nem ASP.NET, nem MediatR). Contém as regras de negócio puras. Começamos por ela porque é onde fica a essência do problema ("o que é um usuário, quais papéis ele pode ter, quais invariantes precisam ser respeitadas"), sem se preocupar ainda com banco de dados ou API.

## O que fazer

1. Criar o projeto `Modulos.Autenticacao.Dominio` (classlib), referenciado pela solution.
2. Organizar o projeto no `.slnx` em Pastas de Solução por Módulo e Camada: `Modulos > Autenticacao > Modulos.Autenticacao.Dominio`.
3. Modelar a entidade `Usuario` com:
   - Identificador (Id)
   - Nome, Email, SenhaHash (nunca a senha em texto puro)
   - Papel (`Cliente`, `Restaurante`, `Entregador`) — enum ou Value Object, a decidir
   - Construtor que valida invariantes (ex: email obrigatório, não pode criar usuário sem papel)
4. (Opcional, recomendado) Value Object `Email` que valida formato.

## Conceito novo: Value Object (VO)

Objeto que representa um valor sem identidade própria (dois VOs são iguais se os valores forem iguais) — diferente de uma Entidade, que tem identidade (Id) e pode mudar de estado ao longo do tempo. Um `Email` como VO garante que, uma vez criado, ele é sempre válido — a validação fica encapsulada em vez de espalhada pelo código.

Referência: https://learn.microsoft.com/pt-br/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/implement-value-objects

## Checklist

- [X] Projeto organizado no `.slnx` em Pastas de Solução por Módulo e Camada, seguindo `Modulos > Autenticacao`
- [X] Projeto `Modulos.Autenticacao.Dominio` criado e referenciado na solution dentro da Pasta `Autenticacao`
- [X] Entidade `Usuario` criada com invariantes validadas no construtor
- [X] Papel modelado (enum ou VO — decidir e justificar)
- [X] (Opcional) VO `Email` implementado

## Notas / decisões tomadas

_(preencher durante a implementação)_

## Histórico de dúvidas
# 02 — Domínio: testes unitários da entidade Usuario

**Módulo:** Autenticação
**Status:** todo
**Camada:** Domínio

## Contexto

A task 01 criou a entidade `Usuario` com invariantes validadas no construtor (e-mail válido, etc.). Duas vezes durante a implementação apareceram bugs justamente nessas invariantes (`throw` faltando, `Id` não gerado) que só foram pegos porque eu revisei o código manualmente. Um teste automatizado pega esse tipo de regressão sozinho, sem depender de revisão manual — e é o próximo passo natural antes de avançar para a camada de Aplicação, para termos confiança de que o Domínio está sólido.

## Conceito novo: Testes unitários (xUnit)

Teste unitário é código que verifica automaticamente se uma unidade isolada do sistema (aqui, a entidade `Usuario`) se comporta como esperado — sem banco de dados, sem API, só a lógica em si. **xUnit** é o framework de testes mais usado no ecossistema .NET moderno. A estrutura básica de um teste é *Arrange* (prepara os dados), *Act* (executa a ação), *Assert* (verifica o resultado).

Referência: https://learn.microsoft.com/pt-br/dotnet/core/testing/unit-testing-with-dotnet-test

## O que fazer

1. Criar o projeto de testes `Modulos.Autenticacao.Dominio.Testes` (xUnit), referenciado pela solution, na Pasta de Solução `Modulos > Autenticacao`.
2. Adicionar referência de projeto do `Modulos.Autenticacao.Dominio.Testes` para o `Modulos.Autenticacao.Dominio`.
3. Escrever testes cobrindo:
   - Criar um `Usuario` com dados válidos gera um `Id` diferente de `Guid.Empty`.
   - Criar um `Usuario` com e-mail inválido lança `ArgumentException`.
   - Criar um `Usuario` com e-mail válido preenche `Email.Valor` corretamente.

## Checklist

- [ ] Projeto `Modulos.Autenticacao.Dominio.Testes` criado e referenciado na solution, dentro da Pasta `Modulos > Autenticacao`
- [ ] Referência ao projeto `Modulos.Autenticacao.Dominio` adicionada
- [ ] Teste: Id gerado é diferente de `Guid.Empty`
- [ ] Teste: e-mail inválido lança `ArgumentException`
- [ ] Teste: e-mail válido é armazenado corretamente
- [ ] Todos os testes passam (`dotnet test`)

## Notas / decisões tomadas

_(preencher durante a implementação)_

## Histórico de dúvidas

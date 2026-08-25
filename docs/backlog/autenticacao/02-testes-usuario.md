# 02 — Domínio: testes unitários da entidade Usuario

**Módulo:** Autenticação
**Camada:** Domínio
**Status:** feito ✅

## Contexto

A task 01 criou a entidade `Usuario` com invariantes validadas no construtor (e-mail válido, Id gerado, etc.). Antes de avançar para a camada de Aplicação, vale garantir que essas invariantes fiquem cobertas por testes automatizados — assim qualquer alteração futura na entidade que quebre uma regra é pega imediatamente, sem depender de revisão manual.

## Conceito novo: Testes unitários (xUnit)

Teste unitário é código que verifica automaticamente se uma unidade isolada do sistema (aqui, a entidade `Usuario`) se comporta como esperado — sem banco de dados, sem API, só a lógica em si. **xUnit** é o framework de testes mais usado no ecossistema .NET moderno. A estrutura básica de um teste é *Arrange* (prepara os dados), *Act* (executa a ação), *Assert* (verifica o resultado).

Referência: https://learn.microsoft.com/pt-br/dotnet/core/testing/unit-testing-with-dotnet-test

## Conceito novo: Bogus (geração de dados fake)

**Bogus** é uma biblioteca (pacote NuGet) que gera dados fictícios realistas (nomes, e-mails, textos) para uso em testes. Em vez de escrever valores fixos "na mão" (`"joao@teste.com"`), o teste gera um e-mail válido aleatório a cada execução — isso ajuda a evitar testes que só passam por coincidência com um valor específico, e facilita gerar muitos casos de teste variados sem repetir código.

Referência: https://github.com/bchavez/Bogus

## O que fazer

1. Criar o projeto de testes `Modulos.Autenticacao.Dominio.Testes` (xUnit) na pasta `Testes/Autenticacao/`, separada da pasta `Modulos/` que tem o código de produção.
2. Referenciar o projeto na solution, na Pasta de Solução `Testes > Autenticacao` (separada de `Modulos > Autenticacao`).
3. Adicionar referência de projeto do `Modulos.Autenticacao.Dominio.Testes` para o `Modulos.Autenticacao.Dominio`.
4. Adicionar o pacote NuGet `Bogus` ao projeto de testes.
5. Criar a pasta `Entidades/` dentro do projeto de testes, espelhando a pasta `Entidades/` do projeto de produção (onde está `Usuario.cs`).
6. Mover/renomear `UnitTest1.cs` (gerado pelo template do xUnit) para `Entidades/UsuarioTestes.cs`, e renomear a classe dentro dele para `UsuarioTestes`.
7. Dentro de `UsuarioTestes.cs`, cobrir os seguintes cenários, usando o Bogus para gerar nome/e-mail válidos onde fizer sentido (não é necessário usar Bogus no cenário de e-mail inválido, já que ali o valor precisa ser deliberadamente malformado):
   - Criar um `Usuario` com dados válidos deve gerar um `Id` diferente de `Guid.Empty`.
   - Criar um `Usuario` com e-mail em formato inválido deve lançar `ArgumentException`.
   - Criar um `Usuario` com e-mail válido deve armazenar o e-mail corretamente.

## Arquivos a criar/alterar

- `Testes/Autenticacao/Modulos.Autenticacao.Dominio.Testes/Modulos.Autenticacao.Dominio.Testes.csproj` (criado pelo template)
- `Testes/Autenticacao/Modulos.Autenticacao.Dominio.Testes/Entidades/UsuarioTestes.cs` (renomeado de `UnitTest1.cs`, movido para dentro de `Entidades/`)
- `PlataformaEntregas.slnx` (projeto referenciado)

## Checklist

- [ ] Projeto `Modulos.Autenticacao.Dominio.Testes` criado em `Testes/Autenticacao/` (separado de `Modulos/`)
- [ ] Projeto referenciado na solution, dentro da Pasta `Testes > Autenticacao`
- [ ] Referência ao projeto `Modulos.Autenticacao.Dominio` adicionada
- [ ] Pacote NuGet `Bogus` adicionado ao projeto de testes
- [ ] `UnitTest1.cs` movido para `Entidades/UsuarioTestes.cs` (arquivo e classe renomeados)
- [ ] Cenário: dados válidos geram Id diferente de `Guid.Empty` — coberto
- [ ] Cenário: e-mail inválido lança `ArgumentException` — coberto
- [ ] Cenário: e-mail válido é armazenado corretamente — coberto
- [ ] Todos os testes passam (`dotnet test`)

## Notas / decisões tomadas

_(preencher durante a implementação)_

## Histórico de dúvidas

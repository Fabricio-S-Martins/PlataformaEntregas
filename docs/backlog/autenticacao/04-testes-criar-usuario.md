# 04 — Aplicação: testes do caso de uso Criar Usuário

**Módulo:** Autenticação
**Camada:** Aplicação
**Status:** feito

## Contexto

A task 03 criou o `CriarUsuarioHandler`, que orquestra a criação de um `Usuario` (com hash de senha) e delega a persistência para `IUsuarioRepositorio`. Como ainda não existe uma implementação real do repositório (isso é Infraestrutura, task futura), o handler só pode ser testado com uma implementação falsa da interface — é exatamente o cenário em que testes com *mock* fazem sentido.

## Conceito novo: Mocking (com a lib Moq)

Um **mock** é um objeto falso que simula o comportamento de uma dependência real (aqui, `IUsuarioRepositorio`), permitindo testar uma classe isoladamente sem precisar de banco de dados de verdade. **Moq** é a biblioteca de mocking mais usada no ecossistema .NET: você cria um mock da interface, configura o que ele deve retornar/fazer quando um método é chamado, e depois consegue verificar se aquele método foi chamado (quantas vezes, com quais argumentos).

Referência: https://github.com/devlooped/moq/wiki/Quickstart

## O que fazer

1. Criar o projeto de testes `Modulos.Autenticacao.Aplicacao.Testes` (xUnit) em `Testes/Autenticacao/Modulos.Autenticacao.Aplicacao.Testes/`.
2. Adicionar referência de projeto do `Modulos.Autenticacao.Aplicacao.Testes` para o `Modulos.Autenticacao.Aplicacao`.
3. Adicionar o pacote NuGet `Moq` ao projeto de testes.
4. Excluir o arquivo `UnitTest1.cs` (gerado pelo template do xUnit).
5. Criar a pasta `CasosDeUso/CriarUsuario/` dentro do projeto de testes, espelhando a estrutura do projeto de produção.
6. Dentro dessa pasta, criar `CriarUsuarioHandlerTestes.cs`, cobrindo os seguintes cenários:
   - Ao processar um Command com dados válidos, o Handler deve chamar o método de adicionar do `IUsuarioRepositorio` exatamente uma vez.

## Arquivos a criar/alterar

- `Testes/Autenticacao/Modulos.Autenticacao.Aplicacao.Testes/Modulos.Autenticacao.Aplicacao.Testes.csproj` (novo projeto)
- `Testes/Autenticacao/Modulos.Autenticacao.Aplicacao.Testes/CasosDeUso/CriarUsuario/CriarUsuarioHandlerTestes.cs`

## Checklist

- [ ] Projeto `Modulos.Autenticacao.Aplicacao.Testes` criado em `Testes/Autenticacao/Modulos.Autenticacao.Aplicacao.Testes/`
- [ ] Referência ao projeto `Modulos.Autenticacao.Aplicacao` adicionada
- [ ] Pacote NuGet `Moq` adicionado
- [ ] `UnitTest1.cs` excluído
- [ ] Cenário: repositório é chamado exatamente uma vez ao processar um Command válido — coberto
- [ ] Cenário: senha persistida está hasheada, diferente da senha em texto puro do Command — coberto
- [ ] Todos os testes passam (`dotnet test`)

## Notas / decisões tomadas

_(preencher durante a implementação)_

## Histórico de dúvidas

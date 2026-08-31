# 07 — API: endpoint de cadastro de usuário com Minimal API

**Módulo:** Autenticação
**Camada:** API
**Status:** feito

## Contexto

Com Domínio, Aplicação e Infraestrutura prontos (tasks 01-06), falta expor o caso de uso "Criar Usuário" como um endpoint HTTP. Este projeto também é o primeiro **host** real da aplicação — o que destrava, em task futura, a fábrica de design-time (`AutenticacaoDbContextFabrica`) e a primeira Migration, que ficaram pendentes na task 06 por falta de um host com configuração.

## Conceito novo: ASP.NET Core Web API com Minimal APIs

Minimal APIs é o estilo mais moderno de expor endpoints HTTP no ASP.NET Core — sem classes de Controller, os endpoints são registrados como funções associadas a uma rota. Combina bem com CQRS: cada endpoint despacha um `Command` via `IMediator.Send(...)` e fica só como porta de entrada fina, sem lógica de negócio.

Referência: https://learn.microsoft.com/pt-br/aspnet/core/fundamentals/minimal-apis

## Conceito novo: Swagger (OpenAPI)

Swagger é uma interface web que lê a descrição dos endpoints da API (formato OpenAPI) e gera automaticamente uma página onde dá pra ver todas as rotas disponíveis e testá-las direto do navegador, sem precisar de Postman/Insomnia. Fica ativo só em ambiente de Desenvolvimento — não faz sentido expor essa página em produção.

Referência: https://learn.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger

## Decisão de design

O endpoint recebe um DTO próprio da API (`CriarUsuarioRequest`), não o `CriarUsuarioCommand` diretamente — isso segue o princípio de não acoplar o contrato HTTP (o que o cliente da API envia) à forma interna do Command da Aplicação, que pode mudar por motivos que não têm nada a ver com a API. O endpoint mapeia `CriarUsuarioRequest` → `CriarUsuarioCommand` antes de despachar via MediatR.

## O que fazer

1. Criar o projeto `Modulos.Autenticacao.Api` (ASP.NET Core Web API, Minimal API) em `Modulos/Autenticacao/Modulos.Autenticacao.Api/`.
2. Adicionar o pacote NuGet `MediatR.Extensions.Microsoft.DependencyInjection` (ou o próprio `MediatR`, se a versão já incluir a extensão) ao projeto, caso necessário para o registro do `IMediator`.
3. Configurar `appsettings.Development.json` (fora do controle de versão) com a connection string real do Postgres local; `appsettings.json` deve conter só a estrutura/chave, sem credencial.
4. No `Program.cs`, chamar os métodos de extensão de DI já existentes: o de registro da Aplicação (task 03) e o de registro da Infraestrutura (task 06), passando a `IConfiguration` do host.
5. Criar `CriarUsuarioRequest` em `Endpoints/CriarUsuario/`, com os campos que o cliente HTTP envia (nome, email, senha, papel).
6. Criar `CriarUsuarioEndpoint` em `Endpoints/CriarUsuario/`, com um método de extensão de `IEndpointRouteBuilder` que mapeia a rota HTTP, mapeia `CriarUsuarioRequest` para `CriarUsuarioCommand`, despacha via `IMediator.Send(...)` e retorna uma resposta HTTP apropriada (sucesso e erro de validação).
7. No `Program.cs`, chamar o método de extensão do endpoint criado no passo anterior.
8. Adicionar o pacote NuGet `Swashbuckle.AspNetCore` ao projeto, e configurar o Swagger no `Program.cs` — ativo somente quando o ambiente for Desenvolvimento (`app.Environment.IsDevelopment()`), nunca em produção.

## Arquivos a criar/alterar

- `Modulos/Autenticacao/Modulos.Autenticacao.Api/Modulos.Autenticacao.Api.csproj` (novo projeto)
- `Modulos/Autenticacao/Modulos.Autenticacao.Api/Program.cs`
- `Modulos/Autenticacao/Modulos.Autenticacao.Api/appsettings.json` (raiz do projeto)
- `Modulos/Autenticacao/Modulos.Autenticacao.Api/appsettings.Development.json` (raiz do projeto, fora do controle de versão)
- `Modulos/Autenticacao/Modulos.Autenticacao.Api/Endpoints/CriarUsuario/CriarUsuarioRequest.cs`
- `Modulos/Autenticacao/Modulos.Autenticacao.Api/Endpoints/CriarUsuario/CriarUsuarioEndpoint.cs`
- `.gitignore` (adicionar `appsettings.Development.json`, se ainda não estiver coberto por padrão genérico)

## Checklist

- [ ] Projeto `Modulos.Autenticacao.Api` criado em `Modulos/Autenticacao/Modulos.Autenticacao.Api/`
- [ ] `appsettings.Development.json` com connection string real, fora do controle de versão
- [ ] `Program.cs` registra DI da Aplicação e da Infraestrutura
- [ ] `CriarUsuarioRequest` criado em `Endpoints/CriarUsuario/`
- [ ] `CriarUsuarioEndpoint` criado em `Endpoints/CriarUsuario/`, mapeando request → command, despachando via `IMediator`
- [ ] Endpoint registrado no `Program.cs`
- [ ] Cenário: requisição válida cria o usuário e retorna sucesso — coberto (teste manual ou automatizado, a critério de quem implementa)
- [ ] Cenário: e-mail inválido retorna erro apropriado ao cliente (não uma exceção não tratada) — coberto
- [ ] Swagger configurado e acessível apenas em ambiente de Desenvolvimento

## Notas / decisões tomadas

- Endpoint usa DTO próprio (`CriarUsuarioRequest`), não expõe o `CriarUsuarioCommand` diretamente na API.

## Histórico de dúvidas

# 07 — Expor endpoint de cadastro de Restaurante e gerar a Migration inicial do Catálogo

**Módulo:** Catálogo
**Camada:** API / Infraestrutura
**Status:** feito

## Contexto

Com Domínio, Aplicação e Infraestrutura do Catálogo prontos (tasks 01–06) e o host único no lugar (Compartilhado 02), falta expor o caso de uso "Cadastrar Restaurante" como endpoint HTTP e criar a tabela no banco. Segue o mesmo padrão de Autenticação: uma biblioteca `Modulos.Catalogo.Api` (endpoints + registro), plugada no host via os dois métodos de extensão do Composition Root.

O `CatalogoDbContext` ainda não tem nenhuma Migration — a tabela `Restaurantes` não existe no banco. O `Microsoft.EntityFrameworkCore.Design` já está no host (Compartilhado 02), então dá pra gerar a Migration usando o host como startup project.

## Decisão de design

O endpoint recebe um DTO próprio da API (`CadastrarRestauranteRequest`), não o `CadastrarRestauranteCommand` diretamente — mesmo princípio de Autenticação: não acoplar o contrato HTTP à forma interna do Command. O endpoint mapeia `CadastrarRestauranteRequest` → `CadastrarRestauranteCommand` antes de despachar via MediatR.

## Contrato do endpoint

- **Rota:** `POST /api/restaurantes`
- **Entrada:** body JSON `CadastrarRestauranteRequest { string Nome, string Cnpj }`
- **Saída:** sem corpo
- **Respostas HTTP:**
  - `201 Created` — restaurante cadastrado
  - `400 Bad Request` — `{ erro: string }` quando `Restaurante.Criar` falha (nome vazio, cnpj inválido)

## O que fazer

**Biblioteca de API do módulo Catálogo:**

1. Na pasta `Modulos/Catalogo/`, criar a pasta `Modulos.Catalogo.Api/` e, dentro dela, o projeto `Modulos.Catalogo.Api` (`Microsoft.NET.Sdk` + `<FrameworkReference Include="Microsoft.AspNetCore.App" />`).
2. Dentro desse projeto, criar a pasta `Endpoints/CadastrarRestaurante/`.
3. Nessa pasta, criar o record `CadastrarRestauranteRequest(string Nome, string Cnpj)`.
4. Nessa pasta, criar `CadastrarRestauranteEndpoint` com um método de extensão de `IEndpointRouteBuilder` que mapeia `POST /restaurantes`, converte `CadastrarRestauranteRequest` em `CadastrarRestauranteCommand`, despacha via `IMediator.Send(...)`, retorna `201 Created` no sucesso e `400 Bad Request` com `{ erro }` ao capturar `ArgumentException`.
5. Na raiz do projeto, criar a classe `InjecaoDeDependencia` com o método de extensão `RegistrarCatalogoApi(this IServiceCollection services, IConfiguration configuration)`, chamando `RegistrarCatalogoInfraestrutura` e `RegistrarCatalogoAplicacao`.
6. Na raiz do projeto, criar a classe `CatalogoEndpoints` com o método de extensão `MapCatalogoEndpoints(this IEndpointRouteBuilder rotas)`, chamando o mapeamento do passo 4.

**Plugar no host:**

7. No `Program.cs` do host, chamar `builder.Services.RegistrarCatalogoApi(builder.Configuration)` e, no grupo `/api`, `MapCatalogoEndpoints()`.

**Migration inicial do Catálogo:**

8. Gerar a Migration inicial do `CatalogoDbContext`, nomeada `InicialRestaurante`, com `Modulos.Catalogo.Infraestrutura` como projeto alvo e `PlataformaEntregas.Api` como startup project.

## Cenários a cobrir

Sem testes automatizados nesta task — validar rodando o host:
- `POST /api/restaurantes` com nome e cnpj válidos retorna `201` e grava a linha na tabela `Restaurantes`.
- `POST /api/restaurantes` com nome vazio ou cnpj inválido retorna `400` com `{ erro }`, sem gravar nada.

## Arquivos a criar/alterar

- `Modulos/Catalogo/Modulos.Catalogo.Api/Modulos.Catalogo.Api.csproj` (novo projeto)
- `Modulos/Catalogo/Modulos.Catalogo.Api/Endpoints/CadastrarRestaurante/CadastrarRestauranteRequest.cs`
- `Modulos/Catalogo/Modulos.Catalogo.Api/Endpoints/CadastrarRestaurante/CadastrarRestauranteEndpoint.cs`
- `Modulos/Catalogo/Modulos.Catalogo.Api/InjecaoDeDependencia.cs`
- `Modulos/Catalogo/Modulos.Catalogo.Api/CatalogoEndpoints.cs`
- `Modulos/Catalogo/Modulos.Catalogo.Infraestrutura/Migrations/*` (gerados pela Migration)
- `Host/PlataformaEntregas.Api/PlataformaEntregas.Api.csproj` (referência nova)
- `Host/PlataformaEntregas.Api/Program.cs`
- `PlataformaEntregas.slnx`

## Notas / decisões tomadas

- Endpoint usa DTO próprio (`CadastrarRestauranteRequest`), não expõe o `CadastrarRestauranteCommand` na API.
- `Modulos.Catalogo.Api` referencia tanto `Modulos.Catalogo.Aplicacao` quanto `Modulos.Catalogo.Infraestrutura` — a camada de API do módulo também é o mini composition root dele (é o que permite o `RegistrarCatalogoApi` chamar `RegistrarCatalogoInfraestrutura`), mesmo arranjo já usado em `Modulos.Autenticacao.Api`.
- `.csproj` do `Modulos.Catalogo.Api` segue o padrão da solution: `ImplicitUsings` habilitado, `Nullable` desabilitado.
- A Migration roda com o host como startup project — não precisa de fábrica de design-time (`IDesignTimeDbContextFactory`), mesma abordagem que Autenticação acabou usando.
- Aplicar a Migration no banco (`dotnet ef database update`) é passo de execução, não de código — fica a critério de quem roda o ambiente.

## Histórico de dúvidas

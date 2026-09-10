# 02 — Criar host único da API e converter Modulos.Autenticacao.Api em biblioteca

**Módulo:** Compartilhado / Autenticação
**Camada:** API
**Status:** feito

## Contexto

Hoje o `Modulos.Autenticacao.Api` acumula dois papéis: é o **host** da aplicação (tem `Program.cs`, abre a porta, roda o processo) e ao mesmo tempo é dono dos endpoints de Autenticação. Num monolito modular, o certo é separar: um host único que compõe todos os módulos, e cada módulo com sua biblioteca de API (só endpoints + registro), sem `Program.cs` próprio.

Sem essa separação, adicionar o Catálogo (Catálogo 07) obrigaria a escolher entre pôr endpoints de Catálogo dentro do projeto de Autenticação (acoplamento errado) ou criar um segundo host (viraria quase-microserviço, contra o que o CLAUDE.md define).

## Conceito: Composition Root

O **Composition Root** é o único lugar da aplicação onde os módulos são montados juntos — onde o host conhece todos os módulos e chama o registro de DI e de endpoints de cada um. Os módulos não se conhecem entre si; só o host os conhece. Cada módulo expõe dois métodos de extensão: um de `IServiceCollection` (registra seus serviços) e um de `IEndpointRouteBuilder` (mapeia suas rotas).

## O que fazer

**Criar o host único:**

1. Na raiz do repositório, criar a pasta `Host/`. Dentro dela, criar a pasta `PlataformaEntregas.Api/` e, dentro dela, o projeto `PlataformaEntregas.Api` (ASP.NET Core Web API, Minimal API), com referência de projeto para `Modulos.Autenticacao.Api`.
2. Adicionar ao `.csproj` do host os pacotes `Swashbuckle.AspNetCore` e `Microsoft.EntityFrameworkCore.Design` (este com `PrivateAssets=all`, igual está hoje no `.Api`).
3. Mover o `appsettings.Development.json` de `Modulos.Autenticacao.Api` para a raiz do projeto `PlataformaEntregas.Api` (continua coberto pelo `.gitignore`, que já ignora `appsettings.*.json` globalmente). O `appsettings.json` do host (só `Logging`/`AllowedHosts`) pode ser o que o template já gera — não precisa mover o de Autenticação.
4. No `.slnx`, criar a pasta de solução `/Host/` (irmã de `/Modulos/`, não dentro dela, porque o host não é um módulo) e referenciar `PlataformaEntregas.Api` ali dentro.

**Converter `Modulos.Autenticacao.Api` em biblioteca:**

5. No `.csproj`, trocar o SDK de `Microsoft.NET.Sdk.Web` para `Microsoft.NET.Sdk` e adicionar `<FrameworkReference Include="Microsoft.AspNetCore.App" />` para manter acesso aos tipos de ASP.NET.
6. Tirar do `.csproj` os pacotes `Swashbuckle.AspNetCore` e `Microsoft.EntityFrameworkCore.Design` (agora estão no host). Conferir se `Microsoft.AspNetCore.OpenApi` ainda é usado — nada chama `AddOpenApi()`, então provavelmente é só remover. Manter `Microsoft.AspNetCore.Authentication.JwtBearer`.
7. Remover o `Program.cs`.
8. Criar, na raiz de `Modulos.Autenticacao.Api`, a classe `InjecaoDeDependencia` com o método de extensão `RegistrarAutenticacaoApi(this IServiceCollection services, IConfiguration configuration)`, concentrando o que hoje está no `Program.cs` ligado a Autenticação: `RegistrarAutenticacaoInfraestrutura`, `RegistrarAutenticacaoAplicacao`, `AddAuthentication(...).AddJwtBearer(...)` (com os `TokenValidationParameters` atuais) e `AddAuthorization()`.
9. Criar, na raiz de `Modulos.Autenticacao.Api`, a classe `AutenticacaoEndpoints` com o método de extensão `MapAutenticacaoEndpoints(this IEndpointRouteBuilder rotas)`, chamando os três mapeamentos já existentes (`MapUsuariosEndpoints`, `MapLoginEndPoint`, `MapObterUsuarioAutenticadoEndpoint`).

**Montar o `Program.cs` do host:**

10. Registrar serviços: `builder.Services.RegistrarAutenticacaoApi(builder.Configuration)`, `AddEndpointsApiExplorer()` e o `AddSwaggerGen(...)` com a `AddSecurityDefinition`/`AddSecurityRequirement` de Bearer que hoje está no `Program.cs` de Autenticação.
11. Montar o pipeline: `UseAuthentication()`, `UseAuthorization()`, `MapGroup("/api")` chamando `MapAutenticacaoEndpoints()`, Swagger só quando `app.Environment.IsDevelopment()`, e `UseHttpsRedirection()`.

**Atualizar a doc:**

12. Atualizar a seção "Estado atual" do `CLAUDE.md` para refletir que o host da aplicação passou a ser o `PlataformaEntregas.Api`.

## Cenários a cobrir

Sem testes automatizados — é refactor de estrutura, sem mudança de comportamento. Validar rodando o host: `POST /api/usuarios` (cadastro), `POST /api/login` e `GET /api/usuarios/autenticado` (com e sem token) devem responder igual ao que respondiam antes.

## Arquivos a criar/alterar

- `Modulos/Autenticacao/Modulos.Autenticacao.Api/Modulos.Autenticacao.Api.csproj` (vira biblioteca)
- `Modulos/Autenticacao/Modulos.Autenticacao.Api/Program.cs` (removido)
- `Modulos/Autenticacao/Modulos.Autenticacao.Api/InjecaoDeDependencia.cs` (novo, raiz do projeto)
- `Modulos/Autenticacao/Modulos.Autenticacao.Api/AutenticacaoEndpoints.cs` (novo, raiz do projeto)
- `Modulos/Autenticacao/Modulos.Autenticacao.Api/appsettings.json` (removido — fica só o do host)
- `Modulos/Autenticacao/Modulos.Autenticacao.Api/appsettings.Development.json` (movido para o host)
- `Host/PlataformaEntregas.Api/PlataformaEntregas.Api.csproj` (novo projeto)
- `Host/PlataformaEntregas.Api/Program.cs` (novo)
- `Host/PlataformaEntregas.Api/appsettings.Development.json` (movido, fora do controle de versão)
- `PlataformaEntregas.slnx`
- `CLAUDE.md` (seção "Estado atual")

## Notas / decisões tomadas

- Host único, não um host por módulo — mantém o monolito modular do CLAUDE.md; extrair um módulo como serviço separado continua sendo épico futuro.
- `Microsoft.EntityFrameworkCore.Design` vai pro host porque o design-time do EF Core roda contra o startup project — é isso que destrava a Migration inicial do Catálogo (Catálogo 07).
- A `AddSecurityDefinition`/`AddSecurityRequirement` de Bearer no Swagger fica no host — é configuração da documentação da API inteira, não de um módulo.
- `Modulos.Autenticacao.Api` continua sendo o nome da biblioteca de API do módulo; o `.Api` agora significa "camada de API do módulo", não "host".
- O `.csproj` do host segue o padrão do resto da solution: `ImplicitUsings` habilitado, `Nullable` desabilitado (o `Modulos.Autenticacao.Api` atual usa o contrário disso, e não vale arrastar essa divergência pro projeto novo).

## Histórico de dúvidas

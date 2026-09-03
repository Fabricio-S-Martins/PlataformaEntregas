# 09 — API: autenticação JWT no pipeline e autorização por papel

**Módulo:** Autenticação
**Camada:** API
**Status:** todo

## Contexto

Cadastro (task 07) e login com geração de JWT (task 08) já funcionam, mas o token gerado no login não é validado em lugar nenhum ainda — nenhum endpoint da API sabe ler um token recebido, e o Papel embutido nas claims não trava nada. Falta a última peça do módulo Autenticação: configurar o pipeline pra validar o JWT recebido em cada requisição e restringir endpoints por papel. Isso é pré-requisito direto pro módulo Pedidos, onde regras como "só um Restaurante aceita um pedido" e "só um Entregador marca como saiu para entrega" dependem de saber, com garantia da própria API, quem está autenticado e qual o papel dele.

## Conceito novo: autenticação vs. autorização no ASP.NET Core

São duas etapas distintas do pipeline HTTP. **Autenticação** (`app.UseAuthentication()`) lê o token JWT do cabeçalho `Authorization: Bearer ...`, valida a assinatura e monta o `ClaimsPrincipal` da requisição (quem é o usuário). **Autorização** (`app.UseAuthorization()`) decide, a partir desse `ClaimsPrincipal` já montado, se a requisição pode acessar aquele endpoint específico — no caso de restrição por papel, checando se a claim de Role bate com o que o endpoint exige. A ordem das duas chamadas no pipeline importa: autenticação sempre antes de autorização.

Referência: https://learn.microsoft.com/pt-br/aspnet/core/security/authentication/jwt-authn
Referência (autorização por papel): https://learn.microsoft.com/pt-br/aspnet/core/security/authorization/roles

## Decisão de design

Como ainda não existe nenhum endpoint que precise de um papel específico (isso só vai aparecer de fato no módulo Pedidos), esta task cria um endpoint mínimo — `GET /api/usuarios/autenticado` — só pra provar que a autenticação e a extração de claims funcionam ponta a ponta: qualquer usuário autenticado (independente do papel) consegue chamá-lo e recebe de volta o próprio Id e Papel, lidos do token, sem tocar no banco. Restrição por papel específico (`.RequireAuthorization(role)`) fica demonstrada nesse mesmo endpoint via um parâmetro de rota simples, não introduzindo um caso de uso de negócio novo só pra isso.

## Contrato do endpoint

`GET /api/usuarios/autenticado`

- **Entrada:** nenhuma (sem body, sem parâmetros de rota/query). A única informação exigida é o cabeçalho `Authorization: Bearer <token>` — por isso não há `Request` (diferente de `CriarUsuarioRequest`/`LoginRequest`, que carregam dados enviados pelo cliente).
- **Saída — `UsuarioAutenticadoResponse`:**
  - `Id` (Guid) — extraído da claim `sub` do token.
  - `Papel` (string) — extraído da claim de Role do token.
- **Respostas HTTP:** `200 OK` com o corpo acima quando autenticado; `401 Unauthorized` sem corpo quando o token estiver ausente, expirado ou com assinatura inválida.

## O que fazer

1. Adicionar o pacote NuGet `Microsoft.AspNetCore.Authentication.JwtBearer` ao projeto `Modulos.Autenticacao.Api`.
2. Extrair a configuração dos parâmetros de validação do token (chave, issuer, audience) pra um método de extensão reutilizável, evitando repetir a leitura de `IConfiguration` que já existe em `TokenServico`.
3. No `Program.cs`, registrar `AddAuthentication` com esquema JWT Bearer e `AddJwtBearer`, usando o método de extensão do passo anterior. Definir também `options.MapInboundClaims = false` (evita o remapeamento automático de nomes de claim do .NET, que faria a leitura de `sub` no passo 9 falhar mesmo com token válido).
4. No `Program.cs`, registrar `AddAuthorization`.
5. No `Program.cs`, chamar `app.UseAuthentication()` antes de `app.UseAuthorization()`, e ambos antes do mapeamento dos grupos de endpoint.
6. Criar `UsuarioAutenticadoResponse` em `Endpoints/ObterUsuarioAutenticado/`, como `record` com os campos Id e Papel — mesmo estilo de `CriarUsuarioRequest`/`LoginRequest`.
7. Criar `ObterUsuarioAutenticadoEndpoint` em `Endpoints/ObterUsuarioAutenticado/`, com um método de extensão de `IEndpointRouteBuilder` que mapeia a rota `GET /usuarios/autenticado` e exige autenticação via `.RequireAuthorization()`.
8. No handler da rota criada no passo anterior, ler o Id e o Papel de `HttpContext.User` e devolver um `UsuarioAutenticadoResponse` preenchido com esses dados.
9. No `Program.cs`, chamar o método de extensão do endpoint criado no passo 7, dentro do grupo `/api`.
10. No `AddSwaggerGen`, configurar `AddSecurityDefinition`/`AddSecurityRequirement` com esquema Bearer, pra habilitar o botão "Authorize" no Swagger UI (necessário pra testar manualmente qualquer endpoint protegido).

## Arquivos a criar/alterar

- `Modulos/Autenticacao/Modulos.Autenticacao.Api/Modulos.Autenticacao.Api.csproj` (alterado — pacote `Microsoft.AspNetCore.Authentication.JwtBearer`)
- `Modulos/Autenticacao/Modulos.Autenticacao.Api/Program.cs` (alterado)
- `Modulos/Autenticacao/Modulos.Autenticacao.Api/Endpoints/ObterUsuarioAutenticado/ObterUsuarioAutenticadoEndpoint.cs`
- `Modulos/Autenticacao/Modulos.Autenticacao.Api/Endpoints/ObterUsuarioAutenticado/UsuarioAutenticadoResponse.cs`

## Cenários a cobrir

- Requisição a `GET /api/usuarios/autenticado` sem cabeçalho `Authorization` retorna 401.
- Requisição com token JWT válido (obtido via `/api/login`) retorna 200 com Id e Papel corretos do usuário autenticado.
- Requisição com token expirado ou com assinatura inválida retorna 401.

## Notas / decisões tomadas

- `ObterUsuarioAutenticadoEndpoint` não passa pelo MediatR — não há Command/Query envolvido, é leitura direta das claims do token já validado pelo pipeline, então despachar via `IMediator` seria uma camada sem função aqui.
- Restrição por papel específico (ex: `.RequireAuthorization(policy => policy.RequireRole("Restaurante"))`) só vai aparecer de fato em endpoints de negócio do módulo Pedidos; aqui serve só de prova de conceito, a critério de quem implementa incluir ou não uma variação com papel restrito no `ObterUsuarioAutenticadoEndpoint`.
- O handler não trata Id/Papel nulos: as claims `sub` e Role são sempre incluídas por `TokenServico` na geração do token, e `.RequireAuthorization()` já garante que só um token com assinatura válida (logo, emitido pelo próprio `TokenServico`) chega ao handler — não existe caminho pra essas claims virem ausentes. Se essa garantia mudar no futuro (ex: `TokenServico` passar a omitir alguma claim), o handler vai lançar exceção não tratada nesse ponto — risco aceito, não coberto defensivamente aqui.

## Histórico de dúvidas

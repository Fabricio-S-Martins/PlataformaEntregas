# 08 — Criar fluxo de login com JWT e extrair serviço de hash de senha

**Módulo:** Autenticação
**Camada:** Aplicação, Infraestrutura, API
**Status:** feito

## Contexto

Com o cadastro funcionando (tasks 01-07), o próximo passo do roadmap do módulo é o Login — autenticar um usuário existente (e-mail + senha) e devolver um token que a API vai usar depois pra saber quem é o usuário e qual o papel dele, sem precisar consultar o banco a cada requisição.

## Conceito novo: JWT (JSON Web Token)

JWT é um formato de token auto-contido: depois que o servidor autentica o usuário uma vez (login), ele gera um token assinado digitalmente contendo informações do usuário (ex: Id, Papel) — o cliente manda esse token em cada requisição futura, e o servidor só precisa validar a assinatura (sem consultar o banco de novo) pra saber que aquele token é legítimo e extrair quem é o usuário. É o mecanismo padrão de autenticação stateless em APIs REST.

Referência: https://jwt.io/introduction
Referência (uso no ASP.NET Core): https://learn.microsoft.com/en-us/aspnet/core/security/authentication/jwt-authn

## Decisão de design

A lógica de hash de senha, hoje presa dentro do `CriarUsuarioHandler`, vira um serviço reaproveitável: interface `ISenhaServico` (Aplicação) com método de gerar hash e método de verificar uma senha em texto puro contra um hash — usado tanto pelo `CriarUsuarioHandler` (ajustado nesta task) quanto pelo novo `LoginHandler`. Segue o mesmo raciocínio de Inversão de Dependência já usado em `IUsuarioRepositorio`.

## O que fazer

### Aplicação (projeto `Modulos.Autenticacao.Aplicacao`)

1. Criar a interface `ISenhaServico` em `Servicos/`, com um método pra gerar hash a partir de uma senha em texto puro, e outro pra verificar se uma senha em texto puro corresponde a um hash existente. **Verificação reaproveita o salt já armazenado, nunca gera um novo.**
2. Criar a implementação `SenhaServico` em `Servicos/`, reaproveitando a lógica de hash com salt que já existe no `CriarUsuarioHandler` (PBKDF2).
3. Ajustar `CriarUsuarioHandler` (em `CasosDeUso/CriarUsuario/`) para usar `ISenhaServico`, injetando em vez do método privado de hash.
4. Adicionar à interface `IUsuarioRepositorio` (em `Repositorios/`) um método para buscar um `Usuario` pelo e-mail (retornando `null`/nulo se não encontrado).
5. Criar a interface `ITokenServico` em `Servicos/`, com um método que recebe um `Usuario` e retorna o JWT gerado (string). O token deve carregar, entre as claims: o Id do usuário (claim `sub`), o Papel, e uma data de expiração — sem data de expiração, um token vazado nunca perde validade.
6. Criar o Command `LoginCommand` em `CasosDeUso/Login/`, como `IRequest<string>`, recebendo e-mail e senha em texto puro.
7. Criar o Handler `LoginHandler` em `CasosDeUso/Login/`: busca o usuário pelo e-mail, verifica a senha via `ISenhaServico`, gera o token via `ITokenServico` e o retorna. Erro genérico se e-mail não existir ou senha não bater.

### Infraestrutura (projeto `Modulos.Autenticacao.Infraestrutura`)

8. Implementar em `UsuarioRepositorio` (em `Persistencia/Repositorios/`) o método de busca por e-mail adicionado na interface.
9. Adicionar o pacote NuGet `System.IdentityModel.Tokens.Jwt` ao projeto (necessário para montar e assinar o JWT).
10. Criar a implementação `TokenServico` em `Servicos/`, usando a chave de assinatura JWT vinda de configuração (`IConfiguration`), não hardcoded.
11. Registrar `ISenhaServico`, `ITokenServico` e a query de e-mail no `InjecaoDeDependencia.cs` de Aplicação e Infraestrutura, conforme a camada de cada um.

### API (projeto `Modulos.Autenticacao.Api`)

12. Adicionar a chave de assinatura JWT e o tempo de expiração do token (valores de desenvolvimento) em `appsettings.Development.json` — nunca no `appsettings.json` commitado.
13. Criar `LoginRequest` (record) em `Endpoints/Login/`, com e-mail e senha.
14. Criar `LoginEndpoint` em `Endpoints/Login/`, mapeando a rota HTTP, convertendo `LoginRequest` → `LoginCommand`, despachando via `IMediator`, e retornando o token em caso de sucesso ou erro apropriado (não exceção não tratada) em caso de credenciais inválidas.
15. Registrar o novo endpoint no `Program.cs`.

## Arquivos a criar/alterar

- `Modulos/Autenticacao/Modulos.Autenticacao.Aplicacao/Servicos/ISenhaServico.cs`
- `Modulos/Autenticacao/Modulos.Autenticacao.Aplicacao/Servicos/SenhaServico.cs`
- `Modulos/Autenticacao/Modulos.Autenticacao.Aplicacao/CasosDeUso/CriarUsuario/CriarUsuarioHandler.cs` (alterado)
- `Modulos/Autenticacao/Modulos.Autenticacao.Aplicacao/Repositorios/IUsuarioRepositorio.cs` (alterado)
- `Modulos/Autenticacao/Modulos.Autenticacao.Aplicacao/Servicos/ITokenServico.cs`
- `Modulos/Autenticacao/Modulos.Autenticacao.Aplicacao/CasosDeUso/Login/LoginCommand.cs`
- `Modulos/Autenticacao/Modulos.Autenticacao.Aplicacao/CasosDeUso/Login/LoginHandler.cs`
- `Modulos/Autenticacao/Modulos.Autenticacao.Infraestrutura/Persistencia/Repositorios/UsuarioRepositorio.cs` (alterado)
- `Modulos/Autenticacao/Modulos.Autenticacao.Infraestrutura/Modulos.Autenticacao.Infraestrutura.csproj` (alterado — pacote `System.IdentityModel.Tokens.Jwt`)
- `Modulos/Autenticacao/Modulos.Autenticacao.Infraestrutura/Servicos/TokenServico.cs`
- `Modulos/Autenticacao/Modulos.Autenticacao.Aplicacao/InjecaoDeDependencia.cs` (alterado)
- `Modulos/Autenticacao/Modulos.Autenticacao.Infraestrutura/InjecaoDeDependencia.cs` (alterado)
- `Modulos/Autenticacao/Modulos.Autenticacao.Api/appsettings.Development.json` (alterado)
- `Modulos/Autenticacao/Modulos.Autenticacao.Api/Endpoints/Login/LoginRequest.cs`
- `Modulos/Autenticacao/Modulos.Autenticacao.Api/Endpoints/Login/LoginEndpoint.cs`
- `Modulos/Autenticacao/Modulos.Autenticacao.Api/Program.cs` (alterado)

## Cenários a cobrir

- Login com credenciais válidas retorna um token.
- Login com senha incorreta retorna erro (não exceção não tratada).
- Login com e-mail inexistente retorna o mesmo tipo de erro do cenário anterior (sem distinguir motivo).

## Notas / decisões tomadas

- `ISenhaServico` extraído nesta task (não em task separada), reaproveitado por `CriarUsuarioHandler` e `LoginHandler`.
- `LoginCommand`/`LoginHandler` retornam o token (`IRequest<string>`), diferente do `CriarUsuarioCommand` (que não retorna nada) — é assim que o token chega até o endpoint.
- Erro de login não distingue "e-mail não existe" de "senha incorreta" — evita vazar informação sobre quais e-mails estão cadastrados (enumeration attack).
- Tempo de expiração do token: a definir por quem implementa, configurável via `appsettings.Development.json` (não fixo no código) — evita token vazado ficar eternamente válido.

## Histórico de dúvidas

# 03 — Aplicação: caso de uso Criar Usuário

**Módulo:** Autenticação
**Camada:** Aplicação
**Status:** todo

## Contexto

Com o Domínio (`Usuario`) modelado e testado, o próximo passo é a camada de **Aplicação** — onde ficam os casos de uso do sistema (orquestração), sem se misturar com regra de negócio pura (Domínio) nem com detalhes técnicos como banco de dados (Infraestrutura, ainda não existe).

Decisão tomada com o usuário: como a Infraestrutura ainda não existe, esta task define apenas a **interface** de persistência (`IUsuarioRepositorio`), sem implementação concreta. O Handler depende da interface, não de uma implementação — isso é Inversão de Dependência, princípio central da Clean Architecture.

## Conceito novo: MediatR e CQRS

**CQRS** (Command Query Responsibility Segregation) é o padrão de separar operações que *mudam estado* (Commands) das que *só leem dados* (Queries), cada uma com seu próprio objeto de entrada e handler — em vez de um "Service" genérico com vários métodos soltos. **MediatR** é a biblioteca .NET mais usada pra implementar isso: você define um `Command` (o quê fazer) e um `Handler` (como fazer), e dispara via um mediador central (`IMediator.Send(command)`), sem quem chama precisar conhecer o handler diretamente. Isso desacopla a camada de API/apresentação da lógica de aplicação.

Referência: https://github.com/jbogard/MediatR/wiki

## O que fazer

1. Criar o projeto `Modulos.Autenticacao.Aplicacao` (classlib) em `Modulos/Autenticacao/Modulos.Autenticacao.Aplicacao/`.
2. Adicionar o pacote NuGet `MediatR` ao projeto de Aplicação.
3. Criar a interface `IUsuarioRepositorio` em `Repositorios/`, com um único método por enquanto: adicionar um `Usuario` (assinatura e nome do método a seu critério, ex: retornando `Task`).
4. Criar o Command `CriarUsuarioCommand` em `CasosDeUso/CriarUsuario/`, recebendo a senha em texto puro (nunca hash — quem chama o comando não deve saber nada sobre hashing).
5. Criar o Handler `CriarUsuarioHandler` em `CasosDeUso/CriarUsuario/`, que:
   - Recebe `IUsuarioRepositorio` via injeção de dependência (construtor).
   - Gera o hash da senha recebida (algoritmo com salt, ex: PBKDF2 — `Microsoft.AspNetCore.Cryptography.KeyDerivation`) **antes** de instanciar o `Usuario`.
   - **O salt gerado precisa ser persistido junto com o hash** (ex: concatenado no mesmo campo, ou em campo separado) — sem o salt original, não é possível validar a senha depois no login.
   - Instancia um `Usuario` a partir dos dados do Command (com a senha já hasheada).
   - Chama o repositório para adicionar o usuário.
6. Criar um método de extensão de `IServiceCollection` em `InjecaoDeDependencia.cs`, solto na raiz do projeto (sem subpasta), que registra o `MediatR` apontando para os handlers deste assembly — para a futura API chamar na composição de DI.

## Arquivos a criar/alterar

- `Modulos/Autenticacao/Modulos.Autenticacao.Aplicacao/Modulos.Autenticacao.Aplicacao.csproj` (novo projeto)
- `Modulos/Autenticacao/Modulos.Autenticacao.Aplicacao/Repositorios/IUsuarioRepositorio.cs`
- `Modulos/Autenticacao/Modulos.Autenticacao.Aplicacao/CasosDeUso/CriarUsuario/CriarUsuarioCommand.cs`
- `Modulos/Autenticacao/Modulos.Autenticacao.Aplicacao/CasosDeUso/CriarUsuario/CriarUsuarioHandler.cs`
- `Modulos/Autenticacao/Modulos.Autenticacao.Aplicacao/InjecaoDeDependencia.cs` (raiz do projeto, sem subpasta)

## Checklist

- [ ] Projeto `Modulos.Autenticacao.Aplicacao` criado em `Modulos/Autenticacao/Modulos.Autenticacao.Aplicacao/`
- [ ] Pacote NuGet `MediatR` adicionado
- [ ] Interface `IUsuarioRepositorio` criada em `Repositorios/`
- [ ] `CriarUsuarioCommand` criado em `CasosDeUso/CriarUsuario/`
- [ ] `CriarUsuarioHandler` criado em `CasosDeUso/CriarUsuario/`, injetando `IUsuarioRepositorio` e criando o `Usuario` a partir do Command
- [ ] Senha é hasheada com algoritmo que usa salt (ex: PBKDF2), antes de criar o `Usuario`
- [ ] Salt é persistido junto com o hash (não descartado após o cálculo)
- [ ] `InjecaoDeDependencia.cs` criado, registrando o `MediatR` para os handlers do assembly

## Notas / decisões tomadas

- Persistência: apenas a interface `IUsuarioRepositorio` é definida nesta task; a implementação concreta (Infraestrutura) fica para task futura.

## Histórico de dúvidas

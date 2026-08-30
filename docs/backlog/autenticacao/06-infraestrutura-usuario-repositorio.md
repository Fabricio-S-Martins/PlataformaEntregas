# 06 — Infraestrutura: implementação real do IUsuarioRepositorio com EF Core

**Módulo:** Autenticação
**Camada:** Infraestrutura
**Status:** todo

## Contexto

Com o Postgres containerizado (task 05) e a interface `IUsuarioRepositorio` definida na Aplicação (task 03), falta a implementação real: gravar o `Usuario` no banco de fato. É a camada de Infraestrutura — onde ficam os detalhes técnicos (ORM, SQL, provider do banco) que Aplicação e Domínio não conhecem.

O VO `Email` e o enum `Papel` vão ter mapeamento explícito via Fluent API (`IEntityTypeConfiguration<Usuario>`), em vez de deixar o EF Core inferir por convenção. `IEntityTypeConfiguration<T>` é o padrão do próprio EF Core para separar a configuração de mapeamento de cada entidade em sua própria classe.

`UsuarioRepositorio` segue o **Repository Pattern**: abstrai o acesso a dados atrás de uma interface (`IUsuarioRepositorio`, já definida na Aplicação), para que a Aplicação não dependa de detalhes do EF Core/SQL diretamente.

## Conceito novo: Entity Framework Core (EF Core) e Migrations

**EF Core** é o ORM (Object-Relational Mapper) oficial da Microsoft para .NET — traduz objetos C# (como `Usuario`) em linhas de tabelas SQL, e vice-versa, sem escrever SQL manualmente na maior parte do tempo. Uma **Migration** é um arquivo gerado pelo EF Core que descreve uma mudança incremental no schema do banco (criar tabela, adicionar coluna, etc.) — o schema evolui versionado junto com o código.

Referência: https://learn.microsoft.com/pt-br/ef/core/

## O que fazer

1. Criar o projeto `Modulos.Autenticacao.Infraestrutura` (classlib) em `Modulos/Autenticacao/Modulos.Autenticacao.Infraestrutura/`.
2. Adicionar os pacotes NuGet `Microsoft.EntityFrameworkCore`, `Npgsql.EntityFrameworkCore.PostgreSQL` e `Microsoft.EntityFrameworkCore.Design` (necessário para os comandos `dotnet ef`, como gerar Migrations) ao projeto.
3. Criar `AutenticacaoDbContext` em `Persistencia/`, herdando de `DbContext`, com um `DbSet<Usuario>`.
4. Criar `UsuarioConfiguracao` em `Persistencia/Configuracoes/`, implementando `IEntityTypeConfiguration<Usuario>`, mapeando:
   - O VO `Email` para uma coluna de texto simples (não um objeto complexo).
   - O enum `Papel` para uma coluna de texto legível (nome do valor, não o número do enum).
5. Criar `UsuarioRepositorio` em `Persistencia/Repositorios/`, implementando `IUsuarioRepositorio` usando o `AutenticacaoDbContext`.
6. Adicionar, em `InjecaoDeDependencia.cs` (raiz do projeto, sem subpasta), um método de extensão de `IServiceCollection` que registra o `AutenticacaoDbContext` (usando a connection string do Postgres) e a implementação de `IUsuarioRepositorio`.

## Arquivos a criar/alterar

- `Modulos/Autenticacao/Modulos.Autenticacao.Infraestrutura/Modulos.Autenticacao.Infraestrutura.csproj` (novo projeto)
- `Modulos/Autenticacao/Modulos.Autenticacao.Infraestrutura/Persistencia/AutenticacaoDbContext.cs`
- `Modulos/Autenticacao/Modulos.Autenticacao.Infraestrutura/Persistencia/Configuracoes/UsuarioConfiguracao.cs`
- `Modulos/Autenticacao/Modulos.Autenticacao.Infraestrutura/Persistencia/Repositorios/UsuarioRepositorio.cs`
- `Modulos/Autenticacao/Modulos.Autenticacao.Infraestrutura/InjecaoDeDependencia.cs` (raiz do projeto, sem subpasta)

## Checklist

- [ ] Projeto `Modulos.Autenticacao.Infraestrutura` criado em `Modulos/Autenticacao/Modulos.Autenticacao.Infraestrutura/`
- [ ] Pacotes `Microsoft.EntityFrameworkCore`, `Npgsql.EntityFrameworkCore.PostgreSQL` e `Microsoft.EntityFrameworkCore.Design` adicionados
- [ ] `AutenticacaoDbContext` criado em `Persistencia/`, com `DbSet<Usuario>`
- [ ] `UsuarioConfiguracao` criada em `Persistencia/Configuracoes/`, mapeando `Email` (VO → coluna simples) e `Papel` (enum → texto)
- [ ] `UsuarioRepositorio` criado em `Persistencia/Repositorios/`, implementando `IUsuarioRepositorio`
- [ ] `InjecaoDeDependencia.cs` criado na raiz do projeto, registrando `AutenticacaoDbContext` e `IUsuarioRepositorio`

## Notas / decisões tomadas

- Mapeamento de `Email`/`Papel`: Fluent API explícita via `IEntityTypeConfiguration<Usuario>`, não convenção implícita do EF Core.

## Histórico de dúvidas

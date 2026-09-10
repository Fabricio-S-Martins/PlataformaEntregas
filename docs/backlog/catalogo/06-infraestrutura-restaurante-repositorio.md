# 06 — Implementar IRestauranteRepositorio com EF Core

**Módulo:** Catálogo
**Camada:** Infraestrutura
**Status:** feito

## Contexto

A interface `IRestauranteRepositorio` (task 04) ainda não tem implementação real — falta gravar o `Restaurante` no banco de fato. Segue exatamente o mesmo padrão já usado em Autenticação (task 06): **EF Core** como ORM, **Migrations** para versionar o schema, mapeamento explícito via Fluent API (`IEntityTypeConfiguration<Restaurante>`), e `RestauranteRepositorio` seguindo o **Repository Pattern** (abstrai o acesso a dados atrás da interface já definida na Aplicação).

Catálogo usa o mesmo Postgres já provisionado para Autenticação (mesma connection string, `BasePlataformaEntregas`) — um monolito modular compartilha a instância física do banco entre módulos, cada um com suas próprias tabelas e histórico de Migrations.

`Cardapio` e `ItemCardapio` ficam fora desta task — ainda não têm caso de uso nem interface de repositório na Aplicação (isso viria de uma task futura de Aplicação, antes de qualquer Infraestrutura pra eles).

## O que fazer

1. Na pasta `Modulos/Catalogo/`, criar a pasta `Modulos.Catalogo.Infraestrutura/` e, dentro dela, o projeto `Modulos.Catalogo.Infraestrutura`.
2. Adicionar os pacotes NuGet `Microsoft.EntityFrameworkCore.Design` e `Npgsql.EntityFrameworkCore.PostgreSQL` ao projeto (mesmas versões já usadas em `Modulos.Autenticacao.Infraestrutura`) — os demais pacotes de EF Core vêm como dependência transitiva desses dois.
3. Dentro desse projeto, criar a pasta `Persistencia/`.
4. Dentro de `Persistencia/`, criar `CatalogoDbContext`, herdando de `DbContext`, com um `DbSet<Restaurante>`.
5. Dentro de `Persistencia/`, criar a pasta `Configuracoes/`.
6. Dentro de `Configuracoes/`, criar `RestauranteConfiguracao`, implementando `IEntityTypeConfiguration<Restaurante>`, mapeando `Nome` e `Cnpj` como colunas de texto obrigatórias.
7. Dentro de `Persistencia/`, criar a pasta `Repositorios/`.
8. Dentro de `Repositorios/`, criar `RestauranteRepositorio`, implementando `IRestauranteRepositorio` usando o `CatalogoDbContext`.
9. Na raiz do projeto, criar `InjecaoDeDependencia`, com um método de extensão de `IServiceCollection` que registra o `CatalogoDbContext` (usando a connection string `BasePlataformaEntregas`) e a implementação de `IRestauranteRepositorio`.

## Cenários a cobrir

Sem testes automatizados nesta task — cobertura de Infraestrutura com banco real fica pra quando o módulo tiver um teste de integração (mesmo padrão adotado em Autenticação até aqui).

## Arquivos a criar/alterar

- `Modulos/Catalogo/Modulos.Catalogo.Infraestrutura/Persistencia/CatalogoDbContext.cs`
- `Modulos/Catalogo/Modulos.Catalogo.Infraestrutura/Persistencia/Configuracoes/RestauranteConfiguracao.cs`
- `Modulos/Catalogo/Modulos.Catalogo.Infraestrutura/Persistencia/Repositorios/RestauranteRepositorio.cs`
- `Modulos/Catalogo/Modulos.Catalogo.Infraestrutura/InjecaoDeDependencia.cs`

## Notas / decisões tomadas

- Mesmo Postgres/connection string já usado por Autenticação — sem container novo.
- Sem schema separado por módulo por enquanto (todas as tabelas no schema `public`) — se a mistura de tabelas de módulos diferentes virar um problema real, isso vira uma task de revisão futura.
- Geração da Migration inicial e fábrica de design-time (`IDesignTimeDbContextFactory`) ficam fora desta task, mesmo padrão que teve task própria em Autenticação (`XX-migration-inicial.md`) — a propor em seguida.

## Histórico de dúvidas

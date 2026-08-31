# XX — Infraestrutura: fábrica de design-time e primeira Migration

**Módulo:** Autenticação
**Camada:** Infraestrutura
**Status:** resolvida (task 07)

## Contexto

Na task 06, ao tentar gerar a primeira Migration via `dotnet ef migrations add`, esbarramos na necessidade de uma `IDesignTimeDbContextFactory<AutenticacaoDbContext>` (as ferramentas do EF Core precisam instanciar o `DbContext` sozinhas, sem host/DI). O problema: sem projeto de API ainda existindo, não há um jeito limpo de fornecer a connection string pra essa fábrica sem cair em hardcode no código-fonte ou depender de um `appsettings.json` que não faz sentido existir numa classlib de Infraestrutura.

Decisão: adiar a fábrica de design-time e a primeira Migration para quando existir um projeto de API/host — nesse momento, a fábrica pode reaproveitar a mesma fonte de configuração do host (variáveis de ambiente, `appsettings.json` do próprio host, etc.), sem gambiarra provisória.

## A discutir quando esta task for detalhada

- Se a fábrica de design-time vai ler do `.env` da raiz do repo diretamente, ou reaproveitar configuração já centralizada no host.
- Se faz sentido já configurar `dotnet ef` para usar o projeto de API como "startup project" (`--startup-project`), evitando a fábrica de design-time por completo.

Esta task será detalhada (com "O que fazer", arquivos exatos e checklist) quando chegar a vez dela no backlog — por ora é só um marcador pra não perder o contexto.

## Resolução

Na task 07, o projeto `Modulos.Autenticacao.Api` foi criado como host real. A fábrica de design-time acabou não sendo necessária: a Migration foi gerada e aplicada usando a própria API como `--startup-project` do `dotnet ef` (segunda opção listada acima em "A discutir"), reaproveitando a configuração/DI já existente no host, sem precisar de `IDesignTimeDbContextFactory`.

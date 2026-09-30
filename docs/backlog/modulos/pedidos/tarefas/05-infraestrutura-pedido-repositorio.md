---
tags: [backlog/tarefa, modulo/pedidos, fluxo/pedido-operacao, status/concluido]
---

# 05 — Implementar IPedidoRepositorio com EF Core

**Módulo:** Pedidos
**Camada:** Infraestrutura

## Contexto

A interface `IPedidoRepositorio` (task 03) ainda não tem implementação real. Segue o mesmo padrão já usado em Autenticação e Catálogo: **EF Core** como ORM, **Migrations** para versionar o schema, mapeamento explícito via Fluent API (`IEntityTypeConfiguration<T>`), e `PedidoRepositorio` seguindo o **Repository Pattern**.

Pedidos usa o mesmo Postgres já provisionado pros outros módulos (mesma connection string, `BasePlataformaEntregas`).

A peça nova aqui: `Pedido` guarda `Itens` numa coleção **privada** (`ItensInterno`), não numa propriedade pública comum — o Fluent API precisa ser instruído explicitamente a mapear essa coleção pelo nome do campo, não por uma expressão lambda normal. `Eventos`/`EventosInterno` não é mapeado de jeito nenhum — é uma lista só de uso em memória durante a operação (task 03), nunca vai pro banco.

## Conceitos novos

### Mapear coleção privada com Fluent API

Quando a navegação de coleção é exposta só como `IReadOnlyList<T>` (sem `Add`, sem setter — não dá pra escrever uma expressão lambda normal apontando pra ela), o Fluent API aceita o **nome do campo/propriedade privada** como string: `builder.HasMany<ItemPedido>("ItensInterno")`. O EF Core usa reflection pra ler e escrever nesse campo diretamente, ignorando o encapsulamento em tempo de mapeamento — só ele tem esse acesso, o resto do código continua vendo só `Pedido.Itens` (somente leitura).

Pelo mesmo motivo, carregar os itens junto do pedido também usa a versão de `Include` que aceita string (`.Include("ItensInterno")`), em vez da versão com lambda.

Referência: https://learn.microsoft.com/en-us/ef/core/modeling/relationships/backing-fields

## O que fazer

1. Na pasta `Modulos/Pedidos/`, criar a pasta `Modulos.Pedidos.Infraestrutura/`.
2. Dentro dessa pasta, criar o projeto `Modulos.Pedidos.Infraestrutura`.
3. Adicionar os pacotes NuGet `Microsoft.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.Relational`, `Microsoft.EntityFrameworkCore.Design` e `Npgsql.EntityFrameworkCore.PostgreSQL` ao projeto, nas mesmas versões já usadas em `Modulos.Catalogo.Infraestrutura`.

### Contexto e mapeamento

4. Dentro do projeto, criar a pasta `Persistencia/`. Dentro dela, criar `PedidosDbContext`, herdando de `DbContext`, com um `DbSet<Pedido>` chamado `Pedidos` — sem `DbSet<ItemPedido>` próprio, já que `ItemPedido` só é acessado através da navegação `Pedido.Itens` (agregado raiz é a única porta de entrada).
5. Dentro de `Persistencia/`, criar a pasta `Configuracoes/`.
6. Dentro de `Configuracoes/`, criar `PedidoConfiguracao`, implementando `IEntityTypeConfiguration<Pedido>`: chave primária em `Id`; mapeamento explícito de `ClienteId`, `RestauranteId` e `CriadoEm` (propriedades sem setter, não mapeadas por convenção); a navegação de `Itens` mapeada via `HasMany<ItemPedido>("ItensInterno")`, relacionada por `ItemPedido.PedidoId` como chave estrangeira, obrigatória.
7. Dentro de `Configuracoes/`, criar `ItemPedidoConfiguracao`, implementando `IEntityTypeConfiguration<ItemPedido>`: chave primária em `Id`; mapeamento explícito de `PedidoId`, `ItemCardapioId`, `Quantidade` e `PrecoUnitario` (mesmo motivo do passo 6 — nenhuma dessas propriedades tem setter).

### Repositório

8. Dentro do projeto, criar a pasta `Persistencia/Repositorios/`. Dentro dela, criar `PedidoRepositorio`, implementando `IPedidoRepositorio` usando o `PedidosDbContext`: `AdicionarAsync` adiciona e salva; `AtualizarAsync` marca o pedido como modificado (`Update`) e salva; `ObterPorIdAsync` busca por `Id` incluindo a navegação `"ItensInterno"` (via `Include` de string, não lambda — ver "Conceitos novos").

### Registro de DI

9. Na raiz do projeto, criar `InjecaoDeDependencia`, com um método de extensão de `IServiceCollection` que registra o `PedidosDbContext` (usando a connection string `BasePlataformaEntregas`) e a implementação de `IPedidoRepositorio`.

### Migration inicial

10. Gerar a Migration inicial (`InicialPedido`) do `PedidosDbContext` e aplicar no Postgres local, seguindo o runbook `docs/runbooks/migrations.md`. Antes de aplicar, ler o `Up()` gerado e confirmar que `Pedidos` e `ItemPedidos` aparecem com todas as colunas esperadas (gotcha já conhecido: propriedade sem setter que ficar de fora do mapeamento some silenciosamente da Migration).

## Notas / decisões tomadas

- Sem schema separado por módulo, mesmo padrão de Autenticação/Catálogo — todas as tabelas no `public`.
- `AtualizarAsync` chama `Update` explicitamente em vez de depender só do change-tracking implícito do EF Core, mesma decisão já tomada na task 03.
- Sem testes automatizados nesta task — mesmo padrão adotado nos outros módulos (cobertura de Infraestrutura com banco real fica pra quando existir teste de integração).

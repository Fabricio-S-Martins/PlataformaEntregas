# 08 — Cache de leitura do Restaurante com Redis (cache-aside + write-through)

**Módulo:** Catálogo
**Camada:** Infraestrutura / Aplicação / API (fatia vertical)
**Status:** feito

## Contexto

O Catálogo é leitura-pesada: um restaurante é cadastrado uma vez e lido milhares de vezes. Hoje toda leitura bate no Postgres. Esta task introduz **Redis** como cache de leitura do `Restaurante`, com o padrão **cache-aside** e aquecimento do cache (**write-through**) no cadastro.

Primeira vez que o projeto usa cache distribuído — a partir daqui o padrão se repete em Cardápio, Pedidos, etc.

## Conceitos novos

### Redis

Banco de dados chave-valor em memória, usado como cache distribuído (compartilhado entre instâncias da aplicação, diferente de um cache em memória local que morre com o processo e não é compartilhado). Leitura/escrita na casa de microssegundos. Aqui entra como container no `docker-compose`.

Referência: https://redis.io/docs/latest/develop/

### Cache-aside (lazy loading)

A aplicação é quem gerencia o cache, não o banco. No fluxo de leitura:
1. Procura a chave no cache.
2. **Hit** → devolve o valor do cache, não toca no banco.
3. **Miss** → busca no banco, grava no cache com um TTL, devolve.

No fluxo de escrita, a aplicação **invalida** (remove) a chave afetada, para a próxima leitura recarregar do banco.

Referência: https://learn.microsoft.com/en-us/azure/architecture/patterns/cache-aside

### `IDistributedCache`

Abstração do ASP.NET Core para cache distribuído (`GetAsync`/`SetAsync`/`RemoveAsync` sobre `byte[]`). O pacote `Microsoft.Extensions.Caching.StackExchangeRedis` liga essa abstração ao Redis. A Aplicação depende só da abstração; o Redis fica na Infraestrutura/host.

Referência: https://learn.microsoft.com/en-us/aspnet/core/performance/caching/distributed

## O que fazer

### Infra — Redis

1. No `docker-compose.yml`, adicionar um serviço `redis` (`redis:7`, porta `6379`), com volume nomeado para persistência.
2. No `appsettings.Development.json` do host, adicionar a connection string do Redis (`localhost:6379`). No `appsettings.json`, só a chave vazia.
3. No projeto `Modulos.Catalogo.Infraestrutura`, adicionar o pacote `Microsoft.Extensions.Caching.StackExchangeRedis`. No `RegistrarCatalogoInfraestrutura`, registrar o Redis via `AddStackExchangeRedisCache(...)`, lendo a connection string do passo 2.

### Leitura — Query + endpoint

4. Em `IRestauranteRepositorio`, adicionar o método `Task<Restaurante> ObterPorIdAsync(Guid id)`. Em `RestauranteRepositorio`, implementar esse método via EF Core (`FindAsync`/`FirstOrDefaultAsync`).
5. No projeto `Modulos.Catalogo.Aplicacao`, dentro da pasta `CasosDeUso/`, criar a pasta `ObterRestaurante/`. Dentro dela, criar, nesta ordem:
   - O record `RestauranteResponse`, com `Id` (`Guid`), `Nome`, `Cnpj` (`string` por enquanto) e `Ativo`.
   - O record `ObterRestauranteQuery`, com `Id` (`Guid`), implementando `IRequest<RestauranteResponse?>`.
   - A classe `ObterRestauranteQueryHandler`.
6. Em `ObterRestauranteQueryHandler`, implementar o cache-aside com a chave `catalogo:restaurante:{id}`:
   - `IDistributedCache.GetAsync` devolve `byte[]?`. Se não for `null`, desserializar com `JsonSerializer.Deserialize<RestauranteResponse>` (usando `System.Text.Json`) e devolver — não bate no banco.
   - Se for `null` (miss), chamar `ObterPorIdAsync`. Se o repositório devolver `null`, devolver `null` sem gravar nada no cache.
   - Se encontrar, montar o `RestauranteResponse`, serializar com `JsonSerializer.SerializeToUtf8Bytes` e gravar via `IDistributedCache.SetAsync` com `DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) }`, depois devolver o `RestauranteResponse`.
7. No projeto `Modulos.Catalogo.Api`, dentro da pasta `Endpoints/`, criar a pasta `ObterRestaurante/`. Dentro dela, criar `ObterRestauranteEndpoint`, com um método de extensão de `IEndpointRouteBuilder` que mapeia `GET /restaurantes/{id}`, despacha a query e retorna `200` com o `RestauranteResponse` ou `404` se `null`.
8. Em `CatalogoEndpoints`, registrar o mapeamento criado no passo 7.

### Aquecer o cache no cadastro (write-through)

9. Em `CadastrarRestauranteHandler`, após persistir com sucesso, montar o `RestauranteResponse` do restaurante recém-criado e gravá-lo direto na chave `catalogo:restaurante:{id}` do cache (mesma serialização e TTL do passo 6) — assim a primeira leitura depois do cadastro já vem do cache, sem bater no banco.

## Cenários a cobrir

**`ObterRestauranteQueryHandler`** (testes com `IDistributedCache` em memória — `AddDistributedMemoryCache` — e `IRestauranteRepositorio` mockado):
- Cache miss (chave `catalogo:restaurante:{id}` ainda não existe): chama `ObterPorIdAsync` do repositório exatamente uma vez, grava o resultado serializado na chave, e devolve um `RestauranteResponse` com os mesmos dados do restaurante retornado pelo repositório.
- Cache hit (chave já populada antes do `Handle`, gravando direto no `IDistributedCache` de memória): **não** chama `ObterPorIdAsync` do repositório nenhuma vez, e devolve o `RestauranteResponse` desserializado do valor já gravado no cache.
- Id inexistente (repositório devolve `null`): devolve `null`, e a chave `catalogo:restaurante:{id}` continua ausente do cache depois do `Handle` (conferir com `GetAsync` na própria chave, não só `Times.Never` no repositório).

**`CadastrarRestauranteHandler`** (mantém os dois cenários já existentes de dados válidos/inválidos, mais):
- Com dados válidos, depois do `Handle` a chave `catalogo:restaurante:{id}` do restaurante recém-criado está gravada no cache com um `RestauranteResponse` cujos `Id`, `Nome`, `Cnpj` e `Ativo` batem com o restaurante persistido — conferir lendo e desserializando o valor da chave via `IDistributedCache.GetAsync`, não só `Times.Once` no `SetAsync`.
- Com dados inválidos, nada é gravado no cache (`IDistributedCache.SetAsync` nunca chamado).

## Notas / decisões tomadas

- Cache-aside com serialização JSON manual (`System.Text.Json`) direto sobre `IDistributedCache`, inline no handler — aceito por ora. Quando repetir num segundo caso de uso cacheado, extrair um `ICacheService` genérico (interface na Aplicação, implementação na Infraestrutura, mesmo padrão de `ISenhaServico`/`SenhaServico`), encapsulando o `Get/SetAsync<T>` e a serialização — vira card próprio.
- TTL de 10 minutos é chute inicial — ajustar quando houver dado real de padrão de acesso.
- **Fora de escopo, vira card próprio:** proteção contra *cache stampede* (várias requisições recarregando a mesma chave expirada ao mesmo tempo) com lock distribuído no Redis; e mover o cache-aside para um **MediatR pipeline behavior** genérico em vez de inline no handler.
- `IDistributedCache` fica registrado no módulo Catálogo (não no host) — cada módulo decide sua estratégia de cache; o host só fornece a connection string via `IConfiguration`.


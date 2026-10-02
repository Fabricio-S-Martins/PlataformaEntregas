---
tags: [backlog, modulo/catalogo, fluxo/catalogo-consulta-restaurante, tarefa/concluido]
---

# 09 — Proteger o cache do Restaurante contra cache stampede com lock distribuído no Redis

**Módulo:** Catálogo | **Camada:** Aplicação / Infraestrutura | **Deps:** Catálogo 08

## O que fazer

- **Lock distribuído:** `StackExchange.Redis` (já usado por baixo do `IDistributedCache`) expõe `LockTakeAsync`/`LockReleaseAsync`, que implementam `SET NX/PX` com liberação segura por token — evita cache stampede quando a chave expira sob concorrência.
- **Duração/retry:** TTL do lock de 5s e polling de 5 tentativas/200ms (1s no total) são chutes iniciais, a ajustar com dado real de latência do Postgres.
- **Fallback sem lock:** esgotado o polling, consulta o banco direto sem gravar cache — prioriza responder a requisição a manter a exclusão mútua a qualquer custo; se virar problema real de carga, uma fila de espera bloqueante entra como card próprio.
- **Reuso:** `ITravaDistribuidaServico` fica genérico (chave/token/duração), não amarrado ao `Restaurante`.
- **Ref:** https://redis.io/docs/latest/develop/use/patterns/distributed-locks/

## Checklist

### 1. Aplicação
- [x] **a:** Na pasta `Modulos.Catalogo.Aplicacao/`, criar a pasta `Servicos/`.
- [x] **b:** Dentro dela, criar a interface `ITravaDistribuidaServico`, com o método `AdquirirAsync`, recebendo a chave, um token de posse e a duração do lock, devolvendo se conseguiu adquirir; e `LiberarAsync`, recebendo a chave e o token, sem retorno.
- [x] **c:** Em `ObterRestauranteQueryHandler`, receber `ITravaDistribuidaServico` no construtor.
- [x] **d:** No `Handle`, no cache miss, montar a chave de lock `catalogo:restaurante:lock:{id}` e um token (`Guid.NewGuid()`), e chamar `AdquirirAsync` com duração de 5 segundos.
- [x] **e:** Se adquiriu, reconsultar o cache; se ainda vazio, chamar `ObterPorIdAsync`, gravar o resultado no cache quando encontrado com a mesma serialização e TTL da task 08, e liberar o lock num `finally`.
- [x] **f:** Se não adquiriu, repetir até 5 vezes, com 200ms de espera, um novo `GetAsync` no cache; se esgotar sem achar, chamar `ObterPorIdAsync` direto, sem lock e sem gravar no cache.

### 2. Infraestrutura
- [x] **a:** No projeto `Modulos.Catalogo.Infraestrutura`, adicionar o pacote `StackExchange.Redis`.
- [x] **b:** Dentro do projeto, criar a pasta `Cache/`.
- [x] **c:** Dentro dela, criar a classe `TravaDistribuidaServico`, implementando `ITravaDistribuidaServico` a partir de um `IConnectionMultiplexer` recebido no construtor — `AdquirirAsync` chama `LockTakeAsync`; `LiberarAsync` chama `LockReleaseAsync`.
- [x] **d:** Em `RegistrarCatalogoInfraestrutura`, registrar `IConnectionMultiplexer` como singleton via `ConnectionMultiplexer.Connect(...)`, reaproveitando a connection string do Redis já usada pelo cache.
- [x] **e:** No mesmo método, registrar `ITravaDistribuidaServico`/`TravaDistribuidaServico`.

### 3. Documentação
- [x] **a:** Criar/atualizar `docs/documentacao/modulos/<modulo>/fluxos/<fluxo>.md` com o fluxo tocado (passos e diagrama Mermaid)
- [x] **b:** Criar/atualizar `docs/documentacao/modulos/<modulo>/<modulo>.md` com o que o módulo é, as etapas e as regras de negócio
- [x] **c:** Perguntar ao Dev se quer o plano de documentação do restante do módulo/fluxo

### 4. QA & Testes
- [x] **a:** Testar cache miss com lock adquirido de primeira: `ObterPorIdAsync` chamado 1x, cache gravado, lock liberado, devolve o `RestauranteResponse`.
- [x] **b:** Testar cache miss com lock adquirido, mas a reconsulta ao cache já encontra o valor gravado por outra instância: `ObterPorIdAsync` nunca chamado, lock liberado, devolve o valor do cache.
- [x] **c:** Testar cache miss com lock não adquirido, mas o cache aparece populado numa tentativa de polling antes de esgotar as 5: `ObterPorIdAsync` nunca chamado, devolve o valor do cache.
- [x] **d:** Testar cache miss com lock não adquirido e cache nunca populado nas 5 tentativas: `ObterPorIdAsync` chamado 1x (fallback), nada gravado no cache.

# XX — Infraestrutura: avaliar Persistence Model separado do Domínio

**Módulo:** Autenticação
**Camada:** Infraestrutura / Domínio
**Status:** backlog (não detalhada ainda — número/ordem definidos quando entrar na fila)

## Contexto

Na task 06, pra permitir que o EF Core materialize `Usuario` a partir do banco, foi adicionado um construtor privado sem parâmetros na entidade (`private Usuario() {}`), usado só por reflexão do ORM. É uma concessão técnica pequena e comum, mas fere um pouco o princípio de **Persistence Ignorance** — a entidade de Domínio passou a ter uma característica que existe só por causa do ORM, não por regra de negócio.

## Conceito: Persistence Model

Padrão onde existe uma classe separada, exclusiva da Infraestrutura, representando o formato de armazenamento (`UsuarioPersistencia`, por exemplo) — diferente da entidade de Domínio (`Usuario`). Um mapper (manual ou com lib tipo AutoMapper/Mapster) converte entre os dois lados. Isso mantém o Domínio 100% livre de qualquer concessão técnica (sem construtor privado pro ORM, sem setters abertos só por causa de reflexão).

## A discutir quando esta task for detalhada

- Se o ganho de pureza do Domínio compensa o custo de manter dois modelos + mapper, dado o tamanho atual do projeto.
- Se faz sentido introduzir isso já no módulo Autenticação, ou só quando um módulo futuro (com agregados mais complexos) sentir essa dor de forma mais concreta.
- Qual biblioteca de mapeamento usar (ou mapeamento manual), caso decidido seguir.

Esta task será detalhada (com "O que fazer", arquivos exatos e checklist) quando chegar a vez dela no backlog — por ora é só um marcador pra não perder o contexto.

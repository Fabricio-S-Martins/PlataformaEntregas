---
tags: [backlog, modulo/autenticacao, decisao/adiada]
---
# Separar um Persistence Model da entidade de Domínio?

## Contexto
Para o EF Core materializar `Usuario`, a entidade ganhou um construtor privado sem parâmetros, usado só por reflexão do ORM. É uma concessão pequena, mas fere o princípio de Persistence Ignorance: o Domínio passou a ter uma característica que existe só por causa do ORM. Adiada: reabre quando um módulo com agregados mais complexos (ex: Pedidos) sentir essa dor de forma concreta.

## Opções
- **Manter a entidade de Domínio mapeada direto pelo EF Core:** menos código e sem mapper; o Domínio mantém a concessão do construtor para o ORM.
- **Persistence Model separado na Infraestrutura (ex: `UsuarioPersistencia`) com mapper:** Domínio 100% livre de concessões técnicas; custo de manter dois modelos e o mapeamento, manual ou com biblioteca (AutoMapper/Mapster).

## O que depende
Se vale introduzir já no módulo Autenticação ou só quando outro módulo sentir a dor, e qual biblioteca de mapeamento usar (ou mapeamento manual), caso a separação seja adotada.

## Decisão

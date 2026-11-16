---
tags: [decisao, modulo/compartilhado, decisao/aceita]
---
# Clean Architecture com CQRS (MediatR) em cada módulo

- **Decisão e porquê:** cada módulo segue Domain → Application → Infrastructure → API, com CQRS via MediatR na Application. Isola o domínio da infraestrutura e separa leitura de escrita por caso de uso.
- **Descartado:** (a confirmar) arquitetura em camadas tradicional sem CQRS — motivo a confirmar com o Dev.
- **Não fazer:** o Domain depender de Application, Infrastructure ou API; regra de negócio fora do Domain/Application.

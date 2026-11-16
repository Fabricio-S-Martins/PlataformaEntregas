---
tags: [decisao, modulo/compartilhado, decisao/aceita]
---
# Monólito modular em vez de microserviços

- **Decisão e porquê:** um só deploy, dividido em módulos com Clean Architecture (Domain → Application → Infrastructure → API). Evita o custo de infra distribuída antes de os limites de domínio estarem maduros.
- **Descartado:** microserviços desde o início — custo de infra distribuída antes de os limites de domínio estarem maduros.
- **Não fazer:** extrair um módulo como serviço separado sem um épico próprio de evolução.

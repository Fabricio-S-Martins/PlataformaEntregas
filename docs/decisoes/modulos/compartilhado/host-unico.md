---
tags: [decisao, modulo/compartilhado, decisao/aceita]
---
# Host único como Composition Root

- **Decisão e porquê:** `PlataformaEntregas.Api` (em `Host/`) compõe todos os módulos. Cada módulo expõe uma biblioteca `Modulos.<Modulo>.Api` com seus endpoints e o registro de DI/endpoints.
- **Descartado:** um `Program.cs` por módulo — mantém um único processo e uma única composição, coerente com o monólito modular.
- **Não fazer:** criar `Program.cs` em módulo; o módulo registra-se no host, nunca sobe sozinho.

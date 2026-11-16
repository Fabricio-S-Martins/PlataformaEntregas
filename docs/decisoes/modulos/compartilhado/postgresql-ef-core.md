---
tags: [decisao, modulo/compartilhado, decisao/aceita]
---
# PostgreSQL com EF Core como persistência dos módulos

- **Decisão e porquê:** os módulos persistem em PostgreSQL via EF Core (Npgsql), com repositório por agregado na Infrastructure e Migrations.
- **Descartado:** (a confirmar) outro banco relacional ou ORM (ex.: SQL Server, Dapper) — motivo a confirmar com o Dev.
- **Não fazer:** a Application ou o Domain referenciarem EF Core; acesso a dados só pelo repositório da Infrastructure.

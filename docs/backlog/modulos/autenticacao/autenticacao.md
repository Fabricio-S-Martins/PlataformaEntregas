---
tags: [backlog/modulo, modulo/autenticacao]
---

# Módulo Autenticação

[← Backlog](../../backlog.md)

| Task |
|---|
| [01 - Domínio: entidade Usuario](tarefas/01-domain-usuario.md) |
| [02 - Domínio: testes unitários da entidade Usuario](tarefas/02-testes-usuario.md) |
| [03 - Aplicação: caso de uso Criar Usuário](tarefas/03-aplicacao-criar-usuario.md) |
| [04 - Aplicação: testes do caso de uso Criar Usuário](tarefas/04-testes-criar-usuario.md) |
| [05 - Infraestrutura: subir PostgreSQL via Docker Compose](tarefas/05-docker-postgres.md) |
| [06 - Infraestrutura: implementação real do IUsuarioRepositorio com EF Core](tarefas/06-infraestrutura-usuario-repositorio.md) |
| [07 - API: endpoint de cadastro de usuário com Minimal API](tarefas/07-api-criar-usuario.md) |
| [08 - Criar fluxo de login com JWT e extrair serviço de hash de senha](tarefas/08-login-jwt.md) |
| [09 - Validar JWT no pipeline e expor endpoint de usuário autenticado](tarefas/09-autorizacao-papel.md) |
| [10 - Cobrir login e hash de senha com testes automatizados](tarefas/10-testes-login-senha-servico.md) |
| [11 - Cobrir geração e validação do token JWT com testes automatizados](tarefas/11-testes-token-servico.md) |
| [12 - Adotar Result Pattern nas invariantes do Usuario](tarefas/12-result-pattern-usuario.md) |
| [XX - Infraestrutura: fábrica de design-time e primeira Migration](tarefas/XX-migration-inicial.md) |

## Decisões

- [Separar um Persistence Model da entidade de Domínio?](decisoes/persistence-model-separado-do-dominio.md)

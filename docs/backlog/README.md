# Backlog — PlataformaEntregas

Índice geral das tasks. Cada módulo tem sua pasta em `docs/backlog/<modulo>/`, com um arquivo por task.

Status possíveis: `todo`, `em andamento`, `feito`, `bloqueado`.

## Compartilhado

| Task | Status |
|---|---|
| [01 - Extrair Resultado\<T\> para um Shared Kernel entre módulos](compartilhado/01-shared-kernel-resultado.md) | feito |

## Módulo Autenticação

| Task | Status |
|---|---|
| [01 - Domínio: entidade Usuario](autenticacao/01-domain-usuario.md) | feito |
| [02 - Domínio: testes unitários da entidade Usuario](autenticacao/02-testes-usuario.md) | feito |
| [03 - Aplicação: caso de uso Criar Usuário](autenticacao/03-aplicacao-criar-usuario.md) | feito |
| [04 - Aplicação: testes do caso de uso Criar Usuário](autenticacao/04-testes-criar-usuario.md) | feito |
| [05 - Infraestrutura: subir PostgreSQL via Docker Compose](autenticacao/05-docker-postgres.md) | feito |
| [06 - Infraestrutura: implementação real do IUsuarioRepositorio com EF Core](autenticacao/06-infraestrutura-usuario-repositorio.md) | feito |
| [07 - API: endpoint de cadastro de usuário com Minimal API](autenticacao/07-api-criar-usuario.md) | feito |
| [08 - Criar fluxo de login com JWT e extrair serviço de hash de senha](autenticacao/08-login-jwt.md) | feito |
| [09 - Validar JWT no pipeline e expor endpoint de usuário autenticado](autenticacao/09-autorizacao-papel.md) | feito |
| [10 - Cobrir login e hash de senha com testes automatizados](autenticacao/10-testes-login-senha-servico.md) | feito |
| [11 - Cobrir geração e validação do token JWT com testes automatizados](autenticacao/11-testes-token-servico.md) | feito |
| [12 - Adotar Result Pattern nas invariantes do Usuario](autenticacao/12-result-pattern-usuario.md) | feito |
| [XX - Infraestrutura: fábrica de design-time e primeira Migration](autenticacao/XX-migration-inicial.md) | resolvida (task 07) |
| [XX - Infraestrutura: avaliar Persistence Model separado do Domínio](autenticacao/XX-persistence-model.md) | adiada (revisitar em Pedidos) |

## Módulo Catálogo

| Task | Status |
|---|---|
| [01 - Modelar a entidade Restaurante no Domínio](catalogo/01-dominio-restaurante.md) | feito |
| [02 - Modelar Cardapio e ItemCardapio no Domínio](catalogo/02-dominio-cardapio-item.md) | feito |

## Módulo Pedidos

_ainda não iniciado_

## Módulo Pagamentos

_ainda não iniciado_

---
tags: [backlog/tarefa, modulo/pagamentos, status/pendente]
---

# 03 — Persistir o Pagamento no Postgres, registrar o módulo no host e gerar a Migration inicial

**Módulo:** Pagamentos | **Camada:** Infraestrutura / API | **Deps:** Pagamentos 02

## O que fazer

- **Padrão:** Repository Pattern, igual a `PedidoRepositorio`; `DbContext` próprio do módulo no mesmo banco (`BasePlataformaEntregas`).
- **Unicidade:** índice único filtrado em `PedidoId` só para `Status = 'Aprovado'`. Vários `Recusado` e `Falhou` por Pedido são permitidos; o banco barra um segundo `Aprovado` (evento duplicado ou dois `fechar` simultâneos).
- **Status:** gravado como texto (`HasConversion<string>()`), para o filtro do índice ser legível.
- **Limite conhecido:** quem perder a corrida do índice recebe `DbUpdateException` no `AtualizarAsync`, que sobe até o `fechar`; tratar isso fica fora desta task.
- **Composição:** Pagamentos não tem endpoints; `Modulos.Pagamentos.Api` só compõe a DI, no mesmo arranjo de `Modulos.Pedidos.Api`.

## Checklist

### 1. Infraestrutura
- [ ] **a:** Na pasta `Modulos/Pagamentos/`, criar a pasta `Modulos.Pagamentos.Infraestrutura/`.
- [ ] **b:** Dentro dela, criar o projeto `Modulos.Pagamentos.Infraestrutura` (biblioteca de classes), com os mesmos pacotes de `Modulos.Catalogo.Infraestrutura`, exceto os de Redis: `Microsoft.EntityFrameworkCore`, `Microsoft.EntityFrameworkCore.Relational`, `Microsoft.EntityFrameworkCore.Design` (`PrivateAssets` `all`) e `Npgsql.EntityFrameworkCore.PostgreSQL`, nas mesmas versões.
- [ ] **c:** Dentro do projeto, criar a pasta `Persistencia/`. Dentro dela, criar a classe `PagamentosDbContext`, com `DbSet<Pagamento> Pagamentos` e as configurações aplicadas pelo assembly, como em `PedidosDbContext`.
- [ ] **d:** Dentro de `Persistencia/`, criar a pasta `Configuracoes/`. Dentro dela, criar a classe `PagamentoConfiguracao` (`internal`), com chave em `Id`; `PedidoId`, `Valor`, `Status` e `CriadoEm` obrigatórios; `Status` convertido para texto; índice único em `PedidoId` com o filtro `"Status" = 'Aprovado'`.
- [ ] **e:** Dentro de `Persistencia/`, criar a pasta `Repositorios/`. Dentro dela, criar a classe `PagamentoRepositorio` (`internal`), implementando `IPagamentoRepositorio`: `AdicionarAsync` e `AtualizarAsync` gravam com `SaveChangesAsync`, como em `PedidoRepositorio`; `ExisteAprovadoPorPedidoAsync` retorna se há `Pagamento` com o `PedidoId` e `Status == StatusPagamento.Aprovado`.
- [ ] **f:** Na raiz do projeto, criar `InjecaoDeDependencia`, com um método de extensão de `IServiceCollection` chamado `RegistrarPagamentosInfraestrutura`, recebendo `IConfiguration`, que registra o `PagamentosDbContext` com `UseNpgsql` na connection string `BasePlataformaEntregas` e o `IPagamentoRepositorio` como `Scoped`.

### 2. API
- [ ] **a:** Na pasta `Modulos/Pagamentos/`, criar a pasta `Modulos.Pagamentos.Api/`. Dentro dela, criar o projeto `Modulos.Pagamentos.Api` (`FrameworkReference Microsoft.AspNetCore.App`, sem endpoints).
- [ ] **b:** Na raiz do projeto, criar `InjecaoDeDependencia`, com um método de extensão de `IServiceCollection` chamado `RegistrarPagamentosApi`, recebendo `IConfiguration`, chamando `RegistrarPagamentosInfraestrutura` e `RegistrarPagamentosAplicacao`.
- [ ] **c:** No `Program.cs` do host `PlataformaEntregas.Api`, chamar `RegistrarPagamentosApi` junto das demais.

### 3. DI & Migrations
- [ ] **a:** Gerar a Migration `InicialPagamento` do `PagamentosDbContext`, com `Modulos.Pagamentos.Infraestrutura` como projeto alvo e `PlataformaEntregas.Api` como startup project. Ler a migration gerada e confirmar a tabela `Pagamentos`, o `Status` como texto e o índice único filtrado.

### 4. QA & Testes
- [ ] **a:** Aplicar a migration no banco e confirmar que a tabela e o índice existem.
- [ ] **b:** Inserir dois `Pagamento` `Aprovado` do mesmo `PedidoId`, por SQL, e confirmar que o segundo falha.
- [ ] **c:** Inserir um `Recusado` e um `Falhou` do mesmo `PedidoId` e confirmar que ambos passam.

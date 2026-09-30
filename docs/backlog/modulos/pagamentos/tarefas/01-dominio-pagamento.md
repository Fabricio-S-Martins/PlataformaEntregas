---
tags: [backlog/tarefa, modulo/pagamentos, status/pendente]
---

# 01 — Modelar o agregado Pagamento com status e transições no Domínio e cobrir com testes automatizados

**Módulo:** Pagamentos | **Camada:** Domínio | **Deps:** —

## O que fazer

- **Status:** `Pendente` é o estado inicial; `Aprovado`, `Recusado` e `Falhou` são finais. Nova tentativa do cliente cria um novo `Pagamento`, nunca reabre um existente.
- **Transições:** `Aprovar`, `Recusar` e `MarcarFalha` só valem a partir de `Pendente`.
- **Padrão:** segue `Pedido` — fábrica estática `Criar` devolvendo `Resultado<T>`, construtor privado e propriedades com `private set`.

## Checklist

### 1. Domínio
- [ ] **a:** Na pasta `Modulos/`, criar a pasta `Pagamentos/`. Dentro dela, criar a pasta `Modulos.Pagamentos.Dominio/`.
- [ ] **b:** Dentro dela, criar o projeto `Modulos.Pagamentos.Dominio` (biblioteca de classes).
- [ ] **c:** Dentro do projeto, criar a pasta `Enums/`. Dentro dela, criar o enum `StatusPagamento`, com `Pendente`, `Aprovado`, `Recusado` e `Falhou`.
- [ ] **d:** Dentro do projeto, criar a pasta `Entidades/`. Dentro dela, criar a classe `Pagamento`, com as propriedades `Id` (`Guid`), `PedidoId` (`Guid`), `Valor` (`decimal`), `Status` (`StatusPagamento`, `private set`) e `CriadoEm` (`DateTime`).
- [ ] **e:** Em `Pagamento`, criar dois construtores: um privado sem parâmetros (para o EF Core) e outro privado recebendo `pedidoId` e `valor`. O `Id` é gerado dentro do construtor, não é parâmetro: `Guid.NewGuid()`; o `Status` recebe `StatusPagamento.Pendente` e o `CriadoEm` recebe `DateTime.UtcNow`.
- [ ] **f:** Em `Pagamento`, criar a fábrica estática `Criar`, recebendo `pedidoId` e `valor` e retornando `Resultado<Pagamento>`, acumulando os erros: `Identificador do Pedido inválido.` se `pedidoId == Guid.Empty`; `Valor inválido.` se `valor <= 0`.
- [ ] **g:** Em `Pagamento`, criar os métodos `Aprovar`, `Recusar` e `MarcarFalha`, cada um retornando `Resultado<Pagamento>`. Se `Status != StatusPagamento.Pendente`, falham com `Pagamento em {Status} não pode ser aprovado.`, `... não pode ser recusado.` e `... não pode ser marcado como falho.`, respectivamente. Se passar, mudam o `Status` para `Aprovado`, `Recusado` e `Falhou`, e retornam `ComSucesso(this)`.

### 2. Documentação
- [ ] **a:** Criar `docs/documentacao/modulos/pagamentos/pagamentos.md` com o que o módulo é, os status do pagamento e as regras de negócio
- [ ] **b:** Perguntar ao Dev se quer o plano de documentação do restante do módulo/fluxo

### 3. QA & Testes
- [ ] **a:** Na pasta `Testes/`, criar a pasta `Pagamentos/`. Dentro dela, criar a pasta `Modulos.Pagamentos.Dominio.Testes/`. Dentro dela, criar o projeto de testes xUnit `Modulos.Pagamentos.Dominio.Testes`, com os mesmos pacotes de `Modulos.Pedidos.Dominio.Testes`.
- [ ] **b:** Dentro do projeto, criar a pasta `Entidades/`. Dentro dela, criar `PagamentoTestes`, cobrindo `Criar` com dados válidos: `Status` `Pendente`, `Id` diferente de `Guid.Empty` e `CriadoEm` preenchido.
- [ ] **c:** Testar `Criar` com `pedidoId` vazio, com `valor` zero, com `valor` negativo e com os dois inválidos juntos: falha com as mensagens, todas acumuladas.
- [ ] **d:** Testar `Aprovar`, `Recusar` e `MarcarFalha` a partir de `Pendente`: sucesso e `Status` correspondente.
- [ ] **e:** Testar cada uma das três transições a partir de um `Pagamento` já `Aprovado`, `Recusado` ou `Falhou`: falha com a mensagem e `Status` inalterado.

---
tags: [tarefa, modulo/pagamentos, tarefa/em-andamento]
---

# 04 — Criar o gateway de pagamento falso com retry, circuit breaker e timeout do Polly e cobrir com testes automatizados

- **Modulo:** Pagamentos
- **Camada:** Infraestrutura

## O que fazer

- **Padrão:** Ports and Adapters — `GatewayPagamentoFalsoServico` é o adaptador de `IGatewayPagamentoServico`; a resiliência é um `ResiliencePipeline` do Polly v8.
- **Percentuais:** ficam na própria `GatewayPagamentoFalsoServico`, como constantes padrão (`PercentualAprovadoPadrao` 70, `PercentualRecusadoPadrao` 20, `PercentualFalhaPadrao` 10). Não há `appsettings.json` nem `IOptions`. O construtor recebe os três valores (os testes passam outros) e lança `ArgumentException` se algum estiver fora de 0 a 100 ou se a soma não for 100.
- **Sorteio:** `Random.Shared.Next(100)` devolve de 0 a 99. Abaixo do aprovado, retorna `true`; abaixo de aprovado + recusado, retorna `false`; no resto, lança `GatewayPagamentoIndisponivelException`.
- **Ordem das estratégias:** de fora para dentro, retry, circuit breaker e timeout. O timeout vale por tentativa e o circuit breaker enxerga cada tentativa.
- **Retry:** 3 novas tentativas depois da primeira, com espera exponencial começando em 200 ms.
- **Timeout:** 2 s por tentativa.
- **Circuit breaker:** abre com 50% de falhas, com no mínimo 5 chamadas na janela padrão do Polly (30 s), e fica aberto por 30 s.
- **O que conta como falha:** só `GatewayPagamentoIndisponivelException` e `TimeoutRejectedException`. Recusa (`false`) não é falha. Com o circuito aberto, o retry não tenta de novo: `BrokenCircuitException` sobe direto e o handler marca `Falhou`.
- **Pior caso:** cerca de 9 s dentro da requisição de `fechar` (4 tentativas de 2 s mais 1,4 s de espera).
- **Estado do circuito:** vive no pipeline, por isso o pipeline é `Singleton`; se fosse criado a cada requisição, o circuito nunca abriria.
- **Ref:** https://www.pollydocs.org/

## Checklist

### 1. Infraestrutura
- [x] **a:** No projeto `Modulos.Pagamentos.Infraestrutura`, adicionar os pacotes `Polly.Core` e `Microsoft.Extensions.Logging.Abstractions`.
- [x] **b:** Na raiz do projeto, criar a pasta `Gateway/`, com:
  - [x] **b.1:** A classe `GatewayPagamentoIndisponivelException`, herdando de `Exception`.
  - [x] **b.2:** A classe estática `PipelineResilienciaGateway`, com o método `Criar`, recebendo um `ILogger` e retornando um `ResiliencePipeline<bool>` com, nesta ordem, retry, circuit breaker e timeout, nos valores e nas condições de falha descritos acima. Registrar aviso em log a cada nova tentativa e na abertura do circuito, e informação no fechamento.
  - [x] **b.3:** A classe `GatewayPagamentoFalsoServico`, implementando `IGatewayPagamentoServico`, com as três constantes padrão e o construtor que recebe os três percentuais e o `ResiliencePipeline<bool>`, validando os percentuais. Em `CobrarAsync`, executar o sorteio dentro do pipeline.
- [x] **c:** Em `RegistrarPagamentosInfraestrutura`, registrar o `ResiliencePipeline<bool>` como `Singleton`, criado por `PipelineResilienciaGateway.Criar` com o `ILogger` do gateway, e registrar `IGatewayPagamentoServico` como `Scoped`, criando `GatewayPagamentoFalsoServico` com as constantes padrão e o pipeline.

### 2. Documentação
- [ ] **a:** Atualizar `docs/documentacoes/modulos/pagamentos/fluxos/pagamentos-processamento.md` com a espera e as tentativas da cobrança, o circuito aberto e o resultado `Falhou`
- [ ] **b:** Atualizar `docs/documentacoes/modulos/pagamentos/pagamentos.md` com o gateway falso e os percentuais padrão
- [ ] **c:** Perguntar ao Dev se quer o plano de documentação do restante do módulo/fluxo

### 3. QA & Testes
- [ ] **a:** Na pasta `Testes/Pagamentos/`, criar a pasta `Modulos.Pagamentos.Infraestrutura.Testes/`, com o projeto de testes xUnit `Modulos.Pagamentos.Infraestrutura.Testes`, com os mesmos pacotes de `Modulos.Pedidos.Aplicacao.Testes`.
- [ ] **b:** No projeto de testes, criar a pasta `Gateway/`, com os testes do `GatewayPagamentoFalsoServico` e do pipeline:
  - [ ] **b.1:** Percentuais 100/0/0 (sempre aprovado) e 0/100/0 (sempre recusado, sem retry).
  - [ ] **b.2:** Percentuais 0/0/100: o serviço tenta 4 vezes (a primeira mais 3 novas) e lança `GatewayPagamentoIndisponivelException`.
  - [ ] **b.3:** Falhas repetidas até abrir o circuito: a chamada seguinte lança `BrokenCircuitException` sem executar o sorteio.
  - [ ] **b.4:** Uma tentativa que passa de 2 s: `TimeoutRejectedException` é tratada como falha e aciona o retry.
  - [ ] **b.5:** Percentuais inválidos (soma diferente de 100, valor negativo ou acima de 100): o construtor lança `ArgumentException`.
- [ ] **c:** Aplicar a migration `InicialPagamento` no banco, com `Modulos.Pagamentos.Infraestrutura` como projeto alvo e `PlataformaEntregas.Api` como startup project, e confirmar que a tabela `Pagamentos` e o índice existem.
- [ ] **d:** Inserir dois `Pagamento` `Aprovado` do mesmo `PedidoId`, por SQL, e confirmar que o segundo falha.
- [ ] **e:** Inserir um `Recusado` e um `Falhou` do mesmo `PedidoId` e confirmar que ambos passam.
- [ ] **f:** Com o módulo rodando, chamar `fechar` várias vezes pelo `PlataformaEntregas.Api.http` e confirmar nos logs os avisos de retry. Para ver a abertura do circuito, trocar temporariamente as constantes padrão para 0/0/100 e voltar para 70/20/10 depois.

---
tags: [backlog/tarefa, modulo/pagamentos, status/pendente]
---

# 04 — Criar o gateway de pagamento falso configurável com retry, circuit breaker e timeout do Polly e cobrir com testes automatizados

**Módulo:** Pagamentos | **Camada:** Infraestrutura | **Deps:** Pagamentos 02, Pagamentos 03

## O que fazer

- **Padrão:** Ports and Adapters — `GatewayPagamentoFalsoServico` é o adaptador de `IGatewayPagamentoServico`; a resiliência é um `ResiliencePipeline` do Polly v8.
- **Resultado do sorteio:** um número de 0 a 99 (`Random.Shared.Next(100)`) decide: abaixo do `PercentualAprovado`, aprovado (`true`); abaixo de `PercentualAprovado + PercentualRecusado`, recusado (`false`); no resto, lança `GatewayPagamentoIndisponivelException`.
- **Configuração:** seção `GatewayPagamentoFalso` do `appsettings.json`, com `PercentualAprovado` 70, `PercentualRecusado` 20 e `PercentualFalha` 10. Cada percentual vai de 0 a 100 e a soma é 100; caso contrário a aplicação não sobe (`ValidateOnStart`).
- **Ordem das estratégias:** de fora para dentro, retry, circuit breaker e timeout — o timeout vale por tentativa, e o circuit breaker enxerga cada tentativa.
- **Retry:** 3 novas tentativas depois da primeira, com espera exponencial começando em 200 ms.
- **Timeout:** 2 s por tentativa.
- **Circuit breaker:** abre com 50% de falhas, com no mínimo 5 chamadas na janela de amostragem padrão do Polly (30 s), e fica aberto por 30 s.
- **O que conta como falha:** só `GatewayPagamentoIndisponivelException` e `TimeoutRejectedException`. Recusa (`false`) não é falha. Com o circuito aberto, o retry não tenta de novo: `BrokenCircuitException` sobe direto e o handler marca `Falhou`.
- **Pior caso:** cerca de 9 s dentro da requisição de `fechar` (4 tentativas de 2 s mais 1,4 s de espera).
- **Estado do circuito:** vive no pipeline, por isso o pipeline é `Singleton`; se fosse criado a cada requisição, o circuito nunca abriria.
- **Ref:** https://www.pollydocs.org/

## Checklist

### 1. Infraestrutura
- [ ] **a:** No projeto `Modulos.Pagamentos.Infraestrutura`, adicionar os pacotes `Polly.Core`, `Microsoft.Extensions.Options.ConfigurationExtensions`, `Microsoft.Extensions.Hosting.Abstractions` e `Microsoft.Extensions.Logging.Abstractions`.
- [ ] **b:** Na raiz do projeto, criar a pasta `Gateway/`.
- [ ] **c:** Dentro dela, criar a classe `GatewayPagamentoFalsoOpcoes`, com `PercentualAprovado`, `PercentualRecusado` e `PercentualFalha` (`int`), e o nome da seção `GatewayPagamentoFalso` como constante.
- [ ] **d:** Dentro dela, criar a classe `GatewayPagamentoIndisponivelException`, herdando de `Exception`.
- [ ] **e:** Dentro dela, criar a classe estática `PipelineResilienciaGateway`, com um método `Criar`, recebendo um `ILogger`, que retorna um `ResiliencePipeline<bool>` com, nesta ordem: retry, circuit breaker e timeout, nos valores e nas condições de falha descritos acima. Registrar aviso em log em cada nova tentativa e na abertura do circuito, e informação no fechamento.
- [ ] **f:** Dentro dela, criar a classe `GatewayPagamentoFalsoServico`, implementando `IGatewayPagamentoServico`, recebendo `IOptions<GatewayPagamentoFalsoOpcoes>` e o `ResiliencePipeline<bool>`. Em `CobrarAsync`, executa o sorteio dentro do pipeline.
- [ ] **g:** Em `RegistrarPagamentosInfraestrutura`, ligar `GatewayPagamentoFalsoOpcoes` à seção `GatewayPagamentoFalso`, validando os percentuais (cada um de 0 a 100 e soma 100) com `ValidateOnStart`.
- [ ] **h:** No mesmo método, registrar o `ResiliencePipeline<bool>` como `Singleton`, criado por `PipelineResilienciaGateway.Criar` com o `ILogger` do gateway, e registrar `IGatewayPagamentoServico`/`GatewayPagamentoFalsoServico` como `Scoped`.
- [ ] **i:** No `appsettings.json` do host `PlataformaEntregas.Api`, adicionar a seção `GatewayPagamentoFalso` com os três percentuais.

### 2. Documentação
- [ ] **a:** Atualizar `docs/documentacao/modulos/pagamentos/fluxos/pagamentos-processamento.md` com a espera e as tentativas da cobrança, o circuito aberto e o resultado `Falhou`
- [ ] **b:** Atualizar `docs/documentacao/modulos/pagamentos/pagamentos.md` com o gateway falso e a configuração dos percentuais
- [ ] **c:** Perguntar ao Dev se quer o plano de documentação do restante do módulo/fluxo

### 3. QA & Testes
- [ ] **a:** Na pasta `Testes/Pagamentos/`, criar a pasta `Modulos.Pagamentos.Infraestrutura.Testes/`. Dentro dela, criar o projeto de testes xUnit `Modulos.Pagamentos.Infraestrutura.Testes`, com os mesmos pacotes de `Modulos.Pedidos.Aplicacao.Testes`.
- [ ] **b:** Dentro do projeto, criar a pasta `Gateway/` com os testes do `GatewayPagamentoFalsoServico` e do pipeline: percentuais 100/0/0 retornam sempre aprovado; 0/100/0 retornam sempre recusado, sem retry.
- [ ] **c:** Testar percentuais 0/0/100: o serviço tenta 4 vezes (a primeira mais 3 novas) e lança `GatewayPagamentoIndisponivelException`.
- [ ] **d:** Testar falhas repetidas até abrir o circuito: a chamada seguinte lança `BrokenCircuitException` sem executar o sorteio.
- [ ] **e:** Testar uma tentativa que passa de 2 s: `TimeoutRejectedException` é tratada como falha e aciona o retry.
- [ ] **f:** Testar percentuais inválidos (soma diferente de 100, valor negativo ou acima de 100): a validação das opções falha.
- [ ] **g:** Com o módulo rodando, chamar `fechar` várias vezes pelo `PlataformaEntregas.Api.http` e confirmar nos logs os avisos de retry e a abertura do circuito quando os percentuais estão em 0/0/100.

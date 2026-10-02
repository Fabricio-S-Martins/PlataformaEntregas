---
tags: [backlog, modulo/pedidos, fluxo/pedido-status, tarefa/pendente]
---

# 09 — Confirmar o pagamento do Pedido ao receber a aprovação de Pagamentos, registrando em log quando o Pedido não puder ser pago

**Módulo:** Pedidos | **Camada:** Aplicação | **Deps:** Compartilhado 03, Pedidos 08

## O que fazer

- **Padrão:** consumidor de Integration Event — o handler escuta `PagamentoAprovadoIntegracao` e reaproveita o `ConfirmarPagamentoCommand`, sem duplicar a regra.
- **Falha esperada:** se o Pedido foi cancelado durante o pagamento, `ConfirmarPagamentoCommand` falha; o handler registra a inconsistência em log e não relança, para o cliente não receber `500` no `fechar`. O estorno fica fora desta task.
- **Log:** aviso com `PedidoId`, `PagamentoId` e `Valor` em campos separados, para consultar quem foi cobrado sem Pedido pago.
- **Escopo:** só `ArgumentException` (regra de negócio) e `InvalidOperationException` (Pedido não encontrado) são tratadas; qualquer outra exceção sobe.

## Checklist

### 1. Aplicação
- [ ] **a:** No projeto `Modulos.Pedidos.Aplicacao`, adicionar o pacote `Microsoft.Extensions.Logging.Abstractions`.
- [ ] **b:** Na raiz do projeto, criar a pasta `Integracao/`. Dentro dela, criar a pasta `PagamentoAprovado/`.
- [ ] **c:** Dentro dela, criar a classe `PagamentoAprovadoHandler`, implementando `INotificationHandler<PagamentoAprovadoIntegracao>`, recebendo `ISender` e `ILogger<PagamentoAprovadoHandler>`.
- [ ] **d:** No `Handle`, despachar `ConfirmarPagamentoCommand` com o `PedidoId` da notificação.
- [ ] **e:** Capturar `ArgumentException` e `InvalidOperationException` e registrar aviso com `PedidoId`, `PagamentoId`, `Valor` e a mensagem da exceção, sem relançar.

### 2. Documentação
- [ ] **a:** Atualizar `docs/documentacao/modulos/pedidos/fluxos/pedido-operacao.md` com o passo de confirmação por pagamento aprovado e o caso de Pedido cancelado
- [ ] **b:** Perguntar ao Dev se quer o plano de documentação do restante do módulo/fluxo

### 3. QA & Testes
- [ ] **a:** Em `Testes/Pedidos/Modulos.Pedidos.Aplicacao.Testes/`, criar a pasta `Integracao/PagamentoAprovado/` com os testes do `PagamentoAprovadoHandler`: notificação com Pedido pagável despacha `ConfirmarPagamentoCommand` com o `PedidoId` e não registra aviso.
- [ ] **b:** Testar `ConfirmarPagamentoCommand` lançando `ArgumentException` (Pedido cancelado): o handler não relança e registra aviso com `PedidoId`, `PagamentoId` e `Valor`.
- [ ] **c:** Testar `ConfirmarPagamentoCommand` lançando `InvalidOperationException` (Pedido não encontrado): o handler não relança e registra aviso.
- [ ] **d:** Testar outra exceção qualquer: o handler a relança.
- [ ] **e:** Com Pagamentos 04 concluído, chamar `fechar` na API e confirmar que o Pedido chega a `Pago`.

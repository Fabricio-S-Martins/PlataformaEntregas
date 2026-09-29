# 03 — Criar o projeto de contratos de integração entre módulos com os eventos de pedido fechado e pagamento aprovado

**Módulo:** Compartilhado | **Camada:** Compartilhado | **Status:** todo | **Deps:** —

## O que fazer

- **Padrão:** Integration Events — records imutáveis que atravessam a fronteira entre módulos, sem dependência de nenhum módulo.
- **Transporte:** implementam `INotification` e são publicados via `IPublisher` (in-process); ao trocar por RabbitMQ os contratos continuam os mesmos.
- **Ref:** https://learn.microsoft.com/en-us/dotnet/architecture/microservices/multi-container-microservice-net-applications/integration-event-based-microservice-communications

## Checklist

### 1. Compartilhado
- [ ] **a:** Na pasta `Compartilhado/`, criar a pasta `Compartilhado.Contratos/`.
- [ ] **b:** Dentro dela, criar o projeto `Compartilhado.Contratos` (biblioteca de classes), com o pacote `MediatR.Contracts`.
- [ ] **c:** Dentro do projeto, criar a pasta `Pedidos/`. Dentro dela, criar o record `PedidoFechadoIntegracao`, implementando `INotification`, com `PedidoId` (`Guid`) e `Valor` (`decimal`).
- [ ] **d:** Dentro do projeto, criar a pasta `Pagamentos/`. Dentro dela, criar o record `PagamentoAprovadoIntegracao`, implementando `INotification`, com `PedidoId` (`Guid`), `PagamentoId` (`Guid`) e `Valor` (`decimal`).

### 2. QA & Testes
- [ ] **a:** Compilar a solution e confirmar zero erros de build.

# 07 — Fechar o Pedido com total calculado, permitindo nova tentativa e bloqueando novos itens

**Módulo:** Pedidos | **Camada:** Domínio | **Status:** todo | **Deps:** —

## O que fazer

- **Fechar:** só em `Criado` e com ao menos 1 item; pode ser repetido enquanto o Pedido estiver `Criado` (nova tentativa de pagamento) — cada chamada regrava `FechadoEm` e gera um novo `PedidoFechadoEvento`.
- **Status:** continua `Criado` ao fechar; não existe novo status.
- **Total:** soma de `Quantidade * PrecoUnitario` dos itens; é calculado, não persistido.
- **Bloqueio:** depois de fechado, `AdicionarItem` falha, para o valor cobrado não mudar entre tentativas.

## Checklist

### 1. Domínio
- [ ] **a:** Na pasta `Modulos/Pedidos/Modulos.Pedidos.Dominio/Eventos/`, criar o record `PedidoFechadoEvento`, implementando `IEventoDominio`, com `PedidoId` (`Guid`) e `Valor` (`decimal`).
- [ ] **b:** Em `Pedido`, adicionar a propriedade `FechadoEm` (`DateTime?`, `private set`), nula até o primeiro fechamento.
- [ ] **c:** Em `Pedido`, adicionar a propriedade somente leitura `Total` (`decimal`), somando `Quantidade * PrecoUnitario` de cada item de `Itens`.
- [ ] **d:** Em `Pedido`, criar o método `Fechar`, retornando `Resultado<Pedido>`, com as validações nesta ordem: falha com `Pedido em {Status} não pode ser fechado.` se `Status != StatusPedido.Criado`; falha com `Pedido sem itens não pode ser fechado.` se `Itens.Count <= 0`. Se passar, define `FechadoEm` com `DateTime.UtcNow`, adiciona `PedidoFechadoEvento(Id, Total)` a `EventosInterno` e retorna `ComSucesso(this)`.
- [ ] **e:** Em `AdicionarItem`, logo após a validação de status, falhar com `Pedido fechado não aceita novos itens.` se `FechadoEm` tiver valor.

### 2. Documentação
- [ ] **a:** Atualizar `docs/modulos/pedidos/pedidos.md` com as regras de fechar o Pedido e do bloqueio de novos itens
- [ ] **b:** Perguntar ao Dev se quer o plano de documentação do restante do módulo/fluxo

### 3. QA & Testes
- [ ] **a:** Em `Testes/Pedidos/Modulos.Pedidos.Dominio.Testes/Entidades/PedidoTestes.cs`, testar fechar Pedido `Criado` com itens: sucesso, `FechadoEm` preenchido e `PedidoFechadoEvento` com o `Total` correto.
- [ ] **b:** Testar o `Total` com mais de um item: soma de quantidade vezes preço unitário de cada um.
- [ ] **c:** Testar fechar Pedido sem itens: falha com a mensagem, sem `FechadoEm` nem evento.
- [ ] **d:** Testar fechar Pedido fora de `Criado` (por exemplo, `Pago`): falha com a mensagem, sem evento.
- [ ] **e:** Testar fechar duas vezes seguidas o mesmo Pedido `Criado`: as duas com sucesso e dois `PedidoFechadoEvento`.
- [ ] **f:** Testar `AdicionarItem` depois de fechar: falha com a mensagem e o item não entra em `Itens`.

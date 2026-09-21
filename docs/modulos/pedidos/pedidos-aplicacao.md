# Pedidos: Aplicação

Projeto `Modulos.Pedidos.Aplicacao`. Módulo: [Pedidos](pedidos.md).

## Responsabilidade
Orquestrar cada operação sobre o pedido: buscar, executar a regra do domínio, salvar e publicar os eventos.

## Expõe
- **Casos de uso** (MediatR, um `Command` + `Handler` cada): CriarPedido, AdicionarItem, ConfirmarPagamento, Aceitar, IniciarPreparo, SairParaEntrega, Entregar, Cancelar.
- **Padrão dos handlers:** obtém o pedido (não achou → `InvalidOperationException`), chama o método do domínio (falha → `ArgumentException`), atualiza e despacha os eventos. `CriarPedido` cria em vez de obter e retorna o id.
- **IPedidoRepositorio:** `AdicionarAsync`, `AtualizarAsync`, `ObterPorIdAsync`.
- **Notificações** `Pedido*Notificacao` e o `DespachanteDeEventosDominio`, que converte cada evento do domínio na notificação correspondente.
- **Registro:** `RegistrarPedidosAplicacao` (MediatR).

## Fluxos
- [O que acontece a cada ação](../../fluxos/pedido-operacao.md)

## Depende de
- [Domínio](pedidos-dominio.md): `Pedido` e eventos.
- MediatR.

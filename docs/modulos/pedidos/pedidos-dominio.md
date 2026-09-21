# Pedidos: Domínio

Projeto `Modulos.Pedidos.Dominio`. Módulo: [Pedidos](pedidos.md).

## Responsabilidade
Regras do pedido, dos itens e das transições de status.

## Expõe
- **Pedido:** nasce em `Criado` por `Criar(clienteId, restauranteId)`; ids não podem ser vazios. Métodos: `AdicionarItem`, `ConfirmarPagamento`, `Aceitar`, `IniciarPreparo`, `SairParaEntrega`, `Entregar`, `Cancelar`. Todos retornam `Resultado<T>` e registram um evento.
- **ItemPedido:** criado só por `Pedido.AdicionarItem`; ids válidos, quantidade > 0 e preço unitário > 0.
- **StatusPedido** e um evento por operação (`Pedido*Evento`, contrato `IEventoDominio`).

## Fluxos
- [Etapas do pedido](../../fluxos/pedido-status.md)

## Depende de
- `Resultado<T>`, do projeto Compartilhado.Dominio.

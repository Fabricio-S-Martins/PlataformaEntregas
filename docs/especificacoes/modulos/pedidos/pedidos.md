---
tags: [especificacao, modulo/pedidos, especificacao/rascunho]
---
# Pedidos: regras de negócio

**Para que existe:** registrar o pedido que um cliente faz a um restaurante e levá-lo por etapas até a entrega.

## Termos
| Termo | Significado |
|---|---|
| Pedido | Compra feita por um cliente a um restaurante, com itens. |
| Item do pedido | Produto pedido, com quantidade e preço. |
| Etapa | Situação do pedido: Criado, Pago, Aceito, Em preparo, Saiu para entrega, Entregue ou Cancelado. |

## Regras
- **Etapas em ordem:** Criado → Pago → Aceito → Em preparo → Saiu para entrega → Entregue, sem pular etapas.
- **Itens só no início:** itens só podem ser adicionados enquanto o pedido está Criado.
- **Item com valor:** cada item precisa de quantidade e preço maiores que zero.
- **Pagamento exige itens:** pedido sem itens não pode ser pago.
- **Cancelamento limitado:** o pedido só pode ser cancelado enquanto está Criado ou Pago.
- **Fechar e pagar só pelo dono:** só o cliente dono do pedido o fecha e paga; outro usuário, mesmo autenticado, é recusado.
- **Aceitar e preparar só pelo Restaurante:** só quem tem o papel Restaurante aceita o pedido e inicia o preparo.
- **Entregar só pelo Entregador:** só quem tem o papel Entregador marca o pedido como Entregue.
- **Preço do Catálogo:** o preço do item é o do Catálogo no momento em que o item é adicionado e fica gravado no item; mudança posterior no Catálogo não altera pedidos existentes.
- **Pedido não pago expira:** pedido que fica Criado por 24 horas, desde a criação, sem ser pago vai para Cancelado sozinho.
- **Itens do restaurante do pedido:** todo item do pedido pertence ao restaurante do pedido; item de outro restaurante é recusado.
- **Cliente e restaurante obrigatórios:** todo pedido tem o cliente, que é o usuário autenticado, e um restaurante que existe.
- **Cancelar pedido Pago reembolsa:** cancelar um pedido Pago, antes de o restaurante aceitar, devolve o valor ao cliente.
- **Sem prazo para aceitar:** por enquanto o restaurante não tem prazo máximo para aceitar o pedido.

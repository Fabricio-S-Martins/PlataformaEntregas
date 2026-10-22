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

## Aguardando Validações
- Validação 1: quem pode executar cada ação (cliente, restaurante, entregador)? Hoje qualquer usuário autenticado pode fechar ou pagar o pedido de outro cliente.
- Validação 2: pedido que ficou Criado e nunca foi pago expira? Depois de quanto tempo?
- Validação 3: o pedido pode misturar itens de restaurantes diferentes, ou todos os itens são do restaurante do pedido?
- Validação 4: o preço do item vem do Catálogo ou é informado no pedido? E se o preço mudar depois?

# Etapas do pedido

## Objetivo
Levar o pedido de Criado até Entregue, ou Cancelado.

## Diagrama
```mermaid
flowchart TD
  Criado["Criado"] -->|"Confirmar pagamento (exige itens)"| Pago["Pago"]
  Pago -->|"Aceitar"| Aceito["Aceito"]
  Aceito -->|"Iniciar preparo"| EmPreparo["Em preparo"]
  EmPreparo -->|"Sair para entrega"| Saiu["Saiu para entrega"]
  Saiu -->|"Entregar"| Entregue["Entregue"]
  Criado -->|"Cancelar"| Cancelado["Cancelado"]
  Pago -->|"Cancelar"| Cancelado
```

## Regras
- Itens só podem ser adicionados na etapa Criado.
- Uma ação fora do diagrama é recusada com uma mensagem e o pedido continua na mesma etapa.
- Toda ação bem-sucedida é registrada como um evento do pedido, inclusive a criação e a adição de item.

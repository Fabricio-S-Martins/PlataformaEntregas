---
tags: [documentacao, modulo/pedidos, camada/api]
---

# Integrar com Pedidos

O contrato completo de cada rota (campos, tipos e status de resposta) está no Swagger da API, disponível em desenvolvimento em `/swagger`. Este guia explica a ordem das chamadas e como ler os erros.

Regras e etapas do pedido: [Pedidos](../pedidos.md).

## Ordem das Chamadas

Todas as rotas começam com `/api`. O pedido percorre as etapas nesta ordem:

| Passo | Método | Rota | Descrição |
| :---: | :---: | :--- | :--- |
| **1** | `POST` | `/api/pedidos` | Cria o pedido e devolve o `id`. |
| **2** | `POST` | `/api/pedidos/{id}/itens` | Adiciona itens ao pedido *(repita para cada item)*. Permitido apenas até o pagamento. |
| **3** | `POST` | `/api/pedidos/{id}/confirmar-pagamento` | Confirma o pagamento. Exige pelo menos um item. |
| **4** | `POST` | `/api/pedidos/{id}/aceitar` | Aceita o pedido. |
| **5** | `POST` | `/api/pedidos/{id}/iniciar-preparo` | Inicia o preparo do pedido. |
| **6** | `POST` | `/api/pedidos/{id}/sair-para-entrega` | Sinaliza que o pedido saiu para entrega. |
| **7** | `POST` | `/api/pedidos/{id}/entregar` | Finaliza e marca o pedido como entregue. |

> ⚠️ **Atenção:** `POST /api/pedidos/{id}/cancelar` só funciona enquanto o pedido não foi aceito.

## Respostas e Erros

| Status | Tipo | Descrição / Regra |
| :---: | :---: | :--- |
| **201** | Sucesso | Criado com sucesso (retorna o `id`). Aplicável na rota `/api/pedidos`. |
| **204** | Sucesso | Sucesso nas demais rotas. |
| **400** | Erro | A regra recusou a ação. O corpo é `{ "erro": "<mensagem>" }` e o pedido não muda.<br>*(Mensagens múltiplas vêm separadas por quebra de linha. Ex: "Pedido em Pago não pode ser aceito.")* |
| **404** | Erro | O `id` não existe (`"Pedido não encontrado."`). |
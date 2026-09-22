# Integrar com Catálogo

O contrato completo de cada rota (campos, tipos e status de resposta) está no Swagger da API, disponível em desenvolvimento em `/swagger`. Este guia explica a ordem das chamadas e como ler as respostas e os erros.

Regras do módulo: [Catálogo](catalogo.md).

## Ordem das Chamadas

Todas as rotas começam com `/api`. Cadastre o restaurante e use o id para consultá-lo:

| Passo | Método | Rota | Descrição |
| :---: | :---: | :--- | :--- |
| **1** | `POST` | `/api/restaurantes` | Cadastra o restaurante. Corpo: `nome` e `cnpj` (14 dígitos numéricos). |
| **2** | `GET` | `/api/restaurantes/{id}` | Consulta o restaurante pelo `id`. |

> ⚠️ **Atenção:** o cadastro não devolve o `id` do restaurante. Nenhuma das duas rotas exige token.

## Respostas e Erros

| Status | Tipo | Descrição / Regra |
| :---: | :---: | :--- |
| **201** | Sucesso | Restaurante cadastrado, sem corpo. Aplicável na rota `POST /api/restaurantes`. |
| **200** | Sucesso | Consulta: `{ "id": "<id>", "nome": "<nome>", "cnpj": "<cnpj>", "ativo": true }`. |
| **400** | Erro | A regra recusou o cadastro. O corpo é `{ "erro": "<mensagem>" }` e nada é criado.<br>*(Mensagens: "Nome inválido." ou "Cnpj inválido.". Mensagens múltiplas vêm separadas por quebra de linha.)* |
| **404** | Erro | O `id` não existe. Resposta sem corpo. Aplicável na rota `GET /api/restaurantes/{id}`. |

---
tags: [especificacao, modulo/catalogo, fluxo/catalogo-cadastro-restaurante, especificacao/rascunho]
---
# Cadastro de restaurante

[Catálogo](../catalogo.md)

**Objetivo:** registrar um novo restaurante com nome e CNPJ.

## Regras
- **Nome obrigatório:** o nome não pode ficar em branco. Mensagem: "Nome inválido."
- **CNPJ com 14 dígitos:** o CNPJ tem exatamente 14 dígitos numéricos. Mensagem: "Cnpj inválido."
- **Todos os erros juntos:** com mais de um erro, as mensagens vêm juntas, separadas por quebra de linha.
- **Restaurante nasce ativo:** o restaurante cadastrado começa ativo.
- **Recusa não cria restaurante:** um cadastro recusado não grava nada.

## Critérios de aceite
| Critério | Quando | Então |
|:-:|---|---|
| Critério 1: cadastro válido | Nome preenchido e CNPJ com 14 dígitos numéricos | O restaurante é criado ativo e o cadastro é concluído. |
| Critério 2: nome em branco | O nome está vazio ou só com espaços | O cadastro é recusado com "Nome inválido." e nenhum restaurante é criado. |
| Critério 3: CNPJ inválido | O CNPJ não tem exatamente 14 dígitos numéricos | O cadastro é recusado com "Cnpj inválido." e nenhum restaurante é criado. |
| Critério 4: vários erros | Nome e CNPJ inválidos | O cadastro é recusado com as duas mensagens, uma por linha. |

## Aguardando Validações
- Validação 1: quem pode cadastrar um restaurante (qualquer usuário ou só quem tem o papel Restaurante)?
- Validação 2: o restaurante cadastrado fica ligado ao usuário que o cadastrou?

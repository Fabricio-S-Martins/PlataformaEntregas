---
tags: [especificacao, modulo/catalogo, fluxo/catalogo-cadastro-restaurante, especificacao/rascunho]
---
# Cadastro de restaurante

[Catálogo](../catalogo.md)

**Objetivo:** registrar um novo restaurante com nome e CNPJ.

## Regras
- **Nome obrigatório:** o nome não pode ficar em branco. Mensagem: "Nome inválido."
- **CNPJ válido:** o CNPJ tem 14 dígitos numéricos e dígitos verificadores corretos. Mensagem: "Cnpj inválido."
- **CNPJ ainda não cadastrado:** se o CNPJ já pertence a outro restaurante, o cadastro é recusado.
- **Só o papel Restaurante:** só quem tem o papel Restaurante cadastra um restaurante.
- **Ligado ao usuário:** o restaurante cadastrado fica ligado ao usuário que o cadastrou.
- **Todos os erros juntos:** com mais de um erro, as mensagens vêm juntas, separadas por quebra de linha.
- **Restaurante nasce ativo:** o restaurante cadastrado começa ativo.
- **Recusa não cria restaurante:** um cadastro recusado não grava nada.

## Critérios de aceite
| Critério | Quando | Então |
|:-:|---|---|
| Critério 1: cadastro válido | Nome preenchido e CNPJ válido, ainda não cadastrado, por usuário com papel Restaurante | O restaurante é criado ativo, ligado a esse usuário, e o cadastro é concluído. |
| Critério 2: nome em branco | O nome está vazio ou só com espaços | O cadastro é recusado com "Nome inválido." e nenhum restaurante é criado. |
| Critério 3: CNPJ inválido | O CNPJ não tem 14 dígitos numéricos ou os dígitos verificadores estão errados | O cadastro é recusado com "Cnpj inválido." e nenhum restaurante é criado. |
| Critério 4: vários erros | Nome e CNPJ inválidos | O cadastro é recusado com as duas mensagens, uma por linha. |
| Critério 5: CNPJ repetido | O CNPJ já pertence a outro restaurante | O cadastro é recusado e nenhum restaurante é criado. |
| Critério 6: outro papel cadastra | Um usuário sem o papel Restaurante tenta cadastrar | O cadastro é recusado e nenhum restaurante é criado. |

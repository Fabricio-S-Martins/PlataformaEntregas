---
tags: [decisao, modulo/compartilhado, decisao/aceita]
---
# Result Pattern no Shared Kernel para o resultado dos casos de uso

- **Decisão e porquê:** `Resultado<T>` fica em `Compartilhado.Dominio` e é usado por todos os módulos para devolver sucesso ou falha dos casos de uso, com um contrato único.
- **Descartado:** (a confirmar) exceções para falhas esperadas de negócio — motivo a confirmar com o Dev.
- **Não fazer:** cada módulo criar o seu próprio tipo de resultado; usar exceção para falha de negócio esperada.

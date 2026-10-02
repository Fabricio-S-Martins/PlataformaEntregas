---
tags: [documentacao, modulo/pagamentos]
---

# Pagamentos

[Documentação](../../documentacao.md)

Um pagamento é a cobrança de um pedido. Cada tentativa de pagar gera um pagamento próprio, com o valor cobrado e o resultado.

## Status

Pendente → Aprovado, Recusado ou Falhou

- **Pendente:** estado inicial, aguardando o resultado da cobrança.
- **Aprovado:** a cobrança foi aceita.
- **Recusado:** a cobrança foi negada.
- **Falhou:** houve um erro técnico ao processar, sem resposta sobre a cobrança.

Aprovado, Recusado e Falhou são finais: não mudam mais.

## Regras

- Todo pagamento é criado Pendente.
- O pagamento precisa de um pedido válido e de valor maior que zero. Se os dois estiverem errados, os dois erros são informados juntos.
- Só um pagamento Pendente pode ser aprovado, recusado ou marcado como falho.
- Um pagamento finalizado nunca é reaberto. Uma nova tentativa do cliente cria um novo pagamento.

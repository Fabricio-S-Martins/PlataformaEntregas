---
tags: [decisao, modulo/autenticacao, decisao/aceita]
---
# Senha armazenada como hash PBKDF2 com salt

- **Decisão e porquê:** a senha nunca é guardada em texto puro: `ISenhaServico` gera o hash com salt (PBKDF2) e verifica reaproveitando o salt armazenado. Trocar o algoritmo depois invalida os hashes já gravados.
- **Descartado:** (a confirmar) outro algoritmo de hash (ex.: bcrypt, Argon2) — motivo a confirmar com o Dev.
- **Não fazer:** gravar senha em texto puro; gerar um salt novo ao verificar; calcular hash fora do `ISenhaServico`.

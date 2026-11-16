---
tags: [decisao, modulo/autenticacao, decisao/aceita]
---
# Autenticação por JWT, sem sessão no servidor

- **Decisão e porquê:** o login devolve um JWT e os endpoints leem Id e Papel das claims do token, sem consultar o banco. Autorização por papel (Cliente, Restaurante, Entregador) com `RequireAuthorization`.
- **Descartado:** (a confirmar) sessão em servidor ou cookie — motivo a confirmar com o Dev.
- **Não fazer:** guardar estado de sessão no servidor; ler Id ou Papel do banco para autorizar uma requisição.

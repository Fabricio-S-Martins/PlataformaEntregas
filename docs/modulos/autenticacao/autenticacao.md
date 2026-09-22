# Autenticação

[Documentação](../../README.md)

Cadastro de usuários e login. Cada usuário tem um papel e entra no sistema com e-mail e senha.

## Papéis

Cliente, Restaurante ou Entregador. O papel é definido no cadastro e acompanha o usuário autenticado.

## Regras

- O nome é obrigatório.
- O e-mail precisa ter formato válido (`nome@dominio.ext`). Ele é guardado sem espaços nas pontas e em letras minúsculas.
- O papel precisa ser Cliente, Restaurante ou Entregador.
- A senha não é guardada em texto puro.
- No login, e-mail ou senha incorretos geram a mesma mensagem, "E-mail ou Senha inválidos.", sem indicar qual dos dois errou.
- O login bem-sucedido devolve um token. As rotas protegidas exigem esse token.

## Fluxos

- [Cadastro de usuário](../../fluxos/autenticacao-cadastro.md)
- [Login](../../fluxos/autenticacao-login.md)

## Integração

Ordem das chamadas e erros: [Integrar com Autenticação](autenticacao-api.md)

# 10 — Cobrir login e hash de senha com testes automatizados

**Módulo:** Autenticação
**Camada:** Aplicação
**Status:** feito

## Contexto

A task 08 criou `LoginHandler` e extraiu `SenhaServico` (hash PBKDF2 com salt), mas nenhum dos dois tem teste — o teste existente do `CriarUsuarioHandler` só mocka `ISenhaServico`, nunca exercita o hash de verdade. Esse tipo de lógica (autenticação, geração/verificação de hash) é justamente onde um bug fica silencioso: o código compila, a aplicação roda, e só um teste automatizado pega uma falha real (ex: senha errada autenticando por engano, ou hash gerado de um jeito que nunca bate na verificação).

## O que fazer

1. Criar a pasta `CasosDeUso/Login/` dentro do projeto `Modulos.Autenticacao.Aplicacao.Testes`, espelhando a estrutura do projeto de produção.
2. Criar `LoginHandlerTestes.cs` nessa pasta, usando **Mock** (biblioteca Moq, já usada em `CriarUsuarioHandlerTestes`) para `IUsuarioRepositorio`, `ISenhaServico` e `ITokenServico`.
3. Criar a pasta `Servicos/` dentro do projeto de testes.
4. Criar `SenhaServicoTestes.cs` nessa pasta, usando a implementação real de `SenhaServico` (sem mock — é a própria lógica de hash sendo testada, não uma dependência dela).

## Cenários a cobrir

**`LoginHandler`:**
- Credenciais válidas (e-mail existente, senha confere no `ISenhaServico`) retornam o token vindo de `ITokenServico`.
- E-mail inexistente lança `ArgumentException`, sem chamar `ITokenServico`.
- Senha incorreta lança `ArgumentException`, sem chamar `ITokenServico`.

**`SenhaServico`:**
- `GerarHash` chamado duas vezes com a mesma senha produz hashes diferentes entre si (salt aleatório a cada chamada).
- `VerificarHash` retorna `true` quando a senha em texto puro corresponde ao hash gerado por `GerarHash` para ela.
- `VerificarHash` retorna `false` quando a senha em texto puro não corresponde ao hash.

## Arquivos a criar/alterar

- `Testes/Autenticacao/Modulos.Autenticacao.Aplicacao.Testes/CasosDeUso/Login/LoginHandlerTestes.cs`
- `Testes/Autenticacao/Modulos.Autenticacao.Aplicacao.Testes/Servicos/SenhaServicoTestes.cs`

## Notas / decisões tomadas

- `TokenServico` (Infraestrutura) fica fora deste card — testar a geração/validação do JWT de ponta a ponta exige um projeto de testes próprio da camada de Infraestrutura, que ainda não existe; entra como task futura a propor separadamente.
- `SenhaServicoTestes` usa a implementação real, não um mock — é a peça que se quer validar, mockar ela invalidaria o teste.

## Histórico de dúvidas

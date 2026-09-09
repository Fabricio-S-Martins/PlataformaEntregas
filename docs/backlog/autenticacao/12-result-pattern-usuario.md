# 12 — Adotar Result Pattern nas invariantes do Usuario

**Módulo:** Autenticação
**Camada:** Domínio, Aplicação
**Status:** feito

## Contexto

Hoje `Usuario` só valida e-mail (via VO `Email`) — Nome vazio, SenhaHash vazio e outras invariantes não são checadas, e a única forma de sinalizar erro é lançando exceção no construtor. Esta task introduz **Result Pattern**: a criação de `Usuario` passa a devolver um resultado explícito (sucesso ou lista de erros) em vez de lançar exceção, e passa a reforçar mais invariantes de uma vez. Esse padrão vai ser reaproveitado no módulo Pedidos (transições de estado do pedido são o próximo lugar natural pra usar Result — falha aí é fluxo de negócio esperado, não exceção).

## Conceito novo: Result Pattern

Em vez de um construtor lançar exceção quando algo é inválido, um **método de fábrica estático** devolve um objeto `Resultado<T>` que representa explicitamente sucesso (contendo o `T` criado) ou falha (contendo a lista de erros). Quem chama é obrigado a checar qual dos dois casos aconteceu antes de acessar o valor — a possibilidade de falha fica visível na assinatura do método, em vez de escondida numa exceção que pode ser esquecida.

Referência: https://enterprisecraftsmanship.com/posts/functional-c-handling-failures-input-errors/

## O que fazer

1. Criar `Resultado<T>` em `Compartilhado/` no projeto `Modulos.Autenticacao.Dominio`, com:
   - Propriedade `Sucesso` (`bool`), definida no construtor e nunca alterada depois.
   - Propriedade `Valor` (`T`), preenchida só quando `Sucesso` é `true`.
   - Propriedade `Erros` (`IReadOnlyList<string>`), preenchida só quando `Sucesso` é `false`.
   - Construtor privado, usado só pelos dois métodos de fábrica abaixo.
   - Método estático `ComSucesso(T valor)`, retornando um `Resultado<T>` com `Sucesso = true` e `Valor` igual ao parâmetro recebido.
   - Método estático `ComFalha(IEnumerable<string> erros)`, retornando um `Resultado<T>` com `Sucesso = false` e `Erros` igual ao parâmetro recebido.
2. Trocar o construtor público de `Usuario` por um construtor privado, mantendo o construtor privado sem parâmetros já existente para o EF Core.
3. Criar o método de fábrica `Usuario.Criar(nome, email, senhaHash, papel)`, retornando `Resultado<Usuario>`, validando: Nome não vazio, Email válido (reaproveitando o VO `Email` já existente), SenhaHash não vazio. Checar os três critérios sem interromper no primeiro erro encontrado — a falha deve acumular todos os erros aplicáveis, não só o primeiro.
4. Em `CriarUsuarioHandler`, trocar `new Usuario(...)` por `Usuario.Criar(...)` — se o resultado for sucesso, deve seguir o fluxo; caso contrário, deve lançar uma `ArgumentException` com os erros.
5. Em `UsuarioTestes.cs`, reescrever (não criar novos) os 3 testes já existentes (usam `new Usuario(...)`, que deixa de existir) para usar `Usuario.Criar(...)` e checar `resultadoUsuario.Sucesso`/`resultadoUsuario.Erros` — em especial `GerarUsuario_SomenteComEmailInvalido_DeveLancarExcecao`, que hoje espera uma exceção e passa a esperar `resultadoUsuario.Sucesso == false`.

## Cenários a cobrir

**`Usuario.Criar`:**
- Dados válidos retorna sucesso com o `Usuario` corretamente preenchido.
- Nome vazio retorna falha com o erro correspondente.
- E-mail inválido retorna falha com o erro correspondente.
- SenhaHash vazio retorna falha com o erro correspondente.
- Mais de um dado inválido ao mesmo tempo retorna falha com todos os erros correspondentes, não só o primeiro.

**`CriarUsuarioHandler`:**
- Dados inválidos lança `ArgumentException`, sem chamar `IUsuarioRepositorio.AdicionarAsync`.

## Arquivos a criar/alterar

- `Modulos/Autenticacao/Modulos.Autenticacao.Dominio/Compartilhado/Resultado.cs`
- `Modulos/Autenticacao/Modulos.Autenticacao.Dominio/Entidades/Usuario.cs` (alterado)
- `Modulos/Autenticacao/Modulos.Autenticacao.Aplicacao/CasosDeUso/CriarUsuario/CriarUsuarioHandler.cs` (alterado)
- `Testes/Autenticacao/Modulos.Autenticacao.Dominio.Testes/Entidades/UsuarioTestes.cs` (alterado)
- `Testes/Autenticacao/Modulos.Autenticacao.Aplicacao.Testes/CasosDeUso/CriarUsuario/CriarUsuarioHandlerTestes.cs` (alterado)

## Notas / decisões tomadas

- Validação de força da senha (ex: tamanho mínimo, caractere especial) fica fora do escopo desta task: `Usuario` nunca recebe a senha em texto puro (só o hash, já gerado por `ISenhaServico` antes de chegar em `Usuario.Criar`), então essa regra não pode viver no Domínio — seria uma validação da Aplicação, a propor como task separada se fizer sentido.
- `Papel` não precisa de validação adicional em `Usuario.Criar` — já chega tipado como enum, o parse de string pra enum (que pode falhar) acontece antes, em `CriarUsuarioHandler`/`CriarUsuarioCommand`, fora do escopo desta task.
- `CriarUsuarioHandler` converte a falha do `Resultado` em `ArgumentException` (erros concatenados numa mensagem) para preservar o contrato de erro que `CriarUsuarioEndpoint` já expõe hoje (`{ erro: string }`). Expor a lista de erros estruturada até a API (`{ erros: string[] }`) é uma melhoria futura, fora do escopo desta task.

## Histórico de dúvidas

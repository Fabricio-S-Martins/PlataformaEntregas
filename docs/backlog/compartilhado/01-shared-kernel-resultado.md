# 01 — Extrair Resultado&lt;T&gt; para um Shared Kernel entre módulos

**Módulo:** Compartilhado
**Camada:** Domínio
**Status:** feito

## Contexto

`Resultado<T>` (Result Pattern) hoje mora em `Modulos.Autenticacao.Dominio/Compartilhado/`, exclusivo do módulo Autenticação. Catálogo vai precisar do mesmo tipo para suas próprias invariantes de domínio, e referenciar o projeto de Domínio de Autenticação a partir de Catálogo quebraria o isolamento entre módulos do Modular Monolith — um módulo não deve depender do Domínio de outro.

## Conceito novo: Shared Kernel

Padrão de DDD para código que múltiplos módulos legitimamente compartilham (tipos genéricos sem regra de negócio de nenhum módulo específico, como `Resultado<T>`) — vive num projeto à parte, referenciado por todos os módulos que precisam dele. Diferente de acoplar um módulo ao Domínio de outro: o Shared Kernel não pertence a nenhum módulo, e só deve conter o que é genuinamente neutro (nenhuma regra de Usuario, Restaurante etc. pode viver ali).

Referência: https://learn.microsoft.com/pt-br/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/implement-value-objects (seção sobre Shared Kernel no contexto de DDD)

## O que fazer

**No disco (estrutura de pastas/projeto):**

1. Na raiz do repositório (mesmo nível da pasta `Modulos/`), criar uma pasta nova chamada `Compartilhado/`. Ela fica **fora** de `Modulos/`, porque o que vai dentro dela não pertence a nenhum módulo específico.
2. Dentro dessa pasta `Compartilhado/`, criar outra pasta chamada `Compartilhado.Dominio/` e, dentro dela, o projeto `Compartilhado.Dominio` (classlib). Caminho final: `Compartilhado/Compartilhado.Dominio/Compartilhado.Dominio.csproj`.
3. No `.csproj` de `Compartilhado.Dominio`, deixar `Nullable` com o mesmo valor usado em todos os outros projetos da solution (`disable`) — hoje ele está diferente (`enable`), sem motivo pra isso, e sozinho nesse padrão.

**Movendo o código:**

4. Pegar o arquivo `Resultado.cs`, que hoje está em `Modulos/Autenticacao/Modulos.Autenticacao.Dominio/Compartilhado/Resultado.cs`, e movê-lo para dentro do novo projeto: `Compartilhado/Compartilhado.Dominio/Resultado.cs` (direto na raiz do projeto, sem subpasta).
5. Trocar o namespace dentro do arquivo, de `Modulos.Autenticacao.Dominio.Compartilhado` para `Compartilhado.Dominio`.
6. Apagar a pasta `Compartilhado/` que sobrou vazia dentro de `Modulos.Autenticacao.Dominio/`.

**Ligando os projetos:**

7. Em `Modulos.Autenticacao.Dominio`, adicionar uma referência de projeto (`ProjectReference`) apontando para `Compartilhado.Dominio`.
8. Em todo arquivo que usa `Resultado<T>` (`Usuario.cs`, `CriarUsuarioHandler.cs`, `UsuarioTestes.cs`, `CriarUsuarioHandlerTestes.cs`), trocar o `using Modulos.Autenticacao.Dominio.Compartilhado;` por `using Compartilhado.Dominio;`.

**Na solution (`.slnx`):**

9. Criar uma pasta de solução nova chamada `/Compartilhado/` — **irmã** da pasta `/Modulos/` que já existe, não uma subpasta dela (porque, de novo, isso não é um módulo).
10. Dentro dessa pasta de solução `/Compartilhado/`, referenciar o projeto `Compartilhado.Dominio`.

## Cenários a cobrir

Nenhum cenário novo — é só mover código já testado, sem alterar comportamento. A suíte de testes existente (`Modulos.Autenticacao.Dominio.Testes`, `Modulos.Autenticacao.Aplicacao.Testes`) deve continuar 100% verde depois da mudança, sem nenhum teste alterado além dos `using`.

## Arquivos a criar/alterar

- `Compartilhado/Compartilhado.Dominio/Resultado.cs` (novo, movido)
- `Modulos/Autenticacao/Modulos.Autenticacao.Dominio/Compartilhado/Resultado.cs` (removido)
- `Modulos/Autenticacao/Modulos.Autenticacao.Dominio/Entidades/Usuario.cs` (using ajustado)
- `Modulos/Autenticacao/Modulos.Autenticacao.Aplicacao/CasosDeUso/CriarUsuario/CriarUsuarioHandler.cs` (using ajustado)
- `Testes/Autenticacao/Modulos.Autenticacao.Dominio.Testes/Entidades/UsuarioTestes.cs` (using ajustado)
- `Testes/Autenticacao/Modulos.Autenticacao.Aplicacao.Testes/CasosDeUso/CriarUsuario/CriarUsuarioHandlerTestes.cs` (using ajustado)

## Notas / decisões tomadas

- Só `Resultado<T>` migra por enquanto — qualquer outro tipo candidato a Shared Kernel no futuro (ex: uma classe base `Entidade` com Id) entra numa task própria quando a necessidade aparecer, não antecipar aqui.

## Histórico de dúvidas

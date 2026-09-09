# 04 — Criar caso de uso de cadastro de Restaurante

**Módulo:** Catálogo
**Camada:** Aplicação
**Status:** todo

## Contexto

Primeira peça da camada de Aplicação do módulo Catálogo. Segue o mesmo padrão CQRS com MediatR já usado em Autenticação (`CriarUsuarioCommand`/`CriarUsuarioHandler`): um Command representando a intenção de cadastrar um Restaurante, e um Handler que aciona `Restaurante.Criar` (Domínio) e persiste via um repositório abstrato pela interface `IRestauranteRepositorio`.

## O que fazer

1. Na pasta `Modulos/Catalogo/`, criar a pasta `Modulos.Catalogo.Aplicacao/` e, dentro dela, o projeto `Modulos.Catalogo.Aplicacao`.
2. Dentro desse projeto, criar a pasta `Repositorios/` e, dentro dela, a interface `IRestauranteRepositorio`, com o método `AdicionarAsync(Restaurante restaurante)`.
3. Dentro desse projeto, criar a pasta `CasosDeUso/CadastrarRestaurante/` e, dentro dela, o record `CadastrarRestauranteCommand(string Nome, string Cnpj)`, implementando `IRequest` do MediatR.
4. Na mesma pasta, criar `CadastrarRestauranteHandler`, implementando `IRequestHandler<CadastrarRestauranteCommand>`: chama `Restaurante.Criar(request.Nome, request.Cnpj)`; se o resultado for sucesso, chama `IRestauranteRepositorio.AdicionarAsync`; caso contrário, lança uma `ArgumentException` com os erros.
5. Na raiz do projeto, criar a classe estática `InjecaoDeDependencia`, com o método de extensão `RegistrarCatalogoAplicacao(this IServiceCollection services)`, registrando o MediatR para o assembly deste projeto.

## Cenários a cobrir

**`CadastrarRestauranteHandler`:**
- Dados válidos persiste o Restaurante via `IRestauranteRepositorio.AdicionarAsync`.
- Dados inválidos lança `ArgumentException`, sem chamar `AdicionarAsync`.

## Arquivos a criar/alterar

- `Modulos/Catalogo/Modulos.Catalogo.Aplicacao/Repositorios/IRestauranteRepositorio.cs`
- `Modulos/Catalogo/Modulos.Catalogo.Aplicacao/CasosDeUso/CadastrarRestaurante/CadastrarRestauranteCommand.cs`
- `Modulos/Catalogo/Modulos.Catalogo.Aplicacao/CasosDeUso/CadastrarRestaurante/CadastrarRestauranteHandler.cs`
- `Modulos/Catalogo/Modulos.Catalogo.Aplicacao/InjecaoDeDependencia.cs`

## Notas / decisões tomadas

- Checar duplicidade de Cnpj (dois restaurantes com o mesmo Cnpj) fica fora de escopo desta task — se fizer sentido, vira uma validação separada no Handler (ou constraint única no banco), a propor depois que a Infraestrutura do módulo existir.
- `CadastrarRestauranteHandler` converte a falha do `Resultado` em `ArgumentException`, mesmo padrão já usado em `CriarUsuarioHandler`.
- Testes automatizados deste Handler ficam para uma task separada, mesmo padrão adotado em Autenticação (task de Aplicação primeiro, testes depois).

## Histórico de dúvidas

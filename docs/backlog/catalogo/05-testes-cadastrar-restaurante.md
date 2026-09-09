# 05 — Cobrir caso de uso de cadastro de Restaurante com testes automatizados

**Módulo:** Catálogo
**Camada:** Aplicação
**Status:** feito

## Contexto

A task 04 criou o `CadastrarRestauranteHandler`, que orquestra a criação de um `Restaurante` e delega a persistência para `IRestauranteRepositorio`. Como ainda não existe uma implementação real do repositório (isso é Infraestrutura, task futura), o handler só pode ser testado com uma implementação falsa da interface, usando **Moq** — mesma técnica já usada em `CriarUsuarioHandlerTestes.cs`.

## O que fazer

1. Na pasta `Testes/Catalogo/`, criar a pasta `Modulos.Catalogo.Aplicacao.Testes/` e, dentro dela, o projeto `Modulos.Catalogo.Aplicacao.Testes`.
2. Dentro desse projeto, criar a pasta `CasosDeUso/CadastrarRestaurante/`.
3. Criar a classe de teste `CadastrarRestauranteHandlerTestes`, usando um mock de `IRestauranteRepositorio`.

## Cenários a cobrir

**`CadastrarRestauranteHandlerTestes` (`CadastrarRestauranteHandler`):**
- Dados válidos chama `IRestauranteRepositorio.AdicionarAsync` exatamente uma vez.
- Dados inválidos lança `ArgumentException`, sem chamar `AdicionarAsync`.

## Arquivos a criar/alterar

- `Testes/Catalogo/Modulos.Catalogo.Aplicacao.Testes/CasosDeUso/CadastrarRestaurante/CadastrarRestauranteHandlerTestes.cs`

## Notas / decisões tomadas

- Nomenclatura dos métodos de teste segue o padrão já fixado: `Metodo_ComCenario_DeveResultado`.

## Histórico de dúvidas

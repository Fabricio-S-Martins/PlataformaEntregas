# 03 — Cobrir Restaurante, Cardapio e ItemCardapio com testes automatizados

**Módulo:** Catálogo
**Camada:** Domínio
**Status:** feito

## Contexto

`Restaurante`, `Cardapio` e `ItemCardapio` (tasks 01 e 02) ainda não têm nenhum teste automatizado — as invariantes de cada `Criar` e o comportamento de `Cardapio.AdicionarItem` só foram checados manualmente até aqui. Mesmo padrão de testes já usado em `UsuarioTestes.cs`: xUnit, Bogus pra gerar dados válidos, checando `resultado.Sucesso`/`resultado.Erros`/`resultado.Valor`.

## O que fazer

1. Na pasta `Testes/`, criar a pasta `Catalogo/`. Dentro dela, criar a pasta `Modulos.Catalogo.Dominio.Testes/` e, dentro dela, o projeto `Modulos.Catalogo.Dominio.Testes`.
2. Dentro desse projeto, criar a pasta `Entidades/`.
3. Criar a classe de teste `RestauranteTestes`.
4. Criar a classe de teste `ItemCardapioTestes`.
5. Criar a classe de teste `CardapioTestes`.

## Cenários a cobrir

**`RestauranteTestes` (`Restaurante.Criar`):**
- Dados válidos retorna sucesso.
- Nome vazio retorna falha com o erro correspondente.
- Cnpj inválido retorna falha com o erro correspondente.
- Nome vazio e Cnpj inválido ao mesmo tempo retorna falha com os dois erros.

**`ItemCardapioTestes` (`ItemCardapio.Criar`):**
- Dados válidos retorna sucesso, com `Disponivel` iniciando `true`.
- `cardapioId` vazio retorna falha com o erro correspondente.
- Nome vazio retorna falha com o erro correspondente.
- Descricao vazia retorna falha com o erro correspondente.
- Preco negativo retorna falha com o erro correspondente.
- Mais de um dado inválido ao mesmo tempo retorna falha com todos os erros.

**`CardapioTestes` (`Cardapio.Criar` e `Cardapio.AdicionarItem`):**
- `Cardapio.Criar` com `restauranteId` válido retorna sucesso, com `Itens` vazia.
- `Cardapio.Criar` com `restauranteId` vazio retorna falha com o erro correspondente.
- `Cardapio.AdicionarItem` com dados válidos retorna sucesso e o item passa a constar em `Itens`.
- `Cardapio.AdicionarItem` com dados inválidos retorna falha e `Itens` não é alterada.

## Arquivos a criar/alterar

- `Testes/Catalogo/Modulos.Catalogo.Dominio.Testes/Entidades/RestauranteTestes.cs`
- `Testes/Catalogo/Modulos.Catalogo.Dominio.Testes/Entidades/ItemCardapioTestes.cs`
- `Testes/Catalogo/Modulos.Catalogo.Dominio.Testes/Entidades/CardapioTestes.cs`

## Notas / decisões tomadas

- Nomenclatura dos métodos de teste segue o padrão já fixado: `Metodo_ComCenario_DeveResultado`.

## Histórico de dúvidas

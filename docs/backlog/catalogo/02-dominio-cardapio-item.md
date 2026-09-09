# 02 — Modelar Cardapio e ItemCardapio no Domínio

**Módulo:** Catálogo
**Camada:** Domínio
**Status:** feito

## Contexto

Segunda entidade do módulo Catálogo. Cada `Restaurante` tem um `Cardapio`, que agrega os `ItemCardapio` que os Clientes vão ver ao navegar o restaurante. Segue o mesmo Result Pattern já usado em `Usuario` e `Restaurante`, agora com um `Cardapio` que também sabe adicionar itens validando as regras deles.

## O que fazer

**`ItemCardapio`:**

1. No projeto `Modulos.Catalogo.Dominio`, dentro da pasta `Entidades/`, criar a classe `ItemCardapio`, com construtor privado com parâmetros (usado só pelo método de fábrica do passo 2) e construtor privado sem parâmetros (usado só pelo EF Core), e propriedades:
   - `Id` (`Guid`)
   - `CardapioId` (`Guid`)
   - `Nome` (`string`)
   - `Descricao` (`string`)
   - `Preco` (`decimal`)
   - `Disponivel` (`bool`) — indica se o item pode ser pedido no momento
2. Criar o método de fábrica estático `ItemCardapio.Criar(cardapioId, nome, descricao, preco)`, retornando `Resultado<ItemCardapio>`, validando: `cardapioId` não vazio, Nome não vazio, Descricao não vazia, Preco maior ou igual a zero. `Disponivel` inicia sempre `true`. Checar os quatro critérios sem interromper no primeiro erro — acumular todos os erros aplicáveis.

**`Cardapio`:**

3. Na mesma pasta `Entidades/`, criar a classe `Cardapio`, com construtor privado com parâmetros (usado só pelo método de fábrica do passo 4) e construtor privado sem parâmetros (usado só pelo EF Core), e propriedades:
   - `Id` (`Guid`)
   - `RestauranteId` (`Guid`)
   - `Itens` (`IReadOnlyList<ItemCardapio>`) — devolve uma propriedade privada `List<ItemCardapio> ItensInterno`, que inicia vazia. Manter essa lista privada por trás de `Itens` permite adicionar itens internamente (passo 5) sem expor mutação pra fora da classe.
4. Criar o método de fábrica estático `Cardapio.Criar(restauranteId)`, retornando `Resultado<Cardapio>`, validando: `restauranteId` não vazio.
5. Criar o método de instância `AdicionarItem(nome, descricao, preco)`, retornando `Resultado<ItemCardapio>` — internamente chama `ItemCardapio.Criar(Id, nome, descricao, preco)` (passo 2); se o resultado for sucesso, adiciona o item criado a `ItensInterno` antes de retornar.

## Cenários a cobrir

**`ItemCardapio.Criar`:**
- Dados válidos retorna sucesso, com `Disponivel` iniciando `true`.
- `cardapioId` vazio retorna falha com o erro correspondente.
- Nome vazio retorna falha com o erro correspondente.
- Descricao vazia retorna falha com o erro correspondente.
- Preco negativo retorna falha com o erro correspondente.
- Mais de um dado inválido ao mesmo tempo retorna falha com todos os erros correspondentes, não só o primeiro.

**`Cardapio.Criar`:**
- `restauranteId` válido retorna sucesso, com `Itens` vazia.
- `restauranteId` vazio retorna falha com o erro correspondente.

**`Cardapio.AdicionarItem`:**
- Dados válidos retorna sucesso e o item passa a constar em `Itens`.
- Dados inválidos (nome vazio, descricao vazia e/ou preço negativo) retorna falha e `Itens` não é alterada.

## Arquivos a criar/alterar

- `Modulos/Catalogo/Modulos.Catalogo.Dominio/Entidades/ItemCardapio.cs`
- `Modulos/Catalogo/Modulos.Catalogo.Dominio/Entidades/Cardapio.cs`

## Notas / decisões tomadas

- Um `Restaurante` tem só um `Cardapio` (não múltiplos cardápios/seções por enquanto) — se aparecer a necessidade de cardápios diferentes por período (ex: almoço/janta) ou categorias de itens, isso vira task separada.
- `Cardapio` não precisa nascer com nenhum item — pode ser criado vazio e receber itens depois via `AdicionarItem`.

## Histórico de dúvidas

# 01 — Modelar a entidade Restaurante no Domínio

**Módulo:** Catálogo
**Camada:** Domínio
**Status:** feito

## Contexto

Início do módulo Catálogo. Primeira entidade: `Restaurante` — quem cadastra o cardápio que os Clientes vão navegar. Segue a mesma abordagem já validada em Autenticação: Domínio puro (sem EF Core, sem ASP.NET), invariantes reforçadas via Result Pattern (`Resultado<T>`, agora vindo do Shared Kernel — depende da task [Compartilhado 01](../compartilhado/01-shared-kernel-resultado.md)).

## O que fazer

1. Na pasta `Modulos/`, criar a pasta `Catalogo/` e, dentro dela, o projeto `Modulos.Catalogo.Dominio` (classlib), com referência ao projeto `Compartilhado.Dominio`.
2. Na solution (`.slnx`), criar a pasta `/Modulos/Catalogo/` e referenciar `Modulos.Catalogo.Dominio` ali dentro.
3. No projeto `Modulos.Catalogo.Dominio`, criar a pasta `Entidades/` e, dentro dela, a classe `Restaurante`, com:
   - Construtor privado com parâmetros (usado só pelo método de fábrica do passo 4) e construtor privado sem parâmetros (usado só pelo EF Core).
   - Propriedades: `Id` (`Guid`), `Nome` (`string`), `Cnpj` (`string`), `Ativo` (`bool`) — indica se o restaurante está aceitando pedidos no momento.
4. Criar o método de fábrica estático `Restaurante.Criar(nome, cnpj)`, retornando `Resultado<Restaurante>`, validando: Nome não vazio, Cnpj não nulo/vazio e com exatamente 14 dígitos numéricos. `Ativo` inicia sempre `true`. Checar os dois critérios sem interromper no primeiro erro — acumular todos os erros aplicáveis.

## Cenários a cobrir

**`Restaurante.Criar`:**
- Dados válidos retorna sucesso, com `Ativo` iniciando `true`.
- Nome vazio retorna falha com o erro correspondente.
- Cnpj fora do formato (não numérico, ou diferente de 14 dígitos) retorna falha com o erro correspondente.
- Nome vazio e Cnpj inválido ao mesmo tempo retorna falha com os dois erros, não só o primeiro.

## Arquivos a criar/alterar

- `Modulos/Catalogo/Modulos.Catalogo.Dominio/Entidades/Restaurante.cs`

## Notas / decisões tomadas

- Cnpj fica como `string` por enquanto (sem VO dedicado) — se aparecer mais de uma entidade validando/formatando Cnpj no módulo, uma VO `Cnpj` vira task separada.
- Cardápio e Item ficam fora desta task — entram como próximas tasks do módulo, depois que `Restaurante` estiver validado.

## Histórico de dúvidas

---
tags: [backlog, modulo/pedidos, fluxo/pedido-status, tarefa/concluido]
---

# 01 — Modelar o agregado Pedido com máquina de estados e eventos de domínio

**Módulo:** Pedidos
**Camada:** Domínio

## Contexto

Início do módulo Pedidos, o núcleo do sistema. O agregado `Pedido` carrega o ciclo de vida inteiro de um pedido — do momento em que o Cliente monta o carrinho até a entrega — e cada mudança de status precisa disparar um evento de domínio, pra outros módulos (Pagamentos, Notificações) reagirem no futuro sem acoplamento direto.

Segue a mesma abordagem já validada em Autenticação e Catálogo (Domínio puro, sem EF Core/MediatR, invariantes reforçadas via `Resultado<T>` do Shared Kernel), incluindo o mesmo padrão de agregado-pai-com-filhos já usado em `Cardapio`/`ItemCardapio`: o pai nasce vazio e os filhos entram um de cada vez por um método `Adicionar...`, que valida e cria o filho internamente. Duas peças novas em relação aos módulos anteriores: uma **máquina de estados** (o agregado só aceita transições de status válidas) e o **Domain Events pattern** (o agregado registra os eventos ocorridos, sem publicá-los — quem publica é a Aplicação, numa task futura).

`Pedido` referencia `Cliente` (Autenticação) e `Restaurante`/itens de cardápio (Catálogo) só pelo Id (Guid) — sem referência de projeto entre módulos, pra manter o isolamento do Modular Monolith.

## Conceitos novos

### Máquina de estados (State Pattern, aplicado como invariante do agregado)

O agregado guarda um status atual e só aceita transições explicitamente permitidas — cada método de transição confere se o status atual permite aquele passo antes de mudar, devolvendo falha (via `Resultado<T>`) se não permitir. Evita um pedido pular etapa (ex: ir de "Criado" direto pra "Entregue") ou voltar (ex: de "Entregue" pra "Aceito").

### Domain Events

Quando algo relevante acontece dentro do agregado (uma transição de status), ele registra um evento descrevendo o que ocorreu, numa lista interna — sem publicar nada nem depender de MediatR (isso manteria o Domínio livre de dependência de framework). Quem lê essa lista e publica os eventos (via MediatR `INotification`) é a Aplicação, depois de persistir a mudança — fica pra uma task futura, quando os primeiros handlers de evento existirem.

Referência: https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/domain-events-design-implementation

## O que fazer

1. Na pasta `Modulos/`, criar a pasta `Pedidos/` e, dentro dela, o projeto `Modulos.Pedidos.Dominio` (classlib).

2. No projeto `Modulos.Pedidos.Dominio`, criar a pasta `Enums/`.

Criar o enum `StatusPedido`, com os valores, nesta ordem:
`Criado`, `Pago`, `Aceito`, `EmPreparo`, `SaiuParaEntrega`, `Entregue`, `Cancelado`.

3. No projeto `Modulos.Pedidos.Dominio`, criar a pasta `Eventos/`.

Criar a interface marcadora `IEventoDominio`, vazia, sem membros.

Criar, nesta ordem, os records que representam cada transição, todos implementando `IEventoDominio` e todos só com a prop `PedidoId`:
`PedidoCriadoEvento`, `PedidoItemAdicionadoEvento`, `PedidoPagoEvento`, `PedidoAceitoEvento`, `PedidoEmPreparoEvento`, `PedidoSaiuParaEntregaEvento`, `PedidoEntregueEvento`, `PedidoCanceladoEvento`.

4. No projeto `Modulos.Pedidos.Dominio`, criar a pasta `Entidades/`.

### ItemPedido

5. Criar `ItemPedido` com as seguintes props:
`Id`, `PedidoId`, `ItemCardapioId`, `Quantidade` e `PrecoUnitario` (snapshot do preço no momento do pedido, não referencia o Catálogo depois de criado).

Com dois construtores: um construtor base privado, sem parâmetros, pro EF Core; e outro construtor privado recebendo `pedidoId`, `itemCardapioId`, `quantidade` e `precoUnitario` — dentro dele, seta manualmente a prop `Id` com `Guid.NewGuid()`.

6. Criar o método de fábrica estático `ItemPedido.Criar(pedidoId, itemCardapioId, quantidade, precoUnitario)`, retornando `Resultado<ItemPedido>`, validando: `pedidoId` diferente de `Guid.Empty`, `itemCardapioId` diferente de `Guid.Empty`, `quantidade` maior que zero, `precoUnitario` maior que zero — acumulando todos os erros aplicáveis, não parando no primeiro. Se válido, delega a criação pro construtor do passo 5.

### Pedido

7. Criar `Pedido` com as seguintes props:
`Id`, `ClienteId`, `RestauranteId`, `Status`, `CriadoEm`, `Itens` (lista somente leitura de `ItemPedido`) e `Eventos` (lista somente leitura de `IEventoDominio`).

Por baixo de `Itens` e `Eventos`, as coleções de verdade ficam em duas props privadas, `ItensInterno` e `EventosInterno` — mesmo padrão já usado em `Cardapio`/`Itens`/`ItensInterno`.

Com dois construtores: um construtor base privado, sem parâmetros, pro EF Core; e outro construtor privado recebendo `clienteId` e `restauranteId` — dentro dele, seta manualmente a prop `Id` com `Guid.NewGuid()`, `Status` com `StatusPedido.Criado`, `CriadoEm` com `DateTime.UtcNow`, e registra `PedidoCriadoEvento` em `EventosInterno`. O pedido nasce sem itens — eles entram pelo método do passo 9.

8. Criar o método de fábrica estático `Pedido.Criar(clienteId, restauranteId)`, retornando `Resultado<Pedido>`, validando: `clienteId` diferente de `Guid.Empty`, `restauranteId` diferente de `Guid.Empty` — acumulando todos os erros aplicáveis, não parando no primeiro. Se válido, delega a criação pro construtor do passo 7.

9. Criar o método `AdicionarItem(itemCardapioId, quantidade, precoUnitario)`, retornando `Resultado<ItemPedido>`: se `Status` for diferente de `Criado`, devolve falha (pedido já avançou, não aceita mais item). Senão, chama `ItemPedido.Criar` (passo 6) passando o próprio `Id`; se inválido, propaga a falha; se válido, adiciona em `ItensInterno`, registra `PedidoItemAdicionadoEvento` em `EventosInterno`, e devolve sucesso com o `ItemPedido` criado.

10. Criar seis métodos de transição, um por método, todos sem parâmetro e retornando `Resultado<Pedido>`. Cada um segue sempre o mesmo formato: confere se `Status` atual é um dos status de origem permitidos pra aquele método; se não for, devolve falha com uma mensagem citando o status atual e a transição negada (ex: "Pedido em Criado não pode ser aceito."), sem mudar nada; se for, muda `Status` pro destino, registra o evento correspondente em `EventosInterno`, e devolve sucesso.

    - `ConfirmarPagamento`: aceita só a partir de `Criado`, e só se `Itens` não estiver vazio (senão, falha com mensagem própria: "Pedido sem itens não pode ser pago."); muda pra `Pago`; registra `PedidoPagoEvento`.
    - `Aceitar`: aceita só a partir de `Pago`; muda pra `Aceito`; registra `PedidoAceitoEvento`.
    - `IniciarPreparo`: aceita só a partir de `Aceito`; muda pra `EmPreparo`; registra `PedidoEmPreparoEvento`.
    - `SairParaEntrega`: aceita só a partir de `EmPreparo`; muda pra `SaiuParaEntrega`; registra `PedidoSaiuParaEntregaEvento`.
    - `Entregar`: aceita só a partir de `SaiuParaEntrega`; muda pra `Entregue`; registra `PedidoEntregueEvento`.
    - `Cancelar`: aceita a partir de `Criado` ou `Pago`; muda pra `Cancelado`; registra `PedidoCanceladoEvento`. Chamado com `Status` em `Aceito` ou qualquer status posterior, devolve falha — a partir daí o restaurante já está preparando, cancelamento não é mais permitido.

## Notas / decisões tomadas

- `Pedido` referencia `Cliente` e `Restaurante`/`ItemCardapio` só pelo Id (Guid) — sem referência de projeto entre módulos. Se telas precisarem exibir nome/dado de outro módulo, isso vira Query própria consultando o módulo dono depois, não navegação direta pelo agregado.
- Segue o mesmo padrão de agregado-pai-com-filhos já usado em `Cardapio`/`ItemCardapio`: `Pedido` nasce vazio e os itens entram um de cada vez via `AdicionarItem`, que valida e cria o `ItemPedido` internamente — nenhum outro código monta `ItemPedido` diretamente.
- Item só pode ser adicionado enquanto o pedido está em `Criado`; confirmar pagamento exige pelo menos um item.
- Publicação dos eventos de domínio (via MediatR `INotification`) fica fora de escopo desta task — entra quando a Aplicação do módulo existir, junto com os primeiros handlers reagindo a eles.
- Endereço de entrega e forma de pagamento ficam fora desta task — entram quando o fluxo de Pagamentos for modelado.

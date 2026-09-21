# 06 — Expor endpoints do ciclo de vida do Pedido

**Módulo:** Pedidos
**Camada:** API
**Status:** todo

## Contexto

Os 8 casos de uso do `Pedido` (task 03) ainda não são alcançáveis de fora — falta a camada API, com um endpoint HTTP por caso de uso. Segue o mesmo padrão já usado em Catálogo: Minimal API, um projeto `Modulos.Pedidos.Api` (biblioteca, `FrameworkReference Microsoft.AspNetCore.App`, sem `Program.cs` próprio — o host `PlataformaEntregas.Api` é quem sobe o servidor), referenciando Aplicação e Infraestrutura, com um método de extensão `RegistrarPedidosApi` compondo o registro de DI dos dois.

Toda falha de validação/invariante do Domínio (`ArgumentException`, lançada pelos Handlers) vira `400` com corpo `{ erro }`; pedido não encontrado (`InvalidOperationException`) vira `404` com o mesmo formato de corpo — decisão já tomada na task 03. Despacho dos Commands via `ISender` do MediatR, mesmo tipo já usado nos endpoints de Catálogo.

## O que fazer

1. Na pasta `Modulos/Pedidos/`, criar a pasta `Modulos.Pedidos.Api/`.
2. Dentro dessa pasta, criar o projeto `Modulos.Pedidos.Api` (`Microsoft.NET.Sdk`, com `FrameworkReference Microsoft.AspNetCore.App`).

### CriarPedido

3. Dentro do projeto, criar a pasta `Endpoints/CriarPedido/`. Dentro dela, criar, nesta ordem: o record `CriarPedidoRequest`, com `ClienteId` (`Guid`) e `RestauranteId` (`Guid`); a classe `CriarPedidoEndpoint`, com um método de extensão de `IEndpointRouteBuilder` que mapeia `POST /pedidos`, despacha `CriarPedidoCommand` via `ISender` com os dados do request, e retorna `201` com o corpo `{ id }` (o `Id` do pedido criado), ou `400` com `{ erro }` se `ArgumentException` for lançada.

### AdicionarItem

4. Dentro do projeto, criar a pasta `Endpoints/AdicionarItem/`. Dentro dela, criar, nesta ordem: o record `AdicionarItemRequest`, com `ItemCardapioId` (`Guid`), `Quantidade` (`int`) e `PrecoUnitario` (`decimal`); a classe `AdicionarItemEndpoint`, com um método de extensão de `IEndpointRouteBuilder` que mapeia `POST /pedidos/{id}/itens`, despacha `AdicionarItemCommand` via `ISender` com o `id` da rota e os dados do request, e retorna `204` se der certo, `400` com `{ erro }` se `ArgumentException` for lançada, `404` com `{ erro }` se `InvalidOperationException` for lançada.

### Transições de status

5. Criar seis pastas em `Endpoints/`, uma por transição, cada uma com uma classe de Endpoint seguindo exatamente o mesmo formato do passo 4 (sem request — só o `id` da rota — `204`/`400`/`404`):
    - `Endpoints/ConfirmarPagamento/ConfirmarPagamentoEndpoint`: `POST /pedidos/{id}/confirmar-pagamento`, despacha `ConfirmarPagamentoCommand`.
    - `Endpoints/Aceitar/AceitarEndpoint`: `POST /pedidos/{id}/aceitar`, despacha `AceitarCommand`.
    - `Endpoints/IniciarPreparo/IniciarPreparoEndpoint`: `POST /pedidos/{id}/iniciar-preparo`, despacha `IniciarPreparoCommand`.
    - `Endpoints/SairParaEntrega/SairParaEntregaEndpoint`: `POST /pedidos/{id}/sair-para-entrega`, despacha `SairParaEntregaCommand`.
    - `Endpoints/Entregar/EntregarEndpoint`: `POST /pedidos/{id}/entregar`, despacha `EntregarCommand`.
    - `Endpoints/Cancelar/CancelarEndpoint`: `POST /pedidos/{id}/cancelar`, despacha `CancelarCommand`.

### Composição

6. Na raiz do projeto, criar `PedidosEndpoints`, com um método de extensão de `IEndpointRouteBuilder` que registra os 8 mapeamentos dos passos 3 a 5.
7. Na raiz do projeto, criar `InjecaoDeDependencia`, com um método de extensão de `IServiceCollection` chamado `RegistrarPedidosApi`, chamando `RegistrarPedidosInfraestrutura` e `RegistrarPedidosAplicacao`.

### Host

8. No `Program.cs` do host, trocar a chamada a `RegistrarPedidosInfraestrutura` (colocada ali pela task 05) por `RegistrarPedidosApi`, e, dentro do grupo `/api` já existente, chamar `MapPedidosEndpoints()`.

## Notas / decisões tomadas

- Rotas de transição usam `kebab-case` no segmento de ação (`confirmar-pagamento`, `iniciar-preparo`...) — convenção comum de API REST pra palavras compostas na URL; os outros módulos ainda não tinham rota composta pra fixar um padrão.
- Sem endpoint de leitura (`GET /pedidos/{id}`) nesta task — não existe query nem caso de uso de leitura pro Pedido ainda; vira task futura.
- Sem autorização (`RequireAuthorization`) por enquanto, mesmo padrão dos endpoints de Catálogo — Pedidos ainda não decidiu papel/autenticação exigida por rota.
- `Modulos.Pedidos.Api` referencia tanto `Modulos.Pedidos.Aplicacao` quanto `Modulos.Pedidos.Infraestrutura` — mesmo arranjo (a API do módulo é o mini composition root dele) já usado em Autenticação e Catálogo.

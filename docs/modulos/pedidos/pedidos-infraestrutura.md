# Pedidos: Infraestrutura

Projeto `Modulos.Pedidos.Infraestrutura`. Módulo: [Pedidos](pedidos.md).

## Responsabilidade
Persistir o pedido e os itens no PostgreSQL.

## Expõe
- **PedidosDbContext:** contexto EF Core; aplica as configurações do assembly.
- **PedidoRepositorio:** implementa o `IPedidoRepositorio`; salva a cada operação e carrega o pedido com os itens.
- **Configurações:** `Pedido` e `ItemPedido` em tabelas relacionadas (itens ligados por `PedidoId`).
- **Migrations:** `InicialPedido`.
- **Registro:** `RegistrarPedidosInfraestrutura` (contexto com a string de conexão `BasePlataformaEntregas` e repositório).

## Depende de
- [Aplicação](pedidos-aplicacao.md): contrato `IPedidoRepositorio`.
- [Domínio](pedidos-dominio.md): entidades mapeadas.
- EF Core com Npgsql.

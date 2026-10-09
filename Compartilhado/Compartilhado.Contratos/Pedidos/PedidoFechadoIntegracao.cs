using MediatR;

namespace Compartilhado.Contratos.Pedidos;

public sealed record PedidoFechadoIntegracao(Guid PedidoId, decimal Valor) : INotification;

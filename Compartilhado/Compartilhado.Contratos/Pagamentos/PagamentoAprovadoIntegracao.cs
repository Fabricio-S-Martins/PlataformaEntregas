using MediatR;

namespace Compartilhado.Contratos.Pagamentos;

public sealed record PagamentoAprovadoIntegracao(Guid PedidoId, Guid PagamentoId, decimal Valor) : INotification;

using MediatR;

namespace Modulos.Pedidos.Aplicacao.CasosDeUso.AdicionarItem
{
    public record AdicionarItemCommand(Guid PedidoId, Guid ItemCardapioId, int Quantidade, decimal PrecoUnitario) : IRequest;
}
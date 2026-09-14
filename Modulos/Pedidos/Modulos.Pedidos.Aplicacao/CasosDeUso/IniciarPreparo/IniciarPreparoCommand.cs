using MediatR;

namespace Modulos.Pedidos.Aplicacao.CasosDeUso.IniciarPreparo
{
    public record IniciarPreparoCommand(Guid PedidoId) : IRequest;
}